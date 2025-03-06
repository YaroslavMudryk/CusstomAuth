using CusstomAuth.Core.Db.Audits;

namespace CusstomAuth.Core.Db.Entities;

public class AuthUserRole : AuditableSoftDeletedBaseModelWithIdentity<int>
{
    public DateTime ActiveFrom { set; get; }
    public DateTime? ActiveTo { set; get; }
    public bool IsActive { get; set; }
    public int UserId { get; set; }
    public AuthUser User { get; set; }
    public int RoleId { get; set; }
    public AuthRole Role { get; set; }
}
