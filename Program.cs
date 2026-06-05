using RiraisTask.DependencyInjection;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.AddSerilogLogging();
    builder.Services.AddApplication();
    builder.Services.AddInfrastructure(builder.Configuration);
    builder.Services.AddGrpcPresentation(builder.Environment);

    var app = builder.Build();

    await app.ApplyDatabaseMigrationsAsync();

    app.MapGrpcEndpoints();
    app.MapGet("/", () => "RiraisTask gRPC server is running. Use a gRPC client to call PeopleService.");

    Log.Information("RiraisTask gRPC server started");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
    throw;
}
finally
{
    await Log.CloseAndFlushAsync();
}
