using System.Text.Json;
using MediatR;

namespace dotnet_ultimate.Behaviors;

public class RequestResponseLoggingBehavior<TRequest, TResponse>(ILogger<RequestResponseLoggingBehavior<TRequest, TResponse>> logger) 
    : IPipelineBehavior<TRequest, TResponse> 
    where TRequest : class
{
    public async Task<TResponse> Handle(
        TRequest request, 
        RequestHandlerDelegate<TResponse> next, 
        CancellationToken cancellationToken)
    {
        var correlationId = Guid.NewGuid();
        
        // Request logging
        // Serialize the request
        var requestJson = JsonSerializer.Serialize(request);
        // Log the serialized request
        logger.LogInformation("Handling request {CorrelationId}: {Request}", correlationId, requestJson);
        
        // Response logging
        var response = await next();
        // Serialize the request
        var responseJson = JsonSerializer.Serialize(response);
        // Log the serialized request
        logger.LogInformation("Response for {CorrelationId}: {Response}", correlationId, responseJson);

        return response;
    }
}