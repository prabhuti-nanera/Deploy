using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CRC.WebPortal.API.Filters;

/// <summary>
/// Filter to ensure only one validation error per field is returned
/// </summary>
public class SingleValidationErrorFilter : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            var cleanedErrors = new Dictionary<string, string[]>();
            
            foreach (var modelError in context.ModelState)
            {
                var fieldName = modelError.Key;
                var errors = modelError.Value?.Errors;
                
                if (errors != null && errors.Count > 0)
                {
                    // Get all error messages and remove duplicates
                    var errorMessages = errors
                        .Select(e => e.ErrorMessage)
                        .Where(msg => !string.IsNullOrEmpty(msg))
                        .Distinct()
                        .ToArray();
                    
                    // Take only the first unique error message
                    if (errorMessages.Length > 0)
                    {
                        cleanedErrors[fieldName] = new[] { errorMessages.First() };
                    }
                }
            }

            var problemDetails = new ValidationProblemDetails(cleanedErrors)
            {
                Type = "https://tools.ietf.org/html/rfc9110#section-15.5.1",
                Title = "One or more validation errors occurred.",
                Status = 400,
                Instance = context.HttpContext.Request.Path
            };

            context.Result = new BadRequestObjectResult(problemDetails);
        }
    }
}
