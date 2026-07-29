using FluentValidation;
using MediatR;

namespace ProductManagement.Application.Behaviours;

public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
        => _validators = validators;

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken ct)
    {
        if (!_validators.Any())
            return await next(); // no validator registered for this request type — pass through

        var failures = _validators
            .Select(v => v.Validate(request))
            .SelectMany(result => result.Errors)
            .Where(f => f is not null)
            .ToList();

        if (failures.Count != 0)
        {
            var errorMessage = string.Join(" | ", failures.Select(f => f.ErrorMessage));

            // This cast works because your commands return Result<T> — see note below
            var resultType = typeof(TResponse).GetGenericArguments()[0];
            var failureMethod = typeof(Result<>).MakeGenericType(resultType).GetMethod("Failure");
            return (TResponse)failureMethod!.Invoke(null, new object[] { errorMessage })!;
        }

        return await next();
    }
}