using AutoMapper;
using MediatR;
using InventoryManagementSystem.Domain.Entities;
using InventoryManagementSystem.Domain.Interfaces;

namespace InventoryManagementSystem.Application.Operations.Commands
{
    public class UpdateOperationCommandHandler : IRequestHandler<UpdateOperationCommandWithApplicationUserId, UpdateOperationCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;


        public UpdateOperationCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)//,  IValidator<Operation> OperationValidator )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<UpdateOperationCommandResponse> Handle(UpdateOperationCommandWithApplicationUserId request, CancellationToken cancellationToken)
        {

            string message = string.Empty;
            var operation = await _unitOfWork.OperationRepository.GetByIdAsync(request.UpdateOperationCommand.Id);
            var inventoryItem = await _unitOfWork.InventoryItemRepository.GetSpecificAsync(request.UpdateOperationCommand.InventoryId, request.UpdateOperationCommand.ItemId);
                                            
            operation!.Date=request.UpdateOperationCommand.Date;
            operation.ProDate=request.UpdateOperationCommand.ProDate;
            operation.ExpiryDate=request.UpdateOperationCommand.ExpiryDate;
            operation.PersonId=request.UpdateOperationCommand.PersonId;

            Console.WriteLine($"\n\n Operation Quantity: {operation.Quantity}, Updated Quantity: {request.UpdateOperationCommand.Quantity} \n\n");
            if (request.UpdateOperationCommand.Type == "Supply Permit")
            {
                if(operation.ItemId != request.UpdateOperationCommand.ItemId || operation.InventoryId != request.UpdateOperationCommand.InventoryId)
                {
                    var oldInventoryItem = await _unitOfWork.InventoryItemRepository.GetSpecificAsync(operation.InventoryId, operation.ItemId);
                    oldInventoryItem!.Quantity -= operation.Quantity;
                    inventoryItem!.Quantity += request.UpdateOperationCommand.Quantity;
                }
                else{
                if (request.UpdateOperationCommand.Quantity < operation!.Quantity)
                {
                    inventoryItem!.Quantity -= (operation.Quantity - request.UpdateOperationCommand.Quantity);
                }
                else
                    if (request.UpdateOperationCommand.Quantity > operation.Quantity)
                    {
                        inventoryItem!.Quantity += (request.UpdateOperationCommand.Quantity - operation.Quantity);
                    }
                  else if (operation.Type == "Exchange Permit")
                {
                    inventoryItem!.Quantity += (request.UpdateOperationCommand.Quantity+operation.Quantity);
                }
                }
            operation.ItemId=request.UpdateOperationCommand.ItemId;
            operation.InventoryId=request.UpdateOperationCommand.InventoryId;
                            operation.Type=request.UpdateOperationCommand.Type;
                        operation.Quantity=request.UpdateOperationCommand.Quantity;

                if (await _unitOfWork.SaveChangesAsync() != 0)
                { message = "Operation updated successfully"; }
                else
                { message = "Failed to update operation"; }
            }
            else
                if (request.UpdateOperationCommand.Type == "Exchange Permit")
                {
                  if(operation.ItemId != request.UpdateOperationCommand.ItemId || operation.InventoryId != request.UpdateOperationCommand.InventoryId)
                {
                    var oldInventoryItem = await _unitOfWork.InventoryItemRepository.GetSpecificAsync(operation.InventoryId, operation.ItemId);
                    oldInventoryItem!.Quantity += operation.Quantity;
                    inventoryItem!.Quantity -= request.UpdateOperationCommand.Quantity;
                }
                else{
                    if (request.UpdateOperationCommand.Quantity < operation!.Quantity)
                    {
                        inventoryItem!.Quantity += (operation.Quantity - request.UpdateOperationCommand.Quantity);
                    }
                    else
                        if (request.UpdateOperationCommand.Quantity > operation.Quantity)
                        {
                            inventoryItem!.Quantity -= (request.UpdateOperationCommand.Quantity - operation.Quantity);
                        }
                          else if (operation.Type == "Supply Permit")
                {
                    inventoryItem!.Quantity -= (request.UpdateOperationCommand.Quantity+operation.Quantity);
                }
                }
                            operation.ItemId=request.UpdateOperationCommand.ItemId;
            operation.InventoryId=request.UpdateOperationCommand.InventoryId;
                        operation.Type=request.UpdateOperationCommand.Type;
            operation.Quantity=request.UpdateOperationCommand.Quantity;
            
                    if (await _unitOfWork.SaveChangesAsync() != 0)
                    { message = "Operation updated successfully"; }
                    else
                    { message = "Failed to update operation"; }

                }
                else
                {
                    message = "Only operations of type Supply Permit or Exchange Permit are allowed to be updated";
                }
            return new UpdateOperationCommandResponse(message);
        }
    }

}