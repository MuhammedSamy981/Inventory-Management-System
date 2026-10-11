using InventoryManagementSystem.Domain.Entities;
using InventoryManagementSystem.Domain.Filters;

namespace InventoryManagementSystem.Domain.Interfaces
{
     public interface IOperationRepository : IGenericRepository<Operation>
     {
          Task<List<Operation>> GetAllByApplicationUserIdAsync(string applicationUserId, int pageNumber, int pageSize, CancellationToken cancellationToken = default);
          Task<List<Operation>> GetAllByTypeAsync(string type, string applicationUserId,int pageNumber, int pageSize, CancellationToken cancellationToken = default);
          Task<int> GetAllQuantitiesAsync(OperationFilters filters, string applicationUserId, CancellationToken cancellationToken = default);
          Task<List<int>> GetAllSupplierIdsAsync(int inventoryId, int itemId, string applicationUserId, CancellationToken cancellationToken = default);
          Task<bool> ExistsAsync(int? id, CancellationToken cancellationToken = default);
     }
}
