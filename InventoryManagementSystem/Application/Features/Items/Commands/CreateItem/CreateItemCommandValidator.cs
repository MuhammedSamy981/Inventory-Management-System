using FluentValidation;
using InventoryManagementSystem.Application.Items.Commands;
using InventoryManagementSystem.Domain.Interfaces;

namespace InventoryManagementSystem.Application.InventoryItems.Commands
{
    
    public class CreateItemCommandValidator : AbstractValidator<CreateItemCommandWithApplicationUserId>
    {
        private readonly IUnitOfWork _unitOfWork;
        public CreateItemCommandValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            RuleFor(i => i.CreateItemCommand.Id)
                .NotEmpty().WithMessage("Id is required")
                .MustAsync(async (id, cancellationToken) =>
            {
                // Returns true if the ID does NOT exist
                bool exists = await _unitOfWork.ItemRepository.ExistsAsync(id, cancellationToken);
                return !exists; 
            })
            .WithMessage("An item with this ID already exists in the database.");

            RuleFor(i => i.CreateItemCommand.Name)
            .NotEmpty().WithMessage("Name is required")
            .Length(2, 20).WithMessage("Name must be between 2 and 20 characters");

            RuleFor(i => i.CreateItemCommand.Code)
           .NotEmpty().WithMessage("Code is required")
           .Length(3, 50).WithMessage("Code must be between 3 and 50 characters");

            RuleFor(i => i.CreateItemCommand.Size)
            .NotEmpty().WithMessage("Size is required")
            .Length(3, 10).WithMessage("Size must be between 3 and 10 characters");

            RuleFor(i => i.ApplicationUserId).NotEmpty().WithMessage("ApplicationUserId is required");
        }
    }
}