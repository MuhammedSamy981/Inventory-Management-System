using InventoryManagementSystem.Domain.Entities;

namespace InventoryManagementSystem.Domain.Interfaces
{
    public interface IItemRepository : IGenericRepository<Item>
    {
        Task<List<Item>> GetAllByApplicationUserIdAsync(string applicationUserId, int pageNumber, int pageSize, CancellationToken cancellationToken = default);
        Task<Object?> GetAllDetailsByIdAndListInventoryIdsAsync(int id, List<int> inventoryIds, string applicationUserId, DateTime? fromDate, DateTime? toDate);
        Task<bool> ExistsAsync(int? id, CancellationToken cancellationToken = default);
    }
}




