namespace CusstomAuth;

public class IdentityUserRole : IdentityBaseModel
{
    public virtual DateTime ActiveFrom { set; get; }
    public virtual DateTime? ActiveTo { set; get; }
    public virtual bool IsActive { get; set; }
    public virtual int UserId { get; set; }
    public virtual IdentityUser User { get; set; } = default!;
    public virtual int RoleId { get; set; }
    public virtual IdentityRole Role { get; set; } = default!;
}