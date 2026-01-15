namespace Shared.DTOs.VendorDashboard;

/// <summary>
/// DTO for latest reviews in the vendor dashboard
/// </summary>
public class LatestReviewDto
{
    /// <summary>
    /// Unique identifier of the review
    /// </summary>
    public Guid ReviewId { get; set; }

    /// <summary>
    /// The reviewed item/product ID (if applicable)
    /// </summary>
    public Guid? ItemId { get; set; }

    /// <summary>
    /// Product/Item name
    /// </summary>
    public string ItemName { get; set; } = string.Empty;

    /// <summary>
    /// Customer name or ID who gave the review
    /// </summary>
    public string CustomerName { get; set; } = string.Empty;

    /// <summary>
    /// Rating given (1-5)
    /// </summary>
    public decimal Rating { get; set; }

    /// <summary>
    /// Review comment/text
    /// </summary>
    public string Comment { get; set; } = string.Empty;

    /// <summary>
    /// Review creation date
    /// </summary>
    public DateTime ReviewDate { get; set; }

    /// <summary>
    /// Review status (Approved, Pending, Rejected)
    /// </summary>
    public string Status { get; set; } = "Approved";
}
