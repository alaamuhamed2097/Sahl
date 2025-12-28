using Shared.DTOs.Order.CouponCode;
using Shared.GeneralModels;

namespace Dashboard.Contracts
{
    public interface ICouponCodeService
    {
        Task<ResponseModel<bool>> DeleteAsync(Guid id);
        Task<ResponseModel<IEnumerable<CouponCodeDto>>> GetAllAsync();
        Task<ResponseModel<CouponCodeDto>> GetByIdAsync(Guid id);
        Task<ResponseModel<bool>> SaveAsync(CouponCodeDto AccountType);
    }
}
