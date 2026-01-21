using Common.Filters;
using DAL.ResultModels;
using Domains.Entities.Order;
using Domains.Entities.Order.Refund;
using Domains.Entities.Order.Returns;
using Domains.Views.Order.Refund;

namespace DAL.Repositories.Order.Refund
{
    public interface IRefundRepository
    {
        Task<SaveResult> CreateAsync(TbRefund refund, Guid userId);
        Task<SaveResult> CreateStatusHistoryAsync(TbRefundStatusHistory historyRecord, Guid userId);
        Task<bool> ExistsForOrderDetailAsync(Guid orderDetailId);
        Task<TbRefund> GetByIdAsync(Guid id);
        // Order related ///////////////////
        Task<TbOrder> GetOrderByIdAsync(Guid id);
        Task<SaveResult> UpdateOrderAsync(TbOrder model, Guid updaterId, CancellationToken cancellationToken = default);
        ///////////////////////////////////
        Task<TbRefund> GetByNumberAsync(string number);
        Task<TbRefund> GetByOrderDetailIdAsync(Guid orderDetailId);
        Task<VwRefundDetails> GetDetailsByIdAsync(Guid id);
        Task<(List<TbRefund> Items, int TotalCount)> GetPagedAsync(RefundSearchCriteria criteria);
        Task UpdateAsync(TbRefund refund, Guid userId);
    }
}