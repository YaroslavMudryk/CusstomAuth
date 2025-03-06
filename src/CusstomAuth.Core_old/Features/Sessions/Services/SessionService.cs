using CusstomAuth.Core.Db;
using CusstomAuth.Core.Db.Entities;
using CusstomAuth.Core.ErrorHandling.Exceptions;
using CusstomAuth.Core.Features.Sessions.Dtos;
using CusstomAuth.Core.Features.Sessions.Mappings;
using CusstomAuth.Core.Services.Auth.SessionsManager;
using Microsoft.EntityFrameworkCore;

namespace CusstomAuth.Core.Features.Sessions.Services;

public interface ISessionService
{
    Task<List<SessionDto>> GetUserSessionsAsync();
    Task<int> CloseSessionsByIdsAsync(string[] ids);
}

public class SessionService(
    AuthDbContext db,
    ISessionManager sessionManager,
    TimeProvider dateTimeProvider,
    ICurrentContext currentContext) : ISessionService
{
    public async Task<int> CloseSessionsByIdsAsync(string[] ids)
    {
        await using var transaction = await db.BeginTransactionAsync();

        var utcNow = dateTimeProvider.GetUtcNow().UtcDateTime;
        var userId = currentContext.User.Id;
        var currentSessionId = currentContext.User.SessionId;

        var sessionsToClose = await db.Sessions.AsNoTracking().Where(s => ids.Contains(s.PublicId) && s.UserId == userId).ToListAsync();

        sessionsToClose.ForEach(session =>
        {
            if (session.Status == SessionStatus.Terminated)
                throw new BadRequestException($"Session on device {session.Client.Device} already closed");

            session.Status = SessionStatus.Terminated;
            session.DeactivatedAt = utcNow;
            session.DeactivatedBySessionId = currentSessionId;
        });

        db.Sessions.UpdateRange(sessionsToClose);
        var affectedSessions = await db.SaveChangesAsync();

        await transaction.CommitAsync();

        await sessionManager.RemoveSessionsAsync(sessionsToClose.Select(s => s.Id));

        return affectedSessions;
    }

    public async Task<List<SessionDto>> GetUserSessionsAsync()
    {
        var userId = currentContext.User.Id;
        var sessionId = currentContext.User.SessionId;

        var query = db.Sessions.AsQueryable();

        query = query.Where(s => s.UserId == userId);
        query = query.Where(s => s.Status == SessionStatus.Active || s.Status == SessionStatus.Pending);
        query = query.OrderBy(s => s.Status).OrderByDescending(s => s.CreatedAt);
        var sessions = await query.ToListAsync();

        var sessionDtos = sessions.MapToDto(sessionId).OrderByDescending(s => s.Current).ThenByDescending(s => s.CreatedAt).ToList();

        return sessionDtos;
    }
}
