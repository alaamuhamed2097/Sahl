using BL.Contracts.Service.HomePageSlider;
using BL.Contracts.Service.Merchandising;
using BL.Contracts.Service.Merchandising.Campaign;
using BL.Contracts.Service.Merchandising.CouponCode;
using BL.Services.HomeSlider;
using BL.Services.Merchandising;
using BL.Services.Merchandising.Campaign;
using BL.Services.Merchandising.CouponCode;

namespace Api.Extensions.Services
{
    /// <summary>
    /// Extension methods for registering merchandising and marketing services.
    /// </summary>
    public static class MerchandisingServiceExtensions
    {
        /// <summary>
        /// Adds merchandising, campaigns, and promotional services.
        /// </summary>
        /// <param name="services">The IServiceCollection instance.</param>
        /// <param name="configuration">The application configuration.</param>
        /// <returns>The IServiceCollection for chaining.</returns>
        public static IServiceCollection AddMerchandisingServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Merchandising Services
            services.AddScoped<IHomepageService, HomepageService>();
            services.AddScoped<ICampaignService, CampaignService>();

            // Promo code
            services.AddScoped<ICouponCodeService, CouponCodeService>();

            services.AddScoped<IHomePageSliderService, HomePageSliderService>();
            services.AddScoped<IAdminBlockService, AdminBlockService>();

            return services;
        }
    }
}
