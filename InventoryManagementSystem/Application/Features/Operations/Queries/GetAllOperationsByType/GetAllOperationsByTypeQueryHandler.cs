using AutoMapper;
using MediatR;
using InventoryManagementSystem.Application.Dtos;
using InventoryManagementSystem.Domain.Interfaces;

namespace InventoryManagementSystem.Application.Operations.Queries
{
public class GetAllOperationsByTypeQueryHandler : IRequestHandler<GetAllOperationsByTypeQuery, List<OperationDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    

    public GetAllOperationsByTypeQueryHandler(IUnitOfWork unitOfWork,IMapper mapper)
        {     
           _unitOfWork = unitOfWork;
           _mapper = mapper;
        }

    public async Task<List<OperationDto>> Handle(GetAllOperationsByTypeQuery request, CancellationToken cancellationToken)
    {
       var operations=await _unitOfWork.OperationRepository.GetAllByTypeAsync(request.Type,request.ApplicationUserId,request.PageNumber, request.PageSize,cancellationToken);
       return _mapper.Map<List<OperationDto>>(operations);
    }
}
}