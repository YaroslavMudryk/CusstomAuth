using CusstomAuth.Core.Stores;

namespace CusstomAuth.EntityFrameworkCore.Stores;

public class EfPasswordStore<DbContext>(DbContext dbContext) : BaseStore<IdentityPassword, DbContext>(dbContext), IPasswordStore
        where DbContext : IdentityDbContext
{

}
