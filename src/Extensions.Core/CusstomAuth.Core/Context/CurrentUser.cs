namespace CusstomAuth;

public class CurrentUser
{
    public int Id { get; set; }
    public string Login { get; set; } = default!;
    public string Language { get; set; } = default!;
    public Guid SessionId { get; set; }
    public string AuthenticationMethod { get; set; } = default!;
    public IEnumerable<string> Roles { get; set; } = default!;
}
