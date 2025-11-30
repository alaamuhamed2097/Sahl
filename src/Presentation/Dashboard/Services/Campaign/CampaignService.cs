using Dashboard.Contracts.Campaign;
using Dashboard.Constants;
using Dashboard.Contracts.General;
using Shared.DTOs.Campaign;
using Shared.GeneralModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dashboard.Services.Campaign
{
    public class CampaignService : ICampaignService
    {
        private readonly IApiService _apiService;

        public CampaignService(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<ResponseModel<IEnumerable<CampaignDto>>> GetAllAsync()
        {
            return await _apiService.GetAsync<IEnumerable<CampaignDto>>(ApiEndpoints.Campaign.Get);
        }

        public async Task<ResponseModel<CampaignDto>> GetByIdAsync(Guid id)
        {
            return await _apiService.GetAsync<CampaignDto>(string.Format(ApiEndpoints.Campaign.GetById, id));
        }

        public async Task<ResponseModel<IEnumerable<CampaignDto>>> GetActiveAsync()
        {
            return await _apiService.GetAsync<IEnumerable<CampaignDto>>("api/Campaign/active");
        }

        public async Task<ResponseModel<CampaignDto>> CreateAsync(CampaignCreateDto dto)
        {
            return await _apiService.PostAsync<CampaignCreateDto, CampaignDto>(ApiEndpoints.Campaign.Create, dto);
        }

        public async Task<ResponseModel<CampaignDto>> UpdateAsync(Guid id, CampaignUpdateDto dto)
        {
            return await _apiService.PutAsync<CampaignUpdateDto, CampaignDto>(string.Format(ApiEndpoints.Campaign.Update, id), dto);
        }

        public async Task<ResponseModel<bool>> DeleteAsync(Guid id)
        {
            return await _apiService.DeleteAsync<bool>(string.Format(ApiEndpoints.Campaign.Delete, id));
        }

        public async Task<ResponseModel<IEnumerable<FlashSaleDto>>> GetAllFlashSalesAsync()
        {
            return await _apiService.GetAsync<IEnumerable<FlashSaleDto>>("api/Campaign/flashsales");
        }
    }
}
