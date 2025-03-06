using CusstomAuth.Core.Stores;

namespace CusstomAuth.EntityFrameworkCore.Stores;

public class EfBlockStore<DbContext>(DbContext dbContext) : BaseStore<IdentityBlock, DbContext>(dbContext), IBlockStore
        where DbContext : IdentityDbContext
{

}
