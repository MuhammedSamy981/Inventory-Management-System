using AutoMapper;
using MediatR;
using InventoryManagementSystem.Application.Dtos;
using InventoryManagementSystem.Domain.Interfaces;
using InventoryManagementSystem.Domain.Filters;

namespace InventoryManagementSystem.Application.Items.Queries
{
    public class GetItemsMovementReportByListInventoryIdsQueryHandler : IRequestHandler<GetItemsMovementReportByListInventoryIdsQuery, List<ItemsMovementReportDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        
        public GetItemsMovementReportByListInventoryIdsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            
        }

        public async Task<List<ItemsMovementReportDto>> Handle(GetItemsMovementReportByListInventoryIdsQuery request, CancellationToken cancellationToken)
        {

            var items = await _unitOfWork.InventoryRepository.GetAllItemsMovementByListInventoryIdsAsync(request.InventoryIds!,request.ApplicationUserId, request.FromDate, request.ToDate);
            if (items != null)
            {

                return items.Select(i => new ItemsMovementReportDto
                {
                    InventoryId = (int)i.GetType().GetProperty("InventoryId")!.GetValue(i)!,
                    InventoryName = (string)i.GetType().GetProperty("InventoryName")!.GetValue(i)!,
                    InventoryAddress = (string)i.GetType().GetProperty("InventoryAddress")!.GetValue(i)!,
                    Items = ((List<object>)i.GetType().GetProperty("Items")!.GetValue(i)!).Select(i => new MovedItemDetailsDto
                    {
                        Id = (int)i.GetType().GetProperty("ItemId")!.GetValue(i)!,
                        Name = (string)i.GetType().GetProperty("ItemName")!.GetValue(i)!,
                        MoveName = (string)i.GetType().GetProperty("MoveName")!.GetValue(i)!,
                        MoveDate = (DateTime)i.GetType().GetProperty("MoveDate")!.GetValue(i)!,
                        Quantity = (int?)i.GetType().GetProperty("Quantity")!.GetValue(i)!
                    }).ToList()
                }).ToList();
            }

            return new ();
        }

    }
}