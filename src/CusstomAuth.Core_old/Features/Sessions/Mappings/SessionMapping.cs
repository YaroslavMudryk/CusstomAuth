using CusstomAuth.Core.Db.Entities;
using CusstomAuth.Core.Features.Sessions.Dtos;

namespace CusstomAuth.Core.Features.Sessions.Mappings;

public static class SessionMapping
{
    public static IEnumerable<SessionDto> MapToDto(this IEnumerable<AuthSession> sessions, Guid? currentSessionId = null)
    {
        return sessions.Select(session => MapToDto(session, currentSessionId));
    }

    public static SessionDto MapToDto(this AuthSession session, Guid? currentSessionId = null)
    {
        if (session == null)
            return null;

        return new SessionDto
        {
            Id = session.PublicId,
            CreatedAt = session.CreatedAt,
            Status = session.Status,
            Current = currentSessionId.HasValue && session.Id == currentSessionId,
            Language = session.Language,
            App = session.App,
            Location = $"{session.Location.Ip} - {session.Location.Country}, {session.Location.City}",
            Device = new DeviceDto
            {
                Id = session.DeviceId.ToString(),
                Name = session.Client.IsBrowser() ? $"{session.Client.Browser} {session.Client.BrowserVersion}".Trim() : $"{session.Client.Device}".Trim(),
                Os = $"{session.Client.Os} {session.Client.OsVersion}".Trim(),
            }
        };
    }

    public static IEnumerable<Guid> MapSessionIds(this IEnumerable<AuthSession> sessions)
    {
        return sessions.Select(s => s.Id);
    }
}
