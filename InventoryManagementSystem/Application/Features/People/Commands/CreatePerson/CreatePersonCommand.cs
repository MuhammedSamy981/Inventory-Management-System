using MediatR;

namespace InventoryManagementSystem.Application.People.Commands
{
      public record CreatePersonCommandResponse(string Message);
      public record CreatePersonCommand(
      int Id,
      string Name,
      string Telephone,
      string Fax,
      string Phone,
      string Email,
      string Website,
      string Type);
      
      public record CreatePersonCommandWithApplicationUserId(
      CreatePersonCommand CreatePersonCommand,
      string ApplicationUserId
      ) : IRequest<CreatePersonCommandResponse>;

}

