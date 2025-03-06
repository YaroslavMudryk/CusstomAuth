namespace CusstomAuth.Core.Options;

public class TokenOptions
{
    public static readonly string DefaultProvider = "Default";
    public static readonly string DefaultEmailProvider = "Email";
    public static readonly string DefaultPhoneProvider = "Phone";
    public static readonly string DefaultAuthenticatorProvider = "Authenticator";
    public string EmailConfirmationTokenProvider { get; set; } = DefaultProvider;
    public string PasswordResetTokenProvider { get; set; } = DefaultProvider;
    public string ChangeEmailTokenProvider { get; set; } = DefaultProvider;
    public string ChangePhoneNumberTokenProvider { get; set; } = DefaultPhoneProvider;
    public string AuthenticatorTokenProvider { get; set; } = DefaultAuthenticatorProvider;
    public string AuthenticatorIssuer { get; set; } = "caIdentity.api";


    public string Issuer { get; set; } = "CusstomAuth";
    public string Audience { get; set; } = "CusstomAuth Client";
    public string SecretKey { get; set; } = "0293fj2093fj3209fhg290gvj23rj032hf";
    public int LifetimeInMinutes { get; set; } = 60;
    public bool UseSessionManager { get; set; } = true;
    public SessionManagerOptions SessionManager { get; set; } = new SessionManagerOptions();
}
