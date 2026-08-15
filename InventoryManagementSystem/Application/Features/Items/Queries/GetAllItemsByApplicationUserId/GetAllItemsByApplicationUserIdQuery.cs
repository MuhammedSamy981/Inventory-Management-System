using MediatR;
using InventoryManagementSystem.Application.Dtos;

namespace InventoryManagementSystem.Application.Items.Queries
{
public record GetAllItemsByApplicationUserIdQuery(string ApplicationUserId, int PageNumber, int PageSize) : IRequest<List<ItemDto>>;
}