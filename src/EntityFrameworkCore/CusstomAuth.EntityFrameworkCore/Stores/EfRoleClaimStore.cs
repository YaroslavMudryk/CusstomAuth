using CusstomAuth.Core.Stores;

namespace CusstomAuth.EntityFrameworkCore.Stores;

public class EfRoleClaimStore<DbContext>(DbContext dbContext) : BaseStore<IdentityRoleClaim, DbContext>(dbContext), IRoleClaimStore
        where DbContext : IdentityDbContext
{

}
