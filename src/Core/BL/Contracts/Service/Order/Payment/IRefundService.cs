using BL.Services.Order.Payment;
using DAL.Models;
using Shared.DTOs.Order.Payment.Refund;

namespace BL.Contracts.Service.Order.Payment;

/// <summary>
/// Interface for Refund Service
/// </summary>
public interface IRefundService
{
    /// <summary>
    /// Creates a new refund request
    /// </summary>
    Task<RefundRequestResult> CreateRefundRequestAsync(
        CreateRefundRequestDto requestDto,
        string customerId);

    /// <summary>
    /// Updates the status of a refund
    /// </summary>
    /// <param name="refundId"></param>
    /// <param name="statusDto"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<RefundStatusUpdateResult> UpdateRefundStatusAsync(
        Guid refundId,
        UpdateRefundStatusDto statusDto,
        string userId);

    /// <summary>
    /// Gets refund request by ID
    /// </summary>
    Task<RefundRequestDto?> GetRefundRequestByIdAsync(Guid refundRequestId);

    /// <summary>
    /// Gets refund request by order ID
    /// </summary>
    Task<RefundRequestDto?> GetRefundRequestByOrderDetailIdAsync(Guid orderId);

    /// <summary>
    /// Gets all refund requests with pagination and filtering
    /// </summary>
    Task<PagedResult<RefundRequestDto>> GetRefundRequestsAsync(
        RefundSearchCriteria criteria);
}