using MediatR;
using InventoryManagementSystem.Application.Dtos;

namespace InventoryManagementSystem.Application.Operations.Queries
{
  public record GetAllOperationsByTypeQuery(string Type,string ApplicationUserId) : IRequest<List<OperationDto>>;
}