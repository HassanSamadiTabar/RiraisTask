using System.Collections.ObjectModel;
using System.Data;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;

namespace RiraisTask.DependencyInjection;

public static class SerilogServiceExtensions
{
    public const string LogsTableName = "Logs";

    public static WebApplicationBuilder AddSerilogLogging(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((context, _, loggerConfiguration) =>
        {
            var connectionString = context.Configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");

            var columnOptions = new ColumnOptions();
            columnOptions.Store.Remove(StandardColumn.Properties);
            columnOptions.Store.Add(StandardColumn.LogEvent);
            columnOptions.LogEvent.ExcludeAdditionalProperties = true;
            columnOptions.LogEvent.ExcludeStandardColumns = true;

            columnOptions.AdditionalColumns = new Collection<SqlColumn>
            {
                new()
                {
                    ColumnName = "CorrelationId",
                    PropertyName = "CorrelationId",
                    DataType = SqlDbType.NVarChar,
                    DataLength = 64,
                    AllowNull = true
                },
                new()
                {
                    ColumnName = "GrpcMethod",
                    PropertyName = "GrpcMethod",
                    DataType = SqlDbType.NVarChar,
                    DataLength = 256,
                    AllowNull = true
                },
                new()
                {
                    ColumnName = "SourceContext",
                    PropertyName = "SourceContext",
                    DataType = SqlDbType.NVarChar,
                    DataLength = 256,
                    AllowNull = true
                }
            };

            loggerConfiguration
                .ReadFrom.Configuration(context.Configuration)
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithProperty("Application", "RiraisTask")
                .WriteTo.MSSqlServer(
                    connectionString,
                    sinkOptions: new MSSqlServerSinkOptions
                    {
                        TableName = LogsTableName,
                        AutoCreateSqlTable = false,
                        BatchPostingLimit = 50,
                        BatchPeriod = TimeSpan.FromSeconds(2)
                    },
                    columnOptions: columnOptions,
                    restrictedToMinimumLevel: LogEventLevel.Information);
        });

        return builder;
    }
}
