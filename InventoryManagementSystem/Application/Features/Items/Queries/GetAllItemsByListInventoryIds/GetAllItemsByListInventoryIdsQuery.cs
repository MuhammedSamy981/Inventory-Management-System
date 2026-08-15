using MediatR;
using InventoryManagementSystem.Application.Dtos;

namespace InventoryManagementSystem.Application.Items.Queries
{
public record GetAllItemsByListInventoryIdsQuery(List<int> InventoryIds,string ApplicationUserId) : IRequest<List<ItemDetailsForEachInventoryDto>>;
}