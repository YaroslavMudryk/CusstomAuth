using CusstomAuth.Core.Stores;

namespace CusstomAuth.EntityFrameworkCore.Stores;

public class EfQrStore<DbContext>(DbContext dbContext) : BaseStore<IdentityQr, DbContext>(dbContext), IQrStore
        where DbContext : IdentityDbContext
{

}
