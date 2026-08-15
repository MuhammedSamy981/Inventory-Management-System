using AutoMapper;
using MediatR;
using InventoryManagementSystem.Domain.Interfaces;

namespace InventoryManagementSystem.Application.Inventories.Commands
{
    public class DeleteInventoryCommandHandler : IRequestHandler<DeleteInventoryCommand, DeleteInventoryCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteInventoryCommandHandler(IUnitOfWork unitOfWork)//,  IValidator<Inventory> InventoryValidator )
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<DeleteInventoryCommandResponse> Handle(DeleteInventoryCommand request, CancellationToken cancellationToken)
        {
            var inventory = await _unitOfWork.InventoryRepository.GetByIdAsync(request.Id);
            if (inventory == null)
            {
                return new DeleteInventoryCommandResponse("The inventory you are trying to delete no longer exists");
            }
            if (request.ApplicationUserId != inventory.ApplicationUserId)
            {
                return new DeleteInventoryCommandResponse("You are not authorized to delete this inventory");
            }

            await _unitOfWork.InventoryRepository.DeleteAsync(inventory);
            return await _unitOfWork.SaveChangesAsync() != 0 ?
            new DeleteInventoryCommandResponse("Inventory deleted successfully")
            : new DeleteInventoryCommandResponse("Failed to delete inventory");
        }
    }

}