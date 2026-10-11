using AutoMapper;
using MediatR;
using InventoryManagementSystem.Application.Dtos;
using InventoryManagementSystem.Domain.Interfaces;
using InventoryManagementSystem.Domain.Filters;

namespace InventoryManagementSystem.Application.Inventories.Queries
{
    public class GetInventoryReportQueryHandler : IRequestHandler<GetInventoryReportQuery, InventoryReportDto?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetInventoryReportQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<InventoryReportDto?> Handle(GetInventoryReportQuery request, CancellationToken cancellationToken)
        {
            var inventory = await _unitOfWork.InventoryRepository.GetAllDetailsByIdAsync(request.Id, request.ApplicationUserId, request.FromDate, request.ToDate,cancellationToken);
            if (inventory != null)
            {
                var items = inventory.GetType().GetProperty("Items")!.GetValue(inventory) as IEnumerable<Object>;

                if (request.FromDate != null && request.ToDate != null)
                {
                    //Console.WriteLine("\n"+request.FromDate+"***"+request.ToDate+"\n");
                    int index = 0;
                    int[] ItemQuantity = new int[items!.Count()];

                    foreach (var item in items!)
                    {
                        var filters = new OperationFilters
                        {
                            InventoryId = (int)inventory.GetType().GetProperty("InventoryId")!.GetValue(inventory)!,
                            ItemId = (int)item.GetType().GetProperty("ItemId")!.GetValue(item)!,
                            FromDate = request.FromDate,
                            ToDate = request.ToDate
                        };

                        ItemQuantity[index] = await _unitOfWork.OperationRepository.GetAllQuantitiesAsync(filters, request.ApplicationUserId, cancellationToken);
                        Console.WriteLine("\n****" + ItemQuantity[index] + "***\n");
                        index++;
                    }

                    index = 0;


                    return new InventoryReportDto
                    {
                        Id = (int)inventory.GetType().GetProperty("InventoryId")!.GetValue(inventory)!,
                        Name = (string)inventory.GetType().GetProperty("InventoryName")!.GetValue(inventory)!,
                        Address = (string)inventory.GetType().GetProperty("InventoryAddress")!.GetValue(inventory)!,
                        Items = items.Select((i, index) => new ItemDetailsDto
                        {
                            Id = (int)i.GetType().GetProperty("ItemId")!.GetValue(i)!,
                            Name = (string)i.GetType().GetProperty("ItemName")!.GetValue(i)!,
                            Size = (string)i.GetType().GetProperty("ItemSize")!.GetValue(i)!,
                            Code=(string)i.GetType().GetProperty("ItemCode")!.GetValue(i)!,
                            Quantity = ItemQuantity[index]
                        }).ToList()
                    };

                }

                return new InventoryReportDto
                {
                    Id = (int)inventory.GetType().GetProperty("InventoryId")!.GetValue(inventory)!,
                    Name = (string)inventory.GetType().GetProperty("InventoryName")!.GetValue(inventory)!,
                    Address = (string)inventory.GetType().GetProperty("InventoryAddress")!.GetValue(inventory)!,
                    Items = items!.Select(i => new ItemDetailsDto
                    {
                        Id = (int)i.GetType().GetProperty("ItemId")!.GetValue(i)!,
                        Name = (string)i.GetType().GetProperty("ItemName")!.GetValue(i)!,
                        Size = (string)i.GetType().GetProperty("ItemSize")!.GetValue(i)!,
                        Code=(string)i.GetType().GetProperty("ItemCode")!.GetValue(i)!,
                        Quantity = (int?)i.GetType().GetProperty("Quantity")!.GetValue(i)!
                    }).ToList()
                };
            }
            return null;
        }
    }
}