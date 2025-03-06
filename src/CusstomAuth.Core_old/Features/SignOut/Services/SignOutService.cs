using CusstomAuth.Core.Db;
using CusstomAuth.Core.Db.Entities;
using CusstomAuth.Core.ErrorHandling.Exceptions;
using CusstomAuth.Core.Services.Auth.SessionsManager;
using Microsoft.EntityFrameworkCore;

namespace CusstomAuth.Core.Features.SignOut.Services;

public interface ISignOutService
{
    Task<bool> LogoutAsync();
}

public class SignOutService(
    AuthDbContext db,
    ISessionManager sessionManager,
    ICurrentContext currentContext,
    TimeProvider dateTimeProvider) : ISignOutService
{
    public async Task<bool> LogoutAsync()
    {
        await using var transaction = await db.BeginTransactionAsync();

        var currentSessionId = currentContext.User.SessionId;

        if (!await sessionManager.IsActiveSessionAsync(currentSessionId))
            throw new BadRequestException("Session is already expired or closed");

        var sessionToClose = await db.Sessions.AsNoTracking().FirstOrDefaultAsync(s => s.Id == currentSessionId);
        if (sessionToClose == null)
            throw new NotFoundException("Session not found");

        var utcNow = dateTimeProvider.GetUtcNow().UtcDateTime;

        sessionToClose.Status = SessionStatus.Completed;
        sessionToClose.DeactivatedAt = utcNow;
        sessionToClose.DeactivatedBySessionId = currentSessionId;

        db.Sessions.Update(sessionToClose);
        await db.SaveChangesAsync();

        await transaction.CommitAsync();

        await sessionManager.RemoveSessionAsync(currentSessionId);

        return true;
    }
}
