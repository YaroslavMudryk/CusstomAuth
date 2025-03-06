using CusstomAuth.Core.Stores;
using Microsoft.EntityFrameworkCore;

namespace CusstomAuth.EntityFrameworkCore.Stores;

public class EfRefreshTokenStore<DbContext>(DbContext dbContext) : BaseStore<IdentityRefreshToken, DbContext>(dbContext), IRefreshTokenStore
        where DbContext : IdentityDbContext
{
    public async Task<IdentityRefreshToken> GetRefreshTokenAsync(string token)
    {
        return await dbContext.RefreshTokens.AsNoTracking().FirstOrDefaultAsync(s => s.Token == token);
    }
}
