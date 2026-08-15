using MediatR;
using InventoryManagementSystem.Application.Dtos;

namespace InventoryManagementSystem.Application.Items.Queries
{
public record GetExpiredItemsReportByInventoryIdQuery(
    int InventoryId,string ApplicationUserId,
    int DaysCount, int MonthsCount, int YearsCount) : IRequest<List<ExpiredItemDetailsDto>>;
}