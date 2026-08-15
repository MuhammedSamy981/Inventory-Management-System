using FluentValidation;
using InventoryManagementSystem.Application.Items.Queries;
using InventoryManagementSystem.Domain.Filters;


namespace InventoryManagementSystem.Application.Operations.Commands
{
    public class GetItemsMovementReportByListInventoryIdsValidator : AbstractValidator<GetItemsMovementReportByListInventoryIdsQuery>
    {
        public GetItemsMovementReportByListInventoryIdsValidator()
        {
            RuleFor(o => o.InventoryIds!.Count()).NotEmpty().WithMessage("At least one inventoryId is required");

            RuleFor(o => o.FromDate)
                .LessThanOrEqualTo(o => o.ToDate)
                .WithMessage("FromDate must be before or equal to ToDate");

            RuleFor(o => o.ToDate)
                .LessThanOrEqualTo(DateTime.UtcNow)
                .WithMessage("ToDate cannot be in the future");

        }
    }
}

