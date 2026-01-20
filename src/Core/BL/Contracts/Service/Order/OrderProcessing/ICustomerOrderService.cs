using DAL.Models;
using Shared.DTOs.Order.OrderProcessing.CustomerOrder;
using Shared.GeneralModels;

namespace BL.Contracts.Service.Order.OrderProcessing;

public interface ICustomerOrderService
{
    Task<AdvancedPagedResult<CustomerOrderListDto>> GetCustomerOrdersListAsync(
        string customerId,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default);

    Task<CustomerOrderDetailsDto?> GetCustomerOrderDetailsAsync(
        Guid orderId,
        string customerId,
        CancellationToken cancellationToken = default);

    Task<ResponseModel<bool>> CancelOrderAsync(
        Guid orderId,
        string customerId,
        string? reason,
        CancellationToken cancellationToken = default);
}