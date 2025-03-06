using CusstomAuth.Core.Constants;

namespace CusstomAuth.Core.Options;

public class CusstomAuthOptions
{
    public string Name { get; set; } = "CusstomAuth";
    public DbConnectionOptions DbConnection { get; set; } = new DbConnectionOptions();
    public PasswordOptions Password { get; set; } = new PasswordOptions();
    public TokenOptions Token { get; set; } = new TokenOptions();
    public RefreshTokenOptions RefreshToken { get; set; } = new RefreshTokenOptions();
    public ServicesOptions Services { get; set; } = new ServicesOptions();
    public Dictionary<string, EndpointOptions> Endpoints { get; set; } = HttpEndpoints.Default;
    public Dictionary<string, CodeOptions> Codes { get; set; } = CodeConfigs.Defaults;
}
