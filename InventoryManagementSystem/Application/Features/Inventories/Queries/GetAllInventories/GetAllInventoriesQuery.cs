using MediatR;
using InventoryManagementSystem.Application.Dtos;

namespace InventoryManagementSystem.Application.Inventories.Queries
{
  public record GetAllInventoriesQuery : IRequest<List<InventoryDto>>;
}