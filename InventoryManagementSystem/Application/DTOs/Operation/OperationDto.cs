namespace InventoryManagementSystem.Application.Dtos{
public class OperationDto
{  
   public int Id { get; set; }
   public DateTime? Date { get; set; }
   public int? Quantity { get; set; }
   public string? Type { get; set; }
   public DateTime? ProDate { get; set; }
   public DateTime? ExpiryDate { get; set; }
   public int ItemId { get; set; }
   public int PersonId { get; set; }
   public int InventoryId { get; set; }

}
}