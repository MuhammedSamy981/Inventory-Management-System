using MediatR;
using InventoryManagementSystem.Application.Dtos;

namespace InventoryManagementSystem.Application.Operations.Queries
{
  public record GetAllOperationsByApplicationUserIdQuery(string ApplicationUserId, int PageNumber, int PageSize) : IRequest<List<OperationDto>>;
}