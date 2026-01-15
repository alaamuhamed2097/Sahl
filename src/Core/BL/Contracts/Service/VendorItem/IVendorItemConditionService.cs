using Common.Filters;
using DAL.Models;
using DAL.ResultModels;
using Shared.DTOs.ECommerce.Offer;

namespace BL.Contracts.Service.VendorItem
{
    public interface IVendorItemConditionService
    {
        Task<bool> DeleteAsync(Guid offerConditionId, Guid userId);
        Task<OfferConditionDto> FindByIdAsync(Guid Id);
        Task<PagedResult<OfferConditionDto>> GetPage(BaseSearchCriteriaModel criteriaModel);
        Task<SaveResult> SaveAsync(OfferConditionDto dto, Guid userId);
        Task<IEnumerable<OfferConditionDto>> GetNewConditions();
    }
}