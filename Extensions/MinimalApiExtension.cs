using dotnet_ultimate.Exceptions;
using dotnet_ultimate.Model;
using dotnet_ultimate.Services;
using FluentValidation;

namespace dotnet_ultimate.Extensions;

public static class MinimalApiExtension
{
    public static void ConfigureMinimalApi(this IEndpointRouteBuilder app)
    {
        // app.MapGet("/", (IDummyService svc) => svc.DoSomething());
        app.MapGet("/", () => { throw new ProductNotFoundException(Guid.NewGuid()); });
        app.MapPost("/register", async (UserRegistrationRequest request, IValidator<UserRegistrationRequest> validator) =>
        {
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return Results.ValidationProblem(validationResult.ToDictionary());
            }
            // perform actual service call to register the user to the system
            // _service.RegisterUser(request);
            return Results.Accepted();
        });
    }
}