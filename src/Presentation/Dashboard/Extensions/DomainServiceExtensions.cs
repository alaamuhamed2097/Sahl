using Dashboard.Contracts;
using Dashboard.Contracts.Brand;
using Dashboard.Contracts.Content;
using Dashboard.Contracts.Currency;
using Dashboard.Contracts.Customer;
using Dashboard.Contracts.ECommerce.Category;
using Dashboard.Contracts.ECommerce.Item;
using Dashboard.Contracts.General;
using Dashboard.Contracts.HomePageSlider;
using Dashboard.Contracts.Location;
using Dashboard.Contracts.Order;
using Dashboard.Contracts.Review;
using Dashboard.Contracts.User;
using Dashboard.Contracts.Vendor;
using Dashboard.Contracts.Warehouse;
using Dashboard.Services;
using Dashboard.Services.Brand;
using Dashboard.Services.Content;
using Dashboard.Services.Currency;
using Dashboard.Services.Customer;
using Dashboard.Services.ECommerce.Category;
using Dashboard.Services.ECommerce.Item;
using Dashboard.Services.General;
using Dashboard.Services.HomePageSlider;
using Dashboard.Services.Location;
using Dashboard.Services.Order;
using Dashboard.Services.Review;
using Dashboard.Services.User;
using Dashboard.Services.Vendor;
using Dashboard.Services.Warehouse;

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

            // Warehouse & Inventory Services
            services.AddScoped<IWarehouseService, WarehouseService>();

            // Content Management Services
            services.AddScoped<IContentAreaService, ContentAreaService>();
            services.AddScoped<IMediaContentService, MediaContentService>();

			services.AddScoped<IItemReviewService, ItemReviewService>();
			services.AddScoped<IReportReviewService, ReportReviewService>();
			services.AddScoped<IHomePageSliderService, HomePageSliderService>();

			// Excel Template Service
			services.AddScoped<ExcelTemplateService>();

            return services;
        }
    }
}
