using CusstomAuth.Core.Db;
using CusstomAuth.Core.Db.Entities;
using CusstomAuth.Core.ErrorHandling.Exceptions;
using CusstomAuth.Core.Features.Mfa.Dtos;
using CusstomAuth.Core.Helpers;
using CusstomAuth.Core.Options;
using Google.Authenticator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace CusstomAuth.Core.Features.Mfa.Services;

public interface IMfaService
{
    Task<bool> DisableMfaAsync(string code);
    Task<MfaDto> EnableMfaAsync(string code = null);
}

public class MfaService(
    AuthDbContext db,
    ICurrentContext currentContext,
    TimeProvider dateTimeProvider,
    IOptions<CusstomAuthOptions> cusstomAuthOptions) : IMfaService
{
    public async Task<bool> DisableMfaAsync(string code)
    {
        await using var transaction = await db.BeginTransactionAsync();

        var userId = currentContext.User.Id;

        var userForDisableMfa = await db.Users.AsNoTracking().FirstOrDefaultAsync(s => s.Id == userId);

        if (userForDisableMfa == null)
            throw new NotFoundException("User not found");

        if (!userForDisableMfa.Mfa)
            throw new BadRequestException("MFA already diactivated");

        var twoFactor = new TwoFactorAuthenticator();

        var currentPin = twoFactor.GetCurrentPIN(userForDisableMfa.MfaSecretKey);

        if (!currentPin.Equals(code))
            throw new BadRequestException("Code is incorrect");

        userForDisableMfa.Mfa = false;
        userForDisableMfa.MfaSecretKey = null;

        var activeMfa = await db.Mfas.FirstOrDefaultAsync(s => s.UserId == userId && s.IsActivated);
        if (activeMfa == null)
            throw new BadRequestException("Some error, please contact support");

        activeMfa.DiactivedAt = dateTimeProvider.GetUtcNow().UtcDateTime;
        activeMfa.DiactivedBySessionId = currentContext.User.SessionId;

        db.Users.Update(userForDisableMfa);
        db.Mfas.UpdateRange(activeMfa);
        await db.SaveChangesAsync();

        await transaction.CommitAsync();

        return true;
    }

    public async Task<MfaDto> EnableMfaAsync(string code = null)
    {
        await using var transaction = await db.BeginTransactionAsync();

        var userId = currentContext.User.Id;

        var user = await db.Users.AsNoTracking().FirstOrDefaultAsync(s => s.Id == userId);

        if (user == null)
            throw new NotFoundException("User not found");

        if (string.IsNullOrEmpty(code))
        {
            var existMFA = await db.Mfas.FirstOrDefaultAsync(s => s.UserId == userId && !s.IsActivated);
            if (existMFA == null)
            {
                var secretKey = Guid.NewGuid().ToString("N");
                var twoFactor = new TwoFactorAuthenticator();
                var setupInfo = twoFactor.GenerateSetupCode(cusstomAuthOptions.Value.Name, user.Login, secretKey, false, 3);

                user.MfaSecretKey = secretKey;
                user.Mfa = false;

                var newMfa = new AuthMfa
                {
                    UserId = userId,
                    EntryCode = setupInfo.ManualEntryKey,
                    QrCodeBase64 = setupInfo.QrCodeSetupImageUrl,
                    Secret = secretKey,
                    IsActivated = false,
                    Activated = null,
                    ActivatedBySessionId = null
                };

                db.Users.Update(user);
                await db.Mfas.AddAsync(newMfa);
                await db.SaveChangesAsync();

                await transaction.CommitAsync();

                return new MfaDto
                {
                    QrCodeImage = setupInfo.QrCodeSetupImageUrl,
                    ManualEntryKey = setupInfo.ManualEntryKey,
                };
            }
            else
            {
                return new MfaDto
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

            var mfaToActivate = await db.Mfas.AsNoTracking().FirstOrDefaultAsync(s => s.UserId == userId && !s.IsActivated);

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
            mfaToActivate.Activated = dateTimeProvider.GetUtcNow().UtcDateTime;
            mfaToActivate.ActivatedBySessionId = currentContext.User.SessionId;
            mfaToActivate.RestoreCodes = Generator.GetRestoreCodes();

            db.Users.Update(user);
            db.Mfas.Update(mfaToActivate);
            await db.SaveChangesAsync();

            await transaction.CommitAsync();

            return new MfaDto
            {
                RestoreCodes = mfaToActivate.RestoreCodes,
            };
        }
    }
}
