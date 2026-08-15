using InventoryManagementSystem.Domain.Entities;

namespace InventoryManagementSystem.Domain.Interfaces
{
    public interface IPersonRepository : IGenericRepository<Person>
    {
        Task<List<Person>> GetAllByApplicationUserIdAsync(string applicationUserId, int pageNumber, int pageSize, CancellationToken cancellationToken = default);
        Task<List<Person>> GetAllByTypeAsync(string type, string applicationUserId, CancellationToken cancellationToken = default);
        Task<bool> ExistsAsync(int? id, CancellationToken cancellationToken = default);
    }
}