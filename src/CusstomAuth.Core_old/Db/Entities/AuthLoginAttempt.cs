using CusstomAuth.Core.Db.Audits;
using CusstomAuth.Core.Db.Entities.Internal;

namespace CusstomAuth.Core.Db.Entities;

public class AuthLoginAttempt : AuditableSoftDeletedBaseModelWithIdentity<Guid>
{
    public string Login { get; set; }
    public string Password { get; set; }
    public ClientModel Client { get; set; }
    public LocationModel Location { get; set; }
    public bool IsSuccess { set; get; }
    public string SessionId { set; get; }
    public int? UserId { get; set; }
    public AuthUser User { get; set; }

    public static AuthLoginAttempt New(AuthSession session, bool isSuccess)
    {
        return new AuthLoginAttempt
        {
            Login = session.User.Login,
            Client = session.Client,
            Location = session.Location,
            IsSuccess = isSuccess,
            UserId = session.UserId
        };
    }
}
