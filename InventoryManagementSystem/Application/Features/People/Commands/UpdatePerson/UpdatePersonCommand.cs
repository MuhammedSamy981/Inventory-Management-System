using MediatR;

namespace InventoryManagementSystem.Application.People.Commands
{
public record UpdatePersonCommandResponse(string Message);
public record UpdatePersonCommand(
int Id,
string Name,
string Telephone,
string Fax,
string Phone,
string Email,
string Website,
string Type);

public record UpdatePersonCommandWithApplicationUserId(
UpdatePersonCommand UpdatePersonCommand,
string ApplicationUserId) : IRequest<UpdatePersonCommandResponse>;

}
