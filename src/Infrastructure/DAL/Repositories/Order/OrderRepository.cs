using BL.Contracts.GeneralService;
using Common.Enumerations.Order;
using Common.Enumerations.Payment;
using DAL.ApplicationContext;
using DAL.Contracts.Repositories.Order;
using DAL.Models;
using Domains.Entities.Order;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace DAL.Repositories.Order
{
    public class OrderRepository : TableRepository<TbOrder>, IOrderRepository
    {
        public OrderRepository(ApplicationDbContext dbContext, ICurrentUserService currentUserService, ILogger logger)
            : base(dbContext, currentUserService, logger)
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

        /// <summary>
        /// Get order by invoice ID
        /// </summary>
        public async Task<TbOrder?> GetByInvoiceIdAsync(
            string invoiceId,
            CancellationToken cancellationToken = default)
        {
            try
            {
                return await _dbContext.Set<TbOrder>()
                    .AsNoTracking()
                    .Where(o => o.InvoiceId == invoiceId && !o.IsDeleted)
                    .FirstOrDefaultAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting order by invoice ID {InvoiceId}", invoiceId);
                throw;
            }
        }

        /// <summary>
        /// Get order by ID (simple version without includes)
        /// </summary>
        public async Task<TbOrder?> GetByIdAsync(
            Guid orderId,
            CancellationToken cancellationToken = default)
        {
            try
            {
                return await _dbContext.Set<TbOrder>()
                    .AsNoTracking()
                    .Where(o => o.Id == orderId && !o.IsDeleted)
                    .FirstOrDefaultAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting order {OrderId}", orderId);
                throw;
            }
        }

        /// <summary>
        /// Create new order
        /// </summary>
        public async Task<TbOrder> CreateAsync(
            TbOrder order,
            CancellationToken cancellationToken = default)
        {
            try
            {
                order.CreatedDateUtc = DateTime.UtcNow;
                order.IsDeleted = false;

                await _dbContext.Set<TbOrder>().AddAsync(order, cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);

                return order;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error creating order");
                throw;
            }
        }

        /// <summary>
        /// Update existing order
        /// </summary>
        public async Task UpdateAsync(
            TbOrder order,
            CancellationToken cancellationToken = default)
        {
            try
            {
                order.UpdatedDateUtc = DateTime.UtcNow;

                _dbContext.Set<TbOrder>().Update(order);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error updating order {OrderId}", order.Id);
                throw;
            }
        }

        /// <summary>
        /// Get orders by status
        /// </summary>
        public async Task<List<TbOrder>> GetOrdersByStatusAsync(
            OrderProgressStatus status,
            CancellationToken cancellationToken = default)
        {
            try
            {
                return await _dbContext.Set<TbOrder>()
                    .AsNoTracking()
                    .Where(o => o.OrderStatus == status && !o.IsDeleted)
                    .OrderByDescending(o => o.CreatedDateUtc)
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting orders by status {Status}", status);
                throw;
            }
        }

        /// <summary>
        /// Get orders by payment status
        /// </summary>
        public async Task<List<TbOrder>> GetOrdersByPaymentStatusAsync(
            PaymentStatus paymentStatus,
            CancellationToken cancellationToken = default)
        {
            try
            {
                return await _dbContext.Set<TbOrder>()
                    .AsNoTracking()
                    .Where(o => o.PaymentStatus == paymentStatus && !o.IsDeleted)
                    .OrderByDescending(o => o.CreatedDateUtc)
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting orders by payment status {Status}", paymentStatus);
                throw;
            }
        }


        /// <summary>
        /// Count orders created today for order number generation
        /// Returns count of orders created on the specified date
        /// </summary>
        public async Task<int> CountTodayOrdersAsync(
            DateTime date,
            CancellationToken cancellationToken = default)
        {
            try
            {
                // Get start and end of day in UTC
                var startOfDay = date.Date;
                var endOfDay = startOfDay.AddDays(1);

                // Count orders created on this date
                var count = await CountAsync(
                    o => o.CreatedDateUtc >= startOfDay &&
                         o.CreatedDateUtc < endOfDay &&
                         !o.IsDeleted,
                    cancellationToken);

                return count;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error counting today's orders for date {Date}", date);
                throw;
            }
        }
    }
}