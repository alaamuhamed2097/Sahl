namespace DAL.Models.ItemSearch;

/// <summary>
/// Available filters data returned from database search
/// Contains aggregated filter options from the search query
/// </summary>
public class AvailableFiltersData
{
    /// <summary>
    /// Available categories with item counts
    /// </summary>
    public List<FilterCategoryOption> Categories { get; set; } = new();

    /// <summary>
    /// Available brands with item counts
    /// </summary>
    public List<FilterBrandOption> Brands { get; set; } = new();

    /// <summary>
    /// Available vendors with item counts
    /// </summary>
    public List<FilterVendorOption> Vendors { get; set; } = new();

    /// <summary>
    /// Minimum price in search results
    /// </summary>
    public decimal? MinPrice { get; set; }

    /// <summary>
    /// Maximum price in search results
    /// </summary>
    public decimal? MaxPrice { get; set; }
}

/// <summary>
/// Single category filter option
/// </summary>
public class FilterCategoryOption
{
    /// <summary>
    /// Category unique identifier
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Category name in Arabic
    /// </summary>
    public string NameAr { get; set; }

    /// <summary>
    /// Category name in English
    /// </summary>
    public string NameEn { get; set; }

    /// <summary>
    /// Number of items in this category matching current filters
    /// </summary>
    public int Count { get; set; }
}

/// <summary>
/// Single brand filter option
/// </summary>
public class FilterBrandOption
{
    /// <summary>
    /// Brand unique identifier
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Brand name in Arabic
    /// </summary>
    public string NameAr { get; set; }

    /// <summary>
    /// Brand name in English
    /// </summary>
    public string NameEn { get; set; }

    /// <summary>
    /// Number of items from this brand matching current filters
    /// </summary>
    public int Count { get; set; }
}

/// <summary>
/// Single vendor filter option
/// </summary>
public class FilterVendorOption
{
    /// <summary>
    /// Vendor unique identifier
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Vendor name in Arabic
    /// </summary>
    public string NameAr { get; set; }

    /// <summary>
    /// Vendor name in English
    /// </summary>
    public string NameEn { get; set; }

    /// <summary>
    /// Number of offers from this vendor matching current filters
    /// </summary>
    public int Count { get; set; }
}
