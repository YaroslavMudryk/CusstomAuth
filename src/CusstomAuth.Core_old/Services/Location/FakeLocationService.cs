using CusstomAuth.Core.Db.Entities.Internal;

namespace CusstomAuth.Core.Services.Location;

public class FakeLocationService : ILocationService
{
    public async Task<LocationModel> GetIpInfoAsync(string ip)
    {
        return await Task.FromResult(new LocationModel
        {
            Ip = ip,
            City = "Washington",
            Country = "United States of Americas",
            Region = "Washington DC",
            Lat = 30,
            Lon = 50,
            Provider = "2Taxi",
        });
    }
}
