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
/// FINAL OrderRepository - No InvoiceId
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

    // REMOVED: GetByInvoiceIdAsync - No InvoiceId field anymore

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
            _logger.Error(ex, "Error getting orders by payment status {PaymentStatus}", paymentStatus);
            throw;
        }
    }

    public async Task<(List<TbOrder> Orders, int TotalCount)> GetCustomerOrdersWithPaginationAsync(
        string customerId,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        try
        {
            pageNumber = Math.Max(1, pageNumber);
            pageSize = Math.Max(1, Math.Min(pageSize, 100));

            IQueryable<TbOrder> query = _dbContext.Set<TbOrder>()
                .AsNoTracking()
                .Where(o => o.UserId == customerId && !o.IsDeleted)
                .Include(o => o.User)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Item)
                .Include(o => o.TbOrderShipments)
                .OrderByDescending(o => o.CreatedDateUtc);

            var totalCount = await query.CountAsync(cancellationToken);

            var orders = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return (orders, totalCount);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error getting customer orders with pagination for {CustomerId}", customerId);
            throw;
        }
    }

    public async Task<TbOrder?> GetOrderWithFullDetailsAsync(
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
                    .ThenInclude(od => od.Vendor)
                .Include(o => o.TbOrderShipments)
                    .ThenInclude(s => s.Items)
                .Include(o => o.OrderPayments)
                .FirstOrDefaultAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error getting order with full details {OrderId}", orderId);
            throw;
        }
    }

    public async Task<(List<TbOrder> Orders, int TotalCount)> GetVendorOrdersWithPaginationAsync(
        string vendorId,
        string? searchTerm,
        int pageNumber,
        int pageSize,
        string? sortBy,
        string? sortDirection,
        CancellationToken cancellationToken = default)
    {
        try
        {
            if (!Guid.TryParse(vendorId, out var vendorGuid))
            {
                return (new List<TbOrder>(), 0);
            }

            IQueryable<TbOrder> query = _dbContext.Set<TbOrder>()
                .AsNoTracking()
                .Where(o => !o.IsDeleted && o.OrderDetails.Any(od => od.VendorId == vendorGuid))
                .Include(o => o.User)
                .Include(o => o.OrderDetails.Where(od => od.VendorId == vendorGuid))
                    .ThenInclude(od => od.Item)
                .Include(o => o.TbOrderShipments);

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var searchLower = searchTerm.ToLower();
                query = query.Where(o =>
                    o.Number.ToLower().Contains(searchLower) ||
                    o.User.FirstName.ToLower().Contains(searchLower) ||
                    o.User.LastName.ToLower().Contains(searchLower));
            }

            var totalCount = await query.CountAsync(cancellationToken);
            query = ApplySorting(query, sortBy, sortDirection);

            var orders = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return (orders, totalCount);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error getting vendor orders for {VendorId}", vendorId);
            throw;
        }
    }

    public async Task<(List<TbOrder> Orders, int TotalCount)> SearchAsync(
        string? searchTerm,
        int pageNumber,
        int pageSize,
        string sortBy,
        string sortDirection,
        CancellationToken cancellationToken = default)
    {
        return await SearchOrdersAsync(searchTerm, pageNumber, pageSize, sortBy, sortDirection, cancellationToken);
    }

    public async Task<(List<TbOrder> Orders, int TotalCount)> SearchOrdersAsync(
        string? searchTerm,
        int pageNumber,
        int pageSize,
        string? sortBy,
        string? sortDirection,
        CancellationToken cancellationToken = default)
    {
        try
        {
            pageNumber = Math.Max(1, pageNumber);
            pageSize = Math.Max(1, Math.Min(pageSize, 100));

            IQueryable<TbOrder> query = _dbContext.Set<TbOrder>()
                .AsNoTracking()
                .Where(o => !o.IsDeleted)
                .Include(o => o.User)
                .Include(o => o.OrderDetails)
                .Include(o => o.TbOrderShipments);

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var searchLower = searchTerm.ToLower();

                if (searchLower.StartsWith("status:"))
                {
                    var statusPart = searchLower.Replace("status:", "").Trim();
                    if (int.TryParse(statusPart, out int statusValue))
                    {
                        query = query.Where(o => (int)o.OrderStatus == statusValue);
                    }
                }
                else if (searchLower.StartsWith("payment:"))
                {
                    var paymentPart = searchLower.Replace("payment:", "").Trim();
                    if (int.TryParse(paymentPart, out int paymentValue))
                    {
                        query = query.Where(o => (int)o.PaymentStatus == paymentValue);
                    }
                }
                else
                {
                    query = query.Where(o =>
                        o.Number.ToLower().Contains(searchLower) ||
                        o.User.FirstName.ToLower().Contains(searchLower) ||
                        o.User.LastName.ToLower().Contains(searchLower) ||
                        o.User.Email.ToLower().Contains(searchLower));
                }
            }

            var totalCount = await query.CountAsync(cancellationToken);
            query = ApplySorting(query, sortBy, sortDirection);

            var orders = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return (orders, totalCount);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error searching orders");
            throw;
        }
    }

    public async Task<int> CountTodayOrdersAsync(
        DateTime date,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var startOfDay = date.Date;
            var endOfDay = startOfDay.AddDays(1);

            return await _dbContext.Set<TbOrder>()
                .AsNoTracking()
                .Where(o => !o.IsDeleted &&
                           o.CreatedDateUtc >= startOfDay &&
                           o.CreatedDateUtc < endOfDay)
                .CountAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error counting today's orders for date {Date}", date);
            throw;
        }
    }

    private IQueryable<TbOrder> ApplySorting(
        IQueryable<TbOrder> query,
        string? sortBy,
        string? sortDirection)
    {
        var isDescending = sortDirection?.ToLower() == "desc";

        return sortBy?.ToLower() switch
        {
            "number" or "ordernumber" => isDescending
                ? query.OrderByDescending(o => o.Number)
                : query.OrderBy(o => o.Number),
            "customername" => isDescending
                ? query.OrderByDescending(o => o.User.FirstName).ThenByDescending(o => o.User.LastName)
                : query.OrderBy(o => o.User.FirstName).ThenBy(o => o.User.LastName),
            "price" or "total" => isDescending
                ? query.OrderByDescending(o => o.Price)
                : query.OrderBy(o => o.Price),
            "orderstatus" or "status" => isDescending
                ? query.OrderByDescending(o => o.OrderStatus)
                : query.OrderBy(o => o.OrderStatus),
            "paymentstatus" => isDescending
                ? query.OrderByDescending(o => o.PaymentStatus)
                : query.OrderBy(o => o.PaymentStatus),
            _ => isDescending
                ? query.OrderByDescending(o => o.CreatedDateUtc)
                : query.OrderBy(o => o.CreatedDateUtc)
        };
    }
}