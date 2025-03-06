using CusstomAuth.Core.Db;
using CusstomAuth.Core.Db.Entities;
using CusstomAuth.Core.Features.SignIn.Dtos;
using CusstomAuth.Core.Mappers;
using Microsoft.EntityFrameworkCore;

namespace CusstomAuth.Core.Features.SignIn.Services;

public interface IDeviceService
{
    Task<AuthDevice> GetOrCreatedDeviceAsync(DeviceDto device, Guid? sessionId = null);
}

public class DeviceService(AuthDbContext dbContext, TimeProvider timeProvider) : IDeviceService
{
    public async Task<AuthDevice> GetOrCreatedDeviceAsync(DeviceDto deviceDto, Guid? sessionId = null)
    {
        var device = await dbContext.Devices.FirstOrDefaultAsync(s => s.DeviceIdentifier == deviceDto.DeviceIdentifier);
        var utcNow = timeProvider.GetUtcNow().UtcDateTime;
        if (device != null)
        {
            if (!device.IsTrusted && deviceDto.IsTrusted)
            {
                device.IsTrusted = true;
                device.TrustedOnSessionId = sessionId;
                device.TrustedAt = utcNow;
                await dbContext.SaveChangesAsync();
            }
        }
        else
        {
            device = deviceDto.MapToEntity();
            device.TrustedOnSessionId = deviceDto.IsTrusted ? sessionId : null;
            device.TrustedAt = deviceDto.IsTrusted ? utcNow : null;
            await dbContext.Devices.AddAsync(device);
            await dbContext.SaveChangesAsync();
        }
        return device;
    }
}
