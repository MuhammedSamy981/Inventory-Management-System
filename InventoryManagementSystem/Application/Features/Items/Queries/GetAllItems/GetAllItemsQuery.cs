using MediatR;
using InventoryManagementSystem.Application.Dtos;

namespace InventoryManagementSystem.Application.Items.Queries
{
public record GetAllItemsQuery : IRequest<List<ItemDto>>;
}