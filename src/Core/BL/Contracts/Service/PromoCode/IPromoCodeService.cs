using DAL.Models;
using Shared.DTOs.ECommerce.PromoCode;
using Shared.GeneralModels.Parameters;
using Shared.GeneralModels.ResultModels;
using Shared.GeneralModels.SearchCriteriaModels;

namespace BL.Contracts.Service.PromoCode
{
    public interface IPromoCodeService
    {
        public PaginatedDataModel<PromoCodeDto> GetPage(BaseSearchCriteriaModel criteriaModel);
        public List<PromoCodeDto> GetAll();
        public PromoCodeDto GetById(Guid id);
        public Task<bool> Save(PromoCodeDto dto, Guid userId);
        public bool Delete(Guid id, Guid userId);
        public Task<ServiceResult<AppliedPromoCodeResult>> ApplyPromoCode(ApplyPromoCodeRequest request);
        public Task<ServiceResult<PromoCodeValidationResult>> ValidatePromoCodeAsync(string code, string userId);
    }
}
