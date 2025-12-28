using Shared.DTOs.Campaign;

namespace BL.Contracts.Service.Merchandising.Campaign;

/// <summary>
/// Campaign Service Interface
/// </summary>
public interface ICampaignService
{
    #region Campaign Management

    /// <summary>
    /// Get campaign by ID
    /// </summary>
    Task<CampaignDto?> GetCampaignByIdAsync(Guid id);

    /// <summary>
    /// Get all active campaigns
    /// </summary>
    Task<List<CampaignDto>> GetActiveCampaignsAsync();

    /// <summary>
    /// Get all active flash sales
    /// </summary>
    Task<List<CampaignDto>> GetActiveFlashSalesAsync();

    /// <summary>
    /// Create new campaign
    /// </summary>
    Task<CampaignDto> CreateCampaignAsync(CreateCampaignDto dto, Guid userId);

    /// <summary>
    /// Update campaign
    /// </summary>
    Task<CampaignDto> UpdateCampaignAsync(UpdateCampaignDto dto, Guid userId);

    /// <summary>
    /// Delete campaign (soft delete)
    /// </summary>
    Task<bool> DeleteCampaignAsync(Guid id, Guid userId);

    #endregion

    #region Campaign Items

    /// <summary>
    /// Get all items in a campaign
    /// </summary>
    Task<List<CampaignItemDto>> GetCampaignItemsAsync(Guid campaignId);

    /// <summary>
    /// Add item to campaign
    /// </summary>
    Task<CampaignItemDto> AddItemToCampaignAsync(AddCampaignItemDto dto, Guid userId);

    /// <summary>
    /// Remove item from campaign
    /// </summary>
    Task<bool> RemoveItemFromCampaignAsync(Guid campaignItemId, Guid userId);

    /// <summary>
    /// Update sold count when item is purchased
    /// </summary>
    Task<bool> UpdateSoldCountAsync(Guid campaignItemId, int quantity);

    #endregion
}