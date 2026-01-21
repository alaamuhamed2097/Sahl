using BL.Services.Order.Payment;
using Common.Filters;
using DAL.Models;
using Shared.DTOs.Order.Payment.Refund;
using Shared.ResultModels.Refund;
using System.Threading.Tasks;

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
        UpdateRefundStatusDto statusDto,
        string userId);

    /// <summary>
    /// Gets refund request by ID
    /// </summary>
    Task<RefundRequestDto?> GetRefundRequestByIdAsync(Guid refundRequestId);

    /// <summary>
    /// Asynchronously retrieves a refund request by its unique number.
    /// </summary>
    /// <param name="number">The unique identifier number of the refund request to retrieve. Cannot be null or empty.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="RefundRequestDto"/> if
    /// a refund request with the specified number exists; otherwise, <see langword="null"/>.</returns>
    Task<RefundRequestDto> GetRefundRequestByNumberAsync(string number);

    /// <summary>
    /// Gets refund request by order ID
    /// </summary>
    Task<RefundRequestDto?> GetRefundRequestByOrderDetailIdAsync(Guid orderId);

    /// <summary>
    /// Asynchronously retrieves the refund details for the specified unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the refund to retrieve.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="RefundDetailsDto"/>
    /// with the details of the refund if found.</returns>
    Task<RefundDetailsDto> FindById(Guid id);

    /// <summary>
    /// Gets all refund requests with pagination and filtering
    /// </summary>
    Task<PagedResult<RefundRequestDto>> GetRefundsPageAsync(
        RefundSearchCriteria criteria);
}