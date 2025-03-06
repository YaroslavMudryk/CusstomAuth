using CusstomAuth.Core.Constants;
using CusstomAuth.Core.Db;
using CusstomAuth.Core.Db.Entities;
using CusstomAuth.Core.Db.Entities.Internal;
using CusstomAuth.Core.ErrorHandling.Exceptions;
using CusstomAuth.Core.Features.SignIn.Dtos;
using CusstomAuth.Core.Mappers;
using CusstomAuth.Core.Options;
using CusstomAuth.Core.Services.Auth.Dtos;
using CusstomAuth.Core.Services.Auth.SessionsManager;
using CusstomAuth.Core.Services.Location;
using CusstomAuth.Core.Services.Notifications;
using CusstomAuth.Core.Services.Notifications.Dtos;
using Extensions.DeviceDetector;
using Extensions.Password;
using Google.Authenticator;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Options;
using System.Text.RegularExpressions;

namespace CusstomAuth.Core.Features.SignIn.Services;

public interface ISignInService
{
    Task<JwtTokenDto> SignInByPasswordAsync(SignInDto signInDto);
    Task<JwtTokenDto> SignInByMfaAsync(SignInMfaDto signInMfa);
}

public class SignInService(
    AuthDbContext db,
    TimeProvider dateTimeProvider,
    ISessionManager sessionManager,
    IDetector detector,
    ILocationService locationService,
    ITokenService tokenService,
    ICurrentContext currentContext,
    INotificationService notificationService,
    IDeviceService deviceService,
    IOptions<CusstomAuthOptions> options) : ISignInService
{
    public async Task<JwtTokenDto> SignInByMfaAsync(SignInMfaDto signInMfa)
    {
        await using var transaction = await db.BeginTransactionAsync();

        var session = await GetSessionAsync(signInMfa);

        VerifyUserMfa(signInMfa, session);

        var refreshTokenItem = tokenService.GetRefreshToken(session.Id);

        var accessToken = await tokenService.GenerateAccessTokenAsync(UserTokenDto.New(session, AuthType.Password));

        session.RefreshTokens = [refreshTokenItem];

        await db.LoginAttempts.AddAsync(AuthLoginAttempt.New(session, true));

        session.Status = SessionStatus.Active;

        db.Sessions.Update(session);

        await db.SaveChangesAsync();

        await transaction.CommitAsync();

        await sessionManager.AddOrUpdateSessionAsync(new SessionModel
        {
            Id = session.Id,
            UserId = session.UserId,
            ExpiredAt = session.ExpiredAt,
            DeviceId = session.DeviceId,
            Language = session.Language,
            Status = session.Status,
            Type = session.Type,
            //Permissions = session
        });

        return new JwtTokenDto
        {
            ExpiredAt = dateTimeProvider.GetUtcNow().UtcDateTime.AddMinutes(options.Value.Token.LifetimeInMinutes),
            RefreshToken = refreshTokenItem.Token,
            Token = accessToken
        };
    }

    public async Task<JwtTokenDto> SignInByPasswordAsync(SignInDto signInDto)
    {
        if (!Regex.IsMatch(signInDto.Password, options.Value.Password.Regex))
            throw new PasswordRequirementsException(options.Value.Password.ErrorRegexMessages["en"]);

        var utcNow = dateTimeProvider.GetUtcNow().UtcDateTime;

        await using var transaction = await db.BeginTransactionAsync();

        var app = await GetAppAsync(signInDto, utcNow);

        var location = await locationService.GetIpInfoAsync(currentContext.GetIp());

        var user = await GetUserOrThrowAsync(signInDto, location);

        if (options.Value.Endpoints[HttpActions.ConfirmAction].IsAvailable)
            if (!user.IsConfirmed)
                throw new BadRequestException("First approve your account");

        signInDto.Device ??= detector.GetClientInfo().MapToDto();

        await VerifyBlockUserAsync(utcNow, user);
        await VerifyPasswordUserAsync(signInDto, location, user);

        var appInfo = AppModel.New(app, signInDto.App.Version);
        var newSession = AuthSession.New(appInfo, signInDto, location, user);

        user.FailedLoginAttempts = 0;

        if (user.Mfa)
        {
            return await GenerateUserMfaAccessAsync(transaction, user, newSession);
        }
        else
        {
            return await GenerateUserAccessAsync(signInDto, transaction, location, user, newSession);
        }
    }

    private static void VerifyUserMfa(SignInMfaDto signInMfa, AuthSession session)
    {
        var secretKey = session.User.MfaSecretKey;
        var twoFactor = new TwoFactorAuthenticator();

        var currentPin = twoFactor.GetCurrentPIN(secretKey);

        if (!currentPin.Equals(signInMfa.Code))
            throw new BadRequestException("Code is incorrect");
    }

    private async Task<AuthSession> GetSessionAsync(SignInMfaDto signInMfa)
    {
        var session = await db.Sessions.AsNoTracking().Include(s => s.User).FirstOrDefaultAsync(s => s.PublicId == signInMfa.MfaHashKey);

        if (session == null)
            throw new NotFoundException($"Session with ID: {signInMfa.MfaHashKey} not found");

        if (session.Status != SessionStatus.Pending)
            throw new BadRequestException("Session already activated or closed");
        return session;
    }

    private async Task<JwtTokenDto> GenerateUserAccessAsync(SignInDto signInDto, IDbContextTransaction transaction, LocationModel location, AuthUser user, AuthSession newSession)
    {
        var refreshTokenItem = tokenService.GetRefreshToken(newSession.Id);

        var accessToken = await tokenService.GenerateAccessTokenAsync(new UserTokenDto
        {
            AuthType = AuthType.Password,
            UserId = user.Id,
            User = user,
            Lang = signInDto.Lang,
            SessionId = newSession.Id,
            Session = newSession
        });

        newSession.Status = SessionStatus.Active;
        newSession.RefreshTokens = [refreshTokenItem];
        var device = await deviceService.GetOrCreatedDeviceAsync(signInDto.Device, newSession.Id);
        newSession.DeviceId = device.Id;

        await db.LoginAttempts.AddAsync(new AuthLoginAttempt
        {
            Login = signInDto.Login,
            Client = signInDto.Device.MapToClient(),
            Location = location,
            IsSuccess = true,
            UserId = user.Id,
            SessionId = newSession.Id.ToString()
        });

        db.Users.Update(user);
        await db.Sessions.AddAsync(newSession);
        await db.SaveChangesAsync();

        await transaction.CommitAsync();

        await sessionManager.AddOrUpdateSessionAsync(new SessionModel
        {
            Id = newSession.Id,
            UserId = newSession.UserId,
            ExpiredAt = newSession.ExpiredAt,
            DeviceId = newSession.DeviceId,
            Language = newSession.Language,
            Status = newSession.Status,
            Type = newSession.Type,
            //Permissions = session
        });

        return new JwtTokenDto
        {
            ExpiredAt = dateTimeProvider.GetUtcNow().UtcDateTime.AddMinutes(options.Value.Token.LifetimeInMinutes),
            RefreshToken = refreshTokenItem.Token,
            Token = accessToken,
        };
    }

    private async Task<JwtTokenDto> GenerateUserMfaAccessAsync(IDbContextTransaction transaction, AuthUser user, AuthSession newSession)
    {
        db.Users.Update(user);
        await db.Sessions.AddAsync(newSession);
        await db.SaveChangesAsync();

        await transaction.CommitAsync();

        return new JwtTokenDto
        {
            MfaHashKey = newSession.PublicId.ToString(),
        };
    }

    private async Task VerifyPasswordUserAsync(SignInDto signInDto, LocationModel location, AuthUser user)
    {
        if (!signInDto.Password.VerifyPasswordHash(user.PasswordHash))
        {
            user.FailedLoginAttempts++;

            await notificationService.SendNotificationAsync(new NotificationDto
            {
                Title = "Attempt login",
                Body = "Someone just tried to access your account"
            });

            await db.LoginAttempts.AddAsync(new AuthLoginAttempt
            {
                Login = signInDto.Login,
                Password = signInDto.Password,
                Client = signInDto.Device.MapToClient(),
                Location = location,
                IsSuccess = false,
                UserId = user.Id
            });

            db.Users.Update(user);
            await db.SaveChangesAsync();

            throw new BadRequestException("Password is incorrect");
        }
    }

    private async Task VerifyBlockUserAsync(DateTime utcNow, AuthUser user)
    {
        if (user.CanBeBlocked)
        {
            if (user.IsBlocked(utcNow))
            {
                throw new BadRequestException($"Your account has been blocked up to {user.BlockedUntil.Value:HH:mm (dd.MM.yyyy)}");
            }

            if (user.FailedLoginAttempts == 5)
            {
                user.FailedLoginAttempts = 0;
                user.BlockedUntil = utcNow.AddHours(1);

                await notificationService.SendNotificationAsync(new NotificationDto
                {
                    Title = "Blocking account",
                    Body = $"Your account has been blocked until {user.BlockedUntil.Value:HH:mm (dd.MM.yyyy)}"
                });
                db.Blocks.Add(new AuthBlock
                {
                    Start = utcNow,
                    Finish = user.BlockedUntil.Value,
                    UserId = user.Id,
                    Cause = "Many failed login attempts"
                });
                db.Users.Update(user);
                await db.SaveChangesAsync();

                throw new BadRequestException($"Account locked up to {user.BlockedUntil.Value:HH:mm (dd.MM.yyyy)}");
            }
        }
    }

    private async Task<AuthUser> GetUserOrThrowAsync(SignInDto signInDto, LocationModel location)
    {
        var user = await db.Users.AsNoTracking().FirstOrDefaultAsync(s => s.Login == signInDto.Login);
        if (user == null)
        {
            await db.LoginAttempts.AddAsync(new AuthLoginAttempt
            {
                Login = signInDto.Login,
                Password = signInDto.Password,
                Client = signInDto.Device.MapToClient(),
                Location = location,
                IsSuccess = false
            });
            await db.SaveChangesAsync();
            throw new BadRequestException("Check your credentials");
        }

        return user;
    }

    private async Task<AuthApp> GetAppAsync(SignInDto signInDto, DateTime utcNow)
    {
        var app = await db.Apps.AsNoTracking().FirstOrDefaultAsync(app => app.ClientId == signInDto.App.Id && app.ClientSecret == signInDto.App.Secret);

        if (app == null)
            throw new NotFoundException();

        if (!app.IsActive || !app.IsActiveByTime(utcNow))
            throw new AppNotActivatedException();
        return app;
    }
}
