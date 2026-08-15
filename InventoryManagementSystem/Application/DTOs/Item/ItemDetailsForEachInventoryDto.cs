
namespace InventoryManagementSystem.Application.Dtos
{

    public class ItemDetailsForEachInventoryDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
        public string? Size { get; set; }
        public string? InventoryName { get; set; }
    }
}