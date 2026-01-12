namespace Shared.DTOs.VendorDashboard;

/// <summary>
/// DTO for best-selling products in the vendor dashboard
/// </summary>
public class BestSellingProductDto
{
    /// <summary>
    /// Unique identifier of the product
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Name of the product
    /// </summary>
    public string ProductName { get; set; } = string.Empty;

    /// <summary>
    /// Product SKU or code
    /// </summary>
    public string? Sku { get; set; }

    /// <summary>
    /// Total quantity sold
    /// </summary>
    public int QuantitySold { get; set; }

    /// <summary>
    /// Total revenue from this product
    /// </summary>
    public decimal Revenue { get; set; }

    /// <summary>
    /// Currency code
    /// </summary>
    public string CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// Average rating of the product
    /// </summary>
    public decimal? AverageRating { get; set; }

    /// <summary>
    /// Image URL of the product
    /// </summary>
    public string? ImageUrl { get; set; }
}
