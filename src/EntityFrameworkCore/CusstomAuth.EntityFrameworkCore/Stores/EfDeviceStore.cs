using CusstomAuth.Core.Stores;
using Microsoft.EntityFrameworkCore;

namespace CusstomAuth.EntityFrameworkCore.Stores;

public class EfDeviceStore<DbContext>(DbContext dbContext) : BaseStore<IdentityDevice, DbContext>(dbContext), IDeviceStore
        where DbContext : IdentityDbContext
{
    public async Task<IdentityDevice> GetDeviceByIdentifierAsync(string identifier)
    {
        return await dbContext.Devices.FirstOrDefaultAsync(s => s.DeviceIdentifier == identifier);
    }

    public async Task<IReadOnlyList<IdentityDevice>> GetUserDevicesAsync(int userId)
    {
        var deviceIds = await dbContext.Sessions.AsNoTracking().GroupBy(b => b.DeviceId).Select(s => s.Key).DistinctBy(s => s).ToListAsync();
        var devices = await dbContext.Devices.AsNoTracking().Where(d => deviceIds.Contains(d.Id)).ToListAsync();
        return devices;
    }
}
