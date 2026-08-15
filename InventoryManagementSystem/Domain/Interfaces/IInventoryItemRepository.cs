using InventoryManagementSystem.Domain.Entities;

namespace InventoryManagementSystem.Domain.Interfaces
{
    public interface IInventoryItemRepository : IGenericRepository<InventoryItem>
    {
        //Task<List<int>> GetAllItemIdsByInventoryIdAsync(int inventoryId, CancellationToken cancellationToken = default);
        Task<List<Object>> GetAllByListInventoryIdsAndItemIdAsync(List<int> inventoryIds, int itemId, string applicationUserId, CancellationToken cancellationToken = default);
        Task<List<Object>> GetAllByListInventoryIdsAsync(List<int> inventoryIds, string applicationUserId, CancellationToken cancellationToken = default);
        Task<List<Object>> GetAllbyInventoryIdAndExpiryDateAsync(int inventoryId, string applicationUserId, DateTime? expiryDate, CancellationToken cancellationToken = default);
        Task<List<Object>> GetAllbyinventoryIdAndStorageDateAsync(int inventoryId, string applicationUserId, DateTime? storageDate, CancellationToken cancellationToken = default);
        Task<InventoryItem?> GetSpecificAsync(int inventoryId, int itemId);
        Task<bool> ExistsAsync(int? iventoryId, int? itemId, CancellationToken cancellationToken = default);

    }
}

