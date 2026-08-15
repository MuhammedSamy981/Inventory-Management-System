using MediatR;
using InventoryManagementSystem.Application.Dtos;

namespace InventoryManagementSystem.Application.Inventories.Queries
{
  public record GetInventoryByIdQuery(int Id,string ApplicationUserId) : IRequest<InventoryDto?>;
}