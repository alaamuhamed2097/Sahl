namespace Shared.DTOs.Campaign
{
    #region Campaign Item DTOs

    /// <summary>
    /// DTO for adding item to campaign
    /// </summary>
    public class AddCampaignItemDto
    {
        public Guid CampaignId { get; set; }
        public Guid ItemId { get; set; }
        public decimal CampaignPrice { get; set; }
        public int? StockLimit { get; set; }
        public int DisplayOrder { get; set; }
    }

    #endregion
}