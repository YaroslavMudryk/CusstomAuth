using CusstomAuth.Core.Db;
using CusstomAuth.Core.Db.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CusstomAuth.Core.Managers;

public interface IUserManager
{
    Task<bool> IsExistUsersAsync(Expression<Func<AuthUser, bool>> expression);
}

public class UserManager(AuthDbContext dbContext) : IUserManager
{
    public async Task<bool> IsExistUsersAsync(Expression<Func<AuthUser, bool>> expression)
    {
        return await dbContext.Users.AnyAsync(expression);
    }
}
