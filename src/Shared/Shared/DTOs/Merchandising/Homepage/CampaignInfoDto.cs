namespace Shared.DTOs.Merchandising.Homepage
{
    /// <summary>
    /// Campaign information
    /// </summary>
    public class CampaignInfoDto
    {
        public Guid Id { get; set; }
        public string NameAr { get; set; } = string.Empty;
        public string NameEn { get; set; } = string.Empty;
        public bool IsFlashSale { get; set; }
        public DateTime? FlashSaleEndTime { get; set; }
        public string? BadgeTextAr { get; set; }
        public string? BadgeTextEn { get; set; }
        public string? BadgeColor { get; set; }
    }
}