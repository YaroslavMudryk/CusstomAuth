namespace CusstomAuth;

public class IdentityMfa : IdentityBaseModel
{
    public virtual DateTime? Activated { get; set; }
    public virtual Guid? ActivatedBySessionId { get; set; }
    public virtual bool IsActivated { get; set; }
    public virtual string Secret { get; set; } = default!;
    public virtual string EntryCode { get; set; } = default!;
    public virtual string QrCodeBase64 { get; set; } = default!;
    public virtual string[] RestoreCodes { get; set; } = default!;
    public virtual DateTime? DiactivedAt { get; set; }
    public virtual Guid? DiactivedBySessionId { get; set; }
    public virtual int UserId { get; set; }
    public virtual IdentityUser User { get; set; } = default!;
}
