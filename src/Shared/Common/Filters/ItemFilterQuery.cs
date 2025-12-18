namespace Common.Filters
{
    /// <summary>
    /// Advanced filter model for customer website item search
    /// Supports filtering by price, rating, availability, vendor info, and more
    /// </summary>
    public class ItemFilterQuery
    {
        public string SearchTerm { get; set; }
        public Guid? CategoryId { get; set; }        // Single ID
        public Guid? VendorId { get; set; }          // Single ID
        public Guid? BrandId { get; set; }           // Single ID
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public decimal? MinItemRating { get; set; }
        public bool? InStockOnly { get; set; }
        public bool? FreeShippingOnly { get; set; }
        public Guid? ConditionId { get; set; }
        public bool? WithWarrantyOnly { get; set; }
        public List<Guid> AttributeIds { get; set; }      // Converted to comma-separated
        public List<string> AttributeValues { get; set; } // Converted to pipe-separated
        public string SortBy { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
