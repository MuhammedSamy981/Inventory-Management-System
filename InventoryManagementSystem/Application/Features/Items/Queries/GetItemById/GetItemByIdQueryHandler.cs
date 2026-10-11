using AutoMapper;
using MediatR;
using InventoryManagementSystem.Application.Dtos;
using InventoryManagementSystem.Domain.Interfaces;

namespace InventoryManagementSystem.Application.Items.Queries
{
public class GetItemByIdQueryHandler : IRequestHandler<GetItemByIdQuery, ItemDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    

    public GetItemByIdQueryHandler(IUnitOfWork unitOfWork,IMapper mapper)
        {
            
        _unitOfWork = unitOfWork;
           _mapper = mapper;
           }

    public async Task<ItemDto?> Handle(GetItemByIdQuery request, CancellationToken cancellationToken)
    {
        var item =await _unitOfWork.ItemRepository.GetByIdAsync(request.Id,cancellationToken);
        if (item != null && item.ApplicationUserId==request.ApplicationUserId)
        {
             return _mapper.Map<ItemDto>(item);
        }
        return null;
    }
}
}