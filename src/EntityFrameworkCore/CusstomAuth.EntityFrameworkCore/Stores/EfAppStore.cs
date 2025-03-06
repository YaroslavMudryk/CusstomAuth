using CusstomAuth.Core.Stores;
using Microsoft.EntityFrameworkCore;

namespace CusstomAuth.EntityFrameworkCore.Stores;

public class EfAppStore<DbContext>(DbContext dbContext) : BaseStore<IdentityApp, DbContext>(dbContext), IAppStore
        where DbContext : IdentityDbContext
{
    public async Task<IdentityApp> GetAppByIdAndSecretAsync(string id, string secret)
    {
        return await dbContext.Apps.AsNoTracking().FirstOrDefaultAsync(app => app.ClientId == id && app.ClientSecret == secret);
    }
}
