using AutoMapper;
using MediatR;
using InventoryManagementSystem.Domain.Entities;
using InventoryManagementSystem.Domain.Interfaces;

namespace InventoryManagementSystem.Application.Operations.Commands
{
    public class DeleteOperationCommandHandler : IRequestHandler<DeleteOperationCommand, DeleteOperationCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;


        public DeleteOperationCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<DeleteOperationCommandResponse> Handle(DeleteOperationCommand request, CancellationToken cancellationToken)
        {
            string message = string.Empty;

            var operation = await _unitOfWork.OperationRepository.GetByIdAsync(request.Id);
            if (operation == null)
            {
                return new DeleteOperationCommandResponse("The operation you are trying to delete no longer exists");
            }
            if (operation.ApplicationUserId != request.ApplicationUserId)
            {
                return new DeleteOperationCommandResponse("You are not authorized to delete this operation");
            }
            var inventoryItem = await _unitOfWork.InventoryItemRepository.GetSpecificAsync(operation.InventoryId, operation.ItemId);
            await _unitOfWork.OperationRepository.DeleteAsync(operation);
            //return await  _unitOfWork.SaveChangesAsync() != 0;

            if (operation.Type == "Supply Permit")
            {
                if (inventoryItem != null)
                {
                    inventoryItem.Quantity -= operation.Quantity;
                if (await _unitOfWork.SaveChangesAsync() != 0)
                { message = "Operation deleted successfully"; }
                else
                { message = "Failed to delete operation"; }
                
                }
            }
            else
                if (operation.Type == "Exchange Permit")
                {
                    if (inventoryItem != null)
                    {
                        inventoryItem.Quantity += operation.Quantity;
                        if (await _unitOfWork.SaveChangesAsync() != 0)
                        { message = "Operation deleted successfully"; }
                        else
                        { message = "Failed to delete operation"; }
                    }
                }
                else
                {
                    message = "Only operations of type Supply Permit or Exchange Permit are allowed to be deleted";
                }

            return new DeleteOperationCommandResponse(message);
        }
    }

}