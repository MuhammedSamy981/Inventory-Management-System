using MediatR;

namespace InventoryManagementSystem.Application.Operations.Commands
{
    public record CreateTransferOperationCommandResponse(string Message);
    public record CreateTransferOperationCommand(
    int Id,
    int FromInventoryId,
    int ToInventoryId,
    int ItemId,
    int Quantity//,int SupplierId
    );
    public record CreateTransferOperationCommandWithApplicationUserId(
    CreateTransferOperationCommand CreateTransferOperationCommand,
    string ApplicationUserId
    ) : IRequest<CreateTransferOperationCommandResponse>;

}
