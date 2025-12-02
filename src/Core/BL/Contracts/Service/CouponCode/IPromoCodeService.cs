using DAL.Models;
using Shared.DTOs.ECommerce.CouponCode;
using Shared.GeneralModels.Parameters;
using Shared.GeneralModels.ResultModels;
using Shared.GeneralModels.SearchCriteriaModels;

namespace BL.Contracts.Service.CouponCode
{
    public interface ICouponCodeService
    {
        public Task<PaginatedDataModel<CouponCodeDto>> GetPage(BaseSearchCriteriaModel criteriaModel);
        public Task<List<CouponCodeDto>> GetAll();
        public Task<CouponCodeDto> GetById(Guid id);
        public Task<bool> Save(CouponCodeDto dto, Guid userId);
        public Task<bool> Delete(Guid id, Guid userId);
        //public Task<ServiceResult<AppliedCouponCodeResult>> ApplyCouponCode(ApplyCouponCodeRequest request);
        public Task<ServiceResult<CouponCodeValidationResult>> ValidateCouponCodeAsync(string code, string userId);
    }
}
