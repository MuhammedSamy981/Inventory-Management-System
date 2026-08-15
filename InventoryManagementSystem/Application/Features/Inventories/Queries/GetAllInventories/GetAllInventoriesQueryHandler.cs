using AutoMapper;
using MediatR;
using InventoryManagementSystem.Application.Dtos;
using InventoryManagementSystem.Domain.Interfaces;

namespace InventoryManagementSystem.Application.Inventories.Queries
{
    public class GetAllInventoriesQueryHandler : IRequestHandler<GetAllInventoriesQuery, List<InventoryDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllInventoriesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<InventoryDto>> Handle(GetAllInventoriesQuery request, CancellationToken cancellationToken)
        {
            var inventories = await _unitOfWork.InventoryRepository.GetAllAsync(cancellationToken);
            return _mapper.Map<List<InventoryDto>>(inventories);
        }
    }
}