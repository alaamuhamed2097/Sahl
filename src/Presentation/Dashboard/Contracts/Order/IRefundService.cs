using Shared.DTOs.ECommerce.Order;
using Shared.GeneralModels;

namespace Dashboard.Contracts.Order
{
    public interface IRefundService
    {
        Task<ResponseModel<bool>> ChangeRefundStatusAsync(RefundResponseDto refund);
        Task<ResponseModel<bool>> DeleteAsync(Guid id);
        Task<ResponseModel<IEnumerable<RefundDto>>> GetAllAsync();
        Task<ResponseModel<RefundDto>> GetByOrderIdAsync(Guid id);
        Task<ResponseModel<bool>> SaveAsync(RefundRequestDto refund);
    }
}