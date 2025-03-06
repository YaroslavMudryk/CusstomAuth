namespace CusstomAuth.Core.Stores;

public interface IMfaStore : IStore<IdentityMfa>
{
    Task<IdentityMfa> GetUserActiveMfaAsync(int userId);
    Task<IdentityMfa> GetUserUnactiveMfaAsync(int userId);
}
