namespace CusstomAuth.Core.Options;

public class SignInOptions
{
    public bool RequireConfirmedEmail { get; set; }
    public bool RequireConfirmedPhoneNumber { get; set; }
    public bool RequireConfirmedAccount { get; set; }
}
