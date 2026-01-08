using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace Api.Extensions.Infrastructure
{
    /// <summary>
    /// Extension methods for configuring Swagger/OpenAPI documentation.
    /// </summary>
    public static class SwaggerExtensions
    {
        /// <summary>
        /// Adds Swagger/OpenAPI generation with JWT bearer security and XML documentation.
        /// </summary>
        /// <param name="services">The IServiceCollection instance.</param>
        /// <returns>The IServiceCollection for chaining.</returns>
        public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                // Define Bearer security scheme
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT"
                });

                // Apply Bearer security globally using OpenApiSecuritySchemeReference
                options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
                {
                    [new OpenApiSecuritySchemeReference("Bearer", document)] = []
                });

                // Add XML comments for better documentation
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (File.Exists(xmlPath))
                {
                    options.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
                }

                // Include XML comments for referenced assemblies
                var sharedXmlFile = "Shared.xml";
                var sharedXmlPath = Path.Combine(AppContext.BaseDirectory, sharedXmlFile);
                if (File.Exists(sharedXmlPath))
                {
                    options.IncludeXmlComments(sharedXmlPath);
                }
            });

            services.ConfigureOptions<ConfigureSwaggerOptions>();

            return services;
        }

        /// <summary>
        /// Configures Swagger UI middleware for viewing API documentation.
        /// </summary>
        /// <param name="app">The IApplicationBuilder instance.</param>
        /// <param name="provider">The API version description provider.</param>
        /// <returns>The IApplicationBuilder for chaining.</returns>
        public static IApplicationBuilder UseSwaggerConfiguration(this IApplicationBuilder app, IApiVersionDescriptionProvider provider)
        {
            app.UseSwagger(options =>
            {
                options.RouteTemplate = "openapi/{documentName}.json";
            });

            app.UseSwaggerUI(options =>
            {
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint(
                        $"/openapi/{description.GroupName}.json",
                        description.GroupName.ToUpperInvariant()
                    );
                }

                options.RoutePrefix = "swagger";
                options.DefaultModelsExpandDepth(-1);
                options.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
                options.EnableDeepLinking();
                options.DisplayRequestDuration();
                options.EnableFilter();
                options.ShowExtensions();
                options.EnableValidator();
            });

            return app;
        }
    }
}
