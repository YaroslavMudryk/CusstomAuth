using CusstomAuth.Core.Stores;

namespace CusstomAuth.EntityFrameworkCore.Stores;

public class EfAppClaimStore<DbContext>(DbContext dbContext) : BaseStore<IdentityAppClaim, DbContext>(dbContext), IAppClaimStore
    where DbContext : IdentityDbContext
{

}
