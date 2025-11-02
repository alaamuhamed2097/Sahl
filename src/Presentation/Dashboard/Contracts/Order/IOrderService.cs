using Shared.DTOs.ECommerce;
using Shared.GeneralModels;
using Shared.GeneralModels.SearchCriteriaModels;
using Dashboard.Models.pagintion;

namespace Dashboard.Contracts.Order
{
    public interface IOrderService
    {
        Task<ResponseModel<PaginatedDataModel<OrderDto>>> GetPage(OrderSearchCriteriaModel searchModel);
        Task<ResponseModel<bool>> DeleteAsync(Guid id);
        Task<ResponseModel<IEnumerable<OrderDto>>> GetAllAsync();
        Task<ResponseModel<OrderDto>> GetByIdAsync(Guid id);
        Task<ResponseModel<string>> GetOrderNumber(string Idetifire);
        Task<ResponseModel<bool>> ChangeOrderStatusAsync(OrderDto Order);
        Task<ResponseModel<OrderDto>> SaveAsync(OrderDto Order);
    }
}