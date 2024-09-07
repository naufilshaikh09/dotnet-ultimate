using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_ultimate.Exceptions;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
        CancellationToken cancellationToken = default)
    {
        var problemDetails = new ProblemDetails
        {
            Instance = httpContext.Request.Path
        };

        switch (exception)
        {
            case BaseException e:
                httpContext.Response.StatusCode = (int)e.StatusCode;
                problemDetails.Title = e.Message;
                break;
            case FluentValidation.ValidationException fluentException:
            {
                problemDetails.Title = "one or more validation errors occurred.";
                problemDetails.Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1";
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                var validationErrors = new List<string>();
                foreach (var error in fluentException.Errors)
                {
                    validationErrors.Add(error.ErrorMessage);
                }
                problemDetails.Extensions.Add("errors", validationErrors);
                break;
            }
            default:
                problemDetails.Title = exception.Message;
                break;
        }
        
        logger.LogError("${ProblemDetailsTitle}", problemDetails.Title);
        
        problemDetails.Status = httpContext.Response.StatusCode;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken).ConfigureAwait(false);
        return true;
    }
}