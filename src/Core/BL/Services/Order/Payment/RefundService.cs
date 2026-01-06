using AutoMapper;
using BL.Contracts.Service.Order.Payment;
using Common.Enumerations.Order;
using Common.Enumerations.Payment;
using DAL.Contracts.UnitOfWork;
using DAL.Models;
using Domains.Entities.Order;
using Domains.Entities.Order.Payment;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Shared.DTOs.Order.Payment.PaymentProcessing;
using Shared.DTOs.Order.Payment.Refund;

namespace BL.Services.Order.Payment;

/// <summary>
/// FIXED RefundService - Uses correct DTO property names
/// </summary>
public class RefundService : IRefundService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPaymentService _paymentService;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public RefundService(
        IUnitOfWork unitOfWork,
        IPaymentService paymentService,
        IMapper mapper,
        ILogger logger)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _paymentService = paymentService ?? throw new ArgumentNullException(nameof(paymentService));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<RefundRequestResult> CreateRefundRequestAsync(
        CreateRefundRequestDto requestDto,
        string customerId)
    {
        try
        {
            _logger.Information(
                "Creating refund request for order {OrderId} by customer {CustomerId}",
                requestDto.OrderId,
                customerId
            );

            await _unitOfWork.BeginTransactionAsync();

            var orderRepo = _unitOfWork.TableRepository<TbOrder>();
            var order = await orderRepo.FindByIdAsync(requestDto.OrderId);

            if (order == null)
            {
                return RefundRequestResult.Fail("Order not found");
            }

            if (order.UserId != customerId)
            {
                return RefundRequestResult.Fail("Order does not belong to customer");
            }

            var eligibilityResult = await ValidateRefundEligibilityAsync(order);
            if (!eligibilityResult.IsEligible)
            {
                return RefundRequestResult.Fail(eligibilityResult.Reason);
            }

            var refundRepo = _unitOfWork.TableRepository<TbRefundRequest>();
            var existingRefund = await refundRepo.GetQueryable()
                .FirstOrDefaultAsync(r => r.OrderId == requestDto.OrderId && !r.IsDeleted);

            if (existingRefund != null)
            {
                return RefundRequestResult.Fail(
                    $"Refund request already exists with status: {existingRefund.RefundStatus}"
                );
            }

            var refundRequest = new TbRefundRequest
            {
                Id = Guid.NewGuid(),
                OrderId = requestDto.OrderId,
                UserId = customerId,
                RefundReason = requestDto.Reason,
                RefundReasonDetails = requestDto.ReasonDetails,
                RequestedAmount = order.Price,
                RefundStatus = RefundStatus.Pending,
                CustomerNotes = requestDto.CustomerNotes,
                CreatedDateUtc = DateTime.UtcNow,
                CreatedBy = Guid.Empty
            };

            await refundRepo.CreateAsync(refundRequest, Guid.Empty);

            order.OrderStatus = OrderProgressStatus.RefundRequested;
            order.UpdatedDateUtc = DateTime.UtcNow;
            await orderRepo.UpdateAsync(order, Guid.Empty);

            await _unitOfWork.CommitAsync();

            _logger.Information(
                "Refund request {RefundId} created for order {OrderId}",
                refundRequest.Id,
                requestDto.OrderId
            );

            return RefundRequestResult.Success(refundRequest.Id);
        }
        catch (Exception ex)
        {
            _logger.Error(
                ex,
                "Error creating refund request for order {OrderId}",
                requestDto.OrderId
            );
            await _unitOfWork.RollbackAsync();
            return RefundRequestResult.Fail(ex.Message);
        }
    }

    public async Task<RefundProcessResult> ProcessRefundAsync(
        Guid refundRequestId,
        ProcessRefundDto processDto,
        string adminUserId)
    {
        try
        {
            _logger.Information(
                "Processing refund request {RefundId} by admin {AdminId}",
                refundRequestId,
                adminUserId
            );

            await _unitOfWork.BeginTransactionAsync();

            var refundRepo = _unitOfWork.TableRepository<TbRefundRequest>();
            var refundRequest = await refundRepo.FindByIdAsync(refundRequestId);

            if (refundRequest == null)
            {
                return RefundProcessResult.Fail("Refund request not found");
            }

            if (refundRequest.RefundStatus != RefundStatus.Pending)
            {
                return RefundProcessResult.Fail(
                    $"Refund request already processed with status: {refundRequest.RefundStatus}"
                );
            }

            var orderRepo = _unitOfWork.TableRepository<TbOrder>();
            var order = await orderRepo.FindByIdAsync(refundRequest.OrderId);

            if (order == null)
            {
                return RefundProcessResult.Fail("Order not found");
            }

            refundRequest.RefundStatus = processDto.IsApproved
                ? RefundStatus.Approved
                : RefundStatus.Rejected;
            refundRequest.RefundAmount = processDto.RefundAmount;
            refundRequest.AdminUserId = adminUserId;
            refundRequest.AdminNotes = processDto.AdminNotes;
            refundRequest.ProcessedDate = DateTime.UtcNow;
            refundRequest.UpdatedDateUtc = DateTime.UtcNow;

            await refundRepo.UpdateAsync(refundRequest, Guid.Parse(adminUserId));

            if (!processDto.IsApproved)
            {
                order.OrderStatus = OrderProgressStatus.Completed;
                order.UpdatedDateUtc = DateTime.UtcNow;
                await orderRepo.UpdateAsync(order, Guid.Parse(adminUserId));

                await _unitOfWork.CommitAsync();

                _logger.Information(
                    "Refund request {RefundId} rejected",
                    refundRequestId
                );

                return RefundProcessResult.Success(false, "Refund rejected");
            }

            // FIXED: Use correct property name
            var refundResult = await ExecuteRefundAsync(order, processDto.RefundAmount);

            if (!refundResult.IsSuccess)
            {
                refundRequest.RefundStatus = RefundStatus.Failed;
                refundRequest.RefundFailureReason = refundResult.ErrorMessage;
                await refundRepo.UpdateAsync(refundRequest, Guid.Parse(adminUserId));
                await _unitOfWork.CommitAsync();

                return RefundProcessResult.Fail($"Refund execution failed: {refundResult.ErrorMessage}");
            }

            refundRequest.RefundStatus = RefundStatus.Completed;
            refundRequest.RefundCompletedDate = DateTime.UtcNow;
            refundRequest.RefundTransactionId = refundResult.RefundTransactionId; // FIXED
            await refundRepo.UpdateAsync(refundRequest, Guid.Parse(adminUserId));

            order.OrderStatus = OrderProgressStatus.Refunded;
            order.PaymentStatus = PaymentStatus.Refunded;
            order.UpdatedDateUtc = DateTime.UtcNow;
            await orderRepo.UpdateAsync(order, Guid.Parse(adminUserId));

            await _unitOfWork.CommitAsync();

            _logger.Information(
                "Refund request {RefundId} completed with amount {Amount}",
                refundRequestId,
                processDto.RefundAmount
            );

            return RefundProcessResult.Success(true, "Refund processed successfully");
        }
        catch (Exception ex)
        {
            _logger.Error(
                ex,
                "Error processing refund request {RefundId}",
                refundRequestId
            );
            await _unitOfWork.RollbackAsync();
            return RefundProcessResult.Fail(ex.Message);
        }
    }

    public async Task<RefundRequestDto?> GetRefundRequestByIdAsync(Guid refundRequestId)
    {
        try
        {
            var refundRepo = _unitOfWork.TableRepository<TbRefundRequest>();
            var refundRequest = await refundRepo.FindByIdAsync(refundRequestId);

            if (refundRequest == null)
            {
                return null;
            }

            return _mapper.Map<RefundRequestDto>(refundRequest);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error getting refund request {RefundId}", refundRequestId);
            return null;
        }
    }

    public async Task<RefundRequestDto?> GetRefundRequestByOrderIdAsync(Guid orderId)
    {
        try
        {
            var refundRepo = _unitOfWork.TableRepository<TbRefundRequest>();
            var refundRequest = await refundRepo.GetQueryable()
                .FirstOrDefaultAsync(r => r.OrderId == orderId && !r.IsDeleted);

            if (refundRequest == null)
            {
                return null;
            }

            return _mapper.Map<RefundRequestDto>(refundRequest);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error getting refund request for order {OrderId}", orderId);
            return null;
        }
    }

    public async Task<PagedResult<RefundRequestDto>> GetRefundRequestsAsync(
        RefundSearchCriteria criteria)
    {
        try
        {
            var refundRepo = _unitOfWork.TableRepository<TbRefundRequest>();

            var query = refundRepo.GetQueryable().Where(r => !r.IsDeleted);

            if (criteria.Status.HasValue)
            {
                query = query.Where(r => r.RefundStatus == criteria.Status.Value);
            }

            if (!string.IsNullOrEmpty(criteria.CustomerId))
            {
                query = query.Where(r => r.UserId == criteria.CustomerId);
            }

            if (criteria.FromDate.HasValue)
            {
                query = query.Where(r => r.CreatedDateUtc >= criteria.FromDate.Value);
            }

            if (criteria.ToDate.HasValue)
            {
                query = query.Where(r => r.CreatedDateUtc <= criteria.ToDate.Value);
            }

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderByDescending(r => r.CreatedDateUtc)
                .Skip((criteria.PageNumber - 1) * criteria.PageSize)
                .Take(criteria.PageSize)
                .ToListAsync();

            var dtos = _mapper.Map<List<RefundRequestDto>>(items);

            return new PagedResult<RefundRequestDto>(dtos, totalCount);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error getting refund requests");
            return new PagedResult<RefundRequestDto>(new List<RefundRequestDto>(), 0);
        }
    }

    private async Task<RefundEligibilityResult> ValidateRefundEligibilityAsync(TbOrder order)
    {
        if (order.OrderStatus != OrderProgressStatus.Completed)
        {
            return RefundEligibilityResult.NotEligible(
                "Only completed orders can be refunded"
            );
        }

        if (order.PaymentStatus != PaymentStatus.Paid)
        {
            return RefundEligibilityResult.NotEligible(
                "Order has not been paid"
            );
        }

        if (!order.OrderDeliveryDate.HasValue)
        {
            return RefundEligibilityResult.NotEligible(
                "Order delivery date not found"
            );
        }

        var daysSinceDelivery = (DateTime.UtcNow - order.OrderDeliveryDate.Value).Days;
        const int refundWindowDays = 15;

        if (daysSinceDelivery > refundWindowDays)
        {
            return RefundEligibilityResult.NotEligible(
                $"Refund window expired (allowed within {refundWindowDays} days of delivery)"
            );
        }

        return RefundEligibilityResult.Eligible();
    }

    private async Task<RefundExecutionResult> ExecuteRefundAsync(
        TbOrder order,
        decimal refundAmount)
    {
        try
        {
            _logger.Information(
                "Executing refund for order {OrderId}, amount {Amount}",
                order.Id,
                refundAmount
            );

            var paymentRepo = _unitOfWork.TableRepository<TbOrderPayment>();
            var payment = await paymentRepo.GetQueryable()
                .FirstOrDefaultAsync(p =>
                    p.OrderId == order.Id &&
                    p.PaymentStatus == PaymentStatus.Paid
                );

            if (payment == null)
            {
                return RefundExecutionResult.Fail("Payment record not found");
            }

            // FIXED: Use correct RefundResultDto properties
            var refundRequest = new RefundProcessRequest
            {
                OrderId = order.Id,
                PaymentId = payment.Id,
                RefundAmount = refundAmount,
                Reason = "Customer requested refund"
            };

            var refundResult = await _paymentService.ProcessRefundAsync(refundRequest);

            if (!refundResult.Success)
            {
                return RefundExecutionResult.Fail(refundResult.Message);
            }

            // FIXED: Use RefundTransactionId instead of TransactionId
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
        public string? RefundTransactionId { get; set; } // FIXED property name
        public string? ErrorMessage { get; set; }

        public static RefundExecutionResult Success(string refundTransactionId) =>
            new RefundExecutionResult { IsSuccess = true, RefundTransactionId = refundTransactionId };

        public static RefundExecutionResult Fail(string errorMessage) =>
            new RefundExecutionResult { IsSuccess = false, ErrorMessage = errorMessage };
    }
}

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

public class RefundProcessResult
{
    public bool IsSuccess { get; set; }
    public bool IsApproved { get; set; }
    public string? Message { get; set; }
    public string? ErrorMessage { get; set; }

    public static RefundProcessResult Success(bool isApproved, string message) =>
        new RefundProcessResult { IsSuccess = true, IsApproved = isApproved, Message = message };

    public static RefundProcessResult Fail(string errorMessage) =>
        new RefundProcessResult { IsSuccess = false, ErrorMessage = errorMessage };
}