namespace CusstomAuth.Core.Data;

public class AppSignInDto
{
    public string Id { get; set; } = default!;
    public string Secret { get; set; } = default!;
    public string Version { get; set; } = default!;
}
