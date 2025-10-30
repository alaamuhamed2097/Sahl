namespace Api.Extensions
{
    public static class AutoMapperExtensions
    {
        public static IServiceCollection AddAutoMapperConfiguration(this IServiceCollection services)
        {
            // This automatically finds and registers all Profile classes
            services.AddAutoMapper(typeof(BL.Mapper.MappingProfile));

            Console.WriteLine("AutoMapper registered with BL project profiles");
            return services;
        }
    }
}
