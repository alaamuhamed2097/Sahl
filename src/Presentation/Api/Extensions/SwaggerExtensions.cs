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
                // Note: XML documentation is currently disabled due to OpenAPI source generator issues in .NET 10
                // Uncomment the following lines once the issue is fixed
                
                // var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                // var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                // if (File.Exists(xmlPath))
                // {
                //     options.IncludeXmlComments(xmlPath);
                // }

                // // Optional: Include XML comments for referenced assemblies (DTOs, etc.)
                // var sharedXmlFile = "Shared.xml";
                // var sharedXmlPath = Path.Combine(AppContext.BaseDirectory, sharedXmlFile);
                // if (File.Exists(sharedXmlPath))
                // {
                //     options.IncludeXmlComments(sharedXmlPath);
                // }
            });

            return services;
        }
    }
}
