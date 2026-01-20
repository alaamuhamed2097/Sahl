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
		/// Get campaign by ID with all related data
		/// </summary>
		Task<TbCampaign> GetCampaignByIdAsync(Guid campaignId);

    }
}