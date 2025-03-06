namespace CusstomAuth;

public class ClientModel
{
    public string Device { get; set; } = default!;
    public string DeviceType { get; set; } = default!;
    public string Os { get; set; } = default!;
    public string OsVersion { get; set; } = default!;
    public string Browser { get; set; } = default!;
    public string BrowserVersion { get; set; } = default!;

    public bool IsBrowser() => !string.IsNullOrWhiteSpace(Browser);
}
