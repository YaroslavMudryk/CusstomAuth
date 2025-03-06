using CusstomAuth.Core.SessionsManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;

namespace CusstomAuth.EntityFrameworkCore;

public class EfSessionService : ISessionService
{
    private readonly TimeProvider _timeProvider;
    private readonly ConcurrentDictionary<Guid, SessionModel> _sessions;

    public EfSessionService(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        _timeProvider = scope.ServiceProvider.GetRequiredService<TimeProvider>();
        _sessions = new ConcurrentDictionary<Guid, SessionModel>(GetActualSessionsFromDb(scope.ServiceProvider.GetRequiredService<IdentityDbContext>()));
    }

    public async Task AddOrUpdateSessionAsync(SessionModel newSession)
    {
        if (_sessions.TryGetValue(newSession.Id, out var session))
        {
            session.Permissions = newSession.Permissions;
            session.DeviceId = newSession.DeviceId;
            session.Status = newSession.Status;
            session.Type = newSession.Type;
            session.Language = newSession.Language;
        }
        else
        {
            _sessions.TryAdd(newSession.Id, newSession);
        }

        await Task.CompletedTask;
    }

    public async Task<bool> IsActiveSessionAsync(Guid sessionId)
    {
        bool isActive = false;

        if (_sessions.TryGetValue(sessionId, out var sessionToCheck))
        {
            var utcNow = _timeProvider.GetUtcNow().UtcDateTime;
            if (sessionToCheck.ExpiredAt.HasValue)
            {
                if (sessionToCheck.ExpiredAt.Value < utcNow)
                    isActive = true;
            }
            else
                isActive = true;

        }

        return await Task.FromResult(isActive);
    }

    public async Task RemoveSessionAsync(Guid sessionId)
    {
        if (_sessions.TryGetValue(sessionId, out var sessionToRemove))
        {
            _sessions.Remove(sessionId, out sessionToRemove);
        }

        await Task.CompletedTask;
    }

    public async Task RemoveSessionsAsync(IEnumerable<Guid> sessionIds)
    {
        foreach (var sessionId in sessionIds)
        {
            await RemoveSessionAsync(sessionId);
        }
    }

    private Dictionary<Guid, SessionModel> GetActualSessionsFromDb(IdentityDbContext db)
    {
        if (db.Database.CanConnect())
            return [];

        var activeSessions = db.Sessions.AsNoTracking().Where(s => s.Status == SessionStatus.Active || s.Status == SessionStatus.Pending).ToList();

        return activeSessions.ToDictionary(s => s.Id, activeSession => new SessionModel
        {
            Id = activeSession.Id,
            DeviceId = activeSession.DeviceId,
            UserId = activeSession.UserId,
            Language = activeSession.Language,
            Status = activeSession.Status,
            Type = activeSession.Type,
            ExpiredAt = activeSession.ExpiredAt
        });
    }
}
