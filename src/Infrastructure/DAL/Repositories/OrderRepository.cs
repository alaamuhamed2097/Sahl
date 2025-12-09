using DAL.ApplicationContext;
using DAL.Contracts.Repositories;
using Domains.Entities.Order;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace DAL.Repositories
{
    public class OrderRepository : TableRepository<TbOrder>, IOrderRepository
    {
        public OrderRepository(ApplicationDbContext dbContext, ILogger logger)
            : base(dbContext, logger)
        {
        }

        /// <summary>
        /// Find order by order number (case-sensitive)
        /// </summary>
        public async Task<TbOrder?> FindByNumberAsync(string orderNumber, CancellationToken cancellationToken = default)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(orderNumber))
                    return null;

                return await _dbContext.Set<TbOrder>()
                    .AsNoTracking()
                    .FirstOrDefaultAsync(
                        o => o.Number == orderNumber &&
                             o.IsDeleted == false,
                        cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in FindByNumberAsync for order number: {OrderNumber}", orderNumber);
                throw; // Re-throw to be handled by global exception handler
            }
        }

        /// <summary>
        /// Get paginated list of customer orders
        /// </summary>
        public async Task<List<TbOrder>> GetCustomerOrdersAsync(
            string customerId,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(customerId))
                    throw new ArgumentException("Customer ID cannot be empty.", nameof(customerId));

                if (pageNumber < 1) pageNumber = 1;
                if (pageSize < 1) pageSize = 10;
                if (pageSize > 100) pageSize = 100;

                var orders = await _dbContext.Set<TbOrder>()
                    .AsNoTracking()
                    .Where(o => o.UserId == customerId &&
                                o.IsDeleted == false)
                    .OrderByDescending(o => o.CreatedDateUtc)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync(cancellationToken);

                return orders ?? new List<TbOrder>();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in GetCustomerOrdersAsync for customer: {CustomerId}, Page: {Page}, Size: {PageSize}",
                    customerId, pageNumber, pageSize);
                throw;
            }
        }
    }
}