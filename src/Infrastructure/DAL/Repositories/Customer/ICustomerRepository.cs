using DAL.Contracts.Repositories;
using Domains.Entities.ECommerceSystem.Customer;

namespace DAL.Repositories.Customer
{
    public interface ICustomerRepository : ITableRepository<TbCustomer>
    {
        Task<TbCustomer> GetCustomerByUserIdAsync(string userId);
    }
}