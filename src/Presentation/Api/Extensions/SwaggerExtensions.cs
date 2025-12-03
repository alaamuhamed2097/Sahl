using System.Reflection;
using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Api.Extensions
{
    public static class SwaggerExtensions
    {
        public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
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

    /// <summary>
    /// Configures swagger generation options for API versioning support.
    /// </summary>
    public class ConfigureSwaggerOptions : IConfigureNamedOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _provider;

        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
        {
            _provider = provider;
        }

        public void Configure(string name, SwaggerGenOptions options)
        {
            Configure(options);
        }

        public void Configure(SwaggerGenOptions options)
        {
            // Generate swagger document for each API version
            foreach (var description in _provider.ApiVersionDescriptions)
            {
                // Create a swagger doc for each version
                // The OpenApiInfo will be generated dynamically by Swagger based on the groupName
                options.SwaggerDoc(description.GroupName, null);
            }
        }
    }
}
