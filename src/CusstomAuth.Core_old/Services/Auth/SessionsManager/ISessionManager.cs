using CusstomAuth.Core.Services.Auth.Dtos;

namespace CusstomAuth.Core.Services.Auth.SessionsManager;

public interface ISessionManager
{
    Task AddOrUpdateSessionAsync(SessionModel session);
    Task<bool> IsActiveSessionAsync(Guid sessionId);
    Task RemoveSessionAsync(Guid sessionId);
    Task RemoveSessionsAsync(IEnumerable<Guid> sessionIds);
}
