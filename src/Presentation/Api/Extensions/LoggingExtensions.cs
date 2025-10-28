using Serilog;
using Serilog.Sinks.MSSqlServer;
using System.Collections.ObjectModel;
using System.Data;

namespace Api.Extensions
{
    public static class LoggingExtensions
    {
        public static IServiceCollection AddSerilogConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            // Configure column options to match your existing table structure
            var columnOptions = new ColumnOptions
            {
                AdditionalColumns = new Collection<SqlColumn>
                {
                    new SqlColumn { ColumnName = "UserId", DataType = SqlDbType.NVarChar, DataLength = -1, AllowNull = true }
                }
            };

            // Enable self-logging to see Serilog internal errors
            Serilog.Debugging.SelfLog.Enable(msg => Console.WriteLine($"SERILOG ERROR: {msg}"));

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.Hosting.Lifetime", Serilog.Events.LogEventLevel.Information)
                .MinimumLevel.Override("Hangfire", Serilog.Events.LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .Enrich.WithProperty("Application", "MultiLevelMarketing")
                .WriteTo.Console(
                    outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}")
                .WriteTo.MSSqlServer(
                    connectionString: connectionString,
                    sinkOptions: new MSSqlServerSinkOptions
                    {
                        TableName = "Log",
                        SchemaName = "dbo",
                        AutoCreateSqlTable = false,
                        BatchPostingLimit = 50,
                        BatchPeriod = TimeSpan.FromSeconds(5)
                    },
                    columnOptions: columnOptions,
                    restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information)
                .CreateLogger();

            // Register Serilog.ILogger for dependency injection
            services.AddSingleton<Serilog.ILogger>(Log.Logger);

            // Test the configuration
            Log.Information("Serilog configured successfully - Application starting");
            Log.Warning("Test warning message for database logging");
            Log.Error("Test error message for database logging");

            return services;
        }

        public static IHostBuilder AddSerilogHost(this IHostBuilder hostBuilder)
        {
            return hostBuilder.UseSerilog();
        }
    }
}
