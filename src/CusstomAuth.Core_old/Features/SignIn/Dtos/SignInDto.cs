namespace CusstomAuth.Core.Features.SignIn.Dtos;

public class SignInDto
{
    public string Login { get; set; }
    public string Password { get; set; }
    public string Lang { get; set; }
    public DeviceDto Device { get; set; }
    public AppSignInDto App { get; set; }
}
