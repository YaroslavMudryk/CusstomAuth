namespace CusstomAuth.Core.Options;

public class TokenOptions
{
    public bool UseSessionManager { get; set; } = true;
    public string Issuer { get; set; } = "CusstomAuth";
    public string Audience { get; set; } = "CusstomAuth Client";
    public string SecretKey { get; set; } = "0293fj2093fj3209fhg290gvj23rj032hf";
    public int LifetimeInMinutes { get; set; } = 60;
    public SessionManagerOptions SessionManager { get; set; } = new SessionManagerOptions();
}
