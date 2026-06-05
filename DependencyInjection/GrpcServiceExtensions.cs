using RiraisTask.Grpc;
using RiraisTask.Grpc.Interceptors;
using RiraisTask.Infrastructure;

namespace RiraisTask.DependencyInjection;

public static class GrpcServiceExtensions
{
    public static IServiceCollection AddGrpcPresentation(
        this IServiceCollection services,
        IHostEnvironment environment)
    {
        services.AddGrpc(options =>
        {
            options.Interceptors.Add<LoggingInterceptor>();
            options.Interceptors.Add<ExceptionInterceptor>();

            if (environment.IsDevelopment())
            {
                options.EnableDetailedErrors = true;
            }
        });

        services.AddGrpcHealthChecks()
            .AddDbContextCheck<AppDbContext>(name: "database");

        services.AddScoped<LoggingInterceptor>();
        services.AddScoped<ExceptionInterceptor>();

        if (environment.IsDevelopment())
        {
            services.AddGrpcReflection();
        }

        return services;
    }

    public static WebApplication MapGrpcEndpoints(this WebApplication app)
    {
        app.MapGrpcService<PeopleGrpcService>();
        app.MapGrpcHealthChecksService();
        app.MapHealthChecks("/health");

        if (app.Environment.IsDevelopment())
        {
            app.MapGrpcReflectionService();
        }

        return app;
    }
}
