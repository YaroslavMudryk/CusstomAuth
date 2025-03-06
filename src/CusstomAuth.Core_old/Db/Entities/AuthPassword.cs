using CusstomAuth.Core.Db.Audits;

namespace CusstomAuth.Core.Db.Entities;

public class AuthPassword : AuditableSoftDeletedBaseModelWithIdentity<int>
{
    public string PasswordHash { set; get; }
    public string Hint { get; set; }
    public bool IsActive { get; set; }
    public DateTime ActivatedAt { get; set; }
    public DateTime? DeactivatedAt { get; set; }
    public int UserId { get; set; }
    public AuthUser User { get; set; }
}
