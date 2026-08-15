using FluentValidation;
using InventoryManagementSystem.Domain.Interfaces;

namespace InventoryManagementSystem.Application.People.Commands
{
    public class UpdatePersonCommandValidator : AbstractValidator<UpdatePersonCommandWithApplicationUserId>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdatePersonCommandValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            RuleFor(i => i.UpdatePersonCommand.Id).NotEmpty().WithMessage("Id is required")
            .MustAsync(async (id, cancellationToken) =>
            {
               // Returns true if the ID exist
               bool exists = await _unitOfWork.PersonRepository.ExistsAsync(id, cancellationToken);
               return exists;
           })
           .WithMessage("A person with this ID doesn't exist in the database");

            RuleFor(p => p.UpdatePersonCommand.Name)
           .NotEmpty().WithMessage("Name is required")
           .Length(2, 20).WithMessage("Name must be between 2 and 20 characters");

            RuleFor(p => p.UpdatePersonCommand.Telephone)
           .NotEmpty().WithMessage("Telephone is required")
           .Length(3, 15).WithMessage("Telephone must be between 3 and 15 numbers");

            RuleFor(p => p.UpdatePersonCommand.Email)
           .NotEmpty().WithMessage("Email is required")
           .EmailAddress().WithMessage("Email is not valid");


            RuleFor(p => !string.IsNullOrEmpty(p.UpdatePersonCommand.Type)
             && (p.UpdatePersonCommand.Type == "Inventory Manager" || p.UpdatePersonCommand.Type == "Supplier" || p.UpdatePersonCommand.Type == "Client")).Equal(true)
            .WithMessage("Type must be 'Inventory Manager', 'Supplier', or 'Client'");

            RuleFor(p => p.ApplicationUserId).NotEmpty().WithMessage("ApplicationUserId is required");

        }
    }
}
