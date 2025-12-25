namespace Shared.DTOs.Campaign
{
    /// <summary>
    /// Campaign DTO for read operations
    /// </summary>
    public class CampaignDto
    {
        public Guid Id { get; set; }
        public string NameAr { get; set; } = string.Empty;
        public string NameEn { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }

        // Flash Sale properties
        public bool IsFlashSale { get; set; }
        public DateTime? FlashSaleEndTime { get; set; }
        public int? MaxQuantityPerUser { get; set; }

        // Badge
        public string? BadgeTextAr { get; set; }
        public string? BadgeTextEn { get; set; }
        public string? BadgeColor { get; set; }

        // Counts
        public int TotalItems { get; set; }
        public int TotalSold { get; set; }

        public DateTime CreatedDateUtc { get; set; }
    }
}