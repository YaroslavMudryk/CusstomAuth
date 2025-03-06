using CusstomAuth.Core.Data;
using CusstomAuth.Core.ErrorHandling.Exceptions;
using CusstomAuth.Core.Helpers;
using CusstomAuth.Core.Notifyer;
using CusstomAuth.Core.Options;
using CusstomAuth.Core.Stores;
using Google.Authenticator;
using Microsoft.Extensions.Options;

namespace CusstomAuth.Core.Managers;

public interface IMfaManager
{
    Task<bool> DisableMfaAsync(string code);
    Task<MfaResponse> EnableMfaAsync(string code);
}

public class MfaManager(
    ICurrentContext currentContext,
    IUserStore userStore,
    IMfaStore mfaStore,
    INotifyer notifyer,
    TimeProvider timeProvider,
    IOptions<IdentityOptions> identityOptions) : IMfaManager
{
    public async Task<bool> DisableMfaAsync(string code)
    {
        var userId = currentContext.User.Id;

        var userForDisableMfa = await userStore.GetAsync(userId);

        if (userForDisableMfa == null)
            throw new NotFoundException("User not found");

        if (!userForDisableMfa.Mfa)
            throw new BadRequestException("MFA already diactivated");

        var twoFactor = new TwoFactorAuthenticator();

        var currentPin = twoFactor.GetCurrentPIN(userForDisableMfa.MfaSecretKey);

        if (!currentPin.Equals(code))
            throw new BadRequestException("Code is incorrect");

        userForDisableMfa.Mfa = false;
        userForDisableMfa.MfaSecretKey = null!;

        var activeMfa = await mfaStore.GetUserActiveMfaAsync(userId) ?? throw new BadRequestException("Some error, please contact support");

        activeMfa.DiactivedAt = timeProvider.GetUtcNow().UtcDateTime;
        activeMfa.DiactivedBySessionId = currentContext.User.SessionId;

        await userStore.UpdateAsync(userForDisableMfa);
        await mfaStore.UpdateAsync(activeMfa);

        await notifyer.DisableMfaAsync(userId);

        return true;
    }

    public async Task<MfaResponse> EnableMfaAsync(string code)
    {
        var userId = currentContext.User.Id;

        var user = await userStore.GetAsync(userId);

        if (user == null)
            throw new NotFoundException("User not found");

        if (string.IsNullOrEmpty(code))
        {
            var existMFA = await mfaStore.GetUserUnactiveMfaAsync(userId);
            if (existMFA == null)
            {
                var secretKey = Guid.NewGuid().ToString("N");
                var twoFactor = new TwoFactorAuthenticator();
                var setupInfo = twoFactor.GenerateSetupCode(identityOptions.Value.Mfa.IssuerName, user.Login, secretKey, false, 3);

                user.MfaSecretKey = secretKey;
                user.Mfa = false;

                var newMfa = new IdentityMfa
                {
                    UserId = userId,
                    EntryCode = setupInfo.ManualEntryKey,
                    QrCodeBase64 = setupInfo.QrCodeSetupImageUrl,
                    Secret = secretKey,
                    IsActivated = false,
                    Activated = null,
                    ActivatedBySessionId = null
                };

                await userStore.UpdateAsync(user);
                await mfaStore.CreateAsync(newMfa);

                return new MfaResponse
                {
                    QrCodeImage = setupInfo.QrCodeSetupImageUrl,
                    ManualEntryKey = setupInfo.ManualEntryKey,
                };
            }
            else
            {
                return new MfaResponse
                {
                    QrCodeImage = existMFA.QrCodeBase64,
                    ManualEntryKey = existMFA.EntryCode
                };
            }
        }
        else
        {
            if (string.IsNullOrEmpty(user.MfaSecretKey))
                throw new BadRequestException("Unable to activate MFA");
            var mfaToActivate = await mfaStore.GetUserUnactiveMfaAsync(userId);

            if (mfaToActivate == null)
                throw new BadRequestException("Unable to activate MFA");

            if (mfaToActivate.Secret != user.MfaSecretKey)
                throw new BadRequestException("Please write to support as soon as possible");

            var twoFactor = new TwoFactorAuthenticator();

            var currentPin = twoFactor.GetCurrentPIN(mfaToActivate.Secret);

            if (!currentPin.Equals(code))
                throw new BadRequestException("Code is incorrect");

            user.Mfa = true;

            mfaToActivate.IsActivated = true;
            mfaToActivate.Activated = timeProvider.GetUtcNow().UtcDateTime;
            mfaToActivate.ActivatedBySessionId = currentContext.User.SessionId;
            if (identityOptions.Value.Mfa.GenerateRestoreCodes)
                mfaToActivate.RestoreCodes = Generator.GetRestoreCodes();

            await userStore.UpdateAsync(user);
            await mfaStore.UpdateAsync(mfaToActivate);

            await notifyer.EnabledMfaAsync(userId);

            return new MfaResponse
            {
                RestoreCodes = mfaToActivate.RestoreCodes,
            };
        }
    }
}
