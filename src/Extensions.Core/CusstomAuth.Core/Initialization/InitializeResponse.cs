namespace CusstomAuth.Core.Initialization;

public class InitializeResponse
{
    public UserInitDto User { get; set; } = default!;
    public IReadOnlyList<AppInitDto> Apps { get; set; } = default!;
}
