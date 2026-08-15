using System.Net;
using MediatR;

namespace InventoryManagementSystem.Application.Payments.Commands
{
  public record CapturePaymentOrderCommand(string OrderId) : IRequest<string?>;
}