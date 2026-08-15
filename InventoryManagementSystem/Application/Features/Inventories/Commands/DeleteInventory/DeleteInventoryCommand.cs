using MediatR;

namespace InventoryManagementSystem.Application.Inventories.Commands
{
  public record DeleteInventoryCommandResponse(string Message);
  public record DeleteInventoryCommand(int Id, string ApplicationUserId) : IRequest<DeleteInventoryCommandResponse>;
}
