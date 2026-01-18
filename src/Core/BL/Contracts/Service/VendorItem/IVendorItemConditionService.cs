using Common.Filters;
using DAL.Models;
using DAL.ResultModels;
using Shared.DTOs.ECommerce.Offer;

namespace BL.Contracts.Service.VendorItem
{
    public interface IVendorItemConditionService
    {
        Task<bool> DeleteAsync(Guid offerConditionId, Guid userId);
        Task<VendorItemConditionDto> FindByIdAsync(Guid Id);
        Task<PagedResult<VendorItemConditionDto>> GetPageAsync(BaseSearchCriteriaModel criteriaModel);
        Task<IEnumerable<VendorItemConditionDto>> GetAllAsync();
        Task<SaveResult> SaveAsync(VendorItemConditionDto dto, Guid userId);
        Task<IEnumerable<VendorItemConditionDto>> GetNewConditionsAsync();
    }
}