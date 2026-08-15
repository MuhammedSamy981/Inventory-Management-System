
namespace InventoryManagementSystem.Application.Dtos
{

    public class ExpiredItemDetailsDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = String.Empty;
        public string Size { get; set; } = String.Empty;
        public int? Quantity { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public int InventoryId { get; set; }
    }
}