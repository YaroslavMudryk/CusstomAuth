using CusstomAuth.Core.Db.Audits;

namespace CusstomAuth.Core.Db.Entities;

public class AuthRoleClaim : AuditableSoftDeletedBaseModelWithIdentity<int>
{
    public DateTime ActiveFrom { set; get; }
    public DateTime? ActiveTo { set; get; }
    public bool IsActive { get; set; }
    public int RoleId { get; set; }
    public AuthRole Role { get; set; }
    public int ClaimId { get; set; }
    public AuthClaim Claim { get; set; }
}
