namespace CusstomAuth;

public class IdentityConfirm : IdentityBaseModel
{
    public virtual string Code { get; set; } = default!;
    public virtual DateTime ActiveFrom { get; set; }
    public virtual DateTime ActiveTo { get; set; }
    public virtual bool IsActivated { get; set; }
    public virtual DateTime? ActivatedAt { get; set; }
    public virtual ConfirmType Type { get; set; }
    public virtual int UserId { get; set; }
    public virtual IdentityUser User { get; set; } = default!;

    public bool IsActualyRequest(DateTime dateTime)
    {
        return dateTime >= ActiveFrom && dateTime <= ActiveTo;
    }

    public static IdentityConfirm NewWithAccount(DateTime utcNow, string code)
    {
        return new IdentityConfirm
        {
            ActiveFrom = utcNow,
            ActiveTo = utcNow.AddDays(1),
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

