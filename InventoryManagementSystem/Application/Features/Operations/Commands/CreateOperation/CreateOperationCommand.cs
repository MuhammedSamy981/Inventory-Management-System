using System.Net;
using MediatR;

namespace InventoryManagementSystem.Application.Operations.Commands
{
    public record CreateOperationCommandResponse(string Message);
    public record CreateOperationCommand(
    int Id,
    DateTime Date,
    int Quantity,
    string Type,
    DateTime? ProDate,
    DateTime? ExpiryDate,
    int ItemId,
    int PersonId,
    int InventoryId);

    public record CreateOperationCommandWithApplicationUserId(
    CreateOperationCommand CreateOperationCommand,
    string ApplicationUserId
    ) : IRequest<CreateOperationCommandResponse>;

}
