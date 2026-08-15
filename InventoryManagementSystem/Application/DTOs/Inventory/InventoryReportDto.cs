namespace InventoryManagementSystem.Application.Dtos
{
        public class InventoryReportDto
        {

                public int Id { get; set; }
                public string? Name { get; set; }
                public string? Address { get; set; }
                public List<ItemDetailsDto>? Items { get; set; }

        }
}