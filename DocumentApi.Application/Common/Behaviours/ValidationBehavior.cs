using DocumentApi.Application.Common.Interfaces;
using FluentValidation;
using MediatR;

namespace DocumentApi.Application.Common.Behaviours
{
    public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
        : IPipelineBehavior<TRequest, TResponse> where TRequest : ICommandBase
    {
        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            ValidationContext<TRequest>? context = new(request);

            var validationFailures = await Task.WhenAll(validators.Select(v => v.ValidateAsync(context)));

            var validationErrors = validationFailures
                .Where(x => !x.IsValid)
                .SelectMany(x => x.Errors)
                .ToList();

            if (validationErrors.Count > 0)
                throw new ValidationException(validationErrors);

            var response = await next();

            return response;
        }
    }
}
