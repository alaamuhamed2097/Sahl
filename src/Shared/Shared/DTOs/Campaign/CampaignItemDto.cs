namespace Shared.DTOs.Campaign
{
    #region Campaign Item DTOs

    /// <summary>
    /// Campaign Item DTO for read operations
    /// </summary>
    public class CampaignItemDto
    {
        public Guid Id { get; set; }
        public string ItemNameAr { get; set; } = string.Empty;
        public string ItemNameEn { get; set; } = string.Empty;
        public string? ItemImageUrl { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDateUtc { get; set; }
    }

    #endregion
}