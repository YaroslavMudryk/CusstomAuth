namespace CusstomAuth;

public class IdentityRefreshToken : IdentityBaseModel<Guid>
{
    public virtual string Token { get; set; } = default!;
    public virtual DateTime? TokenUsedAt { get; set; }
    public virtual DateTime ExpiredAt { set; get; }
    public virtual Guid SessionId { set; get; }
    public virtual IdentitySession Session { set; get; } = default!;
}
