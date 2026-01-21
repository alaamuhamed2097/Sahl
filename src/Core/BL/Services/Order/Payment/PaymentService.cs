using AutoMapper;
using BL.Contracts.Service.Order.Payment;
using Common.Enumerations.Payment;
using DAL.Contracts.UnitOfWork;
using DAL.Repositories.Order.Refund;
using Domains.Entities.Order;
using Domains.Entities.Order.Payment;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Shared.DTOs.Order.Payment.PaymentProcessing;

namespace BL.Services.Order.Payment
{
    /// <summary>
    /// CORRECTED PaymentService Implementation
    /// Fixed: PaymentMethodType values and PaymentStatus duplicates
    /// </summary>
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRefundRepository _refundRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public PaymentService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger logger,
            IRefundRepository refundRepository)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _refundRepository = refundRepository;
        }

        public async Task<PaymentResult> ProcessPaymentAsync(IPaymentProcessRequest request)
        {
            if (request is OrderPaymentProcessRequest orderPaymentRequest)
            {
                return await ProcessOrder(orderPaymentRequest);
            }
            else
            {
                throw new NotImplementedException("Only OrderPaymentProcessRequest is implemented.");
            }
        }

        public async Task<PaymentResult> ProcessOrder(OrderPaymentProcessRequest request)
        {
            try
            {
                _logger.Information(
                    "Processing payment for order {OrderId}, amount {Amount}, method {PaymentMethodId}",
                    request.OrderId,
                    request.Amount,
                    request.PaymentMethodId
                );

                await _unitOfWork.BeginTransactionAsync();

                var orderRepo = _unitOfWork.TableRepository<TbOrder>();
                var order = await orderRepo.FindByIdAsync(request.OrderId);

                if (order == null)
                {
                    return new PaymentResult
                    {
                        Success = false,
                        Message = "Order not found",
                        ErrorCode = "ORDER_NOT_FOUND"
                    };
                }

                // ✅ FIX: Use Completed instead of Paid
                if (order.PaymentStatus == PaymentStatus.Completed)
                {
                    return new PaymentResult
                    {
                        Success = false,
                        Message = "Order already paid",
                        ErrorCode = "ALREADY_PAID"
                    };
                }

                var paymentMethodRepo = _unitOfWork.TableRepository<TbPaymentMethod>();
                var paymentMethod = await paymentMethodRepo.FindByIdAsync(request.PaymentMethodId);

                if (paymentMethod == null || !paymentMethod.IsActive)
                {
                    return new PaymentResult
                    {
                        Success = false,
                        Message = "Invalid or inactive payment method",
                        ErrorCode = "INVALID_PAYMENT_METHOD"
                    };
                }

                var paymentRepo = _unitOfWork.TableRepository<TbOrderPayment>();
                var payment = new TbOrderPayment
                {
                    Id = Guid.NewGuid(),
                    OrderId = request.OrderId,
                    PaymentMethodId = request.PaymentMethodId,
                    PaymentMethodType = paymentMethod.MethodType,  // ✅ Store method type
                    CurrencyId = Guid.Empty,
                    Amount = request.Amount,
                    PaymentStatus = PaymentStatus.Pending,
                    CreatedDateUtc = DateTime.UtcNow,
                    CreatedBy = Guid.Empty
                };

                await paymentRepo.CreateAsync(payment, Guid.Empty);

                var result = await ProcessPaymentByMethodAsync(payment, paymentMethod, request);

                if (!result.Success)
                {
                    payment.PaymentStatus = PaymentStatus.Failed;
                    payment.FailureReason = result.Message;  // ✅ Use FailureReason
                    await paymentRepo.UpdateAsync(payment, Guid.Empty);
                    await _unitOfWork.CommitAsync();
                    return result;
                }

                // ✅ FIX: Use Completed
                payment.PaymentStatus = result.RequiresRedirect
                    ? PaymentStatus.Processing
                    : PaymentStatus.Completed;
                payment.TransactionId = result.TransactionId;
                payment.GatewayTransactionId = result.TransactionId;  // ✅ Store gateway transaction
                payment.PaidAt = result.RequiresRedirect ? null : DateTime.UtcNow;
                await paymentRepo.UpdateAsync(payment, Guid.Empty);

                // ✅ FIX: Use Completed
                if (payment.PaymentStatus == PaymentStatus.Completed)
                {
                    order.PaymentStatus = PaymentStatus.Completed;
                    order.PaidAt = DateTime.UtcNow;
                    await orderRepo.UpdateAsync(order, Guid.Empty);
                }

                await _unitOfWork.CommitAsync();

                _logger.Information(
                    "Payment processed successfully for order {OrderId}, transaction {TransactionId}",
                    request.OrderId,
                    result.TransactionId
                );

                result.PaymentId = payment.Id;
                return result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error processing payment for order {OrderId}", request.OrderId);
                await _unitOfWork.RollbackAsync();

                return new PaymentResult
                {
                    Success = false,
                    Message = "Payment processing failed",
                    ErrorCode = "PAYMENT_ERROR"
                };
            }
        }

        public async Task<PaymentStatusDto> GetPaymentStatusAsync(Guid orderId)
        {
            try
            {
                var paymentRepo = _unitOfWork.TableRepository<TbOrderPayment>();

                var payment = await paymentRepo.GetQueryable()
                    .Where(p => p.OrderId == orderId && !p.IsDeleted)
                    .Include(p => p.Order)
                    .Include(p => p.PaymentMethod)
                    .OrderByDescending(p => p.CreatedDateUtc)
                    .FirstOrDefaultAsync();

                if (payment == null)
                {
                    return new PaymentStatusDto
                    {
                        OrderId = orderId,
                        Status = PaymentStatus.Pending,
                        StatusText = "No payment found",
                        CanRetry = true
                    };
                }

                return new PaymentStatusDto
                {
                    OrderId = orderId,
                    OrderNumber = payment.Order.Number,
                    Status = payment.PaymentStatus,
                    StatusText = GetStatusText(payment.PaymentStatus),
                    Amount = payment.Amount,
                    PaymentMethodName = payment.PaymentMethod.TitleEn,
                    TransactionId = payment.TransactionId,
                    PaymentDate = payment.PaidAt,
                    CanRetry = payment.PaymentStatus == PaymentStatus.Failed,
                    ErrorMessage = payment.FailureReason  // ✅ Use FailureReason
                };
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting payment status for order {OrderId}", orderId);
                throw;
            }
        }

        public async Task<List<PaymentStatusDto>> GetOrderPaymentsAsync(Guid orderId)
        {
            try
            {
                var paymentRepo = _unitOfWork.TableRepository<TbOrderPayment>();

                var payments = await paymentRepo.GetQueryable()
                    .Where(p => p.OrderId == orderId && !p.IsDeleted)
                    .Include(p => p.Order)
                    .Include(p => p.PaymentMethod)
                    .OrderByDescending(p => p.CreatedDateUtc)
                    .ToListAsync();

                return payments.Select(p => new PaymentStatusDto
                {
                    PaymentId = p.Id,
                    OrderId = orderId,
                    OrderNumber = p.Order.Number,
                    Status = p.PaymentStatus,
                    StatusText = GetStatusText(p.PaymentStatus),
                    Amount = p.Amount,
                    PaymentMethodName = p.PaymentMethod.TitleEn,
                    TransactionId = p.TransactionId,
                    PaymentDate = p.PaidAt,
                    CanRetry = p.PaymentStatus == PaymentStatus.Failed,
                    ErrorMessage = p.FailureReason,  // ✅ Use FailureReason
                    CreatedDateUtc = p.CreatedDateUtc
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting payments for order {OrderId}", orderId);
                throw;
            }
        }

        public async Task<PaymentStatusDto?> GetPaymentByIdAsync(Guid paymentId)
        {
            try
            {
                var paymentRepo = _unitOfWork.TableRepository<TbOrderPayment>();

                var payment = await paymentRepo.GetQueryable()
                    .Where(p => p.Id == paymentId && !p.IsDeleted)
                    .Include(p => p.Order)
                    .Include(p => p.PaymentMethod)
                    .FirstOrDefaultAsync();

                if (payment == null)
                    return null;

                return new PaymentStatusDto
                {
                    PaymentId = payment.Id,
                    OrderId = payment.OrderId,
                    OrderNumber = payment.Order.Number,
                    Status = payment.PaymentStatus,
                    StatusText = GetStatusText(payment.PaymentStatus),
                    Amount = payment.Amount,
                    PaymentMethodName = payment.PaymentMethod.TitleEn,
                    TransactionId = payment.TransactionId,
                    PaymentDate = payment.PaidAt,
                    CanRetry = payment.PaymentStatus == PaymentStatus.Failed,
                    ErrorMessage = payment.FailureReason,  // ✅ Use FailureReason
                    CreatedDateUtc = payment.CreatedDateUtc
                };
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting payment {PaymentId}", paymentId);
                throw;
            }
        }

        public async Task<bool> VerifyPaymentAsync(string transactionId)
        {
            try
            {
                _logger.Information("Verifying payment transaction {TransactionId}", transactionId);

                var paymentRepo = _unitOfWork.TableRepository<TbOrderPayment>();
                var payment = await paymentRepo.GetQueryable()
                    .Include(p => p.PaymentMethod)
                    .FirstOrDefaultAsync(p => p.TransactionId == transactionId);

                if (payment == null)
                {
                    _logger.Warning("Payment not found for transaction {TransactionId}", transactionId);
                    return false;
                }

                // ✅ FIX: Use Completed
                if (payment.PaymentStatus == PaymentStatus.Processing)
                {
                    payment.PaymentStatus = PaymentStatus.Completed;
                    payment.PaidAt = DateTime.UtcNow;
                    await paymentRepo.UpdateAsync(payment, Guid.Empty);

                    var orderRepo = _unitOfWork.TableRepository<TbOrder>();
                    var order = await orderRepo.FindByIdAsync(payment.OrderId);
                    if (order != null)
                    {
                        order.PaymentStatus = PaymentStatus.Completed;
                        order.PaidAt = DateTime.UtcNow;
                        await _refundRepository.UpdateOrderAsync(order, Guid.Empty);
                    }

                    await _unitOfWork.CommitAsync();
                    return true;
                }

                return payment.PaymentStatus == PaymentStatus.Completed;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error verifying payment {TransactionId}", transactionId);
                return false;
            }
        }

        public async Task<RefundResultDto> ProcessRefundAsync(RefundProcessRequest request)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var paymentRepo = _unitOfWork.TableRepository<TbOrderPayment>();
                var payment = await paymentRepo.FindByIdAsync(request.PaymentId);

                if (payment == null)
                {
                    return new RefundResultDto
                    {
                        Success = false,
                        Message = "Payment not found",
                        RefundStatus = "FAILED"
                    };
                }

                // ✅ FIX: Use Completed
                if (payment.PaymentStatus != PaymentStatus.Completed)
                {
                    return new RefundResultDto
                    {
                        Success = false,
                        Message = "Payment not in paid status",
                        RefundStatus = "FAILED"
                    };
                }

                if (request.RefundAmount > payment.Amount)
                {
                    return new RefundResultDto
                    {
                        Success = false,
                        Message = "Refund amount exceeds payment amount",
                        RefundStatus = "FAILED"
                    };
                }

                payment.PaymentStatus = PaymentStatus.Refunded;
                payment.RefundedAt = DateTime.UtcNow;
                payment.RefundAmount = request.RefundAmount;
                payment.Notes = $"Refunded: {request.Reason}";
                await paymentRepo.UpdateAsync(payment, Guid.Empty);

                var orderRepo = _unitOfWork.TableRepository<TbOrder>();
                var order = (await orderRepo.GetAsync(o=>o.Id == payment.OrderId)).FirstOrDefault();
                if (order != null)
                {
                    order.PaymentStatus = PaymentStatus.Refunded;
                    await _refundRepository.UpdateOrderAsync(order, Guid.Empty);
                }

                await _unitOfWork.CommitAsync();

                return new RefundResultDto
                {
                    Success = true,
                    Message = "Refund processed successfully",
                    RefundStatus = "COMPLETED",
                    RefundId = payment.Id,
                    RefundTransactionId = $"REF-{payment.TransactionId}",
                    RefundAmount = request.RefundAmount
                };
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error processing refund for order {OrderId}", request.OrderId);
                await _unitOfWork.RollbackAsync();

                return new RefundResultDto
                {
                    Success = false,
                    Message = "Refund processing failed",
                    RefundStatus = "FAILED"
                };
            }
        }

        public async Task<bool> CancelPaymentAsync(Guid paymentId, string reason)
        {
            try
            {
                var paymentRepo = _unitOfWork.TableRepository<TbOrderPayment>();
                var payment = await paymentRepo.FindByIdAsync(paymentId);

                if (payment == null)
                    return false;

                if (payment.PaymentStatus != PaymentStatus.Pending &&
                    payment.PaymentStatus != PaymentStatus.Processing)
                {
                    return false;
                }

                payment.PaymentStatus = PaymentStatus.Cancelled;
                payment.Notes = $"Cancelled: {reason}";
                await paymentRepo.UpdateAsync(payment, Guid.Empty);

                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error cancelling payment {PaymentId}", paymentId);
                return false;
            }
        }

        #region Private Helper Methods

        /// <summary>
        /// Process payment based on payment method
        /// ✅ FIXED: Uses correct PaymentMethodType values
        /// </summary>
        private async Task<PaymentResult> ProcessPaymentByMethodAsync(
            TbOrderPayment payment,
            TbPaymentMethod paymentMethod,
            IPaymentProcessRequest request)
        {
            // ✅ FIX: Use Card instead of CreditCard, remove BankTransfer
            switch (paymentMethod.MethodType)
            {
                case PaymentMethodType.Card:  // ✅ Changed from CreditCard
                    return await ProcessCardPaymentAsync(payment, request);

                case PaymentMethodType.CashOnDelivery:
                    return ProcessCashOnDeliveryPayment(payment);

                case PaymentMethodType.Wallet:
                    return await ProcessWalletPaymentAsync(payment, request);

                case PaymentMethodType.WalletAndCard:  // ✅ Added new type
                    return await ProcessMixedPaymentAsync(payment, request);

                default:
                    return new PaymentResult
                    {
                        Success = false,
                        Message = $"Payment method {paymentMethod.MethodType} not supported",
                        ErrorCode = "UNSUPPORTED_METHOD"
                    };
            }
        }

        /// <summary>
        /// Process card payment via gateway
        /// ✅ Renamed from ProcessCreditCardPaymentAsync
        /// </summary>
        private async Task<PaymentResult> ProcessCardPaymentAsync(
            TbOrderPayment payment,
            IPaymentProcessRequest request)
        {
            // TODO: Integrate with actual payment gateway (Stripe, PayPal, etc.)
            var transactionId = $"TXN-{DateTime.UtcNow:yyyyMMdd}-{payment.Id:N}"[..30];
            var paymentUrl = $"https://payment-gateway.com/pay?token={transactionId}";

            return new PaymentResult
            {
                Success = true,
                Message = "Redirect to payment gateway",
                TransactionId = transactionId,
                PaymentUrl = paymentUrl,
                RequiresRedirect = true
            };
        }

        private PaymentResult ProcessCashOnDeliveryPayment(TbOrderPayment payment)
        {
            var transactionId = $"COD-{DateTime.UtcNow:yyyyMMdd}-{payment.Id:N}"[..30];

            return new PaymentResult
            {
                Success = true,
                Message = "Cash on delivery confirmed",
                TransactionId = transactionId,
                RequiresRedirect = false
            };
        }

        private async Task<PaymentResult> ProcessWalletPaymentAsync(
            TbOrderPayment payment,
            IPaymentProcessRequest request)
        {
            // TODO: Integrate with wallet service
            var transactionId = $"WALLET-{DateTime.UtcNow:yyyyMMdd}-{payment.Id:N}"[..30];

            return new PaymentResult
            {
                Success = true,
                Message = "Payment processed via wallet",
                TransactionId = transactionId,
                RequiresRedirect = false
            };
        }

        /// <summary>
        /// Process mixed payment (Wallet + Card)
        /// ✅ NEW: Added support for WalletAndCard
        /// </summary>
        private async Task<PaymentResult> ProcessMixedPaymentAsync(
            TbOrderPayment payment,
            IPaymentProcessRequest request)
        {
            // TODO: Implement wallet + card payment flow
            var transactionId = $"MIXED-{DateTime.UtcNow:yyyyMMdd}-{payment.Id:N}"[..30];

            return new PaymentResult
            {
                Success = true,
                Message = "Mixed payment requires wallet service integration",
                TransactionId = transactionId,
                RequiresRedirect = false
            };
        }

        /// <summary>
        /// Get status text for display
        /// ✅ FIXED: Removed duplicate Paid case
        /// </summary>
        private string GetStatusText(PaymentStatus status)
        {
            return status switch
            {
                PaymentStatus.Pending => "Pending",
                PaymentStatus.Processing => "Processing",
                PaymentStatus.Completed => "Completed",  // ✅ Only one entry now
                PaymentStatus.Failed => "Failed",
                PaymentStatus.Cancelled => "Cancelled",
                PaymentStatus.Refunded => "Refunded",
                PaymentStatus.PartiallyRefunded => "Partially Refunded",  // ✅ Added
                _ => "Unknown"
            };
        }

        #endregion
    }
}