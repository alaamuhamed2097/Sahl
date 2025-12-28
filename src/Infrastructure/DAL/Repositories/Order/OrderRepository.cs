using DAL.ApplicationContext;
using DAL.Contracts.Repositories.Order;
using DAL.Models;
using Domains.Entities.Order;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace DAL.Repositories.Order
{
    /// <summary>
    /// FIXED: Corrected based on actual entity structure
    /// </summary>
    public class OrderRepository : TableRepository<TbOrder>, IOrderRepository
    {
        public OrderRepository(ApplicationDbContext dbContext, ILogger logger)
            : base(dbContext, logger)
        {
        }

        public async Task<TbOrder?> GetOrderWithDetailsAsync(
            Guid orderId,
            CancellationToken cancellationToken = default)
        {
            try
            {
                return await _dbContext.Set<TbOrder>()
                    .AsNoTracking()
                    .Where(o => o.Id == orderId && !o.IsDeleted)
                    .Include(o => o.User)
                    .Include(o => o.CustomerAddress)
                        .ThenInclude(a => a.City)
                            .ThenInclude(c => c.State) // ✅ State instead of Governorate
                    .Include(o => o.Coupon)
                    .Include(o => o.OrderDetails)
                        .ThenInclude(od => od.Item)
                            .ThenInclude(i => i.ItemImages) // ✅ ItemImages not Images
                    .Include(o => o.OrderDetails)
                        .ThenInclude(od => od.Item)
                            .ThenInclude(i => i.ItemCombinations) // ✅ Include combinations
                    .Include(o => o.OrderDetails)
                        .ThenInclude(od => od.Vendor)
                    .Include(o => o.OrderDetails)
                        .ThenInclude(od => od.OfferCombinationPricing)
                    .FirstOrDefaultAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting order with details for order {OrderId}", orderId);
                throw;
            }
        }

        public async Task<TbOrder?> GetOrderWithShipmentsAsync(
            Guid orderId,
            CancellationToken cancellationToken = default)
        {
            try
            {
                return await _dbContext.Set<TbOrder>()
                    .AsNoTracking()
                    .Where(o => o.Id == orderId && !o.IsDeleted)
                    .Include(o => o.User)
                    .Include(o => o.CustomerAddress)
                        .ThenInclude(a => a.City)
                            .ThenInclude(c => c.State)
                    .Include(o => o.OrderDetails)
                        .ThenInclude(od => od.Item)
                            .ThenInclude(i => i.ItemImages)
                    .Include(o => o.OrderDetails)
                        .ThenInclude(od => od.Vendor)
                    .FirstOrDefaultAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting order with shipments for order {OrderId}", orderId);
                throw;
            }
        }

        public async Task<PagedResult<TbOrder>> GetCustomerOrdersPagedAsync(
            string customerId,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var query = _dbContext.Set<TbOrder>()
                    .AsNoTracking()
                    .Where(o => o.UserId == customerId && !o.IsDeleted)
                    .Include(o => o.OrderDetails)
                        .ThenInclude(od => od.Item)
                            .ThenInclude(i => i.ItemImages) // ✅ ItemImages not Images
                    .OrderByDescending(o => o.CreatedDateUtc);

                var totalCount = await query.CountAsync(cancellationToken);

                var orders = await query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync(cancellationToken);

                return new PagedResult<TbOrder>(orders, totalCount);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting paged orders for customer {CustomerId}", customerId);
                throw;
            }
        }
    }
}