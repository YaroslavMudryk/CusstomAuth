using CusstomAuth.Core.Db.Entities;
using CusstomAuth.Core.Db.Entities.Internal;
using CusstomAuth.Core.Features.Devices.Dtos;
using CusstomAuth.Core.Features.SignIn.Dtos;
using Extensions.DeviceDetector.Models;

namespace CusstomAuth.Core.Mappers;

public static class DeviceMapper
{
    public static IEnumerable<AuthDevice> MapToEntity(this IEnumerable<DeviceDto> devices)
    {
        return devices.Select(MapToEntity);
    }

    public static AuthDevice MapToEntity(this DeviceDto device)
    {
        return new AuthDevice
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

    public static IEnumerable<DeviceDto> MapToDto(this IEnumerable<AuthDevice> devices)
    {
        return devices.Select(MapToDto);
    }

    public static DeviceDto MapToDto(this AuthDevice device)
    {
        return new DeviceDto
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

    public static ClientModel MapToClient(this DeviceDto device)
    {
        if (device == null)
            return new ClientModel();

        return new ClientModel
        {
            Device = device.Model,
            DeviceType = device.Type,
            Browser = device.Browser,
            BrowserVersion = device.BrowserVersion,
            Os = device.Os,
            OsVersion = device.OsVersion
        };
    }

    public static DeviceDto MapToDto(this ClientInfo clientInfo)
    {
        if (clientInfo == null)
            return null;

        return new DeviceDto
        {
            Brand = clientInfo.Device.Brand,
            Model = clientInfo.Device.Model,
            Type = clientInfo.Device.Type,
            Browser = clientInfo.Browser.Name,
            BrowserVersion = clientInfo.Browser.Version,
            BrowserType = clientInfo.Browser.Type,
            BrowserEngine = clientInfo.Browser.Engine,
            BrowserEngineVersion = clientInfo.Browser.EngineVersion,
            Os = clientInfo.OS.Name,
            OsVersion = clientInfo.OS.Version,
            OsShortName = clientInfo.OS.ShortName,
            OsPlatform = clientInfo.OS.Platform,
            IsTrusted = false,
            VendorModel = null,
            OsUI = null,
            DeviceIdentifier = null,
        };
    }

    public static ClientModel MapToClientInfo(this ClientInfo clientInfo)
    {
        if (clientInfo == null)
            return null;

        return new ClientModel
        {
            Browser = clientInfo.Browser.Name,
            BrowserVersion = clientInfo.Browser.Version,
            Device = clientInfo.Device.Model,
            DeviceType = clientInfo.Device.Type,
            Os = clientInfo.OS.Name,
            OsVersion = clientInfo.OS.Version
        };
    }

    public static Device MapToDevice(this ClientInfo clientInfo)
    {
        if (clientInfo == null)
            return null;

        return new Device
        {
            Brand = clientInfo.Device.Brand,
            Model = clientInfo.Device.Model,
            Type = clientInfo.Device.Type,
            BrandShortName = clientInfo.Device.BrandShortName
        };
    }

    public static DeviceInfo MapToInfo(this AuthDevice authDevice)
    {
        if (authDevice == null)
            return null;

        return new DeviceInfo
        {
            Id = authDevice.Id,
            CreatedAt = authDevice.CreatedAt,
            DeviceIdentifier = authDevice.DeviceIdentifier,
            Brand = authDevice.Brand,
            Model = authDevice.Model,
            VendorModel = authDevice.VendorModel,
            Type = authDevice.Type,
            Os = authDevice.Os,
            OsVersion = authDevice.OsVersion,
            OsShortName = authDevice.OsShortName,
            OsPlatform = authDevice.OsPlatform,
            OsUI = authDevice.OsUI,
            Browser = authDevice.Browser,
            BrowserVersion = authDevice.BrowserVersion,
            BrowserEngine = authDevice.BrowserEngine,
            BrowserEngineVersion = authDevice.BrowserEngineVersion,
            BrowserType = authDevice.BrowserType,
            IsTrusted = authDevice.IsTrusted,
            TrustedAt = authDevice.TrustedAt,
        };
    }
}
