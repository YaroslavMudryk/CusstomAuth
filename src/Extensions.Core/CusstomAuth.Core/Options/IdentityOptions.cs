using CusstomAuth.Core.Constants;

namespace CusstomAuth.Core.Options;

public class IdentityOptions
{
    public ClaimsIdentityOptions ClaimsIdentity { get; set; } = new ClaimsIdentityOptions();
    public UserOptions User { get; set; } = new UserOptions();
    public PasswordOptions Password { get; set; } = new PasswordOptions();
    public LockoutOptions Lockout { get; set; } = new LockoutOptions();
    public SignInOptions SignIn { get; set; } = new SignInOptions();
    public TokenOptions Token { get; set; } = new TokenOptions();
    public Dictionary<string, EndpointOptions> Endpoints { get; set; } = HttpEndpoints.Default;
    public Dictionary<string, CodeOptions> Codes { get; set; } = CodeConfigs.Defaults;
    public RefreshTokenOptions RefreshToken { get; set; } = new RefreshTokenOptions();
    public MfaOptions Mfa { get; set; } = new MfaOptions();
}
