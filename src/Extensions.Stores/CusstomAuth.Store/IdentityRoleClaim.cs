namespace CusstomAuth;

public class IdentityRoleClaim : IdentityBaseModel
{
    public virtual DateTime ActiveFrom { set; get; }
    public virtual DateTime? ActiveTo { set; get; }
    public virtual bool IsActive { get; set; }
    public virtual int RoleId { get; set; }
    public virtual IdentityRole Role { get; set; } = default!;
    public virtual int ClaimId { get; set; }
    public virtual IdentityClaim Claim { get; set; } = default!;
}
