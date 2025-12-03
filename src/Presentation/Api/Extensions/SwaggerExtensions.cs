using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
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
                // Show the JSON link
                // ⬅ Show the JSON link in Swagger UI
                options.ConfigObject.AdditionalItems["urls"] = provider.ApiVersionDescriptions
                       .Select(desc => new {
                    url = $"/openapi/{desc.GroupName}.json",
                    name = $"OpenAPI JSON — {desc.GroupName.ToUpperInvariant()}"
                       }).ToList();
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

        public void Configure(string name, SwaggerGenOptions options) => Configure(options);

        public void Configure(SwaggerGenOptions options)
        {
            foreach (var description in _provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName, new OpenApiInfo
                {
                    Title = $"Sahl API {description.ApiVersion}",
                    Version = description.GroupName,
                    Description = "API Documentation for Sahl"
                });
            }
        }
    }

}
