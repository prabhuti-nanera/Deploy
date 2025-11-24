using FluentValidation;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CRC.WebPortal.Application.Common.Models;

namespace CRC.WebPortal.Application.Common.Behaviors;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : class, new()
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(
        TRequest request, 
        RequestHandlerDelegate<TResponse> next, 
        CancellationToken cancellationToken)
    {
        if (!_validators.Any())
        {
            return await next();
        }

        var context = new ValidationContext<TRequest>(request);
        var validationResults = await Task.WhenAll(
            _validators.Select(v => v.ValidateAsync(context, cancellationToken)));
        
        var failures = validationResults
            .SelectMany(r => r.Errors)
            .Where(f => f != null)
            .Select(f => f.ErrorMessage)
            .Distinct()
            .ToList();

        if (failures.Any())
        {
            // For AuthResponse, create a failure response
            if (typeof(TResponse) == typeof(AuthResponse))
            {
                var authResponse = AuthResponse.Failure("Validation failed", failures);
                return (TResponse)(object)authResponse;
            }
            
            // For other response types, try to create a generic response
            var response = new TResponse();
            
            // Use reflection to set properties if they exist
            var succeededProp = typeof(TResponse).GetProperty("Succeeded");
            var messageProp = typeof(TResponse).GetProperty("Message");
            var errorsProp = typeof(TResponse).GetProperty("Errors");
            
            succeededProp?.SetValue(response, false);
            messageProp?.SetValue(response, "Validation failed");
            errorsProp?.SetValue(response, failures.ToArray());
            
            return response;
        }

        return await next();
    }
}
