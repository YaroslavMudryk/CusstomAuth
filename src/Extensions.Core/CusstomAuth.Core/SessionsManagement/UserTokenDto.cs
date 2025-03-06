namespace CusstomAuth.Core.SessionsManagement;

public class UserTokenDto
{
    public int UserId { get; set; }
    public Guid SessionId { get; set; }
}
