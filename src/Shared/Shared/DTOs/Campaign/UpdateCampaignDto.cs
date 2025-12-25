namespace Shared.DTOs.Campaign
{
    /// <summary>
    /// DTO for updating a campaign
    /// </summary>
    public class UpdateCampaignDto
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
    }
}