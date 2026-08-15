using AutoMapper;
using MediatR;
using InventoryManagementSystem.Application.Dtos;
using InventoryManagementSystem.Domain.Interfaces;

namespace InventoryManagementSystem.Application.Payments.Queries
{
public class CheckPaymentStatusQueryHandler : IRequestHandler<CheckPaymentStatusQuery, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CheckPaymentStatusQueryHandler(IUnitOfWork unitOfWork,IMapper mapper)
        {   
           _unitOfWork = unitOfWork;
           _mapper = mapper;
        }

    public async Task<bool> Handle(CheckPaymentStatusQuery request, CancellationToken cancellationToken)
    {
        var paymentExists = await _unitOfWork.PaymentRepository.ExistsAsync(request.ApplicationUserId, cancellationToken);
        return paymentExists;
    }
}
}