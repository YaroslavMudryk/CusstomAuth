namespace CusstomAuth.Core.Data;

public class DeviceInfo
{
    public string Id { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
    public string Name { get; set; } = default!;
    public string DeviceIdentifier { get; set; } = default!;
    public string Brand { get; set; } = default!;
    public string Model { get; set; } = default!;
    public string VendorModel { get; set; } = default!;
    public string Type { get; set; } = default!;
    public string Os { get; set; } = default!;
    public string OsVersion { get; set; } = default!;
    public string OsShortName { get; set; } = default!;
    public string OsUI { get; set; } = default!;
    public string OsPlatform { get; set; } = default!;
    public string Browser { get; set; } = default!;
    public string BrowserVersion { get; set; } = default!;
    public string BrowserType { get; set; } = default!;
    public string BrowserEngine { get; set; } = default!;
    public string BrowserEngineVersion { get; set; } = default!;
    public bool IsTrusted { get; set; }
    public DateTime? TrustedAt { get; set; }
}
