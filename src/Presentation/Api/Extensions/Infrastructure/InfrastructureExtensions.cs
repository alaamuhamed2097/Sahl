namespace Api.Extensions.Infrastructure
{
    /// <summary>
    /// Extension methods for infrastructure services (caching, HTTP client, SignalR, etc.).
    /// </summary>
    public static class InfrastructureExtensions
    {
        /// <summary>
        /// Adds infrastructure services including memory cache, HTTP client, and SignalR.
        /// </summary>
        /// <param name="services">The IServiceCollection instance.</param>
        /// <returns>The IServiceCollection for chaining.</returns>
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            // Add memory cache
            services.AddMemoryCache();
            services.AddResponseCaching();

            // Add HTTP client
            services.AddHttpClient();

            // Add SignalR
            services.AddSignalR();

            return services;
        }
    }
}
