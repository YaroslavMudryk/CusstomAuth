using CusstomAuth.Core.Db.Entities;

namespace CusstomAuth.Core.Features.SignIn.Dtos;

public class UserTokenDto
{
    public int UserId { get; set; }
    public AuthUser User { get; set; }
    public Guid SessionId { get; set; }
    public AuthSession Session { get; set; }
    public string AuthType { get; set; }
    public string Lang { get; set; }

    public static UserTokenDto New(AuthSession session, string authType)
    {
        return new UserTokenDto
        {
            AuthType = authType,
            UserId = session.UserId,
            User = session.User,
            Lang = session.Language,
            SessionId = session.Id,
            Session = session
        };
    }
}
