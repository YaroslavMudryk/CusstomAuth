using CusstomAuth.Core.Db.Entities;

namespace CusstomAuth.Core.Services.Devices;

public static class DeviceDetailsExtensions
{
    public static AuthDevice MapToEntity(this DeviceDetails deviceDetails)
    {
        ArgumentNullException.ThrowIfNull(deviceDetails);

        return new AuthDevice
        {
            DeviceIdentifier = deviceDetails.DeviceIdentifier,
            Brand = deviceDetails.Brand,
            Model = deviceDetails.Model,
            VendorModel = deviceDetails.VendorModel,
            Type = deviceDetails.Type,
            Os = deviceDetails.Os,
            OsShortName = deviceDetails.OsShortName,
            OsVersion = deviceDetails.OsVersion,
            OsPlatform = deviceDetails.OsPlatform,
            OsUI = deviceDetails.OsUI,
            Browser = deviceDetails.Browser,
            BrowserVersion = deviceDetails.BrowserVersion,
            BrowserType = deviceDetails.BrowserType,
            BrowserEngine = deviceDetails.BrowserEngine,
            BrowserEngineVersion = deviceDetails.BrowserEngineVersion,
        };
    }
}
