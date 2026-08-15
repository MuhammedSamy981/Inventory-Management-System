using AutoMapper;
using MediatR;
using InventoryManagementSystem.Domain.Interfaces;

namespace InventoryManagementSystem.Application.Inventories.Queries
{
public class GetAllInventoryIdsQueryHandler : IRequestHandler<GetAllInventoryIdsQuery, List<int>>
{
    private readonly IUnitOfWork _unitOfWork;


    public GetAllInventoryIdsQueryHandler(IUnitOfWork unitOfWork)
        {
            
        _unitOfWork = unitOfWork;
           }

    public async Task<List<int>> Handle(GetAllInventoryIdsQuery request, CancellationToken cancellationToken)
    {
            var ItemIds=await _unitOfWork.InventoryRepository.GetAllIdsAsync(request.ApplicationUserId,cancellationToken);
          return ItemIds;
    }
}
}