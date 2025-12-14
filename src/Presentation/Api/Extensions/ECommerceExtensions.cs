using BL.Contracts.GeneralService.Location;
using BL.Contracts.Service.Brand;
using BL.Contracts.Service.CouponCode;
using BL.Contracts.Service.Currency;
using BL.Contracts.Service.ECommerce.Category;
using BL.Contracts.Service.ECommerce.Item;
using BL.Contracts.Service.ECommerce.Unit;
using BL.Contracts.Service.Pricing;
using BL.Contracts.Service.Review;
using BL.Contracts.Service.Setting;
using BL.Contracts.Service.ShippingCompny;
using BL.Contracts.Service.Vendor;
using BL.GeneralService.Location;
using BL.Service.Brand;
using BL.Service.Currency;
using BL.Service.ECommerce.Category;
using BL.Service.ECommerce.Item;
using BL.Service.ECommerce.Unit;
using BL.Service.Order;
using BL.Service.Pricing;
using BL.Service.PromoCode;
using BL.Service.Review;
using BL.Service.Setting;
using BL.Service.ShippingCompany;
using BL.Service.Vendor;
using BL.Services.Order;
using DAL.Contracts.Repositories.Review;
using DAL.Repositories.Review;

namespace Api.Extensions
{
    public static class ECommerceExtensions
    {
        public static IServiceCollection AddECommerceConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            // General Application Services

            // Vendor Service
            services.AddScoped<IVendorService, VendorService>();

            // Category Services
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IAttributeService, AttributeService>();

            // Item  Services
            services.AddScoped<IItemService, ItemService>();
            services.AddScoped<IItemSearchService, ItemSearchService>();
            services.AddScoped<IUnitService, UnitService>();

            // Brand Services
            services.AddScoped<IBrandService, BrandService>();

            // Currency Services
            services.AddScoped<ICurrencyService, CurrencyService>();
            services.AddScoped<ICurrencyConversionFactory, CurrencyConversionFactory>();
            services.AddScoped<ILocationBasedCurrencyService, LocationBasedCurrencyService>();

            // shipping company
            services.AddScoped<IShippingCompanyService, ShippingCompanyService>();

            // Promo code
            services.AddScoped<ICouponCodeService, CouponCodeService>();

            // Setting Services
            services.AddScoped<ISettingService, SettingService>();

            // Pricing Services
            services.AddScoped<IPricingService, PricingService>();

            // Order Services
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<IOrderService, OrderService>();

            // Register required repositories for CartService
            //services.AddScoped<ICartRepository, CartRepository>();
            //services.AddScoped<IOfferRepository, OfferRepository>();
            //services.AddScoped<IOrderRepository, OrderRepository>();

            // Review repositories
   //         services.AddScoped<IOfferReviewRepository, OfferReviewRepository>();
			//services.AddScoped<IReviewVoteRepository, ReviewVoteRepository>();
			//services.AddScoped<IReviewReportRepository, ReviewReportRepository>();

			// Review Services
			services.AddScoped<IOfferReviewService, OfferReviewService>();
			services.AddScoped<IReviewReportService, ReviewReportService>();
			services.AddScoped<IReviewVoteService, ReviewVoteService>();


			return services;
        }
    }
}
