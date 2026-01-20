//using Common.Enumerations.Order;
//using DAL.Models;
//using Shared.DTOs.Order.OrderProcessing;
//using Shared.DTOs.Order.OrderProcessing.AdminOrder;
//using Shared.DTOs.Order.OrderProcessing.CustomerOrder;
//using Shared.DTOs.Order.OrderProcessing.VendorOrder;
//using Shared.GeneralModels;

//namespace BL.Contracts.Service.Order.OrderProcessing;

///// <summary>
///// Interface for Order Management Service
///// Provides role-specific views: Customer, Vendor, Admin
///// </summary>
//public interface IOrderManagementService
//{
//    // ============================================
//    // CUSTOMER OPERATIONS
//    // ============================================

//    /// <summary>
//    /// Get customer orders list with pagination
//    /// </summary>
//    Task<AdvancedPagedResult<CustomerOrderListDto>> GetCustomerOrdersListAsync(
//        string customerId,
//        int pageNumber = 1,
//        int pageSize = 10,
//        CancellationToken cancellationToken = default);

//    /// <summary>
//    /// Get customer order full details
//    /// </summary>
//    Task<CustomerOrderDetailsDto?> GetCustomerOrderDetailsAsync(
//        Guid orderId,
//        string customerId,
//        CancellationToken cancellationToken = default);

//    /// <summary>
//    /// Cancel order (Customer)
//    /// </summary>
//    Task<ResponseModel<bool>> CancelOrderAsync(
//        Guid orderId,
//        string userId,
//        string reason,
//        CancellationToken cancellationToken = default);

//    // ============================================
//    // VENDOR OPERATIONS
//    // ============================================

//    /// <summary>
//    /// Get vendor orders with pagination and search
//    /// </summary>
//    Task<AdvancedPagedResult<VendorOrderListDto>> GetVendorOrdersAsync(
//        string vendorId,
//        string? searchTerm,
//        int pageNumber,
//        int pageSize,
//        string? sortBy,
//        string? sortDirection,
//        CancellationToken cancellationToken = default);

//    /// <summary>
//    /// Get vendor order details (shows only vendor's items)
//    /// </summary>
//    Task<VendorOrderDetailsDto?> GetVendorOrderDetailsAsync(
//        Guid orderId,
//        string vendorId,
//        CancellationToken cancellationToken = default);

//    // ============================================
//    // ADMIN OPERATIONS
//    // ============================================

//    /// <summary>
//    /// Search all orders with pagination (Admin)
//    /// </summary>
//    Task<AdvancedPagedResult<AdminOrderListDto>> SearchOrdersAsync(
//        string? searchTerm,
//        int pageNumber,
//        int pageSize,
//        string? sortBy,
//        string? sortDirection,
//        CancellationToken cancellationToken = default);

//    /// <summary>
//    /// Get admin order full details
//    /// </summary>
//    Task<AdminOrderDetailsDto?> GetAdminOrderDetailsAsync(
//        Guid orderId,
//        CancellationToken cancellationToken = default);

//    /// <summary>
//    /// Update order (Admin)
//    /// </summary>
//    Task<ResponseModel<bool>> UpdateOrderAsync(
//        UpdateOrderRequest request,
//        CancellationToken cancellationToken = default);

//    /// <summary>
//    /// Change order status (Admin)
//    /// </summary>
//    Task<ResponseModel<bool>> ChangeOrderStatusAsync(
//        Guid orderId,
//        OrderProgressStatus newStatus,
//        string? notes,
//        string adminUserId,
//        CancellationToken cancellationToken = default);
//}
