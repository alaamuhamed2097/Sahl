using Common.Enumerations.Order;
using Shared.DTOs.Order.OrderProcessing;
using Shared.DTOs.Order.OrderProcessing.AdminOrder;
using Shared.GeneralModels;

namespace BL.Contracts.Service.Order.OrderProcessing;

public interface IAdminOrderService
{
    Task<(List<AdminOrderListDto> Orders, int TotalCount)> SearchOrdersAsync(
        string? searchTerm,
        int pageNumber,
        int pageSize,
        string? sortBy,
        string? sortDirection,
        CancellationToken cancellationToken = default);

    Task<AdminOrderDetailsDto?> GetAdminOrderDetailsAsync(
        Guid orderId,
        CancellationToken cancellationToken = default);

    Task<ResponseModel<bool>> ChangeOrderStatusAsync(
        Guid orderId,
        OrderProgressStatus newStatus,
        string? notes,
        string adminUserId,
        CancellationToken cancellationToken = default);

    Task<ResponseModel<bool>> UpdateOrderAsync(
        UpdateOrderRequest request,
        CancellationToken cancellationToken = default);

    Task<int> CountTodayOrdersAsync(
        DateTime date,
        CancellationToken cancellationToken = default);
}