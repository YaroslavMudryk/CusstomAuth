using CusstomAuth.Core.Db.Audits;

namespace CusstomAuth.Core.Db.Entities;

public class AuthAppClaim : AuditableSoftDeletedBaseModelWithIdentity<int>
{
    public DateTime ActiveFrom { set; get; }
    public DateTime? ActiveTo { set; get; }
    public bool IsActive { get; set; }
    public int ClaimId { get; set; }
    public AuthClaim Claim { get; set; }
    public int AppId { get; set; }
    public AuthApp App { get; set; }
}
