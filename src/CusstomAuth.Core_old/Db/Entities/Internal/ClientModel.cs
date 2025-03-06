namespace CusstomAuth.Core.Db.Entities.Internal;

public class ClientModel
{
    public string Device { get; set; }
    public string DeviceType { get; set; }
    public string Os { get; set; }
    public string OsVersion { get; set; }
    public string Browser { get; set; }
    public string BrowserVersion { get; set; }

    public bool IsBrowser() => !string.IsNullOrWhiteSpace(Browser);
}
