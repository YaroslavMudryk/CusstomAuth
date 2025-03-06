namespace CusstomAuth;

public class IdentityLoginAttempt : IdentityBaseModel
{
    public virtual string Login { get; set; } = default!;
    public virtual string Password { get; set; } = default!;
    public virtual ClientModel Client { get; set; } = default!;
    public virtual LocationModel Location { get; set; } = default!;
    public virtual bool IsSuccess { set; get; }
    public virtual string SessionId { set; get; } = default!;
    public virtual int? UserId { get; set; }
    public virtual IdentityUser User { get; set; } = default!;
}
