using Shared.DTOs.ECommerce.PromoCode;
using Shared.GeneralModels;

namespace Dashboard.Contracts
{
    public interface IPromoCodeService
    {
        Task<ResponseModel<bool>> DeleteAsync(Guid id);
        Task<ResponseModel<IEnumerable<PromoCodeDto>>> GetAllAsync();
        Task<ResponseModel<PromoCodeDto>> GetByIdAsync(Guid id);
        Task<ResponseModel<bool>> SaveAsync(PromoCodeDto AccountType);
    }
}
