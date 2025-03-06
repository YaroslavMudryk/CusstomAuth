using CusstomAuth.Core.Stores;
using Microsoft.EntityFrameworkCore;

namespace CusstomAuth.EntityFrameworkCore.Stores;

public class EfMfaStore<DbContext>(DbContext dbContext) : BaseStore<IdentityMfa, DbContext>(dbContext), IMfaStore
        where DbContext : IdentityDbContext
{
    public async Task<IdentityMfa> GetUserActiveMfaAsync(int userId)
    {
        return await dbContext.Mfas.Where(s => s.UserId == userId && s.IsActivated).FirstOrDefaultAsync();
    }

    public async Task<IdentityMfa> GetUserUnactiveMfaAsync(int userId)
    {
        return await dbContext.Mfas.Where(s => s.UserId == userId && !s.IsActivated).FirstOrDefaultAsync();
    }
}
