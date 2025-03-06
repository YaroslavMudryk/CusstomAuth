using System.Text;

namespace CusstomAuth.Core.Services.Devices;

public class DeviceDetails
{
    public string DeviceIdentifier { get; set; }
    public string Brand { get; set; }
    public string Model { get; set; }
    public string VendorModel { get; set; }
    public string Type { get; set; }
    public string Os { get; set; }
    public string OsVersion { get; set; }
    public string OsShortName { get; set; }
    public string OsUI { get; set; }
    public string OsPlatform { get; set; }
    public string Browser { get; set; }
    public string BrowserVersion { get; set; }
    public string BrowserType { get; set; }
    public string BrowserEngine { get; set; }
    public string BrowserEngineVersion { get; set; }

    public string GetBrowserHash()
    {
        var hash = $"{Browser}_{BrowserVersion}_{BrowserType}_{BrowserEngine}_{BrowserEngineVersion}";
        return Convert.ToBase64String(Encoding.UTF8.GetBytes(hash));
    }

    public string GetDeviceHash()
    {
        var hash = $"{DeviceIdentifier}_{Brand}_{Model}_{VendorModel}_{Type}";
        return Convert.ToBase64String(Encoding.UTF8.GetBytes(hash));
    }

    public string GetOsHash()
    {
        var hash = $"{Os}_{OsVersion}_{OsShortName}_{OsUI}_{OsPlatform}";
        return Convert.ToBase64String(Encoding.UTF8.GetBytes(hash));
    }
}
