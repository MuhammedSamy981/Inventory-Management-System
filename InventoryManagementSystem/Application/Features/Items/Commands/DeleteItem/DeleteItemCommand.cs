using MediatR;

namespace InventoryManagementSystem.Application.Items.Commands
{
  public record DeleteItemCommandResponse(string Message);
  public record DeleteItemCommand(int Id,string ApplicationUserId) : IRequest<DeleteItemCommandResponse>;
}
