namespace CusstomAuth.Core.SessionsManagement;

public class TokenDto
{
    public string Token { get; set; } = default!;
    public DateTime? ExpiredAt { get; set; }
    public int? ExpiredIn { get; set; }
}
