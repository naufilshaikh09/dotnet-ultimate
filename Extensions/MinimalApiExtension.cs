using System.ComponentModel.DataAnnotations;
using dotnet_ultimate.Features.Products.Commands.Create;
using dotnet_ultimate.Features.Products.Commands.Delete;
using dotnet_ultimate.Features.Products.Commands.Update;
using dotnet_ultimate.Features.Products.Notifications;
using dotnet_ultimate.Features.Products.Queries.Get;
using dotnet_ultimate.Features.Products.Queries.List;
using MediatR;

namespace dotnet_ultimate.Extensions;

public static class MinimalApiExtension
{
    public static void ConfigureMinimalApi(this IEndpointRouteBuilder app)
    {
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

        app.MapGet("/products/{id:int}", async ([Required]int id, ISender mediatr) =>
        {
            var product = await mediatr.Send(new GetProductQuery(id));
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
            if (productId <= 0) return Results.BadRequest();
            await mediatr.Publish(new ProductCreatedNotification(productId));
            return Results.Created($"/products/{productId}", new { id = productId });
        });

        app.MapPut("/products/{id:int}", async (int id, UpdateProductCommand command, ISender mediatr) =>
        {
            var productId = await mediatr.Send(command);
            return productId is null ? Results.BadRequest() : Results.Created($"/products/{productId}", new { id = (int)productId });
        });

        app.MapDelete("/products/{id:int}", async ([Required]int id, ISender mediatr) =>
        {
            await mediatr.Send(new DeleteProductCommand(id));
            return Results.NoContent();
        });
    }
}