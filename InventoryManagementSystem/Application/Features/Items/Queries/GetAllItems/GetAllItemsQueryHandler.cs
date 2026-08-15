using AutoMapper;
using MediatR;
using InventoryManagementSystem.Application.Dtos;
using InventoryManagementSystem.Domain.Interfaces;

namespace InventoryManagementSystem.Application.Items.Queries
{
public class GetAllItemsQueryHandler : IRequestHandler<GetAllItemsQuery, List<ItemDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    

    public GetAllItemsQueryHandler(IUnitOfWork unitOfWork,IMapper mapper)
        {
            
        _unitOfWork = unitOfWork;
           _mapper = mapper;
           }

    public async Task<List<ItemDto>> Handle(GetAllItemsQuery request, CancellationToken cancellationToken)
    {
        var items =await _unitOfWork.ItemRepository.GetAllAsync(cancellationToken);

           return _mapper.Map<List<ItemDto>>(items);
    }
}
}