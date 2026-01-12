using BL.Contracts.GeneralService;
using Common.Enumerations.Order;
using Common.Enumerations.Payment;
using DAL.ApplicationContext;
using DAL.Contracts.Repositories.Order;
using Domains.Entities.Order;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace DAL.Repositories.Order;

/// <summary>
/// Repository implementation for Order operations
/// Inherits all CRUD operations from TableRepository<TbOrder>
/// Implements order-specific query methods
/// </summary>
public class OrderRepository : TableRepository<TbOrder>, IOrderRepository
{
    public OrderRepository(
        ApplicationDbContext dbContext,
        ICurrentUserService currentUserService,
        ILogger logger)
        : base(dbContext, currentUserService, logger)
    {
    }

    // ============================================
    // ORDER-SPECIFIC READ OPERATIONS
    // ============================================

    /// <summary>
    /// Get order with full details
    /// Includes: OrderDetails, Items, Vendors, Shipments, Payments, Address, User, Coupon
    /// </summary>
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
                        .ThenInclude(c => c.State)
                .Include(o => o.Coupon)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Item)
                        .ThenInclude(i => i.ItemImages)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Item)
                        .ThenInclude(i => i.ItemCombinations)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Vendor)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.OfferCombinationPricing)
                .Include(o => o.TbOrderShipments)
                .Include(o => o.OrderPayments)
                .FirstOrDefaultAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error getting order with details {OrderId}", orderId);
            throw;
        }
    }

    /// <summary>
    /// Get order with shipments
    /// Includes: OrderDetails, Items, Vendors, Shipments, Address, User
    /// </summary>
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
                .Include(o => o.TbOrderShipments)
                .FirstOrDefaultAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error getting order with shipments {OrderId}", orderId);
            throw;
        }
    }

    /// <summary>
    /// Get order by order number
    /// </summary>
    public async Task<TbOrder?> GetByOrderNumberAsync(
        string orderNumber,
        CancellationToken cancellationToken = default)
    {
        try
        {
            return await _dbContext.Set<TbOrder>()
                .AsNoTracking()
                .Where(o => o.Number == orderNumber && !o.IsDeleted)
                .FirstOrDefaultAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error getting order by number {OrderNumber}", orderNumber);
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
            _logger.Error(ex, "Error getting order by invoice {InvoiceId}", invoiceId);
            throw;
        }
    }

    /// <summary>
    /// Get all orders for a customer
    /// </summary>
    public async Task<List<TbOrder>> GetByCustomerIdAsync(
        string customerId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            return await _dbContext.Set<TbOrder>()
                .AsNoTracking()
                .Where(o => o.UserId == customerId && !o.IsDeleted)
                .OrderByDescending(o => o.CreatedDateUtc)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error getting orders for customer {CustomerId}", customerId);
            throw;
        }
    }

    /// <summary>
    /// Get customer orders with pagination
    /// </summary>
    public async Task<List<TbOrder>> GetByCustomerIdAsync(
        string customerId,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        try
        {
            return await _dbContext.Set<TbOrder>()
                .AsNoTracking()
                .Where(o => o.UserId == customerId && !o.IsDeleted)
                .OrderByDescending(o => o.CreatedDateUtc)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error getting paged orders for customer {CustomerId}", customerId);
            throw;
        }
    }

    /// <summary>
    /// Get orders by order status
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
    /// Count orders created on a specific date
    /// Used for order number generation
    /// </summary>
    public async Task<int> CountTodayOrdersAsync(
        DateTime date,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var startOfDay = date.Date;
            var endOfDay = startOfDay.AddDays(1);

            return await _dbContext.Set<TbOrder>()
                .Where(o => o.CreatedDateUtc >= startOfDay &&
                           o.CreatedDateUtc < endOfDay &&
                           !o.IsDeleted)
                .CountAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error counting orders for date {Date}", date);
            throw;
        }
    }

    /// <summary>
    /// Search orders with pagination and filtering
    /// </summary>
    public async Task<(List<TbOrder> Items, int TotalCount)> SearchAsync(
        string? searchTerm = null,
        int pageNumber = 1,
        int pageSize = 10,
        string sortBy = "CreatedDateUtc",
        string sortDirection = "desc",
        CancellationToken cancellationToken = default)
    {
        try
        {
            // Validate pagination parameters
            pageNumber = Math.Max(1, pageNumber);
            pageSize = Math.Max(1, Math.Min(pageSize, 100));

            var query = _dbContext.Set<TbOrder>()
                .AsNoTracking()
                .Where(o => !o.IsDeleted);

            // Apply search filter
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var searchLower = searchTerm.ToLower();

                // Check if it's a status filter (status:value format)
                if (searchLower.StartsWith("status:"))
                {
                    var statusPart = searchLower.Replace("status:", "").Trim();
                    if (int.TryParse(statusPart, out int statusValue))
                    {
                        query = query.Where(o => (int)o.OrderStatus == statusValue);
                    }
                }
                // Check if it's a payment status filter (payment:value format)
                else if (searchLower.StartsWith("payment:"))
                {
                    var paymentStatusPart = searchLower.Replace("payment:", "").Trim();
                    if (int.TryParse(paymentStatusPart, out int paymentStatusValue))
                    {
                        query = query.Where(o => (int)o.PaymentStatus == paymentStatusValue);
                    }
                }
                // Regular search by order number or customer name
                else
                {
                    query = query.Where(o =>
                        o.Number.ToLower().Contains(searchLower) ||
                        o.User.FirstName.ToLower().Contains(searchLower) ||
                        o.User.LastName.ToLower().Contains(searchLower));
                }
            }

            // Get total count before pagination
            var totalCount = await query.CountAsync(cancellationToken);

            // Apply sorting
            query = sortBy.ToLower() switch
            {
                "number" or "ordernumber" => sortDirection == "desc"
                    ? query.OrderByDescending(o => o.Number)
                    : query.OrderBy(o => o.Number),
                "customername" => sortDirection == "desc"
                    ? query.OrderByDescending(o => o.User.FirstName).ThenByDescending(o => o.User.LastName)
                    : query.OrderBy(o => o.User.FirstName).ThenBy(o => o.User.LastName),
                "createddateutc" or "orderdate" => sortDirection == "desc"
                    ? query.OrderByDescending(o => o.CreatedDateUtc)
                    : query.OrderBy(o => o.CreatedDateUtc),
                "orderstatus" or "currentstate" => sortDirection == "desc"
                    ? query.OrderByDescending(o => o.OrderStatus)
                    : query.OrderBy(o => o.OrderStatus),
                "price" => sortDirection == "desc"
                    ? query.OrderByDescending(o => o.Price)
                    : query.OrderBy(o => o.Price),
                "paymentstatus" => sortDirection == "desc"
                    ? query.OrderByDescending(o => o.PaymentStatus)
                    : query.OrderBy(o => o.PaymentStatus),
                _ => sortDirection == "desc"
                    ? query.OrderByDescending(o => o.CreatedDateUtc)
                    : query.OrderBy(o => o.CreatedDateUtc)
            };

            // Apply pagination
            var orders = await query
                .Include(o => o.User)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return (orders, totalCount);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error searching orders with term: {SearchTerm}, page: {PageNumber}", searchTerm, pageNumber);
            throw;
        }
    }
}