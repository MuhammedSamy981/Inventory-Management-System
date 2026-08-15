using MediatR;
using InventoryManagementSystem.Application.Dtos;

namespace InventoryManagementSystem.Application.Items.Queries
{
public record GetItemReportQuery(int Id,List<int> InventoryIds,string ApplicationUserId,
DateTime? FromDate,
DateTime? ToDate) : IRequest<ItemReportDto?>;
}