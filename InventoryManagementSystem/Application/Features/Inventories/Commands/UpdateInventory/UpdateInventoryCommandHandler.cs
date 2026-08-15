using AutoMapper;
using MediatR;
using InventoryManagementSystem.Domain.Entities;
using InventoryManagementSystem.Domain.Interfaces;

namespace InventoryManagementSystem.Application.Inventories.Commands
{
    public class UpdateInventoryCommandHandler : IRequestHandler<UpdateInventoryCommandWithApplicationUserId, UpdateInventoryCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateInventoryCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)//,  IValidator<Inventory> InventoryValidator )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<UpdateInventoryCommandResponse> Handle(UpdateInventoryCommandWithApplicationUserId request, CancellationToken cancellationToken)
        {
            var inventory = _mapper.Map<Inventory>(request);
            await _unitOfWork.InventoryRepository.UpdateAsync(inventory);
            return await _unitOfWork.SaveChangesAsync() != 0 ?
            new UpdateInventoryCommandResponse("Inventory updated successfully")
            : new UpdateInventoryCommandResponse("Failed to update inventory");
        }
    }

}