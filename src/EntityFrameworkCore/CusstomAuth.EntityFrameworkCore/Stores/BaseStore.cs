using CusstomAuth.Core.Stores;
using Microsoft.EntityFrameworkCore;

namespace CusstomAuth.EntityFrameworkCore.Stores;

public class BaseStore<TEntity, TDbContext>(TDbContext dbContext) : IStore<TEntity>
    where TEntity : class
    where TDbContext : IdentityDbContext
{
    public async Task<TEntity> CreateAsync(TEntity entity)
    {
        await dbContext.Set<TEntity>().AddAsync(entity);
        await dbContext.SaveChangesAsync();
        return entity;
    }

    public async Task<TEntity> DeleteAsync(TEntity entity)
    {
        dbContext.Set<TEntity>().Remove(entity);
        await dbContext.SaveChangesAsync();
        return entity;
    }

    public async Task<IReadOnlyList<TEntity>> GetAllAsync()
    {
        return await dbContext.Set<TEntity>().ToListAsync();
    }

    public async Task<TEntity> GetAsync(object id)
    {
        return await dbContext.Set<TEntity>().FindAsync(id);
    }

    public async Task<TEntity> UpdateAsync(TEntity entity)
    {
        dbContext.Set<TEntity>().Update(entity);
        await dbContext.SaveChangesAsync();
        return entity;
    }
}
