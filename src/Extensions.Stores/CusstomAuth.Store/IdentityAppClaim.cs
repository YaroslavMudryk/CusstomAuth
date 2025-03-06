namespace CusstomAuth;

public class IdentityAppClaim : IdentityBaseModel
{
    public virtual DateTime ActiveFrom { set; get; }
    public virtual DateTime? ActiveTo { set; get; }
    public virtual bool IsActive { get; set; }
    public virtual int ClaimId { get; set; }
    public virtual IdentityClaim Claim { get; set; } = default!;
    public virtual int AppId { get; set; }
    public virtual IdentityApp App { get; set; } = default!;
}
