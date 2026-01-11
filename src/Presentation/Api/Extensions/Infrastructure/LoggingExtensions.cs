using Serilog;
using Serilog.Sinks.MSSqlServer;
using System.Collections.ObjectModel;
using System.Data;

namespace Api.Extensions.Infrastructure
{
    /// <summary>
    /// Extension methods for configuring Serilog logging.
    /// </summary>
    public static class LoggingExtensions
    {
        /// <summary>
        /// Configures Serilog with console and SQL Server sinks.
        /// </summary>
        /// <param name="services">The IServiceCollection instance.</param>
        /// <param name="configuration">The application configuration.</param>
        /// <returns>The IServiceCollection for chaining.</returns>
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
                .Enrich.WithProperty("Application", "Sahl")
                .WriteTo.Console(
                    outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}")
                .WriteTo.MSSqlServer(
                    connectionString: connectionString,
                    sinkOptions: new MSSqlServerSinkOptions
                    {
                        TableName = "Log",
                        SchemaName = "dbo",
                        // Allow sink to create the table if it does not exist (helps design-time/migrations)
                        AutoCreateSqlTable = true,
                        BatchPostingLimit = 50,
                        BatchPeriod = TimeSpan.FromSeconds(5)
                    },
                    columnOptions: columnOptions,
                    restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information)
                .CreateLogger();

            // Register Serilog.ILogger for dependency injection
            services.AddSingleton<Serilog.ILogger>(Log.Logger);

            // Avoid emitting test log events here because the Log table might not yet exist (EF migrations / design-time).
            // If you need to verify logging, use the console output or ensure the Log table exists before running.

            return services;
        }

        /// <summary>
        /// Adds Serilog as the logging provider for the host builder.
        /// </summary>
        /// <param name="hostBuilder">The IHostBuilder instance.</param>
        /// <returns>The IHostBuilder for chaining.</returns>
        public static IHostBuilder AddSerilogHost(this IHostBuilder hostBuilder)
        {
            return hostBuilder.UseSerilog();
        }
    }
}
