namespace CusstomAuth.Core.Features.SignIn.Dtos;

public class SignInMfaDto
{
    public string Code { get; set; }
    public string MfaHashKey { get; set; }
}
