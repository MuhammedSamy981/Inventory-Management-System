using FluentValidation;
using InventoryManagementSystem.Domain.Interfaces;

namespace InventoryManagementSystem.Application.Operations.Commands
{
    public class CreateOperationCommandValidator : AbstractValidator<CreateOperationCommandWithApplicationUserId>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateOperationCommandValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            RuleFor(o => o.CreateOperationCommand.Id).NotEmpty().WithMessage("Id is required")
                 .MustAsync(async (id, cancellationToken) =>
            {
                // Returns true if the ID does NOT exist (meaning it is unique)
                bool exists = await _unitOfWork.OperationRepository.ExistsAsync(id, cancellationToken);
                return !exists;
            })
            .WithMessage("An operation with this ID already exists in the database");

            RuleFor(o => o.CreateOperationCommand.Date).NotEmpty().WithMessage("Date is required");

            RuleFor(o => o.CreateOperationCommand.Quantity)
                        .GreaterThan(0).WithMessage("Quantity must be greater than 0");


            RuleFor(o => !string.IsNullOrEmpty(o.CreateOperationCommand.Type)
             && (o.CreateOperationCommand.Type == "Supply Permit" || o.CreateOperationCommand.Type == "Exchange Permit")).Equal(true)
            .WithMessage("Type must be 'Supply Permit' or 'Exchange Permit'");

            RuleFor(o => o.CreateOperationCommand.ProDate)
                   .NotEmpty().WithMessage("Production date is required")
                       .LessThanOrEqualTo(DateTime.UtcNow)
                       .WithMessage("Production date cannot be in the future");

            RuleFor(o => o.CreateOperationCommand.ExpiryDate)
            .NotEmpty().WithMessage("Expiry date is required")
                .GreaterThan(o => o.CreateOperationCommand.ProDate)
                .WithMessage("Expiry date must be after Production date");


            RuleFor(o => o.CreateOperationCommand.ItemId).NotEmpty().WithMessage("ItemId is required");

            RuleFor(o => o.CreateOperationCommand.PersonId).NotEmpty().WithMessage("PersonId is required");

            RuleFor(o => o.CreateOperationCommand.InventoryId).NotEmpty().WithMessage("InventoryId is required");

            RuleFor(o => o.ApplicationUserId).NotEmpty().WithMessage("ApplicationUserId is required");
        }
    }
}