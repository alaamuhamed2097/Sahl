namespace Api.Extensions.Infrastructure
{
    /// <summary>
    /// Extension methods for configuring CORS (Cross-Origin Resource Sharing) policies.
    /// </summary>
    public static class CorsExtensions
    {
        /// <summary>
        /// Adds CORS configuration with allowed origins and methods.
        /// </summary>
        /// <param name="services">The IServiceCollection instance.</param>
        /// <param name="environment">The web host environment.</param>
        /// <returns>The IServiceCollection for chaining.</returns>
        public static IServiceCollection AddCorsConfiguration(this IServiceCollection services, IWebHostEnvironment environment)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("DefaultCors", policy =>
                {
                    policy.WithOrigins(
                            // Local
                            "https://localhost:7049",
                            "https://localhost:7251",
                            "https://localhost:7282",
                            "http://localhost:3000",
                            "https://localhost:3000",

                            // Production
                            "https://sahl.itlegend.net",
                            "https://sahldashboard.itlegend.net",
                            "https://basit-demo.vercel.app",
                            // Paymob
                            "https://accept.paymob.com"
                        )
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });

            return services;
        }
    }
}
