namespace CusstomAuth.Core.Stores;

public interface IRoleStore : IStore<IdentityRole>
{
    Task<IdentityRole> GetDefaultRoleAsync();
    Task<IdentityRole> GetUserRoleAsync();
    Task<IdentityRole> GetAdminRoleAsync();
}
