using CusstomAuth.Core.Db;
using CusstomAuth.Core.Db.Entities;
using CusstomAuth.Core.Services.Auth.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;

namespace CusstomAuth.Core.Services.Auth.SessionsManager;

public class InMemorySessionManager : ISessionManager
{
    private readonly TimeProvider _dateTimeProvider;
    private readonly ConcurrentDictionary<Guid, SessionModel> _sessions;

    public InMemorySessionManager(IServiceScopeFactory serviceScopeFactory)
    {
        using var scope = serviceScopeFactory.CreateScope();
        _dateTimeProvider = scope.ServiceProvider.GetRequiredService<TimeProvider>();
        _sessions = new ConcurrentDictionary<Guid, SessionModel>(GetActualUserSessionsFromDb(scope.ServiceProvider.GetRequiredService<AuthDbContext>()));
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
            var utcNow = _dateTimeProvider.GetUtcNow().UtcDateTime;
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

    private static Dictionary<Guid, SessionModel> GetActualUserSessionsFromDb(AuthDbContext db)
    {
        db.Database.EnsureCreated();
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
