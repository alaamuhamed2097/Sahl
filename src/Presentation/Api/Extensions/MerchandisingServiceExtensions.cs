using BL.Contracts.Service.HomeSlider;
using BL.Contracts.Service.Merchandising;
using BL.Contracts.Service.Merchandising.Campaign;
using BL.Contracts.Service.Merchandising.CouponCode;
using BL.Contracts.Service.HomeSlider;
using BL.Services.HomeSlider;
using BL.Services.Merchandising;
using BL.Services.Merchandising.Campaign;
using BL.Services.Merchandising.CouponCode;

namespace Api.Extensions
{
    public static class MerchandisingServiceExtensions
    {
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
