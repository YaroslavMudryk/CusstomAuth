using CusstomAuth.Core.Db.Audits;

namespace CusstomAuth.Core.Db.Entities;

public class AuthBlock : AuditableSoftDeletedBaseModelWithIdentity<int>
{
    public DateTime Start { get; set; }
    public DateTime Finish { get; set; }
    public string Cause { get; set; }
    public bool IsPermanent => Finish == DateTime.MaxValue;
    public int UserId { get; set; }
    public AuthUser User { get; set; }
}
