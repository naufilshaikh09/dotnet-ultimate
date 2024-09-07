using System.Reflection;
using dotnet_ultimate.Behaviors;

namespace dotnet_ultimate.Extensions;

public static class MediatRExtension
{
    public static void ConfigureMediatR(this IServiceCollection services)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            config.AddOpenBehavior(typeof(RequestResponseLoggingBehavior<,>));
            config.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });
    }
}