namespace Shared.DTOs.Campaign
{
    #region Campaign Item DTOs

    /// <summary>
    /// DTO for adding item to campaign
    /// </summary>
    public class AddCampaignItemDto
    {
		public Guid OfferCompinationPriceId { get; set; }
		public Guid CampaignId { get; set; }
    }

    #endregion
}