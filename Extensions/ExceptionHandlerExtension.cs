using System.Net;
using Microsoft.AspNetCore.Diagnostics;

namespace dotnet_ultimate.Extensions;

public static class ExceptionHandlerExtension
{
    public static void ConfigureExceptionHandler(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(options =>
        {
            options.Run(async context =>
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Response.ContentType = "application/json";
                var exception = context.Features.Get<IExceptionHandlerFeature>();
                if (exception != null)
                {
                    var message = $"{exception.Error.Message}";
                    await context.Response.WriteAsync(message).ConfigureAwait(false);
                }
            });
        });
    }
}