using MediatR;

namespace InventoryManagementSystem.Application.Inventories.Commands
{
      public record CreateInventoryCommandResponse(string Message);
      public record CreateInventoryCommand(
      int Id,
      string Name,
      string Address,
      int ManagerId);
      public record CreateInventoryCommandWithApplicationUserId(
        CreateInventoryCommand CreateInventoryCommand,
        string ApplicationUserId
      ) : IRequest<CreateInventoryCommandResponse>;
}
