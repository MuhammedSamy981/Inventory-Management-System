using AutoMapper;
using MediatR;
using InventoryManagementSystem.Application.Dtos;
using InventoryManagementSystem.Domain.Interfaces;

namespace InventoryManagementSystem.Application.Inventories.Queries
{
    public class GetAllInventoriesByApplicationUserIdQueryHandler : IRequestHandler<GetAllInventoriesByApplicationUserIdQuery, List<InventoryDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllInventoriesByApplicationUserIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<InventoryDto>> Handle(GetAllInventoriesByApplicationUserIdQuery request, CancellationToken cancellationToken)
        {
            var inventories = await _unitOfWork.InventoryRepository.GetAllByApplicationUserIdAsync(request.ApplicationUserId, request.PageNumber, request.PageSize, cancellationToken);
            return _mapper.Map<List<InventoryDto>>(inventories);
        }
    }
}