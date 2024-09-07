using System.Text.Json;
using dotnet_ultimate.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_ultimate.Middleware;

public class ErrorHandlerMiddleware(RequestDelegate _next, ILogger<ErrorHandlerMiddleware> _logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            var response = context.Response;
            response.ContentType = "application/json";
            response.StatusCode = ex switch
            {
                BaseException e => (int)e.StatusCode,
                _ => StatusCodes.Status500InternalServerError
            };

            var problemDetails = new ProblemDetails
            {
                Status = response.StatusCode,
                Title = ex.Message
            };

            _logger.LogError(ex.Message);
            var result = JsonSerializer.Serialize(problemDetails);
            await response.WriteAsync(result);
        }
    }
}