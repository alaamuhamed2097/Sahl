namespace Api.Extensions
{
    public static class InfrastructureExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            // Add memory cache
            services.AddMemoryCache();

            // Add HTTP client
            services.AddHttpClient();

            // Add SignalR
            services.AddSignalR();

            return services;
        }

    }
}
