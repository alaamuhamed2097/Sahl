using BL.Contracts.GeneralService;
using DAL.ApplicationContext;
using DAL.Contracts.Repositories.Customer;
using DAL.Models;
using Domains.Entities.ECommerceSystem.Customer;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Linq.Expressions;

namespace DAL.Repositories.Customer
{
	/// <summary>
	/// Repository for Customer operations
	/// All operations include customer validation for security
	/// </summary>
	public class CustomerRepository : TableRepository<TbCustomer>, ICustomerRepository
	{
		public CustomerRepository(ApplicationDbContext dbContext, ICurrentUserService currentUserService, ILogger logger)
			: base(dbContext, currentUserService, logger)
		{
		}

		/// <summary>
		/// Get customer by user id
		/// </summary>
		/// <param name="userId">ApplicationUser ID</param>
		/// <returns></returns>
		/// <exception cref="ArgumentException"></exception>
		public async Task<TbCustomer> GetCustomerByUserIdAsync(string userId)
		{
			// Validate input
			if (string.IsNullOrEmpty(userId))
				throw new ArgumentException("User ID is required.", nameof(userId));

			// Get customer by user id with user data included
			var customer = await _dbContext.Set<TbCustomer>()
				.Include(c => c.User)
				.FirstOrDefaultAsync(c => c.UserId == userId && !c.IsDeleted);

			return customer;
		}

		/// <summary>
		/// Get customer with user data (for single queries)
		/// </summary>
		public async Task<TbCustomer> FindByIdWithUserAsync(Guid id, CancellationToken cancellationToken = default)
		{
			return await _dbContext.Set<TbCustomer>()
				.Include(c => c.User)
				.ThenInclude(u => u.Orders)
				.Include(c => c.User)
				.ThenInclude(u => u.CustomerWallets)
				.ThenInclude(w => w.Transactions)
				.FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted, cancellationToken);
		}

		/// <summary>
		/// Get paginated customers with user data included
		/// </summary>
		public async Task<PagedResult<TbCustomer>> GetPageWithUserAsync(
			int pageNumber,
			int pageSize,
			Expression<Func<TbCustomer, bool>> filter = null,
			Func<IQueryable<TbCustomer>, IOrderedQueryable<TbCustomer>> orderBy = null,
			CancellationToken cancellationToken = default)
		{
			var query = _dbContext.Set<TbCustomer>()
				.Include(c => c.User)
				.AsQueryable();

			if (filter != null)
				query = query.Where(filter);

			var totalRecords = await query.CountAsync(cancellationToken);

			if (orderBy != null)
				query = orderBy(query);
			else
				query = query.OrderByDescending(c => c.CreatedDateUtc);

			var items = await query
				.Skip((pageNumber - 1) * pageSize)
				.Take(pageSize)
				.ToListAsync(cancellationToken);

			return new PagedResult<TbCustomer>(items, totalRecords);
		}
		public async Task<PagedResult<TbCustomer>> GetPageWithUserOrdersAndWalletsAsync
			(
			int pageNumber,
			int pageSize,
			Expression<Func<TbCustomer, bool>> filter = null,
			Func<IQueryable<TbCustomer>, IOrderedQueryable<TbCustomer>> orderBy = null,
			CancellationToken cancellationToken = default)
			{
			var query = _dbContext.Set<TbCustomer>()
				.Include(c => c.User)
					.ThenInclude(u => u.Orders)
				.Include(c => c.User)
					.ThenInclude(u => u.CustomerWallets)
						.ThenInclude(w => w.Transactions)
				.AsQueryable();

			if (filter != null)
				query = query.Where(filter);

			var totalRecords = await query.CountAsync(cancellationToken);

			if (orderBy != null)
				query = orderBy(query);
			else
				query = query.OrderByDescending(c => c.CreatedDateUtc);

			var items = await query
				.Skip((pageNumber - 1) * pageSize)
				.Take(pageSize)
				.ToListAsync(cancellationToken);

			return new PagedResult<TbCustomer>(items, totalRecords);
		}
		/// <summary>
		/// Get queryable with user data included
		/// </summary>
		public IQueryable<TbCustomer> GetQueryableWithUser()
		{
			return _dbContext.Set<TbCustomer>()
				.Include(c => c.User)
				.AsQueryable();
		}

		/// <summary>
		/// Get wallet transactions for a customer with pagination
		/// </summary>
		public async Task<PagedResult<dynamic>> GetWalletTransactionsAsync(
			Guid customerId,
			int pageNumber,
			int pageSize,
			CancellationToken cancellationToken = default)
		{
			var customer = await _dbContext.Set<TbCustomer>()
				.Include(c => c.User)
				.FirstOrDefaultAsync(c => c.Id == customerId && !c.IsDeleted, cancellationToken);

			if (customer?.User == null)
				return new PagedResult<dynamic>(new List<dynamic>(), 0);

			// Get wallet transactions directly from the context
			var transactionsQuery = _dbContext.Set<Domains.Entities.Wallet.Customer.TbCustomerWalletTransaction>()
				.Where(t => t.Wallet.UserId == customer.UserId)
				.AsQueryable();

			var totalRecords = await transactionsQuery.CountAsync(cancellationToken);

			var transactions = await transactionsQuery
				.OrderByDescending(t => t.CreatedDateUtc)
				.Skip((pageNumber - 1) * pageSize)
				.Take(pageSize)
				.Cast<dynamic>()
				.ToListAsync(cancellationToken);

			return new PagedResult<dynamic>(transactions, totalRecords);
		}
	}
}