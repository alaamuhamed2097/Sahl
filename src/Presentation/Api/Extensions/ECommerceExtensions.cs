using BL.Contracts.GeneralService.Location;
using BL.Contracts.Service.Brand;
using BL.Contracts.Service.Catalog.Category;
using BL.Contracts.Service.Catalog.Item;
using BL.Contracts.Service.Catalog.Pricing;
using BL.Contracts.Service.Catalog.Unit;
using BL.Contracts.Service.CouponCode;
using BL.Contracts.Service.Currency;
using BL.Contracts.Service.Customer.Wishlist;
using BL.Contracts.Service.Merchandising;
using BL.Contracts.Service.Merchandising.Campaign;
using BL.Contracts.Service.Order.Cart;
using BL.Contracts.Service.Order.Checkout;
using BL.Contracts.Service.Order.OrderProcessing;
using BL.Contracts.Service.Pricing;
using BL.Contracts.Service.Review;
using BL.Contracts.Service.Setting;
using BL.Contracts.Service.ShippingCompny;
using BL.Contracts.Service.Vendor;
using BL.GeneralService.Location;
using BL.Services.Brand;
using BL.Services.Catalog.Category;
using BL.Services.Catalog.Item;
using BL.Services.Catalog.Pricing;
using BL.Services.Catalog.Unit;
using BL.Services.Currency;
using BL.Services.Customer.Wishlist;
using BL.Services.Merchandising;
using BL.Services.Merchandising.Campaign;
using BL.Services.Order.Cart;
using BL.Services.Order.Checkout;
using BL.Services.Order.OrderProcessing;
using BL.Services.PromoCode;
using BL.Services.Review;
using BL.Services.Setting;
using BL.Services.Setting.Pricing;
using BL.Services.ShippingCompany;
using BL.Services.Vendor;

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

            // Register services
            services.AddScoped<IItemDetailsService, ItemDetailsService>();

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
            services.AddScoped<IPricingSettingsService, PricingSettingsService>();

            // Order Services
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<IOrderMangmentService, OrderMangmentService>();
            services.AddScoped<ICustomerAddressService, CustomerAddressService>();

            services.AddScoped<IWishlistService, WishlistService>();

            // Review Services
            services.AddScoped<IOfferReviewService, OfferReviewService>();
            services.AddScoped<IReviewReportService, ReviewReportService>();
            services.AddScoped<IReviewVoteService, ReviewVoteService>();

            // Merchandising Services
            services.AddScoped<IHomepageService, HomepageService>();
            services.AddScoped<ICampaignService, CampaignService>();

            services.AddScoped<IPricingStrategy, SimplePricingStrategy>();
            services.AddScoped<IPricingStrategy, CombinationBasedPricingStrategy>();
            services.AddScoped<IPricingStrategy, QuantityBasedPricingStrategy>();
            services.AddScoped<IPricingStrategy, HybridPricingStrategy>();

            services.AddScoped<IPricingService, PricingService>();
            services.AddScoped<SimplePricingStrategy>();
            services.AddScoped<CombinationBasedPricingStrategy>();
            services.AddScoped<QuantityBasedPricingStrategy>();
            services.AddScoped<HybridPricingStrategy>();

            services.AddScoped<IHomepageService, HomepageService>();
            services.AddScoped<IAdminBlockService, AdminBlockService>();

            return services;
        }
    }
}
