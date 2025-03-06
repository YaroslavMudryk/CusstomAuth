using CusstomAuth.Core.Constants;
using CusstomAuth.Core.Data;
using CusstomAuth.Core.ErrorHandling.Exceptions;
using CusstomAuth.Core.Mappers;
using CusstomAuth.Core.Notifyer;
using CusstomAuth.Core.Options;
using CusstomAuth.Core.Passwords;
using CusstomAuth.Core.Services;
using CusstomAuth.Core.SessionsManagement;
using CusstomAuth.Core.Stores;
using Extensions.DeviceDetector;
using Microsoft.Extensions.Options;
using System.Text.RegularExpressions;

namespace CusstomAuth.Core.Managers;

public interface ISignInManager
{
    Task<JwtTokenResponse> SignInAsync(SignInRequest signInRequest);
}

public class SignInManager(
    ICurrentContext currentContext,
    IAppStore appStore,
    IUserStore userStore,
    ILoginAttemptStore loginAttemptStore,
    IBlockStore blockStore,
    ISessionStore sessionStore,
    IPasswordHasher passwordHasher,
    ITokenService tokenService,
    ILocationService locationService,
    IDeviceService deviceService,
    ISessionService sessionService,
    IDetector detector,
    INotifyer notifyer,
    TimeProvider timeProvider,
    IOptions<IdentityOptions> options) : ISignInManager
{
    public async Task<JwtTokenResponse> SignInAsync(SignInRequest signInRequest)
    {
        if (!Regex.IsMatch(signInRequest.Password, options.Value.Password.Regex))
            throw new PasswordRequirementsException(options.Value.Password.ErrorRegexMessages["en"]);

        var utcNow = timeProvider.GetUtcNow().UtcDateTime;

        var app = await appStore.GetAppByIdAndSecretAsync(signInRequest.App.Id, signInRequest.App.Secret);

        if (app == null)
            throw new NotFoundException();

        if (!app.IsActive || !app.IsActiveByTime(utcNow))
            throw new AppNotActivatedException();

        var location = await locationService.GetIpInfoAsync(currentContext.GetIp());

        var user = await userStore.GetUserByLoginAsync(signInRequest.Login);
        if (user == null)
        {
            var failedLoginAttempt = new IdentityLoginAttempt
            {
                Login = signInRequest.Login,
                Password = signInRequest.Password,
                Client = signInRequest.Device.MapToClient(),
                Location = location,
                IsSuccess = false
            };
            await loginAttemptStore.CreateAsync(failedLoginAttempt);
            throw new BadRequestException("Check your credentials");
        }

        if (options.Value.Endpoints[HttpActions.ConfirmAction].IsAvailable)
            if (!user.IsConfirmed)
                throw new BadRequestException("First approve your account");

        signInRequest.Device ??= detector.GetClientInfo().MapToDto();

        if (user.CanBeBlocked)
        {
            if (user.IsBlocked(utcNow))
            {
                throw new BadRequestException($"Your account has been blocked up to {user.BlockedUntil!.Value:HH:mm (dd.MM.yyyy)}");
            }

            if (user.FailedLoginAttempts == 5)
            {
                user.FailedLoginAttempts = 0;
                user.BlockedUntil = utcNow.AddHours(1);

                await notifyer.UserBlockedAsync(user.Id, user.BlockedUntil.Value);

                await blockStore.CreateAsync(new IdentityBlock
                {
                    Start = utcNow,
                    Finish = user.BlockedUntil.Value,
                    UserId = user.Id,
                    Cause = "Many failed login attempts"
                });
                await userStore.UpdateAsync(user);

                throw new BadRequestException($"Account locked up to {user.BlockedUntil.Value:HH:mm (dd.MM.yyyy)}");
            }
        }

        if (!passwordHasher.VerifyHashedPassword(user.PasswordHash,signInRequest.Password))
        {
            user.FailedLoginAttempts++;

            await loginAttemptStore.CreateAsync(new IdentityLoginAttempt
            {
                Login = signInRequest.Login,
                Password = signInRequest.Password,
                Client = signInRequest.Device.MapToClient(),
                Location = location,
                IsSuccess = false,
                UserId = user.Id
            });
            await userStore.UpdateAsync(user);

            await notifyer.TryToLoginIntoAccountAsync(user.Id);

            throw new BadRequestException("Password is incorrect");
        }

        var appInfo = AppModel.New(app, signInRequest.App.Version);
        var newSession = new IdentitySession
        {
            Id = Guid.CreateVersion7(),
            PublicId = Guid.NewGuid().ToString("N"),
            App = appInfo,
            Client = signInRequest.Device.MapToClient(),
            Location = location,
            UserId = user.Id,
            Type = SessionType.Password,
            ViaMFA = user.Mfa,
            Status = SessionStatus.Pending,
            Language = signInRequest.Lang
        };

        user.FailedLoginAttempts = 0;

        if (user.Mfa)
        {
            await userStore.UpdateAsync(user);
            await sessionStore.CreateAsync(newSession);

            await notifyer.LoginInPendingAsync(user.Id, newSession.Id);

            return new JwtTokenResponse
            {
                MfaHashKey = newSession.PublicId.ToString(),
            };
        }
        else
        {
            var refreshTokenItem = tokenService.GetRefreshToken(newSession.Id);

            var accessToken = await tokenService.GenerateAccessTokenAsync(new UserTokenDto
            {
                UserId = user.Id,
                SessionId = newSession.Id,
            });

            newSession.Status = SessionStatus.Active;
            newSession.RefreshTokens = [refreshTokenItem];
            var device = await deviceService.GetOrCreatedDeviceAsync(signInRequest.Device, newSession.Id);
            newSession.DeviceId = device.Id;

            await loginAttemptStore.CreateAsync(new IdentityLoginAttempt
            {
                Login = signInRequest.Login,
                Client = signInRequest.Device.MapToClient(),
                Location = location,
                IsSuccess = true,
                UserId = user.Id,
                SessionId = newSession.Id.ToString()
            });
            await userStore.UpdateAsync(user);
            await sessionStore.CreateAsync(newSession);

            await sessionService.AddOrUpdateSessionAsync(new SessionModel
            {
                Id = newSession.Id,
                UserId = newSession.UserId,
                ExpiredAt = newSession.ExpiredAt,
                DeviceId = newSession.DeviceId,
                Language = newSession.Language,
                Status = newSession.Status,
                Type = newSession.Type,
                Permissions = null!
            });

            await notifyer.LoginSuccessfullyAsync(user.Id, newSession.Id);

            return new JwtTokenResponse
            {
                ExpiredAt = accessToken.ExpiredAt,
                RefreshToken = refreshTokenItem.Token,
                Token = accessToken.Token,
            };
        }
    }
}
