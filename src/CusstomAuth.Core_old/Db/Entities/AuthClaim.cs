using CusstomAuth.Core.Db.Audits;

namespace CusstomAuth.Core.Db.Entities;

public class AuthClaim : AuditableSoftDeletedBaseModelWithIdentity<int>
{
    public string Type { get; set; }
    public string Value { get; set; }
    public string Issuer { get; set; }
    public string DisplayText { get; set; }
    public List<AuthRoleClaim> RoleClaims { get; set; }
    public List<AuthAppClaim> AppClaims { get; set; }
}
