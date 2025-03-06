namespace CusstomAuth.Core.Data;

public class JwtTokenResponse
{
    public string Token { get; set; } = default!;
    public string RefreshToken { get; set; } = default!;
    public string MfaHashKey { get; set; } = default!;
    public DateTime? ExpiredAt { get; set; }
}
