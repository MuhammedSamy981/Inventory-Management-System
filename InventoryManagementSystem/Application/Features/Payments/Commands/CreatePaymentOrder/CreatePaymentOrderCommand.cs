using System.Net;
using MediatR;

namespace InventoryManagementSystem.Application.Payments.Commands
{
  public record CreatePaymentOrderCommand(string ApplicationUserId, decimal Amount, string Currency) 
    : IRequest<CreatePaymentOrderResult>;
  public record CreatePaymentOrderResult(string orderId);
}
