using DAL.Contracts.Repositories;
using Domains.Entities.ECommerceSystem.Customer;

namespace DAL.Contracts.Repositories.Customer
{
    public interface ICustomerItemViewRepository : ITableRepository<TbCustomerItemView>
    {
        Task<IEnumerable<TbCustomerItemView>> GetAllCustomerViewsAsync(Guid customerId);
        Task<IEnumerable<TbCustomerItemView>> GetAllItemCombinationViewsAsync(Guid itemCombinationId);
        Task<IEnumerable<TbCustomerItemView>> GetAllItemViewsAsync(Guid itemId);
        Task<int> GetItemCombinationViewsCountAsync(Guid itemCombinationId);
    }
}