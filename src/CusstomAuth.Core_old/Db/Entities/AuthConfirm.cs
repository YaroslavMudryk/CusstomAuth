using CusstomAuth.Core.Db.Audits;

namespace CusstomAuth.Core.Db.Entities;

public class AuthConfirm : AuditableSoftDeletedBaseModelWithIdentity<int>
{
    public string Code { get; set; }
    public DateTime ActiveFrom { get; set; }
    public DateTime ActiveTo { get; set; }
    public bool IsActivated { get; set; }
    public DateTime? ActivatedAt { get; set; }
    public ConfirmType Type { get; set; }
    public int UserId { get; set; }
    public AuthUser User { get; set; }

    public bool IsActualyRequest(DateTime dateTime)
    {
        return dateTime >= ActiveFrom && dateTime <= ActiveTo;
    }

    public static AuthConfirm NewWithAccount(DateTime now, string code)
    {
        return new AuthConfirm
        {
            ActiveFrom = now,
            ActiveTo = now.AddDays(1),
            Code = code,
            Type = ConfirmType.Account,
            IsActivated = false,
            ActivatedAt = null
        };
    }
}

public enum ConfirmType
{
    Account = 1,
    Phone,
    Email
}
