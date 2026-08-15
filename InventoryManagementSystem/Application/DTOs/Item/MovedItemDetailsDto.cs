namespace InventoryManagementSystem.Application.Dtos
{
    public class MovedItemDetailsDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? MoveName { get; set; }
        public DateTime? MoveDate { get; set; }
        public int? Quantity { get; set; }
    }

}