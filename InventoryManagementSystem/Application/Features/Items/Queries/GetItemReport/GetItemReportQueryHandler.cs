using AutoMapper;
using MediatR;
using InventoryManagementSystem.Application.Dtos;
using InventoryManagementSystem.Domain.Interfaces;
using InventoryManagementSystem.Domain.Filters;

namespace InventoryManagementSystem.Application.Items.Queries
{
public class GetItemReportQueryHandler : IRequestHandler<GetItemReportQuery, ItemReportDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    

    public GetItemReportQueryHandler(IUnitOfWork unitOfWork,IMapper mapper)
        {
        _unitOfWork = unitOfWork;
           _mapper = mapper;
           }

    public async Task<ItemReportDto?> Handle(GetItemReportQuery request, CancellationToken cancellationToken)
    {
        var item =await _unitOfWork.ItemRepository.GetAllDetailsByIdAndListInventoryIdsAsync(request.Id,request.InventoryIds,request.ApplicationUserId,request.FromDate,request.ToDate);
        if (item != null)
        {
            var inventoriesWithItemQuantities = item.GetType().GetProperty("Inventories")!.GetValue(item) as IEnumerable<Object>;

            if (request.FromDate != null && request.ToDate != null)
            {
                int index = 0;
                int[] StoredQuantity = new int[inventoriesWithItemQuantities!.Count()];
                foreach (var inventory in inventoriesWithItemQuantities!)
                {   
                    var filters= new OperationFilters
                {
                    InventoryId=(int)inventory.GetType().GetProperty("InventoryId")!.GetValue(inventory)!,
                    ItemId=(int)item.GetType().GetProperty("ItemId")!.GetValue(item)!,
                    FromDate=request.FromDate,
                    ToDate=request.ToDate
                };

                    StoredQuantity[index] = await _unitOfWork.OperationRepository.GetAllQuantitiesAsync(filters,request.ApplicationUserId,cancellationToken);
                    Console.WriteLine("\n****"+index+":"+StoredQuantity[index]+"***\n");
                    index++;
                }

                index = 0;


            return new ItemReportDto
             {
               Id =(int)item.GetType().GetProperty("ItemId")!.GetValue(item)!,
               Name=(string)item.GetType().GetProperty("ItemName")!.GetValue(item)!,
               Size=(string)item.GetType().GetProperty("ItemSize")!.GetValue(item)!,
               Code=(string)item.GetType().GetProperty("ItemCode")!.GetValue(item)!,
               Inventories=inventoriesWithItemQuantities.Select((i,index)=> new InventoryDetailsDto
          {
            Id = (int)i.GetType().GetProperty("InventoryId")!.GetValue(i)!,
            Name = (string)i.GetType().GetProperty("InventoryName")!.GetValue(i)!,
            Address = (string)i.GetType().GetProperty("InventoryAddress")!.GetValue(i)!,
            Quantity = StoredQuantity[index]
          }).ToList()
             };

            }
            
                         return new ItemReportDto
             {
               Id =(int)item.GetType().GetProperty("ItemId")!.GetValue(item)!,
               Name=(string)item.GetType().GetProperty("ItemName")!.GetValue(item)!,
               Size=(string)item.GetType().GetProperty("ItemSize")!.GetValue(item)!,
               Code=(string)item.GetType().GetProperty("ItemCode")!.GetValue(item)!,
               Inventories=inventoriesWithItemQuantities!.Select((i,index)=> new InventoryDetailsDto
          {
            Id = (int)i.GetType().GetProperty("InventoryId")!.GetValue(i)!,
            Name = (string)i.GetType().GetProperty("InventoryName")!.GetValue(i)!,
            Address = (string)i.GetType().GetProperty("InventoryAddress")!.GetValue(i)!,
            Quantity = (int)i.GetType().GetProperty("Quantity")!.GetValue(i)!
          }).ToList()
             };
        }
        return null;
    }

    }
}