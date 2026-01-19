using Domains.Entities.Campaign;

namespace DAL.Contracts.Repositories.Merchandising
{
    /// <summary>
    /// Campaign Repository Interface
    /// </summary>
    public interface ICampaignRepository : ITableRepository<TbCampaign>
    {
        /// <summary>
        /// Get all active campaigns
        /// </summary>
        Task<IEnumerable<TbCampaign>> GetActiveCampaignsAsync();

		/// <summary>
		/// Get flash sales that are currently active
		/// </summary>
		Task<IEnumerable<TbCampaign>> GetActiveFlashSalesAsync();
		/// <summary>
		/// Get all items in a campaign
		/// </summary>
		Task<IEnumerable<TbCampaignItem>> GetCampaignItemsAsync(Guid campaignId);

		/// <summary>
		/// Get campaign by ID with all related data
		/// </summary>
		Task<TbCampaign> GetCampaignByIdAsync(Guid campaignId);

        /// <summary>
        /// Add item to campaign
        /// </summary>
        Task<TbCampaignItem> AddItemToCampaignAsync(TbCampaignItem campaignItem);

        /// <summary>
        /// Remove item from campaign (soft delete)
        /// </summary>
        Task<bool> RemoveItemFromCampaignAsync(Guid campaignItemId);

        /// <summary>
        /// Update sold count for campaign item
        /// </summary>
        Task<bool> IncrementSoldCountAsync(Guid campaignItemId, int quantity);  
    }
}