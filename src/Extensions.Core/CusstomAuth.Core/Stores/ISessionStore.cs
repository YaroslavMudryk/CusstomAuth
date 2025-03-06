namespace CusstomAuth.Core.Stores;

public interface ISessionStore : IStore<IdentitySession>
{
    Task<List<IdentitySession>> GetActiveSessionsAsync(int userId);
    Task<List<IdentitySession>> GetUnactiveSessionsAsync(int userId);
    Task<List<IdentitySession>> GetSessionsAsync(int userId, string[] sessionIds);
    Task UpdateSessionsAsync(List<IdentitySession> sessions);
}
