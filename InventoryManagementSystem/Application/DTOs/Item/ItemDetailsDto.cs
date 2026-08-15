
namespace InventoryManagementSystem.Application.Dtos
{

    public class ItemDetailsDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = String.Empty;
        public string Size { get; set; } = String.Empty;
        public int? Quantity { get; set; }
    }

}
