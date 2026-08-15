namespace InventoryManagementSystem.Application.Dtos
{
public class ItemReportDto
{
 public int Id { get; set; }
 public string? Name { get; set; }
 public string? Size { get; set; }
 public string? Code { get; set; }
 public List<InventoryDetailsDto>? Inventories { get; set; }
}

}