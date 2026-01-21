using Common.Filters;
using DAL.Models;
using Shared.DTOs.Campaign;
using Shared.GeneralModels;

namespace BL.Contracts.Service.Merchandising.Campaign;

/// <summary>
/// Campaign Service Interface
/// </summary>
public interface ICampaignService
{
    #region Campaign Management

    /// <summary>
    /// Get all campaigns (admin)
    /// </summary>
    Task<IEnumerable<CampaignDto>> GetAllCampaignsAsync();

	/// <summary>
	/// Get all active campaigns
	/// </summary>
	Task<IEnumerable<CampaignDto>> GetActiveCampaignsAsync();

	/// <summary>
	/// Get campaign by ID
	/// </summary>
	Task<CampaignDto> GetCampaignByIdAsync(Guid id);
	/// <summary>
	/// Search campaigns with pagination
	/// </summary>
	Task<ResponseModel<PaginatedSearchResult<CampaignDto>>> SearchCampaignsAsync(CampaignSearchCriteriaModel searchCriteria);

    /// <summary>
    /// Get all active flash sales
    /// </summary>
    Task<IEnumerable<CampaignDto>> GetActiveFlashSalesAsync();

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

}