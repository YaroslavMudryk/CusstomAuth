using CusstomAuth.Core.Db;
using CusstomAuth.Core.Db.Entities;
using Microsoft.EntityFrameworkCore;

namespace CusstomAuth.Core.Managers;

public interface IRoleManager
{
    Task<AuthRole> GetDefaultRoleAsync();
}

public class RoleManager(AuthDbContext db) : IRoleManager
{
    public async Task<AuthRole> GetDefaultRoleAsync()
    {
        return await db.Roles.FirstOrDefaultAsync(role => role.IsDefault);
    }
}
