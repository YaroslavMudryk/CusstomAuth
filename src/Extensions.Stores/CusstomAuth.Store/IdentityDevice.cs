namespace CusstomAuth;

public class IdentityDevice : IdentityBaseModel<Guid>
{
    public virtual string DeviceIdentifier { get; set; } = default!;
    public virtual string Brand { get; set; } = default!;
    public virtual string Model { get; set; } = default!;
    public virtual string VendorModel { get; set; } = default!;
    public virtual string Type { get; set; } = default!;
    public virtual string Os { get; set; } = default!;
    public virtual string OsVersion { get; set; } = default!;
    public virtual string OsShortName { get; set; } = default!;
    public virtual string OsUI { get; set; } = default!;
    public virtual string OsPlatform { get; set; } = default!;
    public virtual string Browser { get; set; } = default!;
    public virtual string BrowserVersion { get; set; } = default!;
    public virtual string BrowserType { get; set; } = default!;
    public virtual string BrowserEngine { get; set; } = default!;
    public virtual string BrowserEngineVersion { get; set; } = default!;

    public virtual bool IsTrusted { get; set; }
    public virtual DateTime? TrustedAt { get; set; }
    public virtual Guid? TrustedOnSessionId { get; set; }

    public virtual List<IdentitySession> Sessions { get; set; } = default!;
}
