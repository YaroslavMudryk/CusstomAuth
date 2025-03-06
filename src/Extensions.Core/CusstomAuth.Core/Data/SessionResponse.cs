namespace CusstomAuth.Core.Data;

public class SessionResponse
{
    public string Id { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
    public bool Current { get; set; }
    public SessionStatus Status { get; set; }
    public string Language { get; set; } = default!;
    public AppModel App { get; set; } = default!;
    public string Location { get; set; } = default!;
    public DeviceInfo Device { get; set; } = default!;
}
