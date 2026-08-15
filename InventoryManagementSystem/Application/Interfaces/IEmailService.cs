
using InventoryManagementSystem.Domain.Models;

namespace InventoryManagementSystem.Application.Interfaces
{
    public interface IEmailService
    {
        Task<bool> SendAsync(Message message);
    
    }
}