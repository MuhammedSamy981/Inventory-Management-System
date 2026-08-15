using FluentValidation;
using InventoryManagementSystem.Domain.Interfaces;

namespace InventoryManagementSystem.Application.People.Commands
{
    public class CreatePersonCommandValidator : AbstractValidator<CreatePersonCommandWithApplicationUserId>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreatePersonCommandValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            RuleFor(p => p.CreatePersonCommand.Id).NotEmpty().WithMessage("Id is required")
            .MustAsync(async (id, cancellationToken) =>
            {
                // Returns true if the ID does NOT exist (meaning it is unique)
                bool exists = await _unitOfWork.PersonRepository.ExistsAsync(id, cancellationToken);
                return !exists;
            })
            .WithMessage("A person with this ID already exists in the database");

            RuleFor(p => p.CreatePersonCommand.Name)
        .NotEmpty().WithMessage("Name is required")
        .Length(2, 20).WithMessage("Name must be between 2 and 20 characters");

            RuleFor(p => p.CreatePersonCommand.Telephone)
       .NotEmpty().WithMessage("Telephone is required")
       .Length(3, 15).WithMessage("Telephone must be between 3 and 15 numbers");

            RuleFor(p => p.CreatePersonCommand.Email)
           .NotEmpty().WithMessage("Email is required")
           .EmailAddress().WithMessage("Email is not valid");

            RuleFor(p => !string.IsNullOrEmpty(p.CreatePersonCommand.Type)
                 && (p.CreatePersonCommand.Type == "Inventory Manager" || p.CreatePersonCommand.Type == "Supplier" || p.CreatePersonCommand.Type == "Client")).Equal(true)
                .WithMessage("Type must be 'Inventory Manager', 'Supplier', or 'Client'");


            RuleFor(p => !string.IsNullOrEmpty(p.CreatePersonCommand.Website) && p.CreatePersonCommand.Website.Contains(".")).Equal(true).WithMessage("Website is not valid");

            RuleFor(p => p.ApplicationUserId).NotEmpty().WithMessage("ApplicationUserId is required");



        }
    }
}
