using FluentValidation;
using InventoryManagementSystem.Application.Items.Commands;
using InventoryManagementSystem.Domain.Interfaces;

namespace InventoryManagementSystem.Application.InventoryItems.Commands
{
    public class UpdateItemCommandValidator : AbstractValidator<UpdateItemCommandWithApplicationUserId>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateItemCommandValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            RuleFor(i => i.UpdateItemCommand.Id).NotEmpty().WithMessage("Id is required")
            .MustAsync(async (id, cancellationToken) =>
             {
               // Returns true if the ID exist
               bool exists = await _unitOfWork.ItemRepository.ExistsAsync(id, cancellationToken);
               return exists;
             })
            .WithMessage("An item with this ID doesn't exist in the database");


            RuleFor(i => i.UpdateItemCommand.Name)
            .NotEmpty().WithMessage("Name is required")
            .Length(2, 20).WithMessage("Name must be between 2 and 20 characters");

            RuleFor(i => i.UpdateItemCommand.Code)
           .NotEmpty().WithMessage("Code is required")
           .Length(3, 50).WithMessage("Code must be between 3 and 50 characters");

            RuleFor(i => i.UpdateItemCommand.Size)
            .NotEmpty().WithMessage("Size is required")
            .Length(3, 10).WithMessage("Size must be between 3 and 10 characters");

            RuleFor(i => i.ApplicationUserId).NotEmpty().WithMessage("ApplicationUserId is required");

        }
    }
}