using MediatR;

namespace InventoryManagementSystem.Application.Items.Commands
{
  public record UpdateItemCommandResponse(string Message);
  public record UpdateItemCommand(
  int Id,
  string Name,
  string Code,
  string Size
  );
  public record UpdateItemCommandWithApplicationUserId(
  UpdateItemCommand UpdateItemCommand,
  string ApplicationUserId
  ) : IRequest<UpdateItemCommandResponse>;
}
