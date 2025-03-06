namespace CusstomAuth.Core.Stores;

public interface IDeviceStore : IStore<IdentityDevice>
{
    Task<IReadOnlyList<IdentityDevice>> GetUserDevicesAsync(int userId);
    Task<IdentityDevice> GetDeviceByIdentifierAsync(string identifier);
}
