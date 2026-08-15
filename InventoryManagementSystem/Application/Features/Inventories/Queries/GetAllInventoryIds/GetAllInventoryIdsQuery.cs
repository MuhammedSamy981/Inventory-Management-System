using MediatR;

namespace InventoryManagementSystem.Application.Inventories.Queries
{
public record GetAllInventoryIdsQuery(string ApplicationUserId) : IRequest<List<int>>;
}