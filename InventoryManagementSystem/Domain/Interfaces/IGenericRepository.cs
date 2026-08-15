namespace InventoryManagementSystem.Domain.Interfaces
{
    public interface IGenericRepository<TEntity>
    where TEntity : class
    {
        Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<TEntity?> GetByIdAsync(int? id);
        Task<TEntity?> GetByIdAsync(string? id);
        Task AddAsync(TEntity entity);
        Task AddRangeAsync(List<TEntity> entities);
        Task UpdateAsync(TEntity entity);
        Task UpdateRangeAsync(List<TEntity> entities);
        Task DeleteAsync(TEntity entity);
    }
}