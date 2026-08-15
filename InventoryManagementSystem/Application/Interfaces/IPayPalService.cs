namespace InventoryManagementSystem.Application.Interfaces
{
    public interface IPayPalService
    {
        Task<string> CreateOrderAsync(decimal amount, string currency);
        Task<bool> CaptureOrderAsync(string orderId);
    }

}