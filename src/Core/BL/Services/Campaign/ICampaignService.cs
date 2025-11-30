using Shared.DTOs.Campaign;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BL.Services.Campaign
{
    /// <summary>
    /// Service interface for Campaign and Flash Sale management
    /// </summary>
    public interface ICampaignService
    {
        #region Campaign Management

        /// <summary>
        /// Gets a campaign by ID
        /// </summary>
        Task<CampaignDto> GetCampaignByIdAsync(Guid id);

        /// <summary>
        /// Gets all campaigns
        /// </summary>
        Task<List<CampaignDto>> GetAllCampaignsAsync();

        /// <summary>
        /// Gets active campaigns
        /// </summary>
        Task<List<CampaignDto>> GetActiveCampaignsAsync();

        /// <summary>
        /// Gets campaigns by status
        /// </summary>
        Task<List<CampaignDto>> GetCampaignsByStatusAsync(int status);

        /// <summary>
        /// Creates a new campaign
        /// </summary>
        Task<CampaignDto> CreateCampaignAsync(CampaignCreateDto dto);

        /// <summary>
        /// Updates an existing campaign
        /// </summary>
        Task<CampaignDto> UpdateCampaignAsync(CampaignUpdateDto dto);

        /// <summary>
        /// Deletes a campaign
        /// </summary>
        Task<bool> DeleteCampaignAsync(Guid id);

        /// <summary>
        /// Activates a campaign
        /// </summary>
        Task<bool> ActivateCampaignAsync(Guid id);

        /// <summary>
        /// Deactivates a campaign
        /// </summary>
        Task<bool> DeactivateCampaignAsync(Guid id);

        /// <summary>
        /// Searches campaigns with filters
        /// </summary>
        Task<List<CampaignDto>> SearchCampaignsAsync(CampaignSearchRequest request);

        #endregion

        #region Campaign Products

        /// <summary>
        /// Gets products for a campaign
        /// </summary>
        Task<List<CampaignProductDto>> GetCampaignProductsAsync(Guid campaignId);

        /// <summary>
        /// Adds a product to campaign
        /// </summary>
        Task<CampaignProductDto> AddProductToCampaignAsync(CampaignProductCreateDto dto);

        /// <summary>
        /// Removes a product from campaign
        /// </summary>
        Task<bool> RemoveProductFromCampaignAsync(Guid campaignProductId);

        /// <summary>
        /// Approves a campaign product
        /// </summary>
        Task<bool> ApproveCampaignProductAsync(Guid campaignProductId, Guid approvedByUserId);

        /// <summary>
        /// Rejects a campaign product
        /// </summary>
        Task<bool> RejectCampaignProductAsync(Guid campaignProductId);

        #endregion

        #region Flash Sales

        /// <summary>
        /// Gets all flash sales
        /// </summary>
        Task<List<FlashSaleDto>> GetAllFlashSalesAsync();

        /// <summary>
        /// Gets active flash sales
        /// </summary>
        Task<List<FlashSaleDto>> GetActiveFlashSalesAsync();

        /// <summary>
        /// Gets a flash sale by ID
        /// </summary>
        Task<FlashSaleDto> GetFlashSaleByIdAsync(Guid id);

        /// <summary>
        /// Creates a new flash sale
        /// </summary>
        Task<FlashSaleDto> CreateFlashSaleAsync(FlashSaleCreateDto dto);

        /// <summary>
        /// Updates a flash sale
        /// </summary>
        Task<FlashSaleDto> UpdateFlashSaleAsync(Guid id, FlashSaleCreateDto dto);

        /// <summary>
        /// Deletes a flash sale
        /// </summary>
        Task<bool> DeleteFlashSaleAsync(Guid id);

        #endregion

        #region Statistics & Reports

        /// <summary>
        /// Gets campaign statistics
        /// </summary>
        Task<CampaignStatisticsDto> GetCampaignStatisticsAsync();

        /// <summary>
        /// Gets campaign statistics by date range
        /// </summary>
        Task<CampaignStatisticsDto> GetCampaignStatisticsAsync(DateTime fromDate, DateTime toDate);

        /// <summary>
        /// Gets top performing campaigns
        /// </summary>
        Task<List<CampaignDto>> GetTopPerformingCampaignsAsync(int count = 10);

        #endregion
    }
}
