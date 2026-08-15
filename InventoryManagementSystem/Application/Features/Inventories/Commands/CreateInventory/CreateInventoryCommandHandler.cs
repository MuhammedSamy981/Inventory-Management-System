using AutoMapper;
using MediatR;
using InventoryManagementSystem.Domain.Entities;
using InventoryManagementSystem.Domain.Interfaces;

namespace InventoryManagementSystem.Application.Inventories.Commands
{
    public class CreateInventoryCommandHandler : IRequestHandler<CreateInventoryCommandWithApplicationUserId, CreateInventoryCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;


        public CreateInventoryCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {

            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<CreateInventoryCommandResponse> Handle(CreateInventoryCommandWithApplicationUserId request, CancellationToken cancellationToken)
        {
            var inventory = _mapper.Map<Inventory>(request);
            await _unitOfWork.InventoryRepository.AddAsync(inventory);
            return await _unitOfWork.SaveChangesAsync() != 0 ?
            new CreateInventoryCommandResponse("Inventory created successfully")
            : new CreateInventoryCommandResponse("Failed to create inventory");
        }
    }

}