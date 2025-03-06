namespace CusstomAuth;

public class IdentitySession : IdentityBaseModel<Guid>
{
    public virtual string PublicId { get; set; } = default!;
    public virtual AppModel App { get; set; } = default!;
    public virtual ClientModel Client { get; set; } = default!;
    public virtual LocationModel Location { get; set; } = default!;
    public virtual bool ViaMFA { get; set; }
    public virtual SessionType Type { get; set; }
    public virtual string Language { get; set; } = default!;
    public virtual DateTime? ExpiredAt { get; set; }
    public virtual SessionStatus Status { get; set; }
    public virtual DateTime? DeactivatedAt { get; set; }
    public virtual Guid? DeactivatedBySessionId { get; set; }
    public virtual DateTime? LastActivity { get; set; }
    public virtual int UserId { get; set; }
    public virtual IdentityUser User { get; set; } = default!;
    public virtual Guid DeviceId { get; set; }
    public virtual IdentityDevice Device { get; set; } = default!;
    public virtual List<IdentityRefreshToken> RefreshTokens { get; set; } = default!;

    public bool IsFinished()
    {
        return Status is SessionStatus.Completed or SessionStatus.Terminated;
    }
}

public enum SessionStatus
{
    Pending = 1,
    Active,
    Completed,
    Terminated
}

public enum SessionType
{
    Password = 1,
    ExternalService,
    Qr
}
