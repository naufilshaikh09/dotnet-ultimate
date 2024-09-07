using System.Net;
using dotnet_ultimate.Exceptions;
using dotnet_ultimate.Middleware;
using dotnet_ultimate.Services;
using Microsoft.AspNetCore.Diagnostics;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

try
{
    Log.Information("Starting up");

    var builder = WebApplication.CreateBuilder(args);

    // Approach 1: Configuration via appSettings.json
    builder.Host.UseSerilog((context, loggerConfiguration) =>
    {
        loggerConfiguration.WriteTo.Console();
        loggerConfiguration.ReadFrom.Configuration(context.Configuration);
    });

    // Approach 2: Configuration via Fluent API 
    // builder.Host.UseSerilog((context, logger) =>
    // {
    //     logger.MinimumLevel.Information();
    //     logger.WriteTo.Console();
    // });

    // Add services to the container.
    builder.Services.AddControllers();

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
    builder.Services.AddProblemDetails();

    builder.Services.AddTransient<IDummyService, DummyService>();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();

    app.UseSerilogRequestLogging();

    // In-built exception middleware
    // app.UseExceptionHandler(options =>
    // {
    //     options.Run(async context =>
    //     {
    //         context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
    //         context.Response.ContentType = "application/json";
    //         var exception = context.Features.Get<IExceptionHandlerFeature>();
    //         if (exception != null)
    //         {
    //             var message = $"{exception.Error.Message}";
    //             await context.Response.WriteAsync(message).ConfigureAwait(false);
    //         }
    //     });
    // });

    // app.UseMiddleware<ErrorHandlerMiddleware>();
    app.UseExceptionHandler();

    // Minimal api
    // app.MapGet("/", (IDummyService svc) => svc.DoSomething());
    app.MapGet("/", () => { throw new ProductNotFoundException(Guid.NewGuid()); });

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.CloseAndFlush();
}