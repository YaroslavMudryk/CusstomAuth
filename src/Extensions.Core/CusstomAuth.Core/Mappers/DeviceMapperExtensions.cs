using CusstomAuth.Core.Data;
using Extensions.DeviceDetector.Models;

namespace CusstomAuth.Core.Mappers;

public static class DeviceMapperExtensions
{
    public static ClientModel MapToClient(this DeviceInfo device)
    {
        if (device == null)
            return null!;

        return new ClientModel
        {
            
        };
    }

    public static DeviceInfo MapToDto(this ClientInfo client)
    {
        if (client == null)
            return null!;

        return new DeviceInfo
        {

        };
    }

    public static IdentityDevice MapToEntity(this DeviceInfo device)
    {
        if (device == null)
            return null!;

        return new IdentityDevice
        {
            DeviceIdentifier = device.DeviceIdentifier,
            Brand = device.Brand,
            Model = device.Model,
            Type = device.Type,
            VendorModel = device.VendorModel,
            Os = device.Os,
            OsVersion = device.OsVersion,
            OsShortName = device.OsShortName,
            OsPlatform = device.OsPlatform,
            OsUI = device.OsUI,
            Browser = device.Browser,
            BrowserEngine = device.BrowserEngine,
            BrowserEngineVersion = device.BrowserEngineVersion,
            BrowserType = device.BrowserType,
            BrowserVersion = device.BrowserVersion,
            IsTrusted = device.IsTrusted
        };
    }
}
