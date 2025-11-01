namespace Api.Extensions
{
    public static class CorsExtensions
    {
        public static IServiceCollection AddCorsConfiguration(this IServiceCollection services, IWebHostEnvironment environment)
        {
            if (environment.IsDevelopment())
            {
                services.AddCors(options =>
                {
                    // Policy for Blazor Dashboard
                    options.AddPolicy("AllowBlazorWasm", policy =>
                    {
                        policy.WithOrigins("https://localhost:7049", "https://localhost:7251", "https://sahl.itlegend.net")
                              .AllowAnyHeader()
                              .AllowAnyMethod()
                              .AllowCredentials();
                    });

                    // Policy for Next.js E-commerce (if running locally)
                    options.AddPolicy("AllowNextJs", policy =>
                    {
                        policy.WithOrigins("http://localhost:3000", "https://localhost:3000")
                              .AllowAnyHeader()
                              .AllowAnyMethod()
                              .AllowCredentials();
                    });

                    // Policy for Paymob payment gateway
                    options.AddPolicy("AllowPaymob", policy =>
                    {
                        policy.WithOrigins("https://accept.paymob.com")
                              .AllowAnyMethod()
                              .AllowAnyHeader();
                    });

                    // Allow all origins for development
                    options.AddPolicy("AllowAll", policy =>
                    {
                        policy.SetIsOriginAllowed(origin => true)
                              .AllowAnyHeader()
                              .AllowAnyMethod()
                              .AllowCredentials();
                    });
                });
            }
            else
            {
                services.AddCors(options =>
                {
                    // Policy for Blazor Dashboard (Production)
                    options.AddPolicy("AllowBlazorWasm", policy =>
                    {
                        policy.WithOrigins("https://dashboard.riseon-group.com", "https://ecommerce.itlegend.net")
                              .AllowAnyHeader()
                              .AllowAnyMethod()
                              .AllowCredentials();
                    });

                    // Policy for Next.js E-commerce (Production)
                    options.AddPolicy("AllowNextJs", policy =>
                    {
                        policy.WithOrigins("https://shop.riseon-group.com", "https://www.riseon-group.com")
                              .AllowAnyHeader()
                              .AllowAnyMethod()
                              .AllowCredentials();
                    });

                    // Policy for Paymob payment gateway
                    options.AddPolicy("AllowPaymob", policy =>
                    {
                        policy.WithOrigins("https://accept.paymob.com")
                              .AllowAnyMethod()
                              .AllowAnyHeader();
                    });

                    // Allow all origins for production (if needed)
                    options.AddPolicy("AllowAll", policy =>
                    {
                        policy.SetIsOriginAllowed(origin => true)
                              .AllowAnyHeader()
                              .AllowAnyMethod()
                              .AllowCredentials();
                    });
                });
            }

            return services;
        }
    }
}
