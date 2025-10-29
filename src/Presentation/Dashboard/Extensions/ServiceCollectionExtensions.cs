namespace Dashboard.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddAuthenticationServices();
            services.AddDomainServices();
            services.AddGeneralServices();

            // Add more groupings here later like AddPaymentServices(), etc.

            return services;
        }
    }
}
