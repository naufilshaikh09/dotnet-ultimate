using Serilog;

namespace dotnet_ultimate.Extensions;

public static class SerilogExtension
{
    public static void ConfigureSerilog(this IHostBuilder host)
    {
        // Approach 1: Configuration via appSettings.json
        host.UseSerilog((context, loggerConfiguration) =>
        {
            loggerConfiguration.WriteTo.Console();
            loggerConfiguration.ReadFrom.Configuration(context.Configuration);
        });

        // Approach 2: Configuration via Fluent API 
        // host.UseSerilog((context, logger) =>
        // {
        //     logger.MinimumLevel.Information();
        //     logger.WriteTo.Console();
        // });
    }
}