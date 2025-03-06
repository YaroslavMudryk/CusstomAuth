using CusstomAuth.Core.Options;
using Microsoft.Extensions.Options;

namespace CusstomAuth.Core.SessionsManagement;

public interface ITokenService
{
    IdentityRefreshToken GetRefreshToken(Guid sessionId);
    Task<TokenDto> GenerateAccessTokenAsync(UserTokenDto userToken);
}

public class TokenService(
    TimeProvider timeProvider,
    IOptions<IdentityOptions> options) : ITokenService
{
    public async Task<TokenDto> GenerateAccessTokenAsync(UserTokenDto userToken)
    {
        throw new NotImplementedException();
    }

    public IdentityRefreshToken GetRefreshToken(Guid sessionId)
    {
        return new IdentityRefreshToken
        {
            ExpiredAt = timeProvider.GetUtcNow().UtcDateTime.AddMinutes(options.Value.RefreshToken.LifetimeInMinutes),
            SessionId = sessionId,
            Token = Guid.CreateVersion7().ToString(),
            TokenUsedAt = null
        };
    }
}
