using FluentValidation;
using ITE.Bookify.Messaging;
using ITE.Bookify.Validations;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ITE.Bookify.Behaviors
{
    internal sealed class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IBaseCommand
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators = validators;

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            if (!_validators.Any())
                return await next(cancellationToken);

            var context = new ValidationContext<TRequest>(request);

            var validationErrors = _validators
                .Select(validator => validator.Validate(context))
                .Where(validationResult => validationResult.Errors.Any())
                .SelectMany(validationResult => validationResult.Errors)
                .Select(validationFailure =>
                    $"PropertyName :{validationFailure.PropertyName}, ErrorMessage :{validationFailure.ErrorMessage}"
                )
                .ToList();

            if (validationErrors.Any())
                throw new ValidationsException(validationErrors);

            return await next(cancellationToken);
        }
    }

}
