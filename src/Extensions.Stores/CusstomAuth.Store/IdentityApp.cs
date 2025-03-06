namespace CusstomAuth;

public class IdentityApp : IdentityBaseModel
{
    public virtual string Name { get; set; } = default!;
    public virtual string ShortName { get; set; } = default!;
    public virtual string Description { get; set; } = default!;
    public virtual string Image { get; set; } = default!;
    public virtual string ClientId { get; set; } = default!;
    public virtual string ClientSecret { get; set; } = default!;
    public virtual bool IsActive { get; set; }
    public virtual DateTime ActiveFrom { set; get; }
    public virtual DateTime? ActiveTo { set; get; }

    public bool IsActiveByTime(DateTime dateTime)
    {
        return dateTime >= ActiveFrom && dateTime <= ActiveTo;
    }
}
