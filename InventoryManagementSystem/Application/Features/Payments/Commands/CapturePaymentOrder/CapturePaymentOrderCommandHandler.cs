using System.Net;
using InventoryManagementSystem.Application.Interfaces;
using InventoryManagementSystem.Domain.Entities;
using InventoryManagementSystem.Domain.Interfaces;
using MediatR;

namespace InventoryManagementSystem.Application.Payments.Commands
{
public class CapturePaymentOrderCommandHandler 
    : IRequestHandler<CapturePaymentOrderCommand, string?>
{
    private readonly IPayPalService _payPalService;
    private readonly IUnitOfWork _unitOfWork;

    public CapturePaymentOrderCommandHandler(IPayPalService payPalService, IUnitOfWork unitOfWork)
    {
        _payPalService = payPalService;
        _unitOfWork = unitOfWork;
    }

    public async Task<string?> Handle(CapturePaymentOrderCommand request, CancellationToken ct)
    {
        var captured = await _payPalService.CaptureOrderAsync(request.OrderId);

        var payment = await _unitOfWork.PaymentRepository
            .GetByIdAsync(request.OrderId);

        if (payment is null) return null;

        payment.Status = captured ? PaymentStatus.Completed : PaymentStatus.Failed;
        await _unitOfWork.PaymentRepository.UpdateAsync(payment);
        await _unitOfWork.SaveChangesAsync();
        return captured?payment.ApplicationUserId:null;
        
    }
}

}
