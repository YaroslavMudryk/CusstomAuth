namespace CusstomAuth;

public class IdentityBlock : IdentityBaseModel
{
    public virtual DateTime Start { get; set; }
    public virtual DateTime Finish { get; set; }
    public virtual string Cause { get; set; } = default!;
    public virtual bool IsPermanent => Finish == DateTime.MaxValue;
    public virtual int UserId { get; set; }
    public virtual IdentityUser User { get; set; } = default!;
}
