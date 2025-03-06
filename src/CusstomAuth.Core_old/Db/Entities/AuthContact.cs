using CusstomAuth.Core.Db.Audits;

namespace CusstomAuth.Core.Db.Entities;

public class AuthContact : AuditableSoftDeletedBaseModelWithIdentity<int>
{
    public string Value { get; set; }
    public string Description { get; set; }
    public ContactType Type { get; set; }
    public ContactStatus Status { get; set; }
    public bool IsConfirmed { get; set; }
    public DateTime? ConfirmedAt { get; set; }
    public int UserId { get; set; }
    public AuthUser User { get; set; }
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
