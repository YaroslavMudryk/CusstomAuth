namespace CusstomAuth.Core.Initialization;

public class UserInitDto
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Login { get; set; } = default!;
    public string Password { get; set; } = default!;
    public string AccessToken { get; set; } = default!;
    public string RefreshToken { get; set; } = default!;
}
