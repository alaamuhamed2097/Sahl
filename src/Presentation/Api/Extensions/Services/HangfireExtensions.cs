// File: Extensions/HangfireExtensions.cs
// NOTE: Hangfire configuration is currently commented out in the codebase
// Uncomment and customize when ready to use background job processing

//namespace Api.Extensions.Services
//{
//    /// <summary>
//    /// Extension methods for configuring Hangfire background job processing.
//    /// </summary>
//    public static class HangfireExtensions
//    {
//        /// <summary>
//        /// Adds Hangfire with SQL Server storage for background job processing.
//        /// </summary>
//        /// <param name="services">The IServiceCollection instance.</param>
//        /// <param name="configuration">The application configuration.</param>
//        /// <returns>The IServiceCollection for chaining.</returns>
//        public static IServiceCollection AddHangfireConfiguration(
//            this IServiceCollection services,
//            IConfiguration configuration)
//        {
//            services.AddHangfire(config =>
//            {
//                config.UseSimpleAssemblyNameTypeSerializer();
//                config.UseRecommendedSerializerSettings();
//                config.UseSqlServerStorage(configuration.GetConnectionString("DefaultConnection"));
//            });

//            services.AddHangfireServer(options =>
//            {
//                options.ServerName = "MyHangfireServer";
//                options.Queues = new[] { "default" };
//                options.WorkerCount = 1;
//            });

//            return services;
//        }
//    }
//}
