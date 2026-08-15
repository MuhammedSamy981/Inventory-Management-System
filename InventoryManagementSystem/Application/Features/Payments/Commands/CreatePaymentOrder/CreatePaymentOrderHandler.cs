using System.Net;
using AutoMapper;
using InventoryManagementSystem.Application.Interfaces;
using InventoryManagementSystem.Domain.Entities;
using InventoryManagementSystem.Domain.Interfaces;
using MediatR;

namespace InventoryManagementSystem.Application.Payments.Commands
{
public class CreatePaymentOrderHandler 
    : IRequestHandler<CreatePaymentOrderCommand, CreatePaymentOrderResult>
{
    private readonly IPayPalService _payPalService;
    private readonly IUnitOfWork _unitOfWork;

    public CreatePaymentOrderHandler(IPayPalService payPalService, IUnitOfWork unitOfWork)
    {
        _payPalService = payPalService;
        _unitOfWork = unitOfWork;
    }

    public async Task<CreatePaymentOrderResult> Handle(CreatePaymentOrderCommand request, CancellationToken ct)
    {
       var orderId = await _payPalService.CreateOrderAsync(request.Amount, request.Currency);

        var payment = new Payment
        {
            Id = orderId,
            ApplicationUserId = request.ApplicationUserId,
            Amount = request.Amount,
            Status = PaymentStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };

       await _unitOfWork.PaymentRepository.AddAsync(payment);
       await _unitOfWork.SaveChangesAsync();
       return new CreatePaymentOrderResult(orderId);
    }
}

}
