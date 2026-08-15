using InventoryManagementSystem.Domain.Entities;

namespace InventoryManagementSystem.Domain.Interfaces
{
    public interface IPaymentRepository : IGenericRepository<Payment>
    {
        Task<bool> ExistsAsync(string applicationUserId, CancellationToken cancellationToken = default);
    }
}