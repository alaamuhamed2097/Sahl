using BL.Contracts.Service.VendorDashboard;
using BL.Services.VendorDashboard;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Extensions.Services;

/// <summary>
/// Extension methods for Vendor Performance Indicators service registration
/// </summary>
public static class VendorPerformanceIndicatorsServiceExtensions
{
    /// <summary>
    /// Adds Vendor Performance Indicators service to the dependency injection container
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddVendorPerformanceIndicatorsService(
        this IServiceCollection services)
    {
        if (services == null)
            throw new ArgumentNullException(nameof(services));

        services.AddScoped<IVendorPerformanceIndicatorsService, VendorPerformanceIndicatorsService>();

        return services;
    }
}
