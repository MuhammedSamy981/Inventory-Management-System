
namespace InventoryManagementSystem.Domain.Filters
{
  public class OperationFilters
  {
    public int InventoryId { get; set; }
    public int ItemId { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
  }
}