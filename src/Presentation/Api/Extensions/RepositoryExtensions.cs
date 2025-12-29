using BL.Contracts.IMapper;
using BL.Mapper.Base;
using DAL.Contracts.Repositories;
using DAL.Contracts.Repositories.Catalog.Item;
using DAL.Contracts.Repositories.Customer;
using DAL.Contracts.Repositories.Merchandising;
using DAL.Contracts.Repositories.Order;
using DAL.Contracts.Repositories.Review;
using DAL.Contracts.UnitOfWork;
using DAL.Repositories;
using DAL.Repositories.Catalog.Item;
using DAL.Repositories.Customer;
using DAL.Repositories.Merchandising;
using DAL.Repositories.Order;
using DAL.Repositories.Review;
using DAL.UnitOfWork;

namespace Api.Extensions
{
    public static class RepositoryExtensions
    {
        public static IServiceCollection AddRepositoryServices(this IServiceCollection services)
        {
            // Unit of Work pattern
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Generic repositories
            services.AddScoped(typeof(ITableRepository<>), typeof(TableRepository<>));
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            // Base mapper
            services.AddScoped(typeof(IBaseMapper), typeof(BaseMapper));

            services.AddScoped<ICartRepository, CartRepository>();
            services.AddScoped<IOfferRepository, OfferRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IItemSearchRepository, ItemSearchRepository>();

            // Customer repositories
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<ICustomerItemViewRepository, CustomerItemViewRepository>();
            services.AddScoped<IWishlistRepository, WishlistRepository>();

            // Register repositories
            services.AddScoped<IItemDetailsRepository, ItemDetailsRepository>();

            // Review repositories
            services.AddScoped<IItemReviewRepository, ItemReviewRepository>();
            services.AddScoped<IReviewReportRepository, ReviewReportRepository>();
            services.AddScoped<IReviewVoteRepository, ReviewVoteRepository>();
            services.AddScoped<IVendorReviewRepository, VendorReviewRepository>();

            // Marketing & Merchandising repositories
            services.AddScoped<IHomepageBlockRepository, HomepageBlockRepository>();
            services.AddScoped<ICampaignRepository, CampaignRepository>();
            services.AddScoped<ICouponCodeRepository, CouponCodeRepository>();

            services.AddScoped<IItemCombinationRepository, ItemCombinationRepository>();

            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IShipmentRepository, ShipmentRepository>();

            return services;
        }
    }
}
