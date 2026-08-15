using MediatR;
using InventoryManagementSystem.Application.Dtos;

namespace InventoryManagementSystem.Application.Operations.Queries
{
  public record GetOperationByIdQuery(int Id,string ApplicationUserId) : IRequest<OperationDto?>;
}