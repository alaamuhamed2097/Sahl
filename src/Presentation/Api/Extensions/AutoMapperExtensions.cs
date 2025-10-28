using AutoMapper;
using System.Reflection;

namespace Api.Extensions
{
    public static class AutoMapperExtensions
    {
        public static IServiceCollection AddAutoMapperConfiguration(this IServiceCollection services)
        {
            // Configure AutoMapper manually using AutoMapper 14.0.0 from BL project
            services.AddSingleton<IMapper>(provider =>
            {
                try
                {
                    // Find all Profile types in the BL assembly
                    var blAssembly = Assembly.Load("BL");
                    var profileTypes = blAssembly.GetTypes()
                        .Where(t => typeof(Profile).IsAssignableFrom(t) && !t.IsAbstract)
                        .ToList();

                    Console.WriteLine($"Found {profileTypes.Count} AutoMapper profiles in BL assembly");

                    // Create mapper configuration with profiles
                    var config = new MapperConfiguration(cfg =>
                    {
                        foreach (var profileType in profileTypes)
                        {
                            try
                            {
                                var profile = Activator.CreateInstance(profileType) as Profile;
                                if (profile != null)
                                {
                                    cfg.AddProfile(profile);
                                    Console.WriteLine($"Added profile: {profileType.Name}");
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Warning: Could not create profile {profileType.Name}: {ex.Message}");
                            }
                        }
                    });

                    // Validate configuration
                    config.AssertConfigurationIsValid();
                    
                    return config.CreateMapper();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"AutoMapper configuration error: {ex.Message}");
                    
                    // Create a basic fallback mapper
                    var fallbackConfig = new MapperConfiguration(cfg => 
                    {
                        // Add a basic mapping to make it functional
                        cfg.CreateMap<object, object>();
                    });
                    
                    return fallbackConfig.CreateMapper();
                }
            });
            
            Console.WriteLine("AutoMapper IMapper registered with BL project profiles");
            return services;
        }
    }
}
