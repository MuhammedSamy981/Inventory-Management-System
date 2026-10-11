using InventoryManagementSystem.Domain.Entities;

namespace InventoryManagementSystem.Domain.Interfaces
{
    public interface IInventoryRepository : IGenericRepository<Inventory>
    {
        Task<List<Inventory>> GetAllByApplicationUserIdAsync(string applicationUserId, int pageNumber, int pageSize, CancellationToken cancellationToken = default);
        Task<Object?> GetAllDetailsByIdAsync(int id, string applicationUserId, DateTime? fromDate, DateTime? toDate, CancellationToken cancellationToken = default);
        Task<List<Object>> GetAllItemsMovementByListInventoryIdsAsync(List<int> inventoryIds, string applicationUserId, DateTime? fromDate, DateTime? toDate, CancellationToken cancellationToken = default);
        Task<bool> ExistsAsync(int? id, CancellationToken cancellationToken = default);
        Task<List<int>> GetAllIdsAsync(string applicationUserId, CancellationToken cancellationToken = default);
        Task<int> GetManagerIdByInventoryIdAsync(int id, string applicationUserId, CancellationToken cancellationToken = default);
    }
}