using FluentValidation;
using InventoryManagementSystem.Domain.Filters;


namespace InventoryManagementSystem.Application.Operations.Commands
{
    public class GetInventoryReportValidator : AbstractValidator<OperationFilters>
    {
        public GetInventoryReportValidator()
        {
            RuleFor(o => o.InventoryId).NotEmpty().WithMessage("InventoryId is required");

            RuleFor(o => o.ItemId).NotEmpty().WithMessage("ItemId is required");

            RuleFor(o => o.FromDate)
                .LessThanOrEqualTo(o => o.ToDate)
                .WithMessage("FromDate must be before or equal to ToDate");

            RuleFor(o => o.ToDate)
                .LessThanOrEqualTo(DateTime.UtcNow)
                .WithMessage("ToDate cannot be in the future");

        }
    }
}

