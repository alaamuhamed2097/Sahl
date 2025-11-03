//// File: Extensions/HangfireExtensions.cs
//namespace Api.Extensions
//{
//    public static class HangfireExtensions
//    {
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