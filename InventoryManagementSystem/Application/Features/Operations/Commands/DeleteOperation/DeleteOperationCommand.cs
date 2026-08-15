using MediatR;

namespace InventoryManagementSystem.Application.Operations.Commands
{
  public record DeleteOperationCommandResponse(string Message);
  public record DeleteOperationCommand(int Id, string ApplicationUserId) : IRequest<DeleteOperationCommandResponse>;
}
