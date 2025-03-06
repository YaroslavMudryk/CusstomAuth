using CusstomAuth.Core.Data;
using CusstomAuth.Core.Stores;
using Microsoft.EntityFrameworkCore;

namespace CusstomAuth.EntityFrameworkCore.Stores;

public class EfUserStore<DbContext>(DbContext dbContext) : BaseStore<IdentityUser, DbContext>(dbContext), IUserStore
        where DbContext : IdentityDbContext
{
    public async Task<IdentityUser> GetUserByLoginAsync(string login)
    {
        return await dbContext.Users.AsNoTracking().FirstOrDefaultAsync(s => s.Login == login);
    }

    public async Task<bool> IsExistUserByLoginAsync(string login)
    {
        return await dbContext.Users.AnyAsync(u => u.Login == login);
    }
}
