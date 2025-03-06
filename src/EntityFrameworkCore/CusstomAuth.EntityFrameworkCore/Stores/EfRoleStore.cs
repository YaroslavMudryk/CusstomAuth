using CusstomAuth.Core.Constants;
using CusstomAuth.Core.Stores;
using Microsoft.EntityFrameworkCore;

namespace CusstomAuth.EntityFrameworkCore.Stores;

public class EfRoleStore<DbContext>(DbContext dbContext) : BaseStore<IdentityRole, DbContext>(dbContext), IRoleStore
        where DbContext : IdentityDbContext
{
    public async Task<IdentityRole> GetAdminRoleAsync()
    {
        return await dbContext.Roles.AsNoTracking().Where(s => s.Name == DefaultsRoles.Administrator).FirstOrDefaultAsync();
    }

    public async Task<IdentityRole> GetDefaultRoleAsync()
    {
        return await dbContext.Roles.AsNoTracking().Where(s => s.IsDefault).FirstOrDefaultAsync();
    }

    public async Task<IdentityRole> GetUserRoleAsync()
    {
        return await dbContext.Roles.AsNoTracking().Where(s => s.Name == DefaultsRoles.User).FirstOrDefaultAsync();
    }
}
