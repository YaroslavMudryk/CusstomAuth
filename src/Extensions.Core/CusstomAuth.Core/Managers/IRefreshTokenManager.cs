using CusstomAuth.Core.Data;
using CusstomAuth.Core.ErrorHandling.Exceptions;
using CusstomAuth.Core.Notifyer;
using CusstomAuth.Core.SessionsManagement;
using CusstomAuth.Core.Stores;

namespace CusstomAuth.Core.Managers;

public interface IRefreshTokenManager
{
    Task<JwtTokenResponse> RefreshTokenAsync(string refreshToken);
}

public class RefreshTokenManager(
    IRefreshTokenStore refreshTokenStore,
    ISessionStore sessionStore,
    ITokenService tokenService,
    ISessionService sessionService,
    INotifyer notifyer,
    TimeProvider timeProvider
    ) : IRefreshTokenManager
{
    public async Task<JwtTokenResponse> RefreshTokenAsync(string token)
    {
        var utcNow = timeProvider.GetUtcNow().UtcDateTime;

        var lastRefreshToken = await refreshTokenStore.GetRefreshTokenAsync(token);
        if (lastRefreshToken == null)
            throw new BadRequestException("Invalid refresh token");

        var session = await sessionStore.GetAsync(lastRefreshToken.SessionId);
        if (session.IsFinished())
            throw new BadRequestException("Current session already closed");

        if (lastRefreshToken.TokenUsedAt != null)
            throw new BadRequestException("Current refresh token already activated");

        if (lastRefreshToken.ExpiredAt > utcNow)
            throw new BadRequestException("Refresh token has been expired");

        var newRefreshTokenItem = tokenService.GetRefreshToken(lastRefreshToken.SessionId);

        var accessToken = await tokenService.GenerateAccessTokenAsync(new UserTokenDto
        {
            UserId = lastRefreshToken.Session.UserId,
            SessionId = lastRefreshToken.Id,
        });

        lastRefreshToken.TokenUsedAt = utcNow;

        await refreshTokenStore.UpdateAsync(lastRefreshToken);
        await refreshTokenStore.CreateAsync(newRefreshTokenItem);

        await sessionService.AddOrUpdateSessionAsync(new SessionModel
        {
            Id = lastRefreshToken.Session.Id,
            DeviceId = lastRefreshToken.Session.DeviceId,
            ExpiredAt = lastRefreshToken.Session.ExpiredAt,
            Language = lastRefreshToken.Session.Language,
            Status = lastRefreshToken.Session.Status,
            Type = lastRefreshToken.Session.Type,
            UserId = lastRefreshToken.Session.UserId,
            Permissions = null!
        });

        await notifyer.SessionExtendedForUserAsync(session.UserId, session.Id);

        return new JwtTokenResponse
        {
            ExpiredAt = accessToken.ExpiredAt,
            RefreshToken = newRefreshTokenItem.Token,
            Token = accessToken.Token
        };
    }
}