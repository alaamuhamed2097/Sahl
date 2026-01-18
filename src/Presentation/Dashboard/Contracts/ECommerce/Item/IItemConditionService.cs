using Shared.DTOs.ECommerce.Offer;
using Shared.GeneralModels;

namespace Dashboard.Contracts.ECommerce.Item
{
    public interface IItemConditionService
    {
        Task<ResponseModel<bool>> DeleteAsync(Guid id);
        Task<ResponseModel<IEnumerable<VendorItemConditionDto>>> GetAllAsync();
        Task<ResponseModel<VendorItemConditionDto>> GetByIdAsync(Guid id);
        Task<ResponseModel<VendorItemConditionDto>> SaveAsync(VendorItemConditionDto itemCondition);
    }
}