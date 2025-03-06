using CusstomAuth.Core.Stores;

namespace CusstomAuth.EntityFrameworkCore.Stores;

public class EfUserRoleStore<DbContext>(DbContext dbContext) : BaseStore<IdentityUserRole, DbContext>(dbContext), IUserRoleStore
        where DbContext : IdentityDbContext
{

}
