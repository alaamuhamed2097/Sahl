namespace Api.Extensions
{
    public static class CorsExtensions
    {
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
