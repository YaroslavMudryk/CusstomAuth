namespace CusstomAuth;

public class IdentityClaim : IdentityBaseModel
{
    public virtual string Type { get; set; } = default!;
    public virtual string Value { get; set; } = default!;
    public virtual string Issuer { get; set; } = default!;
    public virtual string DisplayText { get; set; } = default!;
    public virtual List<IdentityRoleClaim> RoleClaims { get; set; } = default!;
    public virtual List<IdentityAppClaim> AppClaims { get; set; } = default!;
}
