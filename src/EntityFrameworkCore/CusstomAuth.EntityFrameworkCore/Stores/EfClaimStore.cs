using CusstomAuth.Core.Stores;

namespace CusstomAuth.EntityFrameworkCore.Stores;

public class EfClaimStore<DbContext>(DbContext dbContext) : BaseStore<IdentityClaim, DbContext>(dbContext), IClaimStore
        where DbContext : IdentityDbContext
{

}
