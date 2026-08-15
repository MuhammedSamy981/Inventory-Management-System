using AutoMapper;
using MediatR;
using InventoryManagementSystem.Application.Dtos;
using InventoryManagementSystem.Domain.Interfaces;

namespace InventoryManagementSystem.Application.Operations.Queries
{
    public class GetOperationByIdQueryHandler : IRequestHandler<GetOperationByIdQuery, OperationDto?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;


        public GetOperationByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<OperationDto?> Handle(GetOperationByIdQuery request, CancellationToken cancellationToken)
        {
            var operation = await _unitOfWork.OperationRepository.GetByIdAsync(request.Id);
            if (operation != null && operation.ApplicationUserId == request.ApplicationUserId)
            {
                return _mapper.Map<OperationDto>(operation);
            }
            return null;
        }
    }
}