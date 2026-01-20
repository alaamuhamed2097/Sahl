using BL.Contracts.IMapper;
using BL.Contracts.Service.VendorDashboard;
using Common.Enumerations.Order;
using Common.Enumerations.Review;
using DAL.Contracts.Repositories;
using DAL.Contracts.Repositories.Order;
using DAL.Contracts.Repositories.Review;
using Domains.Entities.ECommerceSystem.Review;
using Domains.Entities.ECommerceSystem.Vendor;
using Domains.Entities.Order;
using Serilog;
using Shared.DTOs.VendorDashboard;

namespace BL.Services.VendorDashboard;

/// <summary>
/// Service for Vendor Dashboard analytics and KPI metrics
/// Aggregates data from multiple repositories to provide real-time insights
/// </summary>
public class VendorDashboardService : IVendorDashboardService
{
    private readonly IOrderRepository _orderRepository;
    private readonly ITableRepository<TbOrderDetail> _orderDetailRepository;
    private readonly ITableRepository<TbVendor> _vendorRepository;
    private readonly ITableRepository<TbOrder> _tbOrderRepository;
    private readonly IVendorReviewRepository _vendorReviewRepository;
    private readonly IItemReviewRepository _itemReviewRepository;
    private readonly IBaseMapper _mapper;
    private readonly ILogger _logger;

    public VendorDashboardService(
        IOrderRepository orderRepository,
        ITableRepository<TbOrderDetail> orderDetailRepository,
        ITableRepository<TbVendor> vendorRepository,
        ITableRepository<TbOrder> tbOrderRepository,
        IVendorReviewRepository vendorReviewRepository,
        IItemReviewRepository itemReviewRepository,
        IBaseMapper mapper,
        ILogger logger)
    {
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        _orderDetailRepository = orderDetailRepository ?? throw new ArgumentNullException(nameof(orderDetailRepository));
        _vendorReviewRepository = vendorReviewRepository ?? throw new ArgumentNullException(nameof(vendorReviewRepository));
        _vendorRepository = vendorRepository ?? throw new ArgumentNullException(nameof(vendorRepository));
        _itemReviewRepository = itemReviewRepository ?? throw new ArgumentNullException(nameof(itemReviewRepository));
        _tbOrderRepository = tbOrderRepository ?? throw new ArgumentNullException(nameof(tbOrderRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Gets comprehensive dashboard summary with all KPIs for a vendor
    /// </summary>
    public async Task<VendorDashboardSummaryDto> GetDashboardSummaryAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        if (userId == Guid.Empty)
            throw new ArgumentException("VendorId cannot be empty.", nameof(userId));

        try
        {
            var vendorId = (await _vendorRepository.FindAsync(v => v.UserId == userId.ToString())).Id;

            var dailySales = await GetDailySalesAsync(vendorId, cancellationToken);
            var newOrders = await GetNewOrdersAsync(vendorId, cancellationToken);
            var bestSellingProducts = await GetBestSellingProductsAsync(vendorId, 10, cancellationToken);
            var latestReviews = await GetLatestReviewsAsync(vendorId, 5, cancellationToken);

            // Get overall rating and review count
            var vendorReviews = await _vendorReviewRepository.GetReviewsByVendorIdAsync(vendorId, cancellationToken);
            var vendorReviewsList = vendorReviews?.ToList() ?? new List<TbVendorReview>();
            var overallRating = vendorReviewsList.Any() ? vendorReviewsList.Average(r => r.Rating) : (decimal?)null;
            var totalReviews = vendorReviewsList.Count;

            // Get total products sold
            var vendorOrders = await GetVendorOrdersAsync(vendorId, cancellationToken);
            var totalProductsSold = vendorOrders
                .SelectMany(o => o.OrderDetails ?? new List<TbOrderDetail>())
                .Sum(od => od.Quantity);

            var summary = new VendorDashboardSummaryDto
            {
                VendorId = vendorId,
                DailySales = dailySales,
                NewOrders = newOrders,
                BestSellingProducts = bestSellingProducts,
                LatestReviews = latestReviews,
                OverallRating = overallRating,
                TotalProductsSold = totalProductsSold,
                TotalReviews = totalReviews,
                GeneratedAt = DateTime.UtcNow
            };

            return summary;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error retrieving dashboard summary for vendor {VendorId}", userId);
            throw;
        }
    }

    /// <summary>
    /// Gets daily sales metrics for the current day
    /// </summary>
    public async Task<DailySalesDto> GetDailySalesAsync(
        Guid vendorId,
        CancellationToken cancellationToken = default)
    {
        if (vendorId == Guid.Empty)
            throw new ArgumentException("VendorId cannot be empty.", nameof(vendorId));

        try
        {
            var today = DateTime.UtcNow.Date;
            var vendorOrders = await GetVendorOrdersAsync(vendorId, cancellationToken);

            var todayOrders = vendorOrders
                .Where(o => o.CreatedDateUtc.Date == today && o.IsDeleted == false)
                .ToList();

            var totalSales = todayOrders
                .SelectMany(o => o.OrderDetails ?? new List<TbOrderDetail>())
                .Sum(od => od.UnitPrice * od.Quantity);

            // Calculate percentage change from yesterday
            var yesterday = today.AddDays(-1);
            var yesterdayOrders = vendorOrders
                .Where(o => o.CreatedDateUtc.Date == yesterday && o.IsDeleted == false)
                .ToList();

            var yesterdaySales = yesterdayOrders
                .SelectMany(o => o.OrderDetails ?? new List<TbOrderDetail>())
                .Sum(od => od.UnitPrice * od.Quantity);

            decimal? percentageChange = null;
            if (yesterdaySales > 0)
            {
                percentageChange = ((totalSales - yesterdaySales) / yesterdaySales) * 100;
            }

            return new DailySalesDto
            {
                TotalSales = totalSales,
                OrderCount = todayOrders.Count,
                CurrencyCode = "USD", // TODO: Get from system settings
                PercentageChange = percentageChange
            };
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error retrieving daily sales for vendor {VendorId}", vendorId);
            throw;
        }
    }

    /// <summary>
    /// Gets new/pending orders count and status breakdown
    /// </summary>
    public async Task<NewOrdersDto> GetNewOrdersAsync(
        Guid vendorId,
        CancellationToken cancellationToken = default)
    {
        if (vendorId == Guid.Empty)
            throw new ArgumentException("VendorId cannot be empty.", nameof(vendorId));

        try
        {
            var vendorOrders = await GetVendorOrdersAsync(vendorId, cancellationToken);

            var activeOrders = vendorOrders
                .Where(o => o.IsDeleted == false && o.OrderStatus != OrderProgressStatus.Completed)
                .ToList();

            var pendingOrders = activeOrders
                .Count(o => o.OrderStatus == OrderProgressStatus.Pending);

            var processingOrders = activeOrders
                .Count(o => o.OrderStatus == OrderProgressStatus.Processing);

            var readyForShipment = activeOrders
                .Count(o => o.OrderStatus == OrderProgressStatus.Shipped);

            // Calculate percentage change from previous period (last 7 days)
            var now = DateTime.UtcNow;
            var last7Days = now.AddDays(-7);
            var previous7Days = now.AddDays(-14);

            var thisWeekNewOrders = vendorOrders
                .Where(o => o.CreatedDateUtc >= last7Days && o.CreatedDateUtc < now && o.IsDeleted == false)
                .Count();

            var lastWeekNewOrders = vendorOrders
                .Where(o => o.CreatedDateUtc >= previous7Days && o.CreatedDateUtc < last7Days && o.IsDeleted == false)
                .Count();

            decimal? percentageChange = null;
            if (lastWeekNewOrders > 0)
            {
                percentageChange = ((thisWeekNewOrders - lastWeekNewOrders) / (decimal)lastWeekNewOrders) * 100;
            }

            return new NewOrdersDto
            {
                TotalNewOrders = activeOrders.Count,
                PendingOrders = pendingOrders,
                ProcessingOrders = processingOrders,
                ReadyForShipmentOrders = readyForShipment,
                PercentageChange = percentageChange
            };
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error retrieving new orders for vendor {VendorId}", vendorId);
            throw;
        }
    }

    /// <summary>
    /// Gets best-selling products for the vendor
    /// </summary>
    public async Task<IEnumerable<BestSellingProductDto>> GetBestSellingProductsAsync(
        Guid vendorId,
        int limit = 10,
        CancellationToken cancellationToken = default)
    {
        if (vendorId == Guid.Empty)
            throw new ArgumentException("VendorId cannot be empty.", nameof(vendorId));

        try
        {
            var vendorOrders = await GetVendorOrdersAsync(vendorId, cancellationToken);

            var bestSellingProducts = vendorOrders
                .SelectMany(o => o.OrderDetails ?? new List<TbOrderDetail>())
                .GroupBy(od => od.ItemId)
                .Select(g => new
                {
                    ItemId = g.Key,
                    QuantitySold = g.Sum(od => od.Quantity),
                    Revenue = g.Sum(od => od.UnitPrice * od.Quantity),
                    Item = g.FirstOrDefault()?.Item
                })
                .OrderByDescending(x => x.QuantitySold)
                .Take(limit)
                .Select(p => new BestSellingProductDto
                {
                    ProductId = p.ItemId,
                    ProductName = p.Item?.TitleEn ?? "Unknown Product",
                    Sku = p.Item?.SKU,
                    QuantitySold = p.QuantitySold,
                    Revenue = p.Revenue,
                    CurrencyCode = "USD", // TODO: Get from system settings
                    AverageRating = p.Item?.AverageRating,
                    ImageUrl = p.Item?.ItemImages?.FirstOrDefault()?.Path
                })
                .ToList();

            return bestSellingProducts;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error retrieving best-selling products for vendor {VendorId}", vendorId);
            throw;
        }
    }

    /// <summary>
    /// Gets latest customer reviews for the vendor
    /// </summary>
    public async Task<IEnumerable<LatestReviewDto>> GetLatestReviewsAsync(
        Guid vendorId,
        int limit = 5,
        CancellationToken cancellationToken = default)
    {
        if (vendorId == Guid.Empty)
            throw new ArgumentException("VendorId cannot be empty.", nameof(vendorId));

        try
        {
            var allReviews = new List<LatestReviewDto>();

            // Get vendor reviews (for vendor itself)
            var vendorReviews = await _vendorReviewRepository.GetReviewsByVendorIdAsync(vendorId, cancellationToken);
            var vendorReviewsList = vendorReviews?.ToList() ?? new List<TbVendorReview>();

            var vendorLatestReviews = vendorReviewsList
                .OrderByDescending(r => r.CreatedDateUtc)
                .Select(r => new LatestReviewDto
                {
                    ReviewId = r.Id,
                    ItemId = null,
                    ItemName = "Vendor Review",
                    CustomerName = r.CustomerId.ToString(), // TODO: Get actual customer name
                    Rating = r.Rating,
                    Comment = r.ReviewText ?? "",
                    ReviewDate = r.CreatedDateUtc,
                    Status = r.Status == ReviewStatus.Approved ? "Approved" : "Pending"
                })
                .ToList();

            allReviews.AddRange(vendorLatestReviews);

            // Get item reviews for vendor's items
            var vendorOrders = await GetVendorOrdersAsync(vendorId, cancellationToken);
            var vendorItemIds = vendorOrders
                .SelectMany(o => o.OrderDetails ?? new List<TbOrderDetail>())
                .Select(od => od.ItemId)
                .Distinct()
                .ToList();

            // Get item reviews for each item
            foreach (var itemId in vendorItemIds)
            {
                var itemReviews = await _itemReviewRepository.GetReviewsByItemIdAsync(itemId, cancellationToken);
                var itemReviewsList = itemReviews?.ToList() ?? new List<TbItemReview>();

                var itemLatestReviews = itemReviewsList
                    .OrderByDescending(r => r.CreatedDateUtc)
                    .Select(r => new LatestReviewDto
                    {
                        ReviewId = r.Id,
                        ItemId = r.ItemId,
                        ItemName = r.Item?.TitleEn ?? "Unknown Item",
                        CustomerName = r.CustomerId.ToString(), // TODO: Get actual customer name
                        Rating = r.Rating,
                        Comment = r.ReviewText ?? "",
                        ReviewDate = r.CreatedDateUtc,
                        Status = r.Status == ReviewStatus.Approved ? "Approved" : "Pending"
                    })
                    .ToList();

                allReviews.AddRange(itemLatestReviews);
            }

            var finalReviews = allReviews
                .OrderByDescending(r => r.ReviewDate)
                .Take(limit)
                .ToList();

            return finalReviews;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error retrieving latest reviews for vendor {VendorId}", vendorId);
            throw;
        }
    }

    /// <summary>
    /// Helper method to get all orders for a vendor
    /// </summary>
    private async Task<List<TbOrder>> GetVendorOrdersAsync(Guid vendorId, CancellationToken cancellationToken)
    {
        // Get all orders that have order details from this vendor
        var allOrders = await _tbOrderRepository.GetAllAsync(cancellationToken);
        var vendorOrders = allOrders
            .Where(o => o.OrderDetails != null && o.OrderDetails.Any())
            .Where(o => o.OrderDetails.Any(od => od.VendorId == vendorId))
            .ToList();

        return vendorOrders;
    }
}
