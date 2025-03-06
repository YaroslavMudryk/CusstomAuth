namespace CusstomAuth.Core.Stores;

public interface IRefreshTokenStore : IStore<IdentityRefreshToken>
{
    Task<IdentityRefreshToken> GetRefreshTokenAsync(string token);
}
