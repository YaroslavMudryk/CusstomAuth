using CusstomAuth.Core.Data;
using CusstomAuth.Core.Mappers;
using CusstomAuth.Core.Stores;

namespace CusstomAuth.Core.Services;

public interface IDeviceService
{
    Task<IdentityDevice> GetOrCreatedDeviceAsync(DeviceInfo deviceDto, Guid? sessionId = null);
}

public class DeviceService(
    IDeviceStore deviceStore,
    TimeProvider timeProvider) : IDeviceService
{
    public async Task<IdentityDevice> GetOrCreatedDeviceAsync(DeviceInfo deviceDto, Guid? sessionId = null)
    {
        var device = await deviceStore.GetDeviceByIdentifierAsync(deviceDto.DeviceIdentifier);
        var utcNow = timeProvider.GetUtcNow().UtcDateTime;
        if (device != null)
        {
            if (!device.IsTrusted && deviceDto.IsTrusted)
            {
                device.IsTrusted = true;
                device.TrustedOnSessionId = sessionId;
                device.TrustedAt = utcNow;
                await deviceStore.UpdateAsync(device);
            }
        }
        else
        {
            device = deviceDto.MapToEntity();
            device.TrustedOnSessionId = deviceDto.IsTrusted ? sessionId : null;
            device.TrustedAt = deviceDto.IsTrusted ? utcNow : null;
            await deviceStore.CreateAsync(device);
        }
        return device;
    }
}