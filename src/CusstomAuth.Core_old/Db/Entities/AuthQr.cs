using CusstomAuth.Core.Db.Audits;

namespace CusstomAuth.Core.Db.Entities;

public class AuthQr : AuditableSoftDeletedBaseModelWithIdentity<Guid>
{
    public string QrCodeId { get; set; }
    public string QrBase64 { get; set; }
    public bool IsUsed { set; get; }
    public DateTime ActiveFrom { set; get; }
    public DateTime ActiveTo { set; get; }
    public DateTime? ActivatedAt { set; get; }
    public string Ip { get; set; }
    public string Platform { get; set; }
    public string Device { get; set; }
    public Guid? SessionId { get; set; }
    public int? UserId { get; set; }
    public AuthUser User { get; set; }
}
