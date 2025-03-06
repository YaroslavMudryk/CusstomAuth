namespace CusstomAuth.Core.Services;

public interface ILocationService
{
    Task<LocationModel> GetIpInfoAsync(string ip);
}

public class FakeLocationService : ILocationService
{
    public async Task<LocationModel> GetIpInfoAsync(string ip)
    {
        return await Task.FromResult(new LocationModel
        {
            City = "Kyiv",
            Country = "Ukraine",
            Ip = ip,
            Provider = "Lanet",
            Region = "Kyiv region",
            Lat = 50.4501,
            Lon = 30.5234
        });
    }
}