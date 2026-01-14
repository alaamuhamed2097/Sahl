using Dashboard.Contracts;
using Dashboard.Contracts.Brand;
using Dashboard.Contracts.Campaign;
using Dashboard.Contracts.Content;
using Dashboard.Contracts.Currency;
using Dashboard.Contracts.Customer;
using Dashboard.Contracts.ECommerce.Category;
using Dashboard.Contracts.ECommerce.Item;
using Dashboard.Contracts.General;
using Dashboard.Contracts.HomePageSlider;
using Dashboard.Contracts.Location;
using Dashboard.Contracts.Merchandising;
using Dashboard.Contracts.Order;
using Dashboard.Contracts.Page;
using Dashboard.Contracts.Review;
using Dashboard.Contracts.Setting;
using Dashboard.Contracts.User;
using Dashboard.Contracts.Vendor;
using Dashboard.Contracts.Warehouse;
using Dashboard.Contracts.WithdrawalMethod;
using Dashboard.Services;
using Dashboard.Services.Brand;
using Dashboard.Services.Campaign;
using Dashboard.Services.Content;
using Dashboard.Services.Currency;
using Dashboard.Services.Customer;
using Dashboard.Services.ECommerce.Category;
using Dashboard.Services.ECommerce.Item;
using Dashboard.Services.General;
using Dashboard.Services.HomePageSlider;
using Dashboard.Services.Location;
using Dashboard.Services.Merchandising;
using Dashboard.Services.Order;
using Dashboard.Services.Page;
using Dashboard.Services.Review;
using Dashboard.Services.Setting;
using Dashboard.Services.User;
using Dashboard.Services.Vendor;
using Dashboard.Services.Warehouse;
using Dashboard.Services.WithdrawalMethod;

namespace Dashboard.Extensions
{
    public static class DomainServiceExtensions
    {
        public static IServiceCollection AddDomainServices(this IServiceCollection services)
        {
            services.AddScoped<IBrandService, BrandService>();
            services.AddScoped<IVendorService, VendorService>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IItemService, ItemService>();
            services.AddScoped<IAttributeService, AttributeService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IRefundService, RefundService>();
            services.AddScoped<IUnitService, UnitService>();
            services.AddScoped<ICountryService, CountryService>();
            services.AddScoped<IStateService, StateService>();
            services.AddScoped<ICityService, CityService>();
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<ICouponCodeService, CouponCodeService>();
            services.AddScoped<IShippingCompanyService, ShippingCompanyService>();
            services.AddScoped<ICountryPhoneCodeService, CountryPhoneCodeService>();
            services.AddScoped<ICurrencyService, CurrencyService>();
            services.AddScoped<IPageService, PageService>();

            // Warehouse & Inventory Services
            services.AddScoped<IWarehouseService, WarehouseService>();

            // Content Management Services
            services.AddScoped<IContentAreaService, ContentAreaService>();
            services.AddScoped<IMediaContentService, MediaContentService>();

            services.AddScoped<ICampaignService, CampaignService>();
            services.AddScoped<IAdminBlockService, AdminBlockService>();
            services.AddScoped<IVendorPromoCodeParticipationAdminService, VendorPromoCodeParticipationAdminService>();
            services.AddScoped<IItemReviewService, ItemReviewService>();
            services.AddScoped<IReportReviewService, ReportReviewService>();
            services.AddScoped<IHomePageSliderService, HomePageSliderService>();
            services.AddScoped<ISystemSettingsService, SystemSettingsService>();
            // Excel Template Service
            services.AddScoped<ExcelTemplateService>();

            services.AddScoped<IWithdrawalMethodService, WithdrawalMethodService>();

            // Excel Template Service
            services.AddScoped<ExcelTemplateService>();

            return services;
        }
    }
}
