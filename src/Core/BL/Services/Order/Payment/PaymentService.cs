using AutoMapper;
using BL.Contracts.Service.Order.Payment;
using Common.Enumerations.Payment;
using DAL.Contracts.Repositories;
using DAL.Contracts.Repositories.Order;
using DAL.Contracts.UnitOfWork;
using Domains.Entities.Order.Payment;
using Serilog;
using Shared.DTOs.Order.Payment.PaymentProcessing;

namespace BL.Services.Order.Payment
{
    /// <summary>
    /// FINAL PaymentService - Best Practice
    /// ✅ Direct repository injection (no UnitOfWork for single operations)
    /// ✅ UnitOfWork only for multi-table transactions
    /// </summary>
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderPaymentRepository _paymentRepository;
        private readonly ITableRepository<TbPaymentMethod> _paymentMethodRepository;  // ✅ Direct injection
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public PaymentService(
            IUnitOfWork unitOfWork,
            IOrderRepository orderRepository,
            IOrderPaymentRepository paymentRepository,
            ITableRepository<TbPaymentMethod> paymentMethodRepository,  // ✅ Inject directly
            IMapper mapper,
            ILogger logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            _paymentRepository = paymentRepository ?? throw new ArgumentNullException(nameof(paymentRepository));
            _paymentMethodRepository = paymentMethodRepository ?? throw new ArgumentNullException(nameof(paymentMethodRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<PaymentResult> ProcessPaymentAsync(IPaymentProcessRequest request)
        {
            if (request is OrderPaymentProcessRequest orderPaymentRequest)
            {
                return await ProcessOrder(orderPaymentRequest);
            }

            throw new NotImplementedException("Only OrderPaymentProcessRequest is implemented.");
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

                var order = await _orderRepository.FindByIdAsync(request.OrderId);

                if (order == null)
                {
                    await _unitOfWork.RollbackAsync();
                    return new PaymentResult
                    {
                        Success = false,
                        Message = "Order not found",
                        ErrorCode = "ORDER_NOT_FOUND"
                    };
                }

                if (order.PaymentStatus == PaymentStatus.Completed)
                {
                    await _unitOfWork.RollbackAsync();
                    return new PaymentResult
                    {
                        Success = false,
                        Message = "Order already paid",
                        ErrorCode = "ALREADY_PAID"
                    };
                }

                // ✅ Use injected repository (no UnitOfWork.TableRepository)
                var paymentMethod = await _paymentMethodRepository.FindByIdAsync(request.PaymentMethodId);

                if (paymentMethod == null || !paymentMethod.IsActive)
                {
                    await _unitOfWork.RollbackAsync();
                    return new PaymentResult
                    {
                        Success = false,
                        Message = "Invalid or inactive payment method",
                        ErrorCode = "INVALID_PAYMENT_METHOD"
                    };
                }

                var payment = new TbOrderPayment
                {
                    Id = Guid.NewGuid(),
                    OrderId = request.OrderId,
                    PaymentMethodId = request.PaymentMethodId,
                    PaymentMethodType = paymentMethod.MethodType,
                    Amount = request.Amount,
                    PaymentStatus = PaymentStatus.Pending,
                    CreatedDateUtc = DateTime.UtcNow,
                    CreatedBy = Guid.Empty
                };

                await _paymentRepository.CreateAsync(payment);

                var result = await ProcessPaymentByMethodAsync(payment, paymentMethod, request);

                if (!result.Success)
                {
                    payment.PaymentStatus = PaymentStatus.Failed;
                    payment.FailureReason = result.Message?.Length > 500
                        ? result.Message.Substring(0, 500)
                        : result.Message;
                    await _paymentRepository.UpdateAsync(payment);
                    await _unitOfWork.CommitAsync();
                    return result;
                }

                payment.PaymentStatus = result.RequiresRedirect
                    ? PaymentStatus.Processing
                    : PaymentStatus.Completed;
                payment.TransactionId = result.TransactionId;
                payment.GatewayTransactionId = result.TransactionId;
                payment.PaidAt = result.RequiresRedirect ? null : DateTime.UtcNow;
                await _paymentRepository.UpdateAsync(payment);

                if (payment.PaymentStatus == PaymentStatus.Completed)
                {
                    order.PaymentStatus = PaymentStatus.Completed;
                    order.PaidAt = DateTime.UtcNow;
                    await _orderRepository.UpdateAsync(order);
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
                var payment = await _paymentRepository.GetOrderPaymentWithDetailsAsync(orderId);

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
                    ErrorMessage = payment.FailureReason
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
                var payments = await _paymentRepository.GetOrderPaymentsAsync(orderId);

                return payments.Select(p => new PaymentStatusDto
                {
                    PaymentId = p.Id,
                    OrderId = orderId,
                    OrderNumber = p.Order.Number,
                    Status = p.PaymentStatus,
                    StatusText = GetStatusText(p.PaymentStatus),
                    Amount = p.Amount,
                    PaymentMethodName = p.PaymentMethod?.TitleEn ?? "Unknown",
                    TransactionId = p.TransactionId,
                    PaymentDate = p.PaidAt,
                    CanRetry = p.PaymentStatus == PaymentStatus.Failed,
                    ErrorMessage = p.FailureReason,
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
                var payment = await _paymentRepository.GetPaymentWithDetailsAsync(paymentId);

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
                    PaymentMethodName = payment.PaymentMethod?.TitleEn ?? "Unknown",
                    TransactionId = payment.TransactionId,
                    PaymentDate = payment.PaidAt,
                    CanRetry = payment.PaymentStatus == PaymentStatus.Failed,
                    ErrorMessage = payment.FailureReason,
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

                var payment = await _paymentRepository.GetByTransactionIdAsync(transactionId);

                if (payment == null)
                {
                    _logger.Warning("Payment not found for transaction {TransactionId}", transactionId);
                    return false;
                }

                if (payment.PaymentStatus == PaymentStatus.Processing)
                {
                    await _unitOfWork.BeginTransactionAsync();

                    payment.PaymentStatus = PaymentStatus.Completed;
                    payment.PaidAt = DateTime.UtcNow;
                    await _paymentRepository.UpdateAsync(payment);

                    var order = await _orderRepository.FindByIdAsync(payment.OrderId);
                    if (order != null)
                    {
                        order.PaymentStatus = PaymentStatus.Completed;
                        order.PaidAt = DateTime.UtcNow;
                        await _orderRepository.UpdateAsync(order);
                    }

                    await _unitOfWork.CommitAsync();
                    return true;
                }

                return payment.PaymentStatus == PaymentStatus.Completed;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error verifying payment {TransactionId}", transactionId);
                await _unitOfWork.RollbackAsync();
                return false;
            }
        }

        public async Task<RefundResultDto> ProcessRefundAsync(RefundProcessRequest request)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var payment = await _paymentRepository.FindByIdAsync(request.PaymentId);

                if (payment == null)
                {
                    await _unitOfWork.RollbackAsync();
                    return new RefundResultDto
                    {
                        Success = false,
                        Message = "Payment not found",
                        RefundStatus = "FAILED"
                    };
                }

                if (payment.PaymentStatus != PaymentStatus.Completed)
                {
                    await _unitOfWork.RollbackAsync();
                    return new RefundResultDto
                    {
                        Success = false,
                        Message = "Payment not in completed status",
                        RefundStatus = "FAILED"
                    };
                }

                if (request.RefundAmount > payment.Amount)
                {
                    await _unitOfWork.RollbackAsync();
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
                payment.RefundTransactionId = $"REF-{payment.TransactionId}";
                payment.Notes = $"Refunded: {request.Reason}";
                await _paymentRepository.UpdateAsync(payment);

                var order = await _orderRepository.FindByIdAsync(payment.OrderId);
                if (order != null)
                {
                    order.PaymentStatus = PaymentStatus.Refunded;
                    await _orderRepository.UpdateAsync(order);
                }

                await _unitOfWork.CommitAsync();

                return new RefundResultDto
                {
                    Success = true,
                    Message = "Refund processed successfully",
                    RefundStatus = "COMPLETED",
                    RefundId = payment.Id,
                    RefundTransactionId = payment.RefundTransactionId,
                    RefundAmount = request.RefundAmount
                };
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error processing refund for payment {PaymentId}", request.PaymentId);
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
                var payment = await _paymentRepository.FindByIdAsync(paymentId);

                if (payment == null)
                    return false;

                if (payment.PaymentStatus != PaymentStatus.Pending &&
                    payment.PaymentStatus != PaymentStatus.Processing)
                {
                    return false;
                }

                payment.PaymentStatus = PaymentStatus.Cancelled;
                payment.Notes = $"Cancelled: {reason}";
                await _paymentRepository.UpdateAsync(payment);

                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error cancelling payment {PaymentId}", paymentId);
                return false;
            }
        }

        #region Private Helper Methods

        private async Task<PaymentResult> ProcessPaymentByMethodAsync(
            TbOrderPayment payment,
            TbPaymentMethod paymentMethod,
            IPaymentProcessRequest request)
        {
            switch (paymentMethod.MethodType)
            {
                case PaymentMethodType.Card:
                    return await ProcessCardPaymentAsync(payment, request);

                case PaymentMethodType.CashOnDelivery:
                    return ProcessCashOnDeliveryPayment(payment);

                case PaymentMethodType.Wallet:
                    return await ProcessWalletPaymentAsync(payment, request);

                case PaymentMethodType.WalletAndCard:
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

        private async Task<PaymentResult> ProcessCardPaymentAsync(
            TbOrderPayment payment,
            IPaymentProcessRequest request)
        {
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
            var transactionId = $"WALLET-{DateTime.UtcNow:yyyyMMdd}-{payment.Id:N}"[..30];

            return new PaymentResult
            {
                Success = true,
                Message = "Payment processed via wallet",
                TransactionId = transactionId,
                RequiresRedirect = false
            };
        }

        private async Task<PaymentResult> ProcessMixedPaymentAsync(
            TbOrderPayment payment,
            IPaymentProcessRequest request)
        {
            var transactionId = $"MIXED-{DateTime.UtcNow:yyyyMMdd}-{payment.Id:N}"[..30];

            return new PaymentResult
            {
                Success = true,
                Message = "Mixed payment requires wallet service integration",
                TransactionId = transactionId,
                RequiresRedirect = false
            };
        }

        private string GetStatusText(PaymentStatus status)
        {
            return status switch
            {
                PaymentStatus.Pending => "Pending",
                PaymentStatus.Processing => "Processing",
                PaymentStatus.Completed => "Completed",
                PaymentStatus.Failed => "Failed",
                PaymentStatus.Cancelled => "Cancelled",
                PaymentStatus.Refunded => "Refunded",
                PaymentStatus.PartiallyRefunded => "Partially Refunded",
                PaymentStatus.PartiallyPaid => "Partially Paid",
                _ => "Unknown"
            };
        }

        #endregion
    }
}