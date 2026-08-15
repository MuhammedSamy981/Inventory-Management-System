using FluentValidation;
using InventoryManagementSystem.Application.Items.Queries;
using InventoryManagementSystem.Domain.Filters;


namespace InventoryManagementSystem.Application.Operations.Commands
{
    public class GetExpiredItemsReportByInventoryIdValidator : AbstractValidator<GetExpiredItemsReportByInventoryIdQuery>
    {
        public GetExpiredItemsReportByInventoryIdValidator()
        {
            RuleFor(i => i.InventoryId).NotEmpty().WithMessage("InventoryId is required");
            RuleFor(i => i.YearsCount).GreaterThanOrEqualTo(0).WithMessage("Years count must be greater than or equal 0");
            RuleFor(i => i.MonthsCount).GreaterThanOrEqualTo(0).WithMessage("Months count must be greater than or equal 0");
            RuleFor(i => i.DaysCount).GreaterThanOrEqualTo(0).WithMessage("Days count must be greater than or equal 0");
        }
    }
}

