using AutoMapper;
using MediatR;
using InventoryManagementSystem.Domain.Entities;
using InventoryManagementSystem.Domain.Interfaces;

namespace InventoryManagementSystem.Application.Operations.Commands
{
    public class CreateOperationCommandHandler : IRequestHandler<CreateOperationCommandWithApplicationUserId, CreateOperationCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateOperationCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<CreateOperationCommandResponse> Handle(CreateOperationCommandWithApplicationUserId request, CancellationToken cancellationToken)
        {
            string message = string.Empty;
            var inventoryItem = await _unitOfWork.InventoryItemRepository.GetSpecificAsync(request.CreateOperationCommand.InventoryId, request.CreateOperationCommand.ItemId);
            var operation = _mapper.Map<Operation>(request);
            await _unitOfWork.OperationRepository.AddAsync(operation);
            if (request.CreateOperationCommand.Type == "Supply Permit")
            {
                if (inventoryItem == null)
                {
                    await _unitOfWork.InventoryItemRepository.AddAsync(new InventoryItem
                    {
                        InventoryId = request.CreateOperationCommand.InventoryId,
                        ItemId = request.CreateOperationCommand.ItemId,
                        Quantity = request.CreateOperationCommand.Quantity
                    });
                }
                else
                {
                    inventoryItem.Quantity += request.CreateOperationCommand.Quantity;
                }
                if (await _unitOfWork.SaveChangesAsync() != 0)
                { message = "Operation created successfully"; }
                else
                { message = "Failed to create operation"; }

            }
            else
                if (request.CreateOperationCommand.Type == "Exchange Permit")
                {
                    if (inventoryItem != null)
                    {
                        inventoryItem.Quantity -= request.CreateOperationCommand.Quantity;
                        if (await _unitOfWork.SaveChangesAsync() != 0)
                        { message = "Operation created successfully"; }
                        else
                        { message = "Failed to create operation"; }
                    }
                    else
                    {
                        message = "there is no Items in this store";
                    }
                }
                else
                {
                    message = "Only operations of type Supply Permit or Exchange Permit are allowed to be created";
                }
            return new CreateOperationCommandResponse(message);
        }
    }

}