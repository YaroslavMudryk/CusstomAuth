namespace CusstomAuth;

public class IdentityContact : IdentityBaseModel
{
    public virtual string Value { get; set; } = default!;
    public virtual string Description { get; set; } = default!;
    public virtual ContactType Type { get; set; }
    public virtual ContactStatus Status { get; set; }
    public virtual bool IsConfirmed { get; set; }
    public virtual DateTime? ConfirmedAt { get; set; }
    public virtual int UserId { get; set; }
    public virtual IdentityUser User { get; set; } = default!;
}

public enum ContactType
{
    Email,
    Phone
}

public enum ContactStatus
{
    Main,
    Secondary
}
