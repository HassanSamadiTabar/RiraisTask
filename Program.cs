using RiraisTask.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddGrpcPresentation(builder.Environment);

var app = builder.Build();

await app.ApplyDatabaseMigrationsAsync();

app.MapGrpcEndpoints();
app.MapGet("/", () => "RiraisTask gRPC server is running. Use a gRPC client to call PeopleService.");

app.Run();
