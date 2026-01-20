using Common.Filters;
using Dashboard.Models.pagintion;
using Shared.DTOs.Order.Payment.Refund;
using Shared.GeneralModels;

namespace Dashboard.Contracts.Order
{
    public interface IRefundService
    {
        Task<ResponseModel<bool>> ChangeRefundStatusAsync(RefundResponseDto refund);
        Task<ResponseModel<bool>> DeleteAsync(Guid id);
        Task<ResponseModel<IEnumerable<RefundRequestDto>>> GetAllAsync();
        Task<ResponseModel<RefundDto>> GetByOrderIdAsync(Guid id);
        Task<ResponseModel<RefundDetailsDto>> GetByIdAsync(Guid id);
        Task<ResponseModel<PaginatedDataModel<RefundRequestDto>>> SearchAsync(RefundSearchCriteria model, string Endpoint);
        Task<ResponseModel<bool>> UpdateAsync(RefundRequestDto refund);
    }
}