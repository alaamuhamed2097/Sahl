namespace Domains.Procedures;

/// <summary>
/// Result model for SpSearchItemsMultiVendor stored procedure
/// Amazon-style search results with BuyBox winner selection
/// </summary>
public class SpSearchItemsMultiVendor
{
    /// <summary>
    /// Unique identifier for the item
    /// </summary>
    public Guid ItemId { get; set; }

    /// <summary>
    /// Product title in Arabic
    /// </summary>
    public string TitleAr { get; set; }

    /// <summary>
    /// Product title in English
    /// </summary>
    public string TitleEn { get; set; }

    /// <summary>
    /// Short description in Arabic
    /// </summary>
    public string ShortDescriptionAr { get; set; }

    /// <summary>
    /// Short description in English
    /// </summary>
    public string ShortDescriptionEn { get; set; }

    /// <summary>
    /// Category identifier
    /// </summary>
    public Guid CategoryId { get; set; }

    /// <summary>
    /// Brand identifier
    /// </summary>
    public Guid? BrandId { get; set; }

    /// <summary>
    /// Product thumbnail image URL
    /// </summary>
    public string ThumbnailImage { get; set; }

    /// <summary>
    /// Item creation date in UTC
    /// </summary>
    public DateTime CreatedDateUtc { get; set; }

    /// <summary>
    /// Average rating for this item (0.00 to 5.00)
    /// </summary>
    public decimal? ItemRating { get; set; }

    /// <summary>
    /// Original price (before discount)
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Current sales price (the price customer pays)
    /// </summary>
    public decimal SalesPrice { get; set; }

    /// <summary>
    /// Available quantity in stock
    /// </summary>
    public int AvailableQuantity { get; set; }

    /// <summary>
    /// Stock status as string: InStock, OutOfStock, LimitedStock, ComingSoon
    /// </summary>
    public string StockStatus { get; set; }

    /// <summary>
    /// Whether free shipping is available for this item
    /// </summary>
    public bool IsFreeShipping { get; set; }

    /// <summary>
    /// Total number of records (for pagination)
    /// </summary>
    public int TotalRecords { get; set; }
}