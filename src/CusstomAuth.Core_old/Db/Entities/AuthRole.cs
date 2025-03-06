using CusstomAuth.Core.Db.Audits;

namespace CusstomAuth.Core.Db.Entities;

public class AuthRole : AuditableSoftDeletedBaseModelWithIdentity<int>
{
    public string Name { get; set; }
    public bool IsDefault { get; set; }
    public string NameNormalized { get; set; }
    public List<AuthUserRole> UserRoles { get; set; }
    public List<AuthRoleClaim> RoleClaims { get; set; }
}
