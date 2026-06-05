using RiraisTask.Application;
using RiraisTask.Grpc.Interceptors;

namespace RiraisTask.DependencyInjection;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IPersonService, PersonService>();
        return services;
    }
}
