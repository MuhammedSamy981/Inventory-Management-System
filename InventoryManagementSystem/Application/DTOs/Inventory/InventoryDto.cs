namespace InventoryManagementSystem.Application.Dtos
{
   public class InventoryDto
   {
      public int Id { get; set; }
      public string? Name { get; set; }
      public string? Address { get; set; }
      public int? ManagerId { get; set; }
   }
}
