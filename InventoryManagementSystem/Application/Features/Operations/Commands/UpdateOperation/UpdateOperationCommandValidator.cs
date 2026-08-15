using FluentValidation;
using InventoryManagementSystem.Domain.Interfaces;

namespace InventoryManagementSystem.Application.Operations.Commands
{
    public class UpdateOperationCommandValidator : AbstractValidator<UpdateOperationCommandWithApplicationUserId>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateOperationCommandValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            RuleFor(o => o.UpdateOperationCommand.Id).NotEmpty().WithMessage("Id is required");
/*          
  .MustAsync(async (id, cancellationToken) =>
            {
                 //Returns true if the ID exist
                bool exists = await _unitOfWork.OperationRepository.ExistsAsync(id, cancellationToken);
                return exists;
            })
            .WithMessage("An operation with this ID doesn't exist in the database.");*/

            RuleFor(o => o.UpdateOperationCommand.Date).NotEmpty().WithMessage("Date is required");

            RuleFor(o => o.UpdateOperationCommand.Quantity)
                        .GreaterThan(0).WithMessage("Quantity must be greater than 0");


            RuleFor(o => !string.IsNullOrEmpty(o.UpdateOperationCommand.Type)
            && (o.UpdateOperationCommand.Type == "Supply Permit" || o.UpdateOperationCommand.Type == "Exchange Permit")).Equal(true)
            .WithMessage("Type must be 'Supply Permit' or 'Exchange Permit'");

            RuleFor(o => o.UpdateOperationCommand.ProDate)
                   .NotEmpty().WithMessage("Production date is required")
                       .LessThanOrEqualTo(DateTime.UtcNow)
                       .WithMessage("Production date cannot be in the future");

            RuleFor(o => o.UpdateOperationCommand.ExpiryDate)
            .NotEmpty().WithMessage("Expiry date is required")
                .GreaterThan(o => o.UpdateOperationCommand.ProDate)
                .WithMessage("Expiry date must be after Production date");


            RuleFor(o => o.UpdateOperationCommand.ItemId).NotEmpty().WithMessage("ItemId is required");

            RuleFor(o => o.UpdateOperationCommand.PersonId).NotEmpty().WithMessage("PersonId is required");

            RuleFor(o => o.UpdateOperationCommand.InventoryId).NotEmpty().WithMessage("InventoryId is required");

            RuleFor(o => o.ApplicationUserId).NotEmpty().WithMessage("ApplicationUserId is required");


        }
    }
}