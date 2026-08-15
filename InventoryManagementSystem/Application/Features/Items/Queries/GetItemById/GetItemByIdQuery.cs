using MediatR;
using InventoryManagementSystem.Application.Dtos;

namespace InventoryManagementSystem.Application.Items.Queries
{
public record GetItemByIdQuery(int Id,string ApplicationUserId) : IRequest<ItemDto?>;
}