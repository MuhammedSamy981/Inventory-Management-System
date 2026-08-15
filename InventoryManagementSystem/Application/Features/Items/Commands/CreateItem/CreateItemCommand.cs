using MediatR;

namespace InventoryManagementSystem.Application.Items.Commands
{
        public record CreateItemCommandResponse(string Message);
        public record CreateItemCommand(
        int Id,
        string Name,
        string Code,
        string Size
        );
        public record CreateItemCommandWithApplicationUserId(
        CreateItemCommand CreateItemCommand,
        string ApplicationUserId
        ) : IRequest<CreateItemCommandResponse>;

}