using MediatR;

namespace InventoryManagementSystem.Application.Inventories.Commands
{
      public record UpdateInventoryCommandResponse(string Message);
      public record UpdateInventoryCommand(
      int Id,
      string Name,
      string Address,
      int ManagerId);

      public record UpdateInventoryCommandWithApplicationUserId(
      UpdateInventoryCommand UpdateInventoryCommand,
      string ApplicationUserId
      ) : IRequest<UpdateInventoryCommandResponse>;
}
