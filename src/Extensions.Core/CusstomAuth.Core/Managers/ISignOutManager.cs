using CusstomAuth.Core.ErrorHandling.Exceptions;
using CusstomAuth.Core.Notifyer;
using CusstomAuth.Core.SessionsManagement;
using CusstomAuth.Core.Stores;

namespace CusstomAuth.Core.Managers;

public interface ISignOutManager
{
    Task SignOutAsync();
}

public class SignOutManager(
    ICurrentContext currentContext,
    ISessionStore sessionStore,
    ISessionService sessionService,
    INotifyer notifyer,
    TimeProvider timeProvider
    ) : ISignOutManager
{
    public async Task SignOutAsync()
    {
        var currentSessionId = currentContext.User.SessionId;

        if (!await sessionService.IsActiveSessionAsync(currentSessionId))
            throw new BadRequestException("Session is already expired or closed");

        var sessionToClose = await sessionStore.GetAsync(currentSessionId);
        if (sessionToClose == null)
            throw new NotFoundException("Session not found");

        sessionToClose.Status = SessionStatus.Completed;
        sessionToClose.DeactivatedAt = timeProvider.GetUtcNow().UtcDateTime;
        sessionToClose.DeactivatedBySessionId = currentSessionId;

        await sessionStore.UpdateAsync(sessionToClose);

        await sessionService.RemoveSessionAsync(currentSessionId);

        await notifyer.LogoutAsync(sessionToClose.UserId, sessionToClose.Id);
    }
}
