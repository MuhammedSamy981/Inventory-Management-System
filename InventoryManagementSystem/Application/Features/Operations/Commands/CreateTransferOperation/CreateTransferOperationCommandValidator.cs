using FluentValidation;
using InventoryManagementSystem.Domain.Interfaces;

namespace InventoryManagementSystem.Application.Operations.Commands
{
    public class CreateTransferOperationCommandValidator : AbstractValidator<CreateTransferOperationCommandWithApplicationUserId>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateTransferOperationCommandValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            RuleFor(o => o.CreateTransferOperationCommand.Id).NotEmpty().WithMessage("Id is required")
            .MustAsync(async (id, cancellationToken) =>
             {
               // Returns true if the ID does NOT exist (meaning it is unique)
               bool exists = await _unitOfWork.OperationRepository.ExistsAsync(id, cancellationToken);
               return !exists;
             })
            .WithMessage("A transfer operation with this ID already exists in the database");

            RuleFor(o => o.CreateTransferOperationCommand.FromInventoryId).NotEmpty().WithMessage("From Inventory Id is required");
            RuleFor(o => o.CreateTransferOperationCommand.ToInventoryId).NotEmpty().WithMessage("To Inventory Id is required");
            RuleFor(o => o.CreateTransferOperationCommand.ItemId).NotEmpty().WithMessage("ItemId is required");
            //RuleFor(o => o.Date).NotEmpty().WithMessage("Date is required");

            RuleFor(o => o.CreateTransferOperationCommand.Quantity)
                        .GreaterThan(0).WithMessage("Quantity must be greater than 0");
            RuleFor(o => o.CreateTransferOperationCommand.SupplierId).NotEmpty().WithMessage("Supplier Id is required");

            RuleFor(o => o.ApplicationUserId).NotEmpty().WithMessage("ApplicationUserId is required");

            /* RuleFor(o => o.Type)
         .NotEmpty().WithMessage("Type is required")
         .Length(2, 20).WithMessage("Type must be between 2 and 20 characters");*/

            /* RuleFor(o => o.ProDate)
                    .NotEmpty().WithMessage("Production date is required")
                        .LessThanOrEqualTo(DateTime.UtcNow)
                        .WithMessage("Production date cannot be in the future");

                    RuleFor(o => o.ExpiryDate)
                    .NotEmpty().WithMessage("Expiry date is required")
                        .GreaterThan(o => o.ProDate)
                        .WithMessage("Expiry date must be after Production date");*/




        }
    }
}