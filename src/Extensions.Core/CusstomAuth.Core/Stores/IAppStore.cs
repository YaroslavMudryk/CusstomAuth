namespace CusstomAuth.Core.Stores;

public interface IAppStore : IStore<IdentityApp>
{
    Task<IdentityApp> GetAppByIdAndSecretAsync(string id, string secret);
}
