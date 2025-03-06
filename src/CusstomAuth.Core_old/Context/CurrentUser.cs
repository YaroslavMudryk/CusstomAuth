namespace CusstomAuth;

public class CurrentUser
{
    public int Id { get; set; }
    public string Login { get; set; }
    public string Language { get; set; }
    public Guid SessionId { get; set; }
    public string AuthenticationMethod { get; set; }
    public IEnumerable<string> Roles { get; set; }
}
