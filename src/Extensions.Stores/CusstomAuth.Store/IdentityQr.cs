namespace CusstomAuth;

public class IdentityQr : IdentityBaseModel
{
    public virtual string QrCodeId { get; set; } = default!;
    public virtual string QrBase64 { get; set; } = default!;
    public virtual bool IsUsed { set; get; }
    public virtual DateTime ActiveFrom { set; get; }
    public virtual DateTime ActiveTo { set; get; }
    public virtual DateTime? ActivatedAt { set; get; }
    public virtual string Ip { get; set; } = default!;
    public virtual string Platform { get; set; } = default!;
    public virtual string Device { get; set; } = default!;
    public virtual Guid? SessionId { get; set; }
    public virtual int? UserId { get; set; }
    public virtual IdentityUser User { get; set; } = default!;
}
