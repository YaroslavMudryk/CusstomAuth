namespace CusstomAuth.Core.Data;

public class SignInRequest
{
    public string Login { get; set; } = default!;
    public string Password { get; set; } = default!;
    public string Lang { get; set; } = default!;
    public DeviceInfo Device { get; set; } = default!;
    public AppSignInDto App { get; set; } = default!;

    public string Code { get; set; } = default!;
    public string MfaHashKey { get; set; } = default!;
}