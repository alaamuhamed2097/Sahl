namespace Shared.DTOs.Campaign
{
    /// <summary>
    /// DTO for creating a campaign
    /// </summary>
    public class CreateCampaignDto
    {
        public string NameAr { get; set; } = string.Empty;
        public string NameEn { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        // Flash Sale properties (optional)
        public bool IsFlashSale { get; set; }
        public DateTime? FlashSaleEndTime { get; set; }
        public int? MaxQuantityPerUser { get; set; }

        // Badge (optional)
        public string? BadgeTextAr { get; set; }
        public string? BadgeTextEn { get; set; }
        public string? BadgeColor { get; set; }
    }
}