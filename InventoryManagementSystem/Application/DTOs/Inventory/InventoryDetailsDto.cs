
namespace InventoryManagementSystem.Application.Dtos
{
    public class InventoryDetailsDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = String.Empty;
        public string Address { get; set; } = String.Empty;
        public int Quantity { get; set; }
    }

}
