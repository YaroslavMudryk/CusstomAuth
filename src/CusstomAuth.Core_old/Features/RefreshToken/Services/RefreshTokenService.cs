using CusstomAuth.Core.Constants;
using CusstomAuth.Core.Db;
using CusstomAuth.Core.ErrorHandling.Exceptions;
using CusstomAuth.Core.Features.SignIn.Dtos;
using CusstomAuth.Core.Features.SignIn.Services;
using CusstomAuth.Core.Options;
using CusstomAuth.Core.Services.Auth.Dtos;
using CusstomAuth.Core.Services.Auth.SessionsManager;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace CusstomAuth.Core.Features.RefreshToken.Services;

public interface IRefreshTokenService
{
    Task<JwtTokenDto> RefreshTokenAsync(string refreshToken);
}

public class RefreshTokenService(
    AuthDbContext db,
    ISessionManager sessionManager,
    TimeProvider timeProvider,
    ITokenService tokenService,
    IOptions<CusstomAuthOptions> options) : IRefreshTokenService
{
    public async Task<JwtTokenDto> RefreshTokenAsync(string refreshToken)
    {
        await using var transaction = await db.BeginTransactionAsync();
        var utcNow = timeProvider.GetUtcNow().UtcDateTime;

        var expiredToken = await db.RefreshTokens.AsNoTracking().Include(s => s.Session).ThenInclude(s => s.User).FirstOrDefaultAsync(s => s.Token == refreshToken);
        if (expiredToken == null)
            throw new BadRequestException("Invalid refresh token");

        if (expiredToken.Session.IsFinished())
            throw new BadRequestException("Current session already closed");

        if (expiredToken.TokenUsedAt != null)
            throw new BadRequestException("Current refresh token already activated");

        if (expiredToken.ExpiredAt > utcNow)
            throw new BadRequestException("Refresh token has been expired");

        var refreshTokenItem = tokenService.GetRefreshToken(expiredToken.SessionId);

        var accessToken = await tokenService.GenerateAccessTokenAsync(new UserTokenDto
        {
            AuthType = AuthType.Password,
            UserId = expiredToken.Session.UserId,
            User = expiredToken.Session.User,
            Lang = expiredToken.Session.Language,
            SessionId = expiredToken.Id,
            Session = expiredToken.Session
        });
        refreshTokenItem.Session = null;

        expiredToken.TokenUsedAt = utcNow;

        db.RefreshTokens.Update(expiredToken);
        await db.RefreshTokens.AddAsync(refreshTokenItem);
        await db.SaveChangesAsync();

        await transaction.CommitAsync();

        await sessionManager.AddOrUpdateSessionAsync(new SessionModel
        {
            Id = expiredToken.Session.Id,
            DeviceId = expiredToken.Session.DeviceId,
            ExpiredAt = expiredToken.Session.ExpiredAt,
            Language = expiredToken.Session.Language,
            Status = expiredToken.Session.Status,
            Type = expiredToken.Session.Type,
            UserId = expiredToken.Session.UserId,
            //Permissions = null
        });

        return new JwtTokenDto
        {
            ExpiredAt = timeProvider.GetUtcNow().UtcDateTime.AddMinutes(options.Value.Token.LifetimeInMinutes),
            RefreshToken = refreshTokenItem.Token,
            Token = accessToken
        };
    }
}
