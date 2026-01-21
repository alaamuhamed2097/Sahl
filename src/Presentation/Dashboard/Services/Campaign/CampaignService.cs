using Common.Filters;
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
        private const string BaseItemEndpoint = "api/v1/campaignItem";

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
		public async Task<ResponseModel<CampaignDto>> UpdateCampaignAsync( UpdateCampaignDto dto)
		{
			return await _apiService.PostAsync<UpdateCampaignDto, CampaignDto>($"{BaseEndpoint}/update", dto);
			                                                                                   
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
		/// Search campaign items with pagination
		/// </summary>
		public async Task<ResponseModel<PaginatedSearchResult<CampaignItemDto>>> SearchCampaignItemsAsync(
			Guid campaignId,
			BaseSearchCriteriaModel searchCriteria)
		{
			var queryString = $"?PageNumber={searchCriteria.PageNumber}&PageSize={searchCriteria.PageSize}";

			if (!string.IsNullOrEmpty(searchCriteria.SearchTerm))
			{
				queryString += $"&SearchTerm={Uri.EscapeDataString(searchCriteria.SearchTerm)}";
			}

			return await _apiService.GetAsync<PaginatedSearchResult<CampaignItemDto>>(
				$"{BaseItemEndpoint}/{campaignId}/search{queryString}");
		}
		/// <summary>
		/// Get campaign items
		/// </summary>
		public async Task<ResponseModel<List<CampaignItemDto>>> GetCampaignItemsAsync(Guid campaignId)
        {
            return await _apiService.GetAsync<List<CampaignItemDto>>($"{BaseItemEndpoint}/{campaignId}/items");
        }

        /// <summary>
        /// Add item to campaign
        /// </summary>
        public async Task<ResponseModel<CampaignItemDto>> AddItemToCampaignAsync(Guid campaignId, AddCampaignItemDto dto)
        {
            return await _apiService.PostAsync<AddCampaignItemDto, CampaignItemDto>(
                $"{BaseItemEndpoint}/{campaignId}/items",
                dto);
        }

        /// <summary>
        /// Remove item from campaign
        /// </summary>
        public async Task<ResponseModel<object>> RemoveItemFromCampaignAsync(Guid campaignId, Guid itemId)
        {
            return await _apiService.DeleteAsync<object>($"{BaseItemEndpoint}/{campaignId}/items/{itemId}");
        }

        #endregion
    }
}