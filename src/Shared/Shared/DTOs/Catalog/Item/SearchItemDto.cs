namespace Shared.DTOs.Catalog.Item
{
    /// <summary>
    /// DTO for search item results returned to presentation layer
    /// Contains bilingual support for Arabic and English
    /// </summary>
    public class SearchItemDto
    {
        public Guid ItemId { get; set; }
        public Guid ItemCombinationId { get; set; }
        public Guid OfferCombinationPricingId { get; set; }
        public string TitleAr { get; set; }
        public string TitleEn { get; set; }
        public string ShortDescriptionAr { get; set; }
        public string ShortDescriptionEn { get; set; }
        public Guid CategoryId { get; set; }
        public Guid? BrandId { get; set; }
        public string BrandNameAr { get; set; }
        public string BrandNameEn { get; set; }
        public string ThumbnailImage { get; set; }
        public DateTime CreatedDateUtc { get; set; }
        public decimal? AverageRating { get; set; }
        public decimal Price { get; set; }
        public decimal SalesPrice { get; set; }
        public int AvailableQuantity { get; set; }
        public string StockStatus { get; set; }
        public bool IsFreeShipping { get; set; }
        public int TotalRecords { get; set; }

        // UI Enhancement
        public List<BadgeDto> Badges { get; set; }
    }

    /// <summary>
    /// Badge DTO for UI visual indicators (bilingual)
    /// </summary>
    public class BadgeDto
    {
        public string TextAr { get; set; }
        public string TextEn { get; set; }
        public string Type { get; set; }
        public string Variant { get; set; }
    }
}