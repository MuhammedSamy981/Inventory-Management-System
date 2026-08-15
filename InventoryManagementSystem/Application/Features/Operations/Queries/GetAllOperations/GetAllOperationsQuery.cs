using MediatR;
using InventoryManagementSystem.Application.Dtos;

namespace InventoryManagementSystem.Application.Operations.Queries
{
  public record GetAllOperationsQuery : IRequest<List<OperationDto>>;
}