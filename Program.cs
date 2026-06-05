using Microsoft.EntityFrameworkCore;
using RiraisTask.Application;
using RiraisTask.Grpc;
using RiraisTask.Grpc.Interceptors;
using RiraisTask.Infrastructure;
using RiraisTask.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc(options =>
{
    options.Interceptors.Add<ExceptionInterceptor>();
});

builder.Services.AddGrpcHealthChecks()
    .AddDbContextCheck<AppDbContext>();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<IPersonRepository, PersonRepository>();
builder.Services.AddScoped<IPersonService, PersonService>();
builder.Services.AddScoped<ExceptionInterceptor>();

var app = builder.Build();

app.MapGrpcService<PeopleGrpcService>();
app.MapGrpcHealthChecksService();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
