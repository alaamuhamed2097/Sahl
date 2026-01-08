using Common.Filters;
using DAL.Models;
using Domains.Procedures;

namespace DAL.Contracts.Repositories.Customer
{
    public interface ICustomerRecommendedItemsRepository
    {
        /// <summary>
        /// Get Customer Recommended Items
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        Task<AdvancedPagedResult<SpGetCustomerRecommendedItems>> GetCustomerRecommendedItemsAsync(CustomerFilterQuery filter);
    }
}