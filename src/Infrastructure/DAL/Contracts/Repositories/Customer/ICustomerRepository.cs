using DAL.Contracts.Repositories;
using DAL.Models;
using Domains.Entities.ECommerceSystem.Customer;
using System.Linq.Expressions;

namespace DAL.Contracts.Repositories.Customer
{
    public interface ICustomerRepository : ITableRepository<TbCustomer>
    {
        /// <summary>
        /// Get customer by user ID
        /// </summary>
        Task<TbCustomer> GetCustomerByUserIdAsync(string userId);

        /// <summary>
        /// Get customer with user data (for search/list queries)
        /// </summary>
        Task<TbCustomer> FindByIdWithUserAsync(Guid id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get paginated customers with user data included
        /// </summary>
        Task<PagedResult<TbCustomer>> GetPageWithUserAsync(
            int pageNumber,
            int pageSize,
            Expression<Func<TbCustomer, bool>> filter = null,
            Func<IQueryable<TbCustomer>, IOrderedQueryable<TbCustomer>> orderBy = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Get queryable with user data included
        /// </summary>
        IQueryable<TbCustomer> GetQueryableWithUser();

        /// <summary>
        /// Get wallet transactions for a customer with pagination
        /// </summary>
        Task<PagedResult<dynamic>> GetWalletTransactionsAsync(
            Guid customerId,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default);
        Task<PagedResult<TbCustomer>> GetPageWithUserOrdersAndWalletsAsync
            (
            int pageNumber,
            int pageSize,
            Expression<Func<TbCustomer, bool>> filter = null,
            Func<IQueryable<TbCustomer>, IOrderedQueryable<TbCustomer>> orderBy = null,
            CancellationToken cancellationToken = default);

	}
}