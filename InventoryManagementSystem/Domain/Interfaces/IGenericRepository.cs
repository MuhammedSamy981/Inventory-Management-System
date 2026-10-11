namespace InventoryManagementSystem.Domain.Interfaces
{
    public interface IGenericRepository<TEntity>
    where TEntity : class
    {
        Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<TEntity?> GetByIdAsync(int? id, CancellationToken cancellationToken = default);
        Task<TEntity?> GetByIdAsync(string? id, CancellationToken cancellationToken = default);
        Task AddAsync(TEntity entity);
        Task AddRangeAsync(List<TEntity> entities);
        Task UpdateAsync(TEntity entity);
        Task UpdateRangeAsync(List<TEntity> entities);
        Task DeleteAsync(TEntity entity);
    }
}