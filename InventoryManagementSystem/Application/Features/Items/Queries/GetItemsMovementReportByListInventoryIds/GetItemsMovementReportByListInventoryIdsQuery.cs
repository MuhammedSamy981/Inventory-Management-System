using MediatR;
using InventoryManagementSystem.Application.Dtos;

namespace InventoryManagementSystem.Application.Items.Queries
{
public record GetItemsMovementReportByListInventoryIdsQuery(List<int>? InventoryIds,string ApplicationUserId,
DateTime? FromDate,
DateTime? ToDate) : IRequest<List<ItemsMovementReportDto>>;
}