using AutoMapper;
using MediatR;
using InventoryManagementSystem.Domain.Entities;
using InventoryManagementSystem.Domain.Interfaces;

namespace InventoryManagementSystem.Application.Operations.Commands
{
    public class CreateTransferOperationCommandHandler : IRequestHandler<CreateTransferOperationCommandWithApplicationUserId, CreateTransferOperationCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;


        public CreateTransferOperationCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)//,  IValidator<Operation> OperationValidator )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<CreateTransferOperationCommandResponse> Handle(CreateTransferOperationCommandWithApplicationUserId request, CancellationToken cancellationToken)
        {

            string message = string.Empty;
            var inventoryItemFrom = await _unitOfWork.InventoryItemRepository.GetSpecificAsync(request.CreateTransferOperationCommand.FromInventoryId, request.CreateTransferOperationCommand.ItemId);
            if (inventoryItemFrom != null)
            {
                if (request.CreateTransferOperationCommand.Quantity <= inventoryItemFrom.Quantity)
                {
                    var inventoryItemTo = await _unitOfWork.InventoryItemRepository.GetSpecificAsync(request.CreateTransferOperationCommand.ToInventoryId, request.CreateTransferOperationCommand.ItemId);
                    var operation = _mapper.Map<Operation>(request);
                    await _unitOfWork.OperationRepository.AddAsync(operation);
                    if (inventoryItemTo != null)
                    {
                        inventoryItemFrom.Quantity -= request.CreateTransferOperationCommand.Quantity;

                        inventoryItemTo.Quantity += request.CreateTransferOperationCommand.Quantity;


                    }
                    else
                    {
                        inventoryItemFrom.Quantity -= request.CreateTransferOperationCommand.Quantity;

                        var newInventoryItemTo = new InventoryItem
                        {
                            InventoryId = request.CreateTransferOperationCommand.ToInventoryId,
                            ItemId = request.CreateTransferOperationCommand.ItemId,
                            Quantity = request.CreateTransferOperationCommand.Quantity
                        };
                        await _unitOfWork.InventoryItemRepository.AddAsync(newInventoryItemTo);

                    }

                    if (await _unitOfWork.SaveChangesAsync() != 0)
                    { message = "CreateTransfer was successful"; }
                    else
                    { message = "CreateTransfer was failed"; }

                }
                else
                {
                    message = "You insert quantity more than It's found";
                }

            }

            return new CreateTransferOperationCommandResponse(message);
        }
    }

}