using MediatR;
using InventoryManagementSystem.Application.Dtos;

namespace InventoryManagementSystem.Application.Inventories.Queries
{
    public record GetAllInventoriesByApplicationUserIdQuery(string ApplicationUserId, int PageNumber, int PageSize) : IRequest<List<InventoryDto>>;
}