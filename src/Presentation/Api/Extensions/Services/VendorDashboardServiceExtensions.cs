using BL.Contracts.Service.VendorDashboard;
using BL.Services.VendorDashboard;

namespace Api.Extensions.Services;

/// <summary>
/// Extension methods for registering vendor dashboard services.
/// </summary>
public static class VendorDashboardServiceExtensions
{
    /// <summary>
    /// Adds vendor dashboard analytics and KPI metrics service.
    /// </summary>
    /// <param name="services">The IServiceCollection instance.</param>
    /// <param name="configuration">The application configuration.</param>
    /// <returns>The IServiceCollection for chaining.</returns>
    public static IServiceCollection AddVendorDashboardServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<IVendorDashboardService, VendorDashboardService>();

        return services;
    }
}
