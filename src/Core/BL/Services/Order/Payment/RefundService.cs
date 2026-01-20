using BL.Contracts.IMapper;
using BL.Contracts.Service.Customer;
using BL.Contracts.Service.Order.Payment;
using Common.Enumerations.Order;
using Common.Enumerations.Payment;
using Common.Filters;
using DAL.Contracts.Repositories.Order;
using DAL.Contracts.UnitOfWork;
using DAL.Exceptions;
using DAL.Models;
using DAL.Repositories.Order.Refund;
using Domains.Entities.Order;
using Domains.Entities.Order.Payment;
using Domains.Entities.Order.Refund;
using Domains.Entities.Order.Returns;
using Domains.Views.Order.Refund;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Shared.DTOs.Order.Payment.PaymentProcessing;
using Shared.DTOs.Order.Payment.Refund;
using Shared.ResultModels.Refund;
using System.ComponentModel.DataAnnotations;

namespace BL.Services.Order.Payment;

public class RefundService : IRefundService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRefundRepository _refundRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IPaymentService _paymentService;
    private readonly ICustomerService _customerService;
    private readonly IBaseMapper _mapper;
    private readonly ILogger _logger;

    public RefundService(
        IUnitOfWork unitOfWork,
        IPaymentService paymentService,
        ILogger logger,
        IBaseMapper mapper,
        ICustomerService customerService,
        IRefundRepository refundRepository,
        IOrderRepository orderRepository)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _paymentService = paymentService ?? throw new ArgumentNullException(nameof(paymentService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _mapper = mapper;
        _customerService = customerService;
        _refundRepository = refundRepository;
        _orderRepository = orderRepository;
    }

    public async Task<RefundRequestResult> CreateRefundRequestAsync(
        CreateRefundRequestDto requestDto,
        string userId)
    {
        // Validate userId
        if(string.IsNullOrEmpty(userId))
            throw new ArgumentNullException(nameof(userId));
        // Get customer by userId
        var customer = await _customerService.GetByUserIdAsync(userId);
        // Validate customer
        if (customer == null)
            return RefundRequestResult.Fail("Customer not found");
        try
        {
            _logger.Information(
                "Creating refund request for order detail {OrderDetailId} by customer {userId}",
                requestDto.OrderDetailId,
                userId
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

            if (order.UserId != userId)
                return RefundRequestResult.Fail("Order does not belong to customer");

            var eligibilityResult = await ValidateRefundEligibilityAsync(
                order,
                orderDetails,
                requestDto.RequestedItemsCount
            );
            if (!eligibilityResult.IsEligible)
                return RefundRequestResult.Fail(eligibilityResult.Reason ?? "Something went wrong!!");

            // Validate delivery address if provided
            if (requestDto.DeliveryAddressId != null)
            {
                var addressRepo = _unitOfWork.TableRepository<TbCustomerAddress>();
                var address = await addressRepo.FindByIdAsync(requestDto.DeliveryAddressId.Value);
                if(address == null || address.UserId != userId)
                    return RefundRequestResult.Fail("Invalid delivery address");
            }

            // Check if refund already exists using repository
            var existingRefund = await _refundRepository.GetByOrderDetailIdAsync(requestDto.OrderDetailId);
            if (existingRefund != null)
            {
                return RefundRequestResult.Fail(
                    $"Refund request already exists with status: {existingRefund.RefundStatus}"
                );
            }

            // Refund amount calculation
            var unitPriceWithTax = orderDetails.UnitPrice + (orderDetails.TaxAmount / orderDetails.Quantity);
            var refundAmount = unitPriceWithTax * requestDto.RequestedItemsCount;

            var refundRequest = new TbRefund
            {
                Number = $"REF-{DateTime.UtcNow:yyyyMMddHHmmss}-{new Random().Next(1000, 9999)}",
                OrderDetailId = requestDto.OrderDetailId,
                CustomerId = customer.Id,
                VendorId = orderDetails.VendorId,
                RefundReason = requestDto.Reason,
                RefundReasonDetails = requestDto.ReasonDetails,
                RefundAmount = refundAmount,
                RequestedItemsCount = requestDto.RequestedItemsCount,
                DeliveryAddressId = requestDto.DeliveryAddressId ?? order.DeliveryAddressId,
                RequestDateUTC = DateTime.UtcNow,
                RefundStatus = RefundStatus.Open
            };
            // Use repository to create refund
            var refundSaved = await _refundRepository.CreateAsync(refundRequest, Guid.Parse(userId));

            order.OrderStatus = OrderProgressStatus.RefundRequested;
            order.UpdatedDateUtc = DateTime.UtcNow;
            await orderRepo.UpdateAsync(order, Guid.Parse(userId));

            await _unitOfWork.CommitAsync();

            _logger.Information(
                "Refund request {RefundId} created for order detail {OrderDetailId}",
                refundSaved.Id,
                requestDto.OrderDetailId
            );

            return RefundRequestResult.Success(refundRequest.Number);
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

    //public async Task<RefundStatusUpdateResult> UpdateRefundStatusAsync(
    //    Guid refundId,
    //    UpdateRefundStatusDto statusDto,
    //    string userId)
    //{
    //    try
    //    {
    //        _logger.Information(
    //            "Updating refund request {RefundId} status to {NewStatus} by user {UserId}",
    //            refundId,
    //            statusDto.NewStatus,
    //            userId
    //        );

    //        await _unitOfWork.BeginTransactionAsync();

    //        var refundRepo = _unitOfWork.TableRepository<TbRefund>();
    //        var refundRequest = await refundRepo.FindByIdAsync(refundId);

    //        if (refundRequest == null)
    //            return RefundStatusUpdateResult.Fail("Refund request not found");

    //        var previousStatus = refundRequest.RefundStatus;

    //        if (previousStatus == RefundStatus.Closed)
    //            return RefundStatusUpdateResult.Fail("Cannot update a closed refund request");

    //        if (!IsValidStatusTransition(previousStatus, statusDto.NewStatus))
    //        {
    //            return RefundStatusUpdateResult.Fail(
    //                $"Invalid status transition from {previousStatus} to {statusDto.NewStatus}"
    //            );
    //        }

    //        var orderDetailsRepo = _unitOfWork.TableRepository<TbOrderDetail>();
    //        var orderDetails = await orderDetailsRepo.FindByIdAsync(refundRequest.OrderDetailId);
    //        if (orderDetails == null)
    //            return RefundStatusUpdateResult.Fail("Order details not found");

    //        var orderRepo = _unitOfWork.TableRepository<TbOrder>();
    //        var order = await orderRepo.FindByIdAsync(orderDetails.OrderId);
    //        if (order == null)
    //            return RefundStatusUpdateResult.Fail("Order not found");

    //        refundRequest.RefundStatus = statusDto.NewStatus;
    //        refundRequest.AdminUserId = userId;

    //        if (!string.IsNullOrEmpty(statusDto.Notes))
    //            refundRequest.AdminNotes = statusDto.Notes;

    //        var statusUpdateResult = await HandleStatusSpecificUpdates(
    //            refundRequest,
    //            order,
    //            statusDto,
    //            userId
    //        );

    //        if (!statusUpdateResult.IsSuccess)
    //        {
    //            await _unitOfWork.RollbackAsync();
    //            return RefundStatusUpdateResult.Fail(statusUpdateResult.ErrorMessage ?? "Failed to update refund status");
    //        }

    //        order.UpdatedDateUtc = DateTime.UtcNow;
    //        refundRequest.UpdatedDateUtc = DateTime.UtcNow;

    //        await refundRepo.UpdateAsync(refundRequest, Guid.Parse(userId));
    //        await orderRepo.UpdateAsync(order, Guid.Parse(userId));

    //        await CreateStatusHistoryAsync(
    //            refundRequest.Id,
    //            previousStatus,
    //            statusDto.NewStatus,
    //            statusDto.Notes,
    //            userId
    //        );

    //        await _unitOfWork.CommitAsync();

    //        _logger.Information(
    //            "Refund request {RefundId} status updated from {OldStatus} to {NewStatus}",
    //            refundId,
    //            previousStatus,
    //            statusDto.NewStatus
    //        );

    //        return RefundStatusUpdateResult.Success(
    //            refundRequest.Id,
    //            previousStatus,
    //            statusDto.NewStatus,
    //            refundRequest.RefundTransactionId
    //        );
    //    }
    //    catch (Exception ex)
    //    {
    //        _logger.Error(ex, "Error updating refund request {RefundId} status", refundId);
    //        await _unitOfWork.RollbackAsync();
    //        return RefundStatusUpdateResult.Fail(ex.Message);
    //    }
    //}

    public async Task<RefundStatusUpdateResult> UpdateRefundStatusAsync(
    Guid refundId,
    UpdateRefundStatusDto statusDto,
    string userId)
    {
        TbRefund refundRequest;
        TbOrder order;
        RefundStatus previousStatus;

        try
        {
            _logger.Information(
                "Updating refund request {RefundId} status to {NewStatus} by user {UserId}",
                refundId,
                statusDto.NewStatus,
                userId
            );

            var refundRepo = _unitOfWork.TableRepository<TbRefund>();
            refundRequest = await refundRepo.FindByIdAsync(refundId);
            // Use repository to get refund
            refundRequest = await _refundRepository.GetByIdAsync(refundId);

            if (refundRequest == null)
                return RefundStatusUpdateResult.Fail("Refund request not found");

            previousStatus = refundRequest.RefundStatus;

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
            order = await orderRepo.FindByIdAsync(orderDetails.OrderId);
            if (order == null)
                return RefundStatusUpdateResult.Fail("Order not found");

            await _unitOfWork.BeginTransactionAsync();

            refundRequest.RefundStatus = statusDto.NewStatus;
            refundRequest.AdminUserId = userId;

            if (!string.IsNullOrWhiteSpace(statusDto.Notes))
                refundRequest.AdminNotes = statusDto.Notes;

            var statusUpdateResult = await HandleStatusSpecificUpdates(
                refundRequest,
                order,
                statusDto
            );

            if (!statusUpdateResult.IsSuccess)
            {
                await _unitOfWork.RollbackAsync();
                return RefundStatusUpdateResult.Fail(statusUpdateResult.ErrorMessage!);
            }

            refundRequest.UpdatedDateUtc = DateTime.UtcNow;
            order.UpdatedDateUtc = DateTime.UtcNow;

            // Use repository to update refund
            await _refundRepository.UpdateAsync(refundRequest, Guid.Parse(userId));
            await _orderRepository.UpdateAsync(order, Guid.Parse(userId));

            // Use repository to create status history
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
                refundRequest.Id,
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
        catch (DbUpdateConcurrencyException)
        {
            await _unitOfWork.RollbackAsync();
            return RefundStatusUpdateResult.Fail(
                "Refund was updated by another user. Please refresh and try again."
            );
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            _logger.Error(ex, "Error updating refund request {RefundId}", refundId);
            return RefundStatusUpdateResult.Fail("Unexpected error occurred");
        }
    }

    public async Task<RefundRequestDto?> GetRefundRequestByIdAsync(Guid refundRequestId)
    {
        try
        {
            // Use repository to get refund
            var refundRequest = await _refundRepository.GetByIdAsync(refundRequestId);
            return refundRequest == null ? null : _mapper.MapModel<TbRefund, RefundRequestDto>(refundRequest);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error getting refund request {RefundId}", refundRequestId);
            return null;
        }
    }

    public async Task<RefundRequestDto> GetRefundRequestByNumberAsync(string number)
    {
        try
        {
            // Find refund request by number
            var refundRequest = await _refundRepository.GetByNumberAsync(number);

            // Validation to ensure refund request exists
            if (refundRequest == null)
                throw new ValidationException($"Refund request with number {number} not found.");

            return _mapper.MapModel<TbRefund, RefundRequestDto>(refundRequest);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error getting refund request with number {number}", number);
            return null;
        }
    }

    public async Task<RefundRequestDto?> GetRefundRequestByOrderDetailIdAsync(Guid orderDetailId)
    {
        try
        {
            // Use repository to get refund by order detail ID
            var refundRequest = await _refundRepository.GetByOrderDetailIdAsync(orderDetailId);
            return refundRequest == null ? null : _mapper.MapModel<TbRefund, RefundRequestDto>(refundRequest);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error getting refund request for order {OrderDetailId}", orderDetailId);
            return null;
        }
    }
    public async Task<RefundDetailsDto> FindById(Guid id)
    {
        try
        {
            // Use repository to find refund details
            var refundRequest = await _refundRepository.GetDetailsByIdAsync(id);

            // Validation to ensure refund request exists
            if (refundRequest == null)
                throw new NotFoundException($"Can't find Refund with Id: {id}",_logger);

            // Map to DTO and return
            return _mapper.MapModel<VwRefundDetails, RefundDetailsDto>(refundRequest); ;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error getting refund details for refund {RefundId}", id);
            throw new ApplicationException("Error getting refund details.");
        }
    }

    public async Task<PagedResult<RefundRequestDto>> GetRefundsPageAsync(RefundSearchCriteria criteria)
    {
        try
        {
            // Use repository to get paged refunds
            var (items, totalCount) = await _refundRepository.GetPagedAsync(criteria);

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
    UpdateRefundStatusDto statusDto)
    {
        switch (statusDto.NewStatus)
        {
            case RefundStatus.UnderReview:
                break;

            case RefundStatus.NeedMoreInfo:
                if (string.IsNullOrWhiteSpace(statusDto.Notes))
                    return StatusUpdateOperationResult.Fail(
                        "Notes are required when requesting more information"
                    );
                break;

            case RefundStatus.InfoApproved:
                break;

            case RefundStatus.ItemShippedBack:
                if (!string.IsNullOrWhiteSpace(statusDto.TrackingNumber))
                    refundRequest.ReturnTrackingNumber = statusDto.TrackingNumber;

                refundRequest.ReturnedDateUTC = DateTime.UtcNow;
                break;

            case RefundStatus.ItemReceived:
                refundRequest.ReturnedDateUTC ??= DateTime.UtcNow;
                break;

            case RefundStatus.Inspecting:
                break;

            case RefundStatus.Approved:
                refundRequest.ApprovedDateUTC = DateTime.UtcNow;
                refundRequest.ApprovedItemsCount =
                    statusDto.ApprovedItemsCount ?? refundRequest.RequestedItemsCount;

                var refundAmount = statusDto.RefundAmount ?? refundRequest.RefundAmount;
                if (refundAmount <= 0)
                    return StatusUpdateOperationResult.Fail("Invalid refund amount");

                refundRequest.RefundAmount = refundAmount;
                break;

            case RefundStatus.Rejected:
                if (string.IsNullOrWhiteSpace(statusDto.RejectionReason))
                    return StatusUpdateOperationResult.Fail(
                        "Rejection reason is required when rejecting a refund"
                    );

                refundRequest.RejectionReason = statusDto.RejectionReason;
                refundRequest.ApprovedItemsCount = 0;

                order.OrderStatus = OrderProgressStatus.Completed;

                _logger.Information(
                    "Refund request {RefundId} rejected: {Reason}",
                    refundRequest.Id,
                    statusDto.RejectionReason
                );
                break;

            case RefundStatus.Refunded:
                if (string.IsNullOrWhiteSpace(refundRequest.RefundTransactionId))
                    return StatusUpdateOperationResult.Fail(
                        "Cannot mark as Refunded without a refund transaction ID"
                    );

                refundRequest.RefundedDateUTC = DateTime.UtcNow;
                order.OrderStatus = OrderProgressStatus.Refunded;
                order.PaymentStatus = PaymentStatus.Refunded;
                break;

            case RefundStatus.Closed:
                if (refundRequest.RefundStatus is not (RefundStatus.Refunded or RefundStatus.Rejected))
                    return StatusUpdateOperationResult.Fail(
                        "Only Refunded or Rejected refunds can be closed"
                    );
                break;

            default:
                return StatusUpdateOperationResult.Fail(
                    $"Unsupported status: {statusDto.NewStatus}"
                );
        }

        return StatusUpdateOperationResult.Success();
    }

    //private async Task<StatusUpdateOperationResult> HandleStatusSpecificUpdates(
    //    TbRefund refundRequest,
    //    TbOrder order,
    //    UpdateRefundStatusDto statusDto,
    //    string userId)
    //{
    //    switch (statusDto.NewStatus)
    //    {
    //        case RefundStatus.UnderReview:
    //            break;

    //        case RefundStatus.NeedMoreInfo:
    //            if (string.IsNullOrEmpty(statusDto.Notes))
    //                return StatusUpdateOperationResult.Fail("Notes are required when requesting more information");
    //            break;

    //        case RefundStatus.InfoApproved:
    //            break;

    //        case RefundStatus.ItemShippedBack:
    //            if (!string.IsNullOrEmpty(statusDto.TrackingNumber))
    //                refundRequest.ReturnTrackingNumber = statusDto.TrackingNumber;
    //            refundRequest.ReturnedDateUTC = DateTime.UtcNow;
    //            break;

    //        case RefundStatus.ItemReceived:
    //            if (!refundRequest.ReturnedDateUTC.HasValue)
    //                refundRequest.ReturnedDateUTC = DateTime.UtcNow;
    //            break;

    //        case RefundStatus.Inspecting:
    //            break;

    //        case RefundStatus.Approved:
    //            refundRequest.ApprovedDateUTC = DateTime.UtcNow;
    //            refundRequest.ApprovedItemsCount = statusDto.ApprovedItemsCount ?? refundRequest.RequestedItemsCount;

    //            var refundAmount = statusDto.RefundAmount ?? refundRequest.RefundAmount;
    //            refundRequest.RefundAmount = refundAmount;

    //            //_logger.Information(
    //            //    "Executing refund payment for request {RefundId}, amount {Amount}",
    //            //    refundRequest.Id,
    //            //    refundAmount
    //            //);

    //            //var refundResult = await ExecuteRefundAsync(order, refundAmount);

    //            //if (refundResult.IsSuccess)
    //            //{
    //            //    refundRequest.RefundStatus = RefundStatus.Refunded;
    //            //    refundRequest.RefundedDateUTC = DateTime.UtcNow;
    //            //    refundRequest.RefundTransactionId = refundResult.RefundTransactionId;
    //            //    order.OrderStatus = OrderProgressStatus.Refunded;
    //            //    order.PaymentStatus = PaymentStatus.Refunded;

    //            //    _logger.Information(
    //            //        "Refund executed successfully for request {RefundId}, transaction {TransactionId}",
    //            //        refundRequest.Id,
    //            //        refundResult.RefundTransactionId
    //            //    );
    //            //}
    //            //else
    //            //{
    //            //    _logger.Error(
    //            //        "Refund execution failed for request {RefundId}: {Error}",
    //            //        refundRequest.Id,
    //            //        refundResult.ErrorMessage
    //            //    );
    //            //    return StatusUpdateOperationResult.Fail($"Refund payment failed: {refundResult.ErrorMessage}");
    //            //}
    //            break;

    //        case RefundStatus.Rejected:
    //            if (string.IsNullOrEmpty(statusDto.RejectionReason))
    //                return StatusUpdateOperationResult.Fail("Rejection reason is required when rejecting a refund");

    //            refundRequest.RejectionReason = statusDto.RejectionReason;
    //            refundRequest.ApprovedItemsCount = 0;
    //            order.OrderStatus = OrderProgressStatus.Completed;

    //            _logger.Information("Refund request {RefundId} rejected: {Reason}", refundRequest.Id, statusDto.RejectionReason);
    //            break;

    //        case RefundStatus.Refunded:
    //            if (string.IsNullOrEmpty(refundRequest.RefundTransactionId))
    //                return StatusUpdateOperationResult.Fail("Cannot manually mark as Refunded without a valid refund transaction ID");

    //            refundRequest.RefundedDateUTC = DateTime.UtcNow;
    //            order.OrderStatus = OrderProgressStatus.Refunded;
    //            order.PaymentStatus = PaymentStatus.Refunded;
    //            break;

    //        case RefundStatus.Closed:
    //            if (refundRequest.RefundStatus != RefundStatus.Refunded && refundRequest.RefundStatus != RefundStatus.Rejected)
    //                return StatusUpdateOperationResult.Fail("Can only close refund requests that are Refunded or Rejected");
    //            break;

    //        default:
    //            return StatusUpdateOperationResult.Fail($"Unsupported status: {statusDto.NewStatus}");
    //    }

    //    return StatusUpdateOperationResult.Success();
    //}

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

            // Use repository to create status history
            await _refundRepository.CreateStatusHistoryAsync(historyRecord, Guid.Parse(userId));
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
        // Added quantity validation
        if (requestedQuantity <= 0)
            return RefundEligibilityResult.NotEligible("Requested quantity must be greater than zero");

        if (requestedQuantity > orderDetails.Quantity)
            return RefundEligibilityResult.NotEligible(
                $"Requested quantity ({requestedQuantity}) exceeds ordered quantity ({orderDetails.Quantity})"
            );

        if (order.OrderStatus != OrderProgressStatus.Completed)
            return RefundEligibilityResult.NotEligible("Only completed orders can be refunded");

        if (order.PaymentStatus != PaymentStatus.Completed)
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
                .FirstOrDefaultAsync(p => p.OrderId == order.Id && p.PaymentStatus == PaymentStatus.Completed);

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
}

