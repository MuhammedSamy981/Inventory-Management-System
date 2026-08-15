namespace InventoryManagementSystem.Infrastructure.Services.Payment
{
    public class PayPalOptions
    {
        public string? ClientId { get; set; }
        public string? Secret { get; set; }
        public string BaseUrl { get; set; } = "https://api-m.sandbox.paypal.com";
    }

}