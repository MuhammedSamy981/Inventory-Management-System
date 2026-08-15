using MediatR;
using InventoryManagementSystem.Application.Dtos;

namespace InventoryManagementSystem.Application.Items.Queries
{
public record GetStoredItemsReportByInventoryIdQuery(
    int InventoryId,string ApplicationUserId,
    int DaysCount, int MonthsCount, int YearsCount) : IRequest<List<StoredItemDetailsDto>>;
}