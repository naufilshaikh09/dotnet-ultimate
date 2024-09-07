using dotnet_ultimate.Exceptions;
using dotnet_ultimate.Extensions;
using dotnet_ultimate.Model;
using dotnet_ultimate.Services;
using dotnet_ultimate.Validators;
using FluentValidation;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

try
{
    Log.Information("Starting up");
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container :
    builder.Host.ConfigureSerilog();
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
    builder.Services.AddProblemDetails();
    builder.Services.ConfigureServices(builder.Configuration);
    builder.Services.AddValidatorsFromAssemblyContaining<UserRegistrationValidator>();

    var app = builder.Build();

    // Configure the HTTP request pipeline :
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();
    app.UseSerilogRequestLogging();
    app.UseExceptionHandler();
    // Built-in exception middleware :
    // app.ConfigureExceptionHandler();
    // Custom middleware :
    // app.UseMiddleware<ErrorHandlerMiddleware>();
    // Minimal api :
    // app.ConfigureMinimalApi();

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