using CusstomAuth.Core.Db;
using CusstomAuth.Core.Features.Devices.Dtos;
using CusstomAuth.Core.Mappers;
using Microsoft.EntityFrameworkCore;

namespace CusstomAuth.Core.Features.Devices.Services;

public interface IDeviceService
{
    Task<IReadOnlyList<DeviceInfo>> GetUserDevicesAsync();
}

public class DeviceService(AuthDbContext dbContext, ICurrentContext currentContext) : IDeviceService
{
    public async Task<IReadOnlyList<DeviceInfo>> GetUserDevicesAsync()
    {
        var currentSessionId = currentContext.User.SessionId;
        var userId = currentContext.User.Id;

        Dictionary<Guid, IEnumerable<Guid>> deviceSessionsPair = 
            await dbContext
            .Sessions
            .AsNoTracking()
            .GroupBy(s => s.DeviceId)
            .ToDictionaryAsync(s => s.Key, s => s.Select(s => s.Id));

        var devices = await dbContext.Devices.AsNoTracking().Where(d => deviceSessionsPair.Select(s => s.Key).Contains(d.Id)).ToListAsync();
        var currentDeviceId = deviceSessionsPair.FirstOrDefault(s => s.Value.Contains(currentSessionId)).Key;
        var currentSessionDevice = devices.FirstOrDefault(d => d.Id == currentDeviceId);
        var orderedDevices = new List<DeviceInfo>([currentSessionDevice.MapToInfo()]);
        devices.Remove(currentSessionDevice);
        orderedDevices.AddRange(devices.OrderByDescending(s => s.CreatedAt).Select(s => s.MapToInfo()));
        return orderedDevices;
    }
}
