using AutoMapper;
using MediatR;
using InventoryManagementSystem.Application.Dtos;
using InventoryManagementSystem.Domain.Interfaces;
using InventoryManagementSystem.Domain.Filters;

namespace InventoryManagementSystem.Application.Items.Queries
{
public class GetAllItemsByListInventoryIdsQueryHandler : IRequestHandler<GetAllItemsByListInventoryIdsQuery, List<ItemDetailsForEachInventoryDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    

    public GetAllItemsByListInventoryIdsQueryHandler(IUnitOfWork unitOfWork,IMapper mapper)
        {
        _unitOfWork = unitOfWork;
           _mapper = mapper;
           }

    public async Task<List<ItemDetailsForEachInventoryDto>> Handle(GetAllItemsByListInventoryIdsQuery request, CancellationToken cancellationToken)
    {
            var items = await _unitOfWork.InventoryItemRepository.GetAllByListInventoryIdsAsync(request.InventoryIds,request.ApplicationUserId);
        if (items.Count() != 0 && request.InventoryIds.Count() != 0)
        {      
        if (request.InventoryIds.Count() == 1)
        {   
             return items.Select(ii=> new ItemDetailsForEachInventoryDto
                 {
            Id = (int)ii.GetType().GetProperty("ItemId")!.GetValue(ii)!,
            Name = (string)ii.GetType().GetProperty("ItemName")!.GetValue(ii)!,
            Code= (string)ii.GetType().GetProperty("ItemCode")!.GetValue(ii)!,
            Size = (string)ii.GetType().GetProperty("ItemSize")!.GetValue(ii)!
        }).ToList();
        }

        return items.Select(ii=> new ItemDetailsForEachInventoryDto
        {
            Id = (int)ii.GetType().GetProperty("ItemId")!.GetValue(ii)!,
            Name = (string)ii.GetType().GetProperty("ItemName")!.GetValue(ii)!,
            Code= (string)ii.GetType().GetProperty("ItemCode")!.GetValue(ii)!,
            Size = (string)ii.GetType().GetProperty("ItemSize")!.GetValue(ii)!,
            InventoryName = (string)ii.GetType().GetProperty("InventoryName")!.GetValue(ii)!
        }).ToList();
        }
        return new();
    }

    }
}