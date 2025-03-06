using CusstomAuth.Core.Stores;

namespace CusstomAuth.EntityFrameworkCore.Stores;

public class EfLoginAttemptStore<DbContext>(DbContext dbContext) : BaseStore<IdentityLoginAttempt, DbContext>(dbContext), ILoginAttemptStore
        where DbContext : IdentityDbContext
{

}
