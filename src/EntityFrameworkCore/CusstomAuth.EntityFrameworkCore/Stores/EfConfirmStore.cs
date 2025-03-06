using CusstomAuth.Core.Stores;
using Microsoft.EntityFrameworkCore;

namespace CusstomAuth.EntityFrameworkCore.Stores;

public class EfConfirmStore<DbContext>(DbContext dbContext) : BaseStore<IdentityConfirm, DbContext>(dbContext), IConfirmStore
        where DbContext : IdentityDbContext
{
    public async Task<IdentityConfirm> GetUserConfirmByAccountAsync(string code, int userId)
    {
        return await dbContext.Confirms
            .Where(s => s.Code == code && s.UserId == userId && s.Type == ConfirmType.Account)
            .FirstOrDefaultAsync();
    }
}
