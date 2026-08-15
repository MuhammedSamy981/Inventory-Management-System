using AutoMapper;
using MediatR;
using InventoryManagementSystem.Application.Dtos;
using InventoryManagementSystem.Domain.Interfaces;

namespace InventoryManagementSystem.Application.Operations.Queries
{
public class GetAllOperationsQueryHandler : IRequestHandler<GetAllOperationsQuery, List<OperationDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper; 

    public GetAllOperationsQueryHandler(IUnitOfWork unitOfWork,IMapper mapper)
        { 
           _unitOfWork = unitOfWork;
           _mapper = mapper;
        }

    public async Task<List<OperationDto>> Handle(GetAllOperationsQuery request, CancellationToken cancellationToken)
    {
        var operations =await _unitOfWork.OperationRepository.GetAllAsync( cancellationToken);
        return _mapper.Map<List<OperationDto>>(operations);
    }
}
}