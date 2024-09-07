using dotnet_ultimate.Exceptions;
using dotnet_ultimate.Extensions;
using dotnet_ultimate.Services;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

try
{
    Log.Information("Starting up");
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    builder.Host.ConfigureSerilog();
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
    builder.Services.AddProblemDetails();
    builder.Services.ConfigureServices(builder.Configuration);
    
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
    // app.ConfigureExceptionHandler();

    // app.UseMiddleware<ErrorHandlerMiddleware>();
    app.UseExceptionHandler();

    // Minimal api
    // app.MapGet("/", (IDummyService svc) => svc.DoSomething());
    // app.MapGet("/", () => { throw new ProductNotFoundException(Guid.NewGuid()); });

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