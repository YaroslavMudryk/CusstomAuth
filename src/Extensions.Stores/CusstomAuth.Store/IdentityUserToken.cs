namespace CusstomAuth;

public class IdentityUserToken : IdentityBaseModel
{
    public virtual string Token { get; set; } = default!;
    public virtual string Provider { get; set; } = default!;
    public virtual bool IsActive { set; get; }
    public virtual DateTime? DeactivatedAt { get; set; }
    public virtual int UserId { get; set; }
    public virtual IdentityUser User { get; set; } = default!;
}
