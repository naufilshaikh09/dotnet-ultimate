using dotnet_ultimate.Exceptions;
using dotnet_ultimate.Features.Products.Commands.Create;
using dotnet_ultimate.Features.Products.Commands.Delete;
using dotnet_ultimate.Features.Products.Commands.Update;
using dotnet_ultimate.Features.Products.DTOs;
using dotnet_ultimate.Features.Products.Notifications;
using dotnet_ultimate.Features.Products.Queries.Get;
using dotnet_ultimate.Features.Products.Queries.List;
using dotnet_ultimate.Model;
using dotnet_ultimate.Services;
using FluentValidation;
using MediatR;

namespace dotnet_ultimate.Extensions;

public static class MinimalApiExtension
{
    public static void ConfigureMinimalApi(this IEndpointRouteBuilder app)
    {
        // app.MapGet("/", (IDummyService svc) => svc.DoSomething());
        // app.MapGet("/", () => { throw new ProductNotFoundException(Guid.NewGuid()); });
        // app.MapPost("/register", async (UserRegistrationRequest request, IValidator<UserRegistrationRequest> validator) =>
        // {
        //     var validationResult = await validator.ValidateAsync(request);
        //     if (!validationResult.IsValid)
        //     {
        //         return Results.ValidationProblem(validationResult.ToDictionary());
        //     }
        //     // perform actual service call to register the user to the system
        //     // _service.RegisterUser(request);
        //     return Results.Accepted();
        // });
        
        app.MapGet("/products/{id:guid}", async (Guid id, ISender mediatr) =>
        {
            var product = await mediatr.Send(new GetProductQuery(id));
            if (product == null) return Results.NotFound();
            return Results.Ok(product);
        });

        app.MapGet("/products", async (ISender mediatr) =>
        {
            var products = await mediatr.Send(new ListProductsQuery());
            return Results.Ok(products);
        });

        app.MapPost("/products", async (CreateProductCommand command, IMediator mediatr) =>
        {
            var productId = await mediatr.Send(command);
            if (Guid.Empty == productId) return Results.BadRequest();
            await mediatr.Publish(new ProductCreatedNotification(productId));
            return Results.Created($"/products/{productId}", new { id = productId });
        });
        
        app.MapPut("/products/{id:guid}", async (Guid id, UpdateProductCommand command, ISender mediatr) =>
        {
            var productId = await mediatr.Send(command);
            if (productId is null) return Results.BadRequest();
            return Results.Created($"/products/{productId}", new { id = productId });
        });
        
        app.MapDelete("/products/{id:guid}", async (Guid id, ISender mediatr) =>
        {
            await mediatr.Send(new DeleteProductCommand(id));
            return Results.NoContent();
        });
    }
}