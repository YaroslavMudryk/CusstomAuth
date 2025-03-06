using CusstomAuth.Core.Db.Audits;

namespace CusstomAuth.Core.Db.Entities;

public class AuthRefreshToken : AuditableSoftDeletedBaseModelWithIdentity<Guid>
{
    public string Token { get; set; }
    public DateTime? TokenUsedAt { get; set; }
    public DateTime ExpiredAt { set; get; }
    public Guid SessionId { set; get; }
    public AuthSession Session { set; get; }
}
