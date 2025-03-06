using CusstomAuth.Core.Db.Audits;

namespace CusstomAuth.Core.Db.Entities;

public class AuthExternalLogin : AuditableSoftDeletedBaseModelWithIdentity<int>
{
    public string Key { get; set; }
    public string Provider { get; set; }
    public bool IsActive { set; get; }
    public DateTime? DeactivatedAt { get; set; }
    public int UserId { get; set; }
    public AuthUser User { get; set; }
}
