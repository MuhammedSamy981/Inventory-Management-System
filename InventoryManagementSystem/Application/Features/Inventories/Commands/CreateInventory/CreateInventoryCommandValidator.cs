using FluentValidation;
using InventoryManagementSystem.Application.Inventories.Commands;
using InventoryManagementSystem.Domain.Interfaces;

namespace InventoryManagementSystem.Application.InventoryItems.Commands
{
    public class CreateInventoryCommandValidator : AbstractValidator<CreateInventoryCommandWithApplicationUserId>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateInventoryCommandValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            RuleFor(i => i.CreateInventoryCommand.Id).NotEmpty().WithMessage("Id is required")
            .MustAsync(async (id, cancellationToken) =>
            {
              // Returns true if the ID does NOT exist (meaning it is unique)
              bool exists = await _unitOfWork.InventoryRepository.ExistsAsync(id, cancellationToken);
              return !exists;
            })
            .WithMessage("An inventory with this ID already exists in the database");

            RuleFor(i => i.CreateInventoryCommand.Name)
            .NotEmpty().WithMessage("Name is required")
            .Length(2, 20).WithMessage("Name must be between 2 and 20 characters");

            RuleFor(i => i.CreateInventoryCommand.Address).NotEmpty().WithMessage("Address is required");

            RuleFor(i => i.CreateInventoryCommand.ManagerId).NotEmpty().WithMessage("ManagerId is required");
            RuleFor(i => i.ApplicationUserId).NotEmpty().WithMessage("ApplicationUserId is required");
        }
    }

}