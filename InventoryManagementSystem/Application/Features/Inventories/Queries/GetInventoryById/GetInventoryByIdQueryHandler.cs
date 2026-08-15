using AutoMapper;
using MediatR;
using InventoryManagementSystem.Application.Dtos;
using InventoryManagementSystem.Domain.Interfaces;

namespace InventoryManagementSystem.Application.Inventories.Queries
{
public class GetInventoryByIdQueryHandler : IRequestHandler<GetInventoryByIdQuery, InventoryDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    

    public GetInventoryByIdQueryHandler(IUnitOfWork unitOfWork,IMapper mapper)
        {
           _unitOfWork = unitOfWork;
           _mapper = mapper;
        }

    public async Task<InventoryDto?> Handle(GetInventoryByIdQuery request, CancellationToken cancellationToken)
    {
        var inventory =await _unitOfWork.InventoryRepository.GetByIdAsync(request.Id);
        if (inventory != null && request.ApplicationUserId==inventory.ApplicationUserId)
        {
            return _mapper.Map<InventoryDto>(inventory);
        }
        return null;
    }
}
}