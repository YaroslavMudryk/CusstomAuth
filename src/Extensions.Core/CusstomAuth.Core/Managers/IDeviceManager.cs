using CusstomAuth.Core.Data;
using CusstomAuth.Core.Stores;

namespace CusstomAuth.Core.Managers;

public interface IDeviceManager
{
    Task<DevicesResponse> GetUserDevicesAsync(string[] deviceIds = null!);
}

public class DeviceManager(
    ICurrentContext currentContext,
    IDeviceStore deviceStore) : IDeviceManager
{
    public async Task<DevicesResponse> GetUserDevicesAsync(string[] deviceIds = null!)
    {
        var currentSessionId = currentContext.User.SessionId;
        var userId = currentContext.User.Id;

        var devices = await deviceStore.GetUserDevicesAsync(userId);

        return new DevicesResponse
        {
            Devices = [.. devices.OrderByDescending(s => s.CreatedAt).Select(s => new DeviceInfo
            {
                Id = s.Id.ToString(),
                CreatedAt = s.CreatedAt,
                Brand = s.Brand,
                Browser = s.Browser,
                BrowserEngine = s.BrowserEngine,
                BrowserEngineVersion = s.BrowserEngineVersion,
                BrowserType = s.BrowserType,
                BrowserVersion = s.BrowserVersion,
                DeviceIdentifier = s.DeviceIdentifier,
                IsTrusted = s.IsTrusted,
                Model = s.Model,
                Os = s.Os,
                OsPlatform = s.OsPlatform,
                OsShortName = s.OsShortName,
                OsUI = s.OsUI,
                OsVersion = s.OsVersion,
                TrustedAt = s.TrustedAt,
                Type = s.Type,
                VendorModel = s.VendorModel,
            })],
        };
    }
}
