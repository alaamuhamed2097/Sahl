using Shared.DTOs.Campaign;
using Shared.GeneralModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dashboard.Contracts.Campaign
{
    public interface ICampaignService
    {
        Task<ResponseModel<IEnumerable<CampaignDto>>> GetAllAsync();
        Task<ResponseModel<CampaignDto>> GetByIdAsync(Guid id);
        Task<ResponseModel<IEnumerable<CampaignDto>>> GetActiveAsync();
        Task<ResponseModel<CampaignDto>> CreateAsync(CampaignCreateDto dto);
        Task<ResponseModel<CampaignDto>> UpdateAsync(Guid id, CampaignUpdateDto dto);
        Task<ResponseModel<bool>> DeleteAsync(Guid id);
        Task<ResponseModel<IEnumerable<FlashSaleDto>>> GetAllFlashSalesAsync();
    }
}
