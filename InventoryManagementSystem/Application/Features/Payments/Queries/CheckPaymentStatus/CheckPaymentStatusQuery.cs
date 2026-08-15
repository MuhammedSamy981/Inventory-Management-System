using MediatR;
using InventoryManagementSystem.Application.Dtos;

namespace InventoryManagementSystem.Application.Payments.Queries
{
  public record CheckPaymentStatusQuery(string ApplicationUserId) : IRequest<bool>;
}