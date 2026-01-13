namespace Shared.DTOs.VendorDashboard;

/// <summary>
/// DTO for new orders metric in the vendor dashboard
/// </summary>
public class NewOrdersDto
{
    /// <summary>
    /// Total count of new orders (pending or processing)
    /// </summary>
    public int TotalNewOrders { get; set; }

    /// <summary>
    /// Count of pending orders awaiting processing
    /// </summary>
    public int PendingOrders { get; set; }

    /// <summary>
    /// Count of orders currently being processed
    /// </summary>
    public int ProcessingOrders { get; set; }

    /// <summary>
    /// Count of orders ready for shipment
    /// </summary>
    public int ReadyForShipmentOrders { get; set; }

    /// <summary>
    /// Percentage change in new orders compared to previous period
    /// </summary>
    public decimal? PercentageChange { get; set; }
}
