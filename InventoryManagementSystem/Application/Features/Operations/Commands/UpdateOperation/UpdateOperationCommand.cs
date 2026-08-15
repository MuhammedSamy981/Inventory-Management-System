using MediatR;

namespace InventoryManagementSystem.Application.Operations.Commands
{
    public record UpdateOperationCommandResponse(string Message);
    public record UpdateOperationCommand(
    int Id,
    DateTime Date,
    int Quantity,
    string Type,
    DateTime ProDate,
    DateTime ExpiryDate,
    int ItemId,
    int PersonId,
    int InventoryId);

    public record UpdateOperationCommandWithApplicationUserId(
    UpdateOperationCommand UpdateOperationCommand,
    string ApplicationUserId
    ) : IRequest<UpdateOperationCommandResponse>;

}
