using Dashboard.Contracts.Campaign;
using Dashboard.Contracts.General;
using Shared.DTOs.Campaign;
using Shared.GeneralModels;

namespace Dashboard.Services.Campaign
{
    /// <summary>
    /// Blazor service for Campaign and Flash Sale management
    /// </summary>
    public class CampaignService : ICampaignService
    {
        private readonly IApiService _apiService;
        private const string BaseEndpoint = "api/v1/campaign";

        public CampaignService(IApiService apiService)
        {
            _apiService = apiService;
        }

        #region Campaign Queries

        /// <summary>
        /// Get all campaigns (admin)
        /// </summary>
        public async Task<ResponseModel<List<CampaignDto>>> GetAllAsync()
        {
            return await _apiService.GetAsync<List<CampaignDto>>($"{BaseEndpoint}");
        }

        /// <summary>
        /// Get active campaigns
        /// </summary>
        public async Task<ResponseModel<List<CampaignDto>>> GetActiveCampaignsAsync()
        {
            return await _apiService.GetAsync<List<CampaignDto>>($"{BaseEndpoint}/active");
        }

        /// <summary>
        /// Get active flash sales
        /// </summary>
        public async Task<ResponseModel<List<CampaignDto>>> GetActiveFlashSalesAsync()
        {
            return await _apiService.GetAsync<List<CampaignDto>>($"{BaseEndpoint}/flash-sales/active");
        }

        /// <summary>
        /// Get campaign by ID (admin)
        /// </summary>
        public async Task<ResponseModel<CampaignDto>> GetCampaignByIdAsync(Guid id)
        {
            return await _apiService.GetAsync<CampaignDto>($"{BaseEndpoint}/{id}");
        }

        #endregion

        #region Campaign Management (Admin)

        /// <summary>
        /// Create new campaign
        /// </summary>
        public async Task<ResponseModel<CampaignDto>> CreateCampaignAsync(CreateCampaignDto dto)
        {
            return await _apiService.PostAsync<CreateCampaignDto, CampaignDto>(BaseEndpoint, dto);
        }

        /// <summary>
        /// Update campaign
        /// </summary>
        public async Task<ResponseModel<CampaignDto>> UpdateCampaignAsync(Guid id, UpdateCampaignDto dto)
        {
            return await _apiService.PostAsync<UpdateCampaignDto, CampaignDto>($"{BaseEndpoint}", dto);
        }

        /// <summary>
        /// Delete campaign
        /// </summary>
        public async Task<ResponseModel<object>> DeleteCampaignAsync(Guid id)
        {
            return await _apiService.DeleteAsync<object>($"{BaseEndpoint}/{id}");
        }

        #endregion

        #region Campaign Items

        /// <summary>
        /// Get campaign items
        /// </summary>
        public async Task<ResponseModel<List<CampaignItemDto>>> GetCampaignItemsAsync(Guid campaignId)
        {
            return await _apiService.GetAsync<List<CampaignItemDto>>($"{BaseEndpoint}/{campaignId}/items");
        }

        /// <summary>
        /// Add item to campaign
        /// </summary>
        public async Task<ResponseModel<CampaignItemDto>> AddItemToCampaignAsync(Guid campaignId, AddCampaignItemDto dto)
        {
            return await _apiService.PostAsync<AddCampaignItemDto, CampaignItemDto>(
                $"{BaseEndpoint}/{campaignId}/items",
                dto);
        }

        /// <summary>
        /// Remove item from campaign
        /// </summary>
        public async Task<ResponseModel<object>> RemoveItemFromCampaignAsync(Guid campaignId, Guid itemId)
        {
            return await _apiService.DeleteAsync<object>($"{BaseEndpoint}/{campaignId}/items/{itemId}");
        }

        #endregion
    }
}