using Microsoft.OpenApi;
using System.Reflection;

namespace Api.Extensions
{
    public static class SwaggerExtensions
    {
        public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Sahl API",
                    Version = "v1",
                    Description = "API for Sahl Project"
                });

                // Add XML comments for better documentation
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (File.Exists(xmlPath))
                {
                    options.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
                }

                // Include XML comments for referenced assemblies (DTOs, etc.)
                var sharedXmlFile = "Shared.xml";
                var sharedXmlPath = Path.Combine(AppContext.BaseDirectory, sharedXmlFile);
                if (File.Exists(sharedXmlPath))
                {
                    options.IncludeXmlComments(sharedXmlPath);
                }
            });

            return services;
        }

        public static IApplicationBuilder UseSwaggerConfiguration(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Sahl API v1");
                options.RoutePrefix = "swagger";
            });

            return app;
        }
    }
}
