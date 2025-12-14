using BL.Contracts.IMapper;
using BL.Mapper.Base;
using DAL.Contracts.Repositories;
using DAL.Contracts.UnitOfWork;
using DAL.Repositories;
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


            return services;
        }
    }
}
