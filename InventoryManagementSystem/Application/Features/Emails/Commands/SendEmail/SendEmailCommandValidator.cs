using FluentValidation;
using InventoryManagementSystem.Domain.Interfaces;
using InventoryManagementSystem.Domain.Models;

namespace InventoryManagementSystem.Application.People.Commands
{
    public class SendEmailCommandValidator : AbstractValidator<Message>
    {

        public SendEmailCommandValidator(IUnitOfWork unitOfWork)
        {
            RuleFor(m => m.Email)
           .NotEmpty().WithMessage("Email is required")
           .EmailAddress().WithMessage("Email is not valid");
        }
    }
}
