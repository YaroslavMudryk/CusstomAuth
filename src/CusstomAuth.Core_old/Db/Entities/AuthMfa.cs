using CusstomAuth.Core.Db.Audits;

namespace CusstomAuth.Core.Db.Entities;

public class AuthMfa : AuditableSoftDeletedBaseModelWithIdentity<int>
{
    public DateTime? Activated { get; set; }
    public Guid? ActivatedBySessionId { get; set; }
    public bool IsActivated { get; set; }
    public string Secret { get; set; }
    public string EntryCode { get; set; }
    public string QrCodeBase64 { get; set; }
    public string[] RestoreCodes { get; set; }
    public DateTime? DiactivedAt { get; set; }
    public Guid? DiactivedBySessionId { get; set; }
    public int UserId { get; set; }
    public AuthUser User { get; set; }
}
