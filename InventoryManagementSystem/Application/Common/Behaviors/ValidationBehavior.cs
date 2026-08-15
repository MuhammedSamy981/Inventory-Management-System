using FluentValidation;
using MediatR;

namespace InventoryManagementSystem.Application.Common.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        // MediatR will automatically inject all registered validators for the specific TRequest
        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            // If there are no validators for this request, skip validation
            if (!_validators.Any())
            {
                return await next(cancellationToken);
            }

            // Create a validation context
            var context = new ValidationContext<TRequest>(request);

            // Run all validators concurrently
            var validationResults = await Task.WhenAll(
                _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

            // Collect all validation failures
            var failures = validationResults
                .SelectMany(r => r.Errors)
                .Where(f => f != null)
                .ToList();

            // If there are failures, throw an exception
            if (failures.Count != 0)
            {
                throw new ValidationException(failures);
            }

            // If validation passes, continue to the handler
            return await next(cancellationToken);
        }
    }
}