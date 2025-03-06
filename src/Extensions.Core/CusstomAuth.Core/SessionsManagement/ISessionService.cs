namespace CusstomAuth.Core.SessionsManagement;

public interface ISessionService
{
    Task AddOrUpdateSessionAsync(SessionModel session);
    Task<bool> IsActiveSessionAsync(Guid sessionId);
    Task RemoveSessionAsync(Guid sessionId);
    Task RemoveSessionsAsync(IEnumerable<Guid> sessionIds);
}
