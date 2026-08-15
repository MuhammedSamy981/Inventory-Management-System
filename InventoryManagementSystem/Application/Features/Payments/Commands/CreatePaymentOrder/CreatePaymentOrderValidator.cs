using System.Net;
using FluentValidation;
using MediatR;

namespace InventoryManagementSystem.Application.Payments.Commands
{

public class CreatePaymentOrderValidator : AbstractValidator<CreatePaymentOrderCommand>
{
    public CreatePaymentOrderValidator()
    {
        RuleFor(x => x.ApplicationUserId).NotEmpty();
        RuleFor(x => x.Amount).GreaterThan(0);
        RuleFor(x => x.Currency).NotEmpty();
    }
}

}
