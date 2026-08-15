
namespace InventoryManagementSystem.Application.Dtos{

    public class StoredItemDetailsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }=String.Empty;
        public string Size { get; set; }=String.Empty;
        public int InventoryId { get; set; }
        public int? Quantity { get; set; }
        public DateTime? StorageDate { get; set; }
    }
}