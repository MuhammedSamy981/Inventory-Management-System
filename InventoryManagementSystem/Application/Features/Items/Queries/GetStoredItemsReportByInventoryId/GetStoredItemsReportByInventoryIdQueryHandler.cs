using AutoMapper;
using MediatR;
using InventoryManagementSystem.Application.Dtos;
using InventoryManagementSystem.Domain.Interfaces;
using InventoryManagementSystem.Domain.Filters;

namespace InventoryManagementSystem.Application.Items.Queries
{
public class GetStoredItemsReportByInventoryIdQueryHandler : IRequestHandler<GetStoredItemsReportByInventoryIdQuery, List<StoredItemDetailsDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    

    public GetStoredItemsReportByInventoryIdQueryHandler(IUnitOfWork unitOfWork,IMapper mapper)
        {
        _unitOfWork = unitOfWork;
           _mapper = mapper;
           }

    public async Task<List<StoredItemDetailsDto>> Handle(GetStoredItemsReportByInventoryIdQuery request, CancellationToken cancellationToken)
    {
        DateTime enteredDate = Convert.ToDateTime(DateTime.Now
                    .AddYears(-request.YearsCount)
                    .AddMonths(-request.MonthsCount)
                    .AddDays(-request.DaysCount)
                    .ToShortDateString());
            var items = await _unitOfWork.InventoryItemRepository.GetAllbyinventoryIdAndStorageDateAsync(request.InventoryId,request.ApplicationUserId,enteredDate,cancellationToken);
        if (items.Count() != 0)
        {   
             return items.Select(ii=> new StoredItemDetailsDto
                {
            Id = (int)ii.GetType().GetProperty("ItemId")!.GetValue(ii)!,
            Name = (string)ii.GetType().GetProperty("ItemName")!.GetValue(ii)!,
            Size = (string)ii.GetType().GetProperty("ItemSize")!.GetValue(ii)!,
            InventoryId = (int)ii.GetType().GetProperty("InventoryId")!.GetValue(ii)!,
            Quantity = (int?)ii.GetType().GetProperty("Quantity")!.GetValue(ii)!,
            StorageDate = (DateTime?)ii.GetType().GetProperty("StorageDate")!.GetValue(ii)!
        }).ToList();
        }
        
        return new();
    }

    }
}