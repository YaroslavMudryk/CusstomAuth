namespace CusstomAuth.Core.Stores;

public interface IStore<TEntity> where TEntity : class
{
    Task<TEntity> CreateAsync(TEntity entity);
    Task<TEntity> UpdateAsync(TEntity entity);
    Task<TEntity> DeleteAsync(TEntity entity);
    Task<TEntity> GetAsync(object id);
    Task<IReadOnlyList<TEntity>> GetAllAsync();
}
