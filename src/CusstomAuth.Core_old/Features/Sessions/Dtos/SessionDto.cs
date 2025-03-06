using CusstomAuth.Core.Db.Entities;
using CusstomAuth.Core.Db.Entities.Internal;

namespace CusstomAuth.Core.Features.Sessions.Dtos;

public class SessionDto
{
    public string Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool Current { get; set; }
    public SessionStatus Status { get; set; }
    public string Language { get; set; }
    public AppModel App { get; set; }
    public string Location { get; set; }
    public DeviceDto Device { get; set; }
}
