using dotnet_ultimate.Services;

namespace dotnet_ultimate.Extensions;

public static class ServicesExtensions
{
    public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IDummyService, DummyService>();
    }
}