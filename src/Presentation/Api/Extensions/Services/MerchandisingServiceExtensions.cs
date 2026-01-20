using BL.Contracts.Service.HomePageSlider;
using BL.Contracts.Service.Merchandising;
using BL.Contracts.Service.Merchandising.Campaign;
using BL.Contracts.Service.Merchandising.CouponCode;
using BL.Contracts.Service.Merchandising.PromoCode;
using BL.Services.HomeSlider;
using BL.Services.Merchandising;
using BL.Services.Merchandising.Campaign;
using BL.Services.Merchandising.CouponCode;
using BL.Services.Merchandising.PromoCode;

namespace Api.Extensions.Services
{
    public static class MerchandisingServiceExtensions
    {
        public static IServiceCollection AddMerchandisingServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Merchandising Services
            services.AddScoped<IHomepageService, HomepageService>();
            services.AddScoped<ICampaignService, CampaignService>();
            services.AddScoped<ICampaignItemService, CampaignItemService>();

            // Promo code
            services.AddScoped<ICouponCodeService, CouponCodeService>();
            services.AddScoped<IVendorPromoCodeParticipationService, VendorPromoCodeParticipationService>();

            services.AddScoped<IHomePageSliderService, HomePageSliderService>();
            services.AddScoped<IAdminBlockService, AdminBlockService>();

            return services;
        }
    }
}
