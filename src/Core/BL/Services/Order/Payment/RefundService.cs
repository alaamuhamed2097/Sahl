using BL.Contracts.IMapper;
using BL.Contracts.Service.Order.Payment;
using Common.Enumerations.Order;
using Common.Enumerations.Payment;
using DAL.Contracts.UnitOfWork;
using DAL.Models;
using Domains.Entities.Order;
using Domains.Entities.Order.Payment;
using Domains.Entities.Order.Refund;
using Domains.Entities.Order.Returns;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Shared.DTOs.Order.Payment.PaymentProcessing;
using Shared.DTOs.Order.Payment.Refund;
using System.ComponentModel.DataAnnotations;

namespace BL.Services.Order.Payment;

public class RefundService : IRefundService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPaymentService _paymentService;
    private readonly IBaseMapper _mapper;
    private readonly ILogger _logger;

    public RefundService(
        IUnitOfWork unitOfWork,
        IPaymentService paymentService,
        ILogger logger,
        IBaseMapper mapper)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _paymentService = paymentService ?? throw new ArgumentNullException(nameof(paymentService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _mapper = mapper;
    }

    public async Task<RefundRequestResult> CreateRefundRequestAsync(
        CreateRefundRequestDto requestDto,
        string customerId)
    {
        try
        {
            _logger.Information(
                "Creating refund request for order detail {OrderDetailId} by customer {CustomerId}",
                requestDto.OrderDetailId,
                customerId
            );

            await _unitOfWork.BeginTransactionAsync();

            var orderDetailsRepo = _unitOfWork.TableRepository<TbOrderDetail>();
            var orderDetails = await orderDetailsRepo.FindByIdAsync(requestDto.OrderDetailId);
            if (orderDetails == null)
                return RefundRequestResult.Fail("Order details not found");

            var orderRepo = _unitOfWork.TableRepository<TbOrder>();
            var order = await orderRepo.FindByIdAsync(orderDetails.OrderId);
            if (order == null)
                return RefundRequestResult.Fail("Order not found");

            if (order.UserId != customerId)
                return RefundRequestResult.Fail("Order does not belong to customer");

            var eligibilityResult = await ValidateRefundEligibilityAsync(
                order,
                orderDetails,
                requestDto.RequestedItemsCount
            );
            if (!eligibilityResult.IsEligible)
                return RefundRequestResult.Fail(eligibilityResult.Reason);

            var refundRepo = _unitOfWork.TableRepository<TbRefund>();
            var existingRefund = await refundRepo.GetQueryable()
                .FirstOrDefaultAsync(r => r.OrderDetailId == requestDto.OrderDetailId && !r.IsDeleted);

            if (existingRefund != null)
            {
                return RefundRequestResult.Fail(
                    $"Refund request already exists with status: {existingRefund.RefundStatus}"
                );
            }

            // FIXED: Correct refund amount calculation
            var unitPriceWithTax = orderDetails.UnitPrice + (orderDetails.TaxAmount / orderDetails.Quantity);
            var refundAmount = unitPriceWithTax * requestDto.RequestedItemsCount;

            var refundRequest = new TbRefund
            {
                Number = $"REF-{DateTime.UtcNow:yyyyMMddHHmmss}-{new Random().Next(1000, 9999)}",
                OrderDetailId = requestDto.OrderDetailId,
                CustomerId = Guid.Parse(customerId),
                VendorId = orderDetails.VendorId,
                RefundReason = requestDto.Reason,
                RefundReasonDetails = requestDto.ReasonDetails,
                RefundAmount = refundAmount,
                RequestedItemsCount = requestDto.RequestedItemsCount,
                DeliveryAddressId = order.DeliveryAddressId,
                RequestDateUTC = DateTime.UtcNow,
                RefundStatus = RefundStatus.Open
            };

            var refundSaved = await refundRepo.CreateAsync(refundRequest, Guid.Parse(customerId));

            order.OrderStatus = OrderProgressStatus.RefundRequested;
            order.UpdatedDateUtc = DateTime.UtcNow;
            await orderRepo.UpdateAsync(order, Guid.Parse(customerId));

            await _unitOfWork.CommitAsync();

            _logger.Information(
                "Refund request {RefundId} created for order detail {OrderDetailId}",
                refundSaved.Id,
                requestDto.OrderDetailId
            );

            return RefundRequestResult.Success(refundRequest.Id);
        }
        catch (Exception ex)
        {
            _logger.Error(
                ex,
                "Error creating refund request for order detail {OrderDetailId}",
                requestDto.OrderDetailId
            );
            await _unitOfWork.RollbackAsync();
            return RefundRequestResult.Fail(ex.Message);
        }
    }

    public async Task<RefundStatusUpdateResult> UpdateRefundStatusAsync(
        Guid refundId,
        UpdateRefundStatusDto statusDto,
        string userId)
    {
        try
        {
            _logger.Information(
                "Updating refund request {RefundId} status to {NewStatus} by user {UserId}",
                refundId,
                statusDto.NewStatus,
                userId
            );

            await _unitOfWork.BeginTransactionAsync();

            var refundRepo = _unitOfWork.TableRepository<TbRefund>();
            var refundRequest = await refundRepo.FindByIdAsync(refundId);

            if (refundRequest == null)
                return RefundStatusUpdateResult.Fail("Refund request not found");

            var previousStatus = refundRequest.RefundStatus;

            if (previousStatus == RefundStatus.Closed)
                return RefundStatusUpdateResult.Fail("Cannot update a closed refund request");

            if (!IsValidStatusTransition(previousStatus, statusDto.NewStatus))
            {
                return RefundStatusUpdateResult.Fail(
                    $"Invalid status transition from {previousStatus} to {statusDto.NewStatus}"
                );
            }

            var orderDetailsRepo = _unitOfWork.TableRepository<TbOrderDetail>();
            var orderDetails = await orderDetailsRepo.FindByIdAsync(refundRequest.OrderDetailId);
            if (orderDetails == null)
                return RefundStatusUpdateResult.Fail("Order details not found");

            var orderRepo = _unitOfWork.TableRepository<TbOrder>();
            var order = await orderRepo.FindByIdAsync(orderDetails.OrderId);
            if (order == null)
                return RefundStatusUpdateResult.Fail("Order not found");

            refundRequest.RefundStatus = statusDto.NewStatus;
            refundRequest.AdminUserId = userId;

            if (!string.IsNullOrEmpty(statusDto.Notes))
                refundRequest.AdminNotes = statusDto.Notes;

            var statusUpdateResult = await HandleStatusSpecificUpdates(
                refundRequest,
                order,
                statusDto,
                userId
            );

            if (!statusUpdateResult.IsSuccess)
            {
                await _unitOfWork.RollbackAsync();
                return RefundStatusUpdateResult.Fail(statusUpdateResult.ErrorMessage);
            }

            order.UpdatedDateUtc = DateTime.UtcNow;
            refundRequest.UpdatedDateUtc = DateTime.UtcNow;

            await refundRepo.UpdateAsync(refundRequest, Guid.Parse(userId));
            await orderRepo.UpdateAsync(order, Guid.Parse(userId));

            await CreateStatusHistoryAsync(
                refundRequest.Id,
                previousStatus,
                statusDto.NewStatus,
                statusDto.Notes,
                userId
            );

            await _unitOfWork.CommitAsync();

            _logger.Information(
                "Refund request {RefundId} status updated from {OldStatus} to {NewStatus}",
                refundId,
                previousStatus,
                statusDto.NewStatus
            );

            return RefundStatusUpdateResult.Success(
                refundRequest.Id,
                previousStatus,
                statusDto.NewStatus,
                refundRequest.RefundTransactionId
            );
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error updating refund request {RefundId} status", refundId);
            await _unitOfWork.RollbackAsync();
            return RefundStatusUpdateResult.Fail(ex.Message);
        }
    }

    public async Task<RefundRequestDto?> GetRefundRequestByIdAsync(Guid refundRequestId)
    {
        try
        {
            var refundRepo = _unitOfWork.TableRepository<TbRefund>();
            var refundRequest = await refundRepo.FindByIdAsync(refundRequestId);
            return refundRequest == null ? null : _mapper.MapModel<TbRefund, RefundRequestDto>(refundRequest);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error getting refund request {RefundId}", refundRequestId);
            return null;
        }
    }

    public async Task<RefundRequestDto?> GetRefundRequestByOrderDetailIdAsync(Guid orderDetailId)
    {
        try
        {
            var refundRepo = _unitOfWork.TableRepository<TbRefund>();
            var refundRequest = await refundRepo.GetQueryable()
                .FirstOrDefaultAsync(r => r.OrderDetailId == orderDetailId && !r.IsDeleted);
            return refundRequest == null ? null : _mapper.MapModel<TbRefund, RefundRequestDto>(refundRequest);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error getting refund request for order {OrderDetailId}", orderDetailId);
            return null;
        }
    }

    public async Task<PagedResult<RefundRequestDto>> GetRefundRequestsAsync(RefundSearchCriteria criteria)
    {
        try
        {
            var refundRepo = _unitOfWork.TableRepository<TbRefund>();
            var query = refundRepo.GetQueryable().Where(r => !r.IsDeleted);

            if (criteria.Status.HasValue)
                query = query.Where(r => r.RefundStatus == criteria.Status.Value);

            if (!string.IsNullOrEmpty(criteria.CustomerId))
                query = query.Where(r => r.CustomerId == Guid.Parse(criteria.CustomerId));

            if (criteria.FromDate.HasValue)
                query = query.Where(r => r.CreatedDateUtc >= criteria.FromDate.Value);

            if (criteria.ToDate.HasValue)
                query = query.Where(r => r.CreatedDateUtc <= criteria.ToDate.Value);

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderByDescending(r => r.CreatedDateUtc)
                .Skip((criteria.PageNumber - 1) * criteria.PageSize)
                .Take(criteria.PageSize)
                .ToListAsync();

            var dtos = _mapper.MapList<TbRefund, RefundRequestDto>(items);
            return new PagedResult<RefundRequestDto>(dtos.ToList(), totalCount);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error getting refund requests");
            return new PagedResult<RefundRequestDto>(new List<RefundRequestDto>(), 0);
        }
    }

    // Helper methods
    private async Task<StatusUpdateOperationResult> HandleStatusSpecificUpdates(
        TbRefund refundRequest,
        TbOrder order,
        UpdateRefundStatusDto statusDto,
        string userId)
    {
        switch (statusDto.NewStatus)
        {
            case RefundStatus.UnderReview:
                break;

            case RefundStatus.NeedMoreInfo:
                if (string.IsNullOrEmpty(statusDto.Notes))
                    return StatusUpdateOperationResult.Fail("Notes are required when requesting more information");
                break;

            case RefundStatus.InfoApproved:
                break;

            case RefundStatus.ItemShippedBack:
                if (!string.IsNullOrEmpty(statusDto.TrackingNumber))
                    refundRequest.ReturnTrackingNumber = statusDto.TrackingNumber;
                refundRequest.ReturnedDateUTC = DateTime.UtcNow;
                break;

            case RefundStatus.ItemReceived:
                if (!refundRequest.ReturnedDateUTC.HasValue)
                    refundRequest.ReturnedDateUTC = DateTime.UtcNow;
                break;

            case RefundStatus.Inspecting:
                break;

            case RefundStatus.Approved:
                refundRequest.ApprovedDateUTC = DateTime.UtcNow;
                refundRequest.ApprovedItemsCount = statusDto.ApprovedItemsCount ?? refundRequest.RequestedItemsCount;

                var refundAmount = statusDto.RefundAmount ?? refundRequest.RefundAmount;
                refundRequest.RefundAmount = refundAmount;

                _logger.Information(
                    "Executing refund payment for request {RefundId}, amount {Amount}",
                    refundRequest.Id,
                    refundAmount
                );

                var refundResult = await ExecuteRefundAsync(order, refundAmount);

                if (refundResult.IsSuccess)
                {
                    refundRequest.RefundStatus = RefundStatus.Refunded;
                    refundRequest.RefundedDateUTC = DateTime.UtcNow;
                    refundRequest.RefundTransactionId = refundResult.RefundTransactionId;
                    order.OrderStatus = OrderProgressStatus.Refunded;
                    order.PaymentStatus = PaymentStatus.Refunded;

                    _logger.Information(
                        "Refund executed successfully for request {RefundId}, transaction {TransactionId}",
                        refundRequest.Id,
                        refundResult.RefundTransactionId
                    );
                }
                else
                {
                    _logger.Error(
                        "Refund execution failed for request {RefundId}: {Error}",
                        refundRequest.Id,
                        refundResult.ErrorMessage
                    );
                    return StatusUpdateOperationResult.Fail($"Refund payment failed: {refundResult.ErrorMessage}");
                }
                break;

            case RefundStatus.Rejected:
                if (string.IsNullOrEmpty(statusDto.RejectionReason))
                    return StatusUpdateOperationResult.Fail("Rejection reason is required when rejecting a refund");

                refundRequest.RejectionReason = statusDto.RejectionReason;
                refundRequest.ApprovedItemsCount = 0;
                order.OrderStatus = OrderProgressStatus.Completed;

                _logger.Information("Refund request {RefundId} rejected: {Reason}", refundRequest.Id, statusDto.RejectionReason);
                break;

            case RefundStatus.Refunded:
                if (string.IsNullOrEmpty(refundRequest.RefundTransactionId))
                    return StatusUpdateOperationResult.Fail("Cannot manually mark as Refunded without a valid refund transaction ID");

                refundRequest.RefundedDateUTC = DateTime.UtcNow;
                order.OrderStatus = OrderProgressStatus.Refunded;
                order.PaymentStatus = PaymentStatus.Refunded;
                break;

            case RefundStatus.Closed:
                if (refundRequest.RefundStatus != RefundStatus.Refunded && refundRequest.RefundStatus != RefundStatus.Rejected)
                    return StatusUpdateOperationResult.Fail("Can only close refund requests that are Refunded or Rejected");
                break;

            default:
                return StatusUpdateOperationResult.Fail($"Unsupported status: {statusDto.NewStatus}");
        }

        return StatusUpdateOperationResult.Success();
    }

    private async Task CreateStatusHistoryAsync(
        Guid refundId,
        RefundStatus oldStatus,
        RefundStatus newStatus,
        string? notes,
        string userId)
    {
        try
        {
            var historyRepo = _unitOfWork.TableRepository<TbRefundStatusHistory>();
            var historyRecord = new TbRefundStatusHistory
            {
                RefundId = refundId,
                OldStatus = oldStatus,
                NewStatus = newStatus,
                UpdatedBy = Guid.Parse(userId),
                UpdatedDateUtc = DateTime.UtcNow,
                Notes = notes
            };

            await historyRepo.CreateAsync(historyRecord, Guid.Parse(userId));
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error creating status history for refund {RefundId}", refundId);
        }
    }

    private bool IsValidStatusTransition(RefundStatus currentStatus, RefundStatus newStatus)
    {
        return currentStatus switch
        {
            RefundStatus.Open => newStatus is RefundStatus.UnderReview or RefundStatus.Rejected,
            RefundStatus.UnderReview => newStatus is RefundStatus.NeedMoreInfo or RefundStatus.Approved
                or RefundStatus.ItemShippedBack or RefundStatus.Rejected,
            RefundStatus.NeedMoreInfo => newStatus is RefundStatus.InfoApproved or RefundStatus.Rejected,
            RefundStatus.InfoApproved => newStatus is RefundStatus.ItemShippedBack or RefundStatus.Approved
                or RefundStatus.Rejected,
            RefundStatus.ItemShippedBack => newStatus is RefundStatus.ItemReceived or RefundStatus.Rejected,
            RefundStatus.ItemReceived => newStatus is RefundStatus.Inspecting or RefundStatus.Approved
                or RefundStatus.Rejected,
            RefundStatus.Inspecting => newStatus is RefundStatus.Approved or RefundStatus.Rejected,
            RefundStatus.Approved => newStatus is RefundStatus.Refunded,
            RefundStatus.Rejected => newStatus is RefundStatus.Closed,
            RefundStatus.Refunded => newStatus is RefundStatus.Closed,
            RefundStatus.Closed => false,
            _ => false
        };
    }

    private async Task<RefundEligibilityResult> ValidateRefundEligibilityAsync(
        TbOrder order,
        TbOrderDetail orderDetails,
        int requestedQuantity)
    {
        // FIXED: Added quantity validation
        if (requestedQuantity <= 0)
            return RefundEligibilityResult.NotEligible("Requested quantity must be greater than zero");

        if (requestedQuantity > orderDetails.Quantity)
            return RefundEligibilityResult.NotEligible(
                $"Requested quantity ({requestedQuantity}) exceeds ordered quantity ({orderDetails.Quantity})"
            );

        if (order.OrderStatus != OrderProgressStatus.Completed)
            return RefundEligibilityResult.NotEligible("Only completed orders can be refunded");

        if (order.PaymentStatus != PaymentStatus.Paid)
            return RefundEligibilityResult.NotEligible("Order has not been paid");

        if (!order.OrderDeliveryDate.HasValue)
            return RefundEligibilityResult.NotEligible("Order delivery date not found");

        var daysSinceDelivery = (DateTime.UtcNow - order.OrderDeliveryDate.Value).Days;
        const int refundWindowDays = 15;

        if (daysSinceDelivery > refundWindowDays)
            return RefundEligibilityResult.NotEligible(
                $"Refund window expired (allowed within {refundWindowDays} days of delivery)"
            );

        if (requestedQuantity < orderDetails.Quantity && orderDetails.DiscountAmount > 0)
            return RefundEligibilityResult.NotEligible("Partial refunds are not allowed on discounted items");

        return RefundEligibilityResult.Eligible();
    }

    private async Task<RefundExecutionResult> ExecuteRefundAsync(TbOrder order, decimal refundAmount)
    {
        try
        {
            _logger.Information("Executing refund for order {OrderId}, amount {Amount}", order.Id, refundAmount);

            var paymentRepo = _unitOfWork.TableRepository<TbOrderPayment>();
            var payment = await paymentRepo.GetQueryable()
                .FirstOrDefaultAsync(p => p.OrderId == order.Id && p.PaymentStatus == PaymentStatus.Paid);

            if (payment == null)
                return RefundExecutionResult.Fail("Payment record not found");

            var refundRequest = new RefundProcessRequest
            {
                OrderId = order.Id,
                PaymentId = payment.Id,
                RefundAmount = refundAmount,
                Reason = "Customer requested refund"
            };

            var refundResult = await _paymentService.ProcessRefundAsync(refundRequest);

            if (!refundResult.Success)
                return RefundExecutionResult.Fail(refundResult.Message);

            return RefundExecutionResult.Success(refundResult.RefundTransactionId ?? "REF-" + Guid.NewGuid().ToString("N"));
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error executing refund for order {OrderId}", order.Id);
            return RefundExecutionResult.Fail(ex.Message);
        }
    }

    private class RefundEligibilityResult
    {
        public bool IsEligible { get; set; }
        public string? Reason { get; set; }

        public static RefundEligibilityResult Eligible() =>
            new RefundEligibilityResult { IsEligible = true };

        public static RefundEligibilityResult NotEligible(string reason) =>
            new RefundEligibilityResult { IsEligible = false, Reason = reason };
    }

    private class RefundExecutionResult
    {
        public bool IsSuccess { get; set; }
        public string? RefundTransactionId { get; set; }
        public string? ErrorMessage { get; set; }

        public static RefundExecutionResult Success(string refundTransactionId) =>
            new RefundExecutionResult { IsSuccess = true, RefundTransactionId = refundTransactionId };

        public static RefundExecutionResult Fail(string errorMessage) =>
            new RefundExecutionResult { IsSuccess = false, ErrorMessage = errorMessage };
    }

    private class StatusUpdateOperationResult
    {
        public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; }

        public static StatusUpdateOperationResult Success() =>
            new StatusUpdateOperationResult { IsSuccess = true };

        public static StatusUpdateOperationResult Fail(string errorMessage) =>
            new StatusUpdateOperationResult { IsSuccess = false, ErrorMessage = errorMessage };
    }
}

// Public result classes
public class RefundRequestResult
{
    public bool IsSuccess { get; set; }
    public Guid? RefundRequestId { get; set; }
    public string? ErrorMessage { get; set; }

    public static RefundRequestResult Success(Guid refundRequestId) =>
        new RefundRequestResult { IsSuccess = true, RefundRequestId = refundRequestId };

    public static RefundRequestResult Fail(string errorMessage) =>
        new RefundRequestResult { IsSuccess = false, ErrorMessage = errorMessage };
}

public class RefundStatusUpdateResult
{
    public bool IsSuccess { get; set; }
    public Guid? RefundId { get; set; }
    public RefundStatus? OldStatus { get; set; }
    public RefundStatus? NewStatus { get; set; }
    public string? RefundTransactionId { get; set; }
    public string? ErrorMessage { get; set; }

    public static RefundStatusUpdateResult Success(
        Guid refundId,
        RefundStatus oldStatus,
        RefundStatus newStatus,
        string? transactionId = null) =>
        new RefundStatusUpdateResult
        {
            IsSuccess = true,
            RefundId = refundId,
            OldStatus = oldStatus,
            NewStatus = newStatus,
            RefundTransactionId = transactionId
        };

    public static RefundStatusUpdateResult Fail(string errorMessage) =>
        new RefundStatusUpdateResult { IsSuccess = false, ErrorMessage = errorMessage };
}

public class UpdateRefundStatusDto
{
    [Required]
    public RefundStatus NewStatus { get; set; }
    public string? Notes { get; set; }
    public string? RejectionReason { get; set; }
    public decimal? RefundAmount { get; set; }
    public int? ApprovedItemsCount { get; set; }
    public string? TrackingNumber { get; set; }
}