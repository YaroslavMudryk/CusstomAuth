using CusstomAuth.Core.Data;
using CusstomAuth.Core.ErrorHandling.Exceptions;
using CusstomAuth.Core.Notifyer;
using CusstomAuth.Core.SessionsManagement;
using CusstomAuth.Core.Stores;

namespace CusstomAuth.Core.Managers;

public interface ISessionManager
{
    Task<IReadOnlyList<SessionResponse>> GetUserSessionsAsync();
    Task<int> CloseSessionsByIdsAsync(string[] ids);
}

public class SessionManager(
    ICurrentContext currentContext,
    ISessionStore sessionStore,
    ISessionService sessionService,
    INotifyer notifyer,
    TimeProvider timeProvider) : ISessionManager
{
    public async Task<int> CloseSessionsByIdsAsync(string[] ids)
    {
        var utcNow = timeProvider.GetUtcNow().UtcDateTime;
        var userId = currentContext.User.Id;
        var currentSessionId = currentContext.User.SessionId;

        var sessionsToClose = await sessionStore.GetSessionsAsync(userId, ids);

        sessionsToClose.ForEach(session =>
        {
            if (session.Status == SessionStatus.Terminated)
                throw new BadRequestException("Session on device {session.Client.Device} already closed");

            session.Status = SessionStatus.Terminated;
            session.DeactivatedAt = utcNow;
            session.DeactivatedBySessionId = currentSessionId;
        });

        await sessionStore.UpdateSessionsAsync(sessionsToClose);

        await sessionService.RemoveSessionsAsync(sessionsToClose.Select(s => s.Id));

        await notifyer.SessionsClosedAsync(userId, ids);

        return sessionsToClose.Capacity;
    }

    public async Task<IReadOnlyList<SessionResponse>> GetUserSessionsAsync()
    {
        var userId = currentContext.User.Id;
        var currentSessionId = currentContext.User.SessionId;

        var sessions = await sessionStore.GetActiveSessionsAsync(userId);

        var sessionDtos = sessions.Select(session => new SessionResponse
        {
            Id = session.PublicId,
            CreatedAt = session.CreatedAt,
            Status = session.Status,
            Current = session.Id == currentSessionId,
            Language = session.Language,
            App = session.App,
            Location = $"{session.Location.Ip} - {session.Location.Country}, {session.Location.City}",
            Device = new DeviceInfo
            {
                Id = session.DeviceId.ToString(),
                Name = session.Client.IsBrowser() ? $"{session.Client.Browser} {session.Client.BrowserVersion}".Trim() : $"{session.Client.Device}".Trim(),
                Os = $"{session.Client.Os} {session.Client.OsVersion}".Trim(),
            }

        }).OrderByDescending(s => s.Current).ThenByDescending(s => s.CreatedAt).ToList();

        return sessionDtos;
    }
}
