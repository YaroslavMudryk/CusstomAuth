using CusstomAuth.Core.Stores;

namespace CusstomAuth.EntityFrameworkCore.Stores;

public class EfUserTokenStore<DbContext>(DbContext dbContext) : BaseStore<IdentityUserToken, DbContext>(dbContext), IUserTokenStore
        where DbContext : IdentityDbContext
{

}
