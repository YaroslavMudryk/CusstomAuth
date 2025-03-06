namespace CusstomAuth.Core.SessionsManagement;

public class SessionModel
{
    public Guid Id { get; set; }
    public string Language { get; set; } = default!;
    public SessionType Type { get; set; }
    public SessionStatus Status { get; set; }
    public Guid DeviceId { get; set; }
    public DateTime? ExpiredAt { get; set; }
    public int UserId { get; set; }
    public Dictionary<string, List<string>> Permissions { get; set; } = default!;
}
