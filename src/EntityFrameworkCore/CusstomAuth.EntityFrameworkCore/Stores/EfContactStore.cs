using CusstomAuth.Core.Stores;

namespace CusstomAuth.EntityFrameworkCore.Stores;

public class EfContactStore<DbContext>(DbContext dbContext) : BaseStore<IdentityContact, DbContext>(dbContext), IContactStore
        where DbContext : IdentityDbContext
{

}
