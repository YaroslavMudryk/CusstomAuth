using System.Text.Json.Serialization;

namespace CusstomAuth.Core.Features.SignIn.Dtos;

public class JwtTokenDto
{
    public string Token { get; set; }
    public string RefreshToken { get; set; }
    public string MfaHashKey { get; set; }
    public DateTime? ExpiredAt { get; set; }
}
