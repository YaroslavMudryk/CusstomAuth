namespace CusstomAuth;

public class IdentityPassword : IdentityBaseModel
{
    public virtual string PasswordHash { set; get; } = default!;
    public virtual string Hint { get; set; } = default!;
    public virtual bool IsActive { get; set; }
    public virtual DateTime ActivatedAt { get; set; }
    public virtual DateTime? DeactivatedAt { get; set; }
    public virtual int UserId { get; set; }
    public virtual IdentityUser User { get; set; } = default!;
}
