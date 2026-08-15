namespace InventoryManagementSystem.Application.Dtos
{
public class ItemsMovementReportDto
{
public int InventoryId { get; set; }
public string? InventoryName { get; set; }
public string? InventoryAddress { get; set; }
public List<MovedItemDetailsDto>? Items { get; set; }
}

}