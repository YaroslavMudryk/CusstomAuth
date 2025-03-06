using CusstomAuth.Core.Db.Entities.Internal;

namespace CusstomAuth.Core.Services.Location;

public interface ILocationService
{
    Task<LocationModel> GetIpInfoAsync(string ip);
}
