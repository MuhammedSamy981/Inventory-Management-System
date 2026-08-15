using MediatR;

namespace InventoryManagementSystem.Application.People.Commands
{
   public record DeletePersonCommandResponse(string Message);
   public record DeletePersonCommand(int Id,string ApplicationUserId) : IRequest<DeletePersonCommandResponse>;
}
