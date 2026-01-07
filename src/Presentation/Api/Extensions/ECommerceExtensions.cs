using BL.Contracts.GeneralService.Location;
using BL.Contracts.Service.Brand;
using BL.Contracts.Service.Catalog.Category;
using BL.Contracts.Service.Catalog.Item;
using BL.Contracts.Service.Catalog.Pricing;
using BL.Contracts.Service.Catalog.Unit;
using BL.Contracts.Service.Currency;
using BL.Contracts.Service.Customer;
using BL.Contracts.Service.Customer.Wishlist;
using BL.Contracts.Service.HomePageSlider;
using BL.Contracts.Service.Merchandising;
using BL.Contracts.Service.Merchandising.Campaign;
using BL.Contracts.Service.Merchandising.CouponCode;
using BL.Contracts.Service.Order.Cart;
using BL.Contracts.Service.Order.Checkout;
using BL.Contracts.Service.Order.Fulfillment;
using BL.Contracts.Service.Order.OrderProcessing;
using BL.Contracts.Service.Order.Payment;
using BL.Contracts.Service.Pricing;
using BL.Contracts.Service.Review;
using BL.Contracts.Service.Setting;
using BL.Contracts.Service.ShippingCompny;
using BL.Contracts.Service.Vendor;
using BL.Contracts.Service.VendorItem;
using BL.Contracts.Service.Wallet.Customer;
using BL.GeneralService.Location;
using BL.Services.Brand;
using BL.Services.Catalog.Category;
using BL.Services.Catalog.Item;
using BL.Services.Catalog.Pricing;
using BL.Services.Catalog.Unit;
using BL.Services.Currency;
using BL.Services.Customer;
using BL.Services.Customer.Wishlist;
using BL.Services.HomeSlider;
using BL.Services.Merchandising;
using BL.Services.Merchandising.Campaign;
using BL.Services.Merchandising.CouponCode;
using BL.Services.Order.Cart;
using BL.Services.Order.Checkout;
using BL.Services.Order.Fulfillment;
using BL.Services.Order.OrderProcessing;
using BL.Services.Order.Payment;
using BL.Services.Review;
using BL.Services.Setting;
using BL.Services.Setting.Pricing;
using BL.Services.ShippingCompany;
using BL.Services.Vendor;
using BL.Services.VendorItem;
using BL.Services.Wallet.Customer;

namespace Api.Extensions
{
    public static class ECommerceExtensions
    {
        public static IServiceCollection AddECommerceConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            // General Application Services

            // Payment Services
            services.AddScoped<IPaymentService, PaymentService>();

            // Vendor Service
            services.AddScoped<IVendorService, VendorService>();

            // Category Services
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IAttributeService, AttributeService>();

            // Item  Services
            services.AddScoped<IItemService, ItemService>();
            services.AddScoped<IItemSearchService, ItemSearchService>();
            services.AddScoped<IUnitService, UnitService>();

            // Custom Items Services
            services.AddScoped<ICustomerItemViewService, CustomerItemViewService>();
            services.AddScoped<ICustomerRecommendedItemsService, CustomerRecommendedItemsService>();

            // Vendor Items Services
            services.AddScoped<IVendorItemService, VendorItemService>();

            // Register services
            services.AddScoped<IItemDetailsService, ItemDetailsService>();

            // Brand Services
            services.AddScoped<IBrandService, BrandService>();

            // Currency Services
            services.AddScoped<ICurrencyService, CurrencyService>();
            services.AddScoped<ICurrencyConversionFactory, CurrencyConversionFactory>();
            services.AddScoped<ILocationBasedCurrencyService, LocationBasedCurrencyService>();

            // shipping company
            services.AddScoped<IShippingCalculationService, ShippingCalculationService>();
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
            services.AddScoped<IItemReviewService, ItemReviewService>();
            services.AddScoped<IReviewReportService, ReviewReportService>();
            services.AddScoped<IReviewVoteService, ReviewVoteService>();
            services.AddScoped<IVendorReviewService, VendorReviewService>();

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
            services.AddScoped<IHomePageSliderService, HomePageSliderService>();
            services.AddScoped<IAdminBlockService, AdminBlockService>();

            // Wallet Services
            services.AddScoped<ICustomerWalletService, CustomerWalletService>();
            services.AddScoped<ICustomerWalletTransactionService, CustomerWalletTransactionService>();
            services.AddScoped<IWalletSettingService, WalletSettingService>();

            services.AddScoped<ISystemSettingsService, SystemSettingsService>();

            services.AddScoped<ICheckoutService, CheckoutService>();
            return services;
        }
    }
}
