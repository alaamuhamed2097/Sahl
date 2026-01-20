using DAL.Models;
using Shared.DTOs.Order.OrderProcessing.VendorOrder;

namespace BL.Contracts.Service.Order.OrderProcessing;

public interface IVendorOrderService
{
    Task<AdvancedPagedResult<VendorOrderListDto>> GetVendorOrdersAsync(
        string vendorId,
        string? searchTerm,
        int pageNumber,
        int pageSize,
        string? sortBy,
        string? sortDirection,
        CancellationToken cancellationToken = default);

    Task<VendorOrderDetailsDto?> GetVendorOrderDetailsAsync(
        Guid orderId,
        string vendorId,
        CancellationToken cancellationToken = default);
}