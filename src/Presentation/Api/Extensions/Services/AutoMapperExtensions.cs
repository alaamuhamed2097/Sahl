using System.Reflection;

namespace Api.Extensions.Services
{
    /// <summary>
    /// Extension methods for configuring AutoMapper and mapping profiles.
    /// </summary>
    public static class AutoMapperExtensions
    {
        /// <summary>
        /// Adds AutoMapper with profiles from the BL assembly.
        /// </summary>
        /// <param name="services">The IServiceCollection instance.</param>
        /// <returns>The IServiceCollection for chaining.</returns>
        public static IServiceCollection AddAutoMapperConfiguration(this IServiceCollection services)
        {
            // This automatically finds and registers all Profile classes
            services.AddAutoMapper(cfg => { }, Assembly.GetAssembly(typeof(BL.Mapper.MappingProfile)));

            Console.WriteLine("AutoMapper registered with BL project profiles");
            return services;
        }
    }
}
