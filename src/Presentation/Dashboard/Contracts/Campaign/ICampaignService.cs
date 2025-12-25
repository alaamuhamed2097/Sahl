using Shared.DTOs.Campaign;
using Shared.GeneralModels;

namespace Dashboard.Contracts.Campaign
{
    /// <summary>
    /// Interface for Campaign service in Blazor
    /// </summary>
    public interface ICampaignService
    {
        #region Campaign Queries

        /// <summary>
        /// Get active campaigns
        /// </summary>
        Task<ResponseModel<List<CampaignDto>>> GetActiveCampaignsAsync();

        /// <summary>
        /// Get active flash sales
        /// </summary>
        Task<ResponseModel<List<CampaignDto>>> GetActiveFlashSalesAsync();

        /// <summary>
        /// Get campaign by ID (admin)
        /// </summary>
        Task<ResponseModel<CampaignDto>> GetCampaignByIdAsync(Guid id);

        #endregion

        #region Campaign Management (Admin)

        /// <summary>
        /// Create new campaign
        /// </summary>
        Task<ResponseModel<CampaignDto>> CreateCampaignAsync(CreateCampaignDto dto);

        /// <summary>
        /// Update campaign
        /// </summary>
        Task<ResponseModel<CampaignDto>> UpdateCampaignAsync(Guid id, UpdateCampaignDto dto);

        /// <summary>
        /// Delete campaign
        /// </summary>
        Task<ResponseModel<object>> DeleteCampaignAsync(Guid id);

        #endregion

        #region Campaign Items

        /// <summary>
        /// Get campaign items
        /// </summary>
        Task<ResponseModel<List<CampaignItemDto>>> GetCampaignItemsAsync(Guid campaignId);

        /// <summary>
        /// Add item to campaign
        /// </summary>
        Task<ResponseModel<CampaignItemDto>> AddItemToCampaignAsync(Guid campaignId, AddCampaignItemDto dto);

        /// <summary>
        /// Remove item from campaign
        /// </summary>
        Task<ResponseModel<object>> RemoveItemFromCampaignAsync(Guid campaignId, Guid itemId);

        #endregion
    }
}