using FluentValidation;
using InventoryManagementSystem.Application.Inventories.Commands;
using InventoryManagementSystem.Domain.Interfaces;

namespace InventoryManagementSystem.Application.InventoryItems.Commands
{

    public class UpdateInventoryCommandValidator : AbstractValidator<UpdateInventoryCommandWithApplicationUserId>
    {
        private readonly IUnitOfWork _unitOfWork;
        public UpdateInventoryCommandValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            RuleFor(i => i.UpdateInventoryCommand.Id).NotEmpty().WithMessage("Id is required")
            .MustAsync(async (id, cancellationToken) =>
            {
                // Returns true if the ID exist
                bool exists = await _unitOfWork.InventoryRepository.ExistsAsync(id, cancellationToken);
                return exists;
            })
            .WithMessage("An inventory with this ID doesn't exist in the database");

            RuleFor(i => i.UpdateInventoryCommand.Name)
            .NotEmpty().WithMessage("Name is required")
            .Length(2, 20).WithMessage("Name must be between 2 and 20 characters");

            RuleFor(i => i.UpdateInventoryCommand.Address).NotEmpty().WithMessage("Address is required");

            RuleFor(i => i.UpdateInventoryCommand.ManagerId).NotEmpty().WithMessage("ManagerId is required");

            RuleFor(i => i.ApplicationUserId).NotEmpty().WithMessage("ApplicationUserId is required");
        }
    }

}