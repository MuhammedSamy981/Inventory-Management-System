using AutoMapper;
using MediatR;
using InventoryManagementSystem.Application.Dtos;
using InventoryManagementSystem.Domain.Interfaces;

namespace InventoryManagementSystem.Application.Items.Queries
{
    public class GetAllItemsByApplicationUserIdQueryHandler : IRequestHandler<GetAllItemsByApplicationUserIdQuery, List<ItemDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;


        public GetAllItemsByApplicationUserIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {

            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<ItemDto>> Handle(GetAllItemsByApplicationUserIdQuery request, CancellationToken cancellationToken)
        {
            var items = await _unitOfWork.ItemRepository.GetAllByApplicationUserIdAsync(request.ApplicationUserId, request.PageNumber, request.PageSize, cancellationToken);

            return _mapper.Map<List<ItemDto>>(items);
        }
    }
}