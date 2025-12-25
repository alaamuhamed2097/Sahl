namespace Shared.DTOs.Campaign
{
    #region Campaign Item DTOs

    /// <summary>
    /// Campaign Item DTO for read operations
    /// </summary>
    public class CampaignItemDto
    {
        public Guid Id { get; set; }
        public Guid CampaignId { get; set; }
        public Guid ItemId { get; set; }

        // Item info (from join)
        public string ItemNameAr { get; set; } = string.Empty;
        public string ItemNameEn { get; set; } = string.Empty;
        public string? ItemImageUrl { get; set; }

        // Pricing
        public decimal CampaignPrice { get; set; }

        // Stock tracking (for flash sales)
        public int? StockLimit { get; set; }
        public int SoldCount { get; set; }
        public int? RemainingStock => StockLimit.HasValue ? StockLimit.Value - SoldCount : null;

        // Display
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }

        public DateTime CreatedDateUtc { get; set; }
    }

    #endregion
}