namespace Shared.DTOs.AdminDashboard;

/// <summary>
/// Main DTO for the Admin Dashboard Summary containing all KPI metrics
/// </summary>
public class AdminDashboardSummaryDto
{
    /// <summary>
    /// Total number of active users (customers + vendors)
    /// </summary>
    public int TotalActiveUsers { get; set; }

    /// <summary>
    /// Total number of new users in the current period
    /// </summary>
    public int NewUsersThisPeriod { get; set; }

    /// <summary>
    /// Total number of active vendors
    /// </summary>
    public int TotalVendors { get; set; }

    /// <summary>
    /// Total number of active customers
    /// </summary>
    public int TotalCustomers { get; set; }

    /// <summary>
    /// Total number of products in the system
    /// </summary>
    public int TotalProducts { get; set; }

    /// <summary>
    /// Total number of products added in the current period
    /// </summary>
    public int NewProductsThisPeriod { get; set; }

    /// <summary>
    /// Total system revenue
    /// </summary>
    public decimal TotalRevenue { get; set; }

    /// <summary>
    /// Revenue for the current period
    /// </summary>
    public decimal RevenueThisPeriod { get; set; }

    /// <summary>
    /// Total number of orders in the system
    /// </summary>
    public int TotalOrders { get; set; }

    /// <summary>
    /// Number of pending orders
    /// </summary>
    public int PendingOrders { get; set; }

    /// <summary>
    /// Number of completed orders in the current period
    /// </summary>
    public int CompletedOrdersThisPeriod { get; set; }

    /// <summary>
    /// Total number of categories
    /// </summary>
    public int TotalCategories { get; set; }

    /// <summary>
    /// Total number of brands
    /// </summary>
    public int TotalBrands { get; set; }

    /// <summary>
    /// Average customer rating
    /// </summary>
    public decimal AverageCustomerRating { get; set; }

    /// <summary>
    /// Total number of customer reviews/ratings
    /// </summary>
    public int TotalReviews { get; set; }

    /// <summary>
    /// Percentage change in revenue compared to previous period
    /// </summary>
    public decimal RevenueChangePercentage { get; set; }

    /// <summary>
    /// Percentage change in orders compared to previous period
    /// </summary>
    public decimal OrdersChangePercentage { get; set; }

    /// <summary>
    /// Percentage change in users compared to previous period
    /// </summary>
    public decimal UsersChangePercentage { get; set; }

    /// <summary>
    /// List of top-selling products
    /// </summary>
    public IEnumerable<TopProductDto> TopProducts { get; set; } = new List<TopProductDto>();

    /// <summary>
    /// List of vendor performance summaries
    /// </summary>
    public IEnumerable<VendorPerformanceDto> TopVendors { get; set; } = new List<VendorPerformanceDto>();

    /// <summary>
    /// Timestamp when the dashboard data was generated
    /// </summary>
    public DateTime GeneratedAt { get; set; }

    /// <summary>
    /// Period for which the data is calculated (e.g., "This Month", "This Week")
    /// </summary>
    public string Period { get; set; } = "This Month";
}

/// <summary>
/// DTO for top-selling products information
/// </summary>
public class TopProductDto
{
    /// <summary>
    /// Product ID
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Product name
    /// </summary>
    public string ProductName { get; set; } = string.Empty;

    /// <summary>
    /// Vendor name
    /// </summary>
    public string VendorName { get; set; } = string.Empty;

    /// <summary>
    /// Number of units sold
    /// </summary>
    public int UnitsSold { get; set; }

    /// <summary>
    /// Product revenue
    /// </summary>
    public decimal Revenue { get; set; }

    /// <summary>
    /// Product rating
    /// </summary>
    public decimal Rating { get; set; }
}

/// <summary>
/// DTO for vendor performance summary
/// </summary>
public class VendorPerformanceDto
{
    /// <summary>
    /// Vendor ID
    /// </summary>
    public Guid VendorId { get; set; }

    /// <summary>
    /// Vendor name
    /// </summary>
    public string VendorName { get; set; } = string.Empty;

    /// <summary>
    /// Number of products from this vendor
    /// </summary>
    public int ProductCount { get; set; }

    /// <summary>
    /// Vendor revenue
    /// </summary>
    public decimal Revenue { get; set; }

    /// <summary>
    /// Number of orders from this vendor
    /// </summary>
    public int OrderCount { get; set; }

    /// <summary>
    /// Vendor rating
    /// </summary>
    public decimal Rating { get; set; }

    /// <summary>
    /// Number of completed orders
    /// </summary>
    public int CompletedOrders { get; set; }
}
