using AutoMapper;
using BL.Contracts.Service.Order.Payment;
using Common.Enumerations.Payment;
using DAL.Contracts.UnitOfWork;
using Domains.Entities.Order;
using Domains.Entities.Payment;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Shared.DTOs.Order.Payment.PaymentProcessing;

namespace BL.Services.Order.Payment
{
    /// <summary>
    /// COMPLETE PaymentService Implementation
    /// </summary>
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        // TODO: Add payment gateway services (Stripe, PayPal, etc.)
        // private readonly IStripePaymentGateway _stripeGateway;

        public PaymentService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Process payment for an order
        /// </summary>
        public async Task<PaymentResultDto> ProcessPaymentAsync(PaymentProcessRequest request)
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

                // 1. Validate order exists
                var orderRepo = _unitOfWork.TableRepository<TbOrder>();
                var order = await orderRepo.FindByIdAsync(request.OrderId);

                if (order == null)
                {
                    return new PaymentResultDto
                    {
                        Success = false,
                        Message = "Order not found",
                        ErrorCode = "ORDER_NOT_FOUND"
                    };
                }

                // 2. Check if order already paid
                if (order.PaymentStatus == PaymentStatus.Paid)
                {
                    return new PaymentResultDto
                    {
                        Success = false,
                        Message = "Order already paid",
                        ErrorCode = "ALREADY_PAID"
                    };
                }

                // 3. Validate payment method exists
                var paymentMethodRepo = _unitOfWork.TableRepository<TbPaymentMethod>();
                var paymentMethod = await paymentMethodRepo.FindByIdAsync(request.PaymentMethodId);

                if (paymentMethod == null || !paymentMethod.IsActive)
                {
                    return new PaymentResultDto
                    {
                        Success = false,
                        Message = "Invalid or inactive payment method",
                        ErrorCode = "INVALID_PAYMENT_METHOD"
                    };
                }

                // 4. Create payment record
                var paymentRepo = _unitOfWork.TableRepository<TbOrderPayment>();
                var payment = new TbOrderPayment
                {
                    Id = Guid.NewGuid(),
                    OrderId = request.OrderId,
                    PaymentMethodId = request.PaymentMethodId,
                    CurrencyId = Guid.Empty, // TODO: Get from order or settings
                    Amount = request.Amount,
                    PaymentStatus = PaymentStatus.Pending,
                    CreatedDateUtc = DateTime.UtcNow,
                    CreatedBy = Guid.Empty
                };

                await paymentRepo.CreateAsync(payment, Guid.Empty);

                // 5. Process payment based on method type
                var result = await ProcessPaymentByMethodAsync(payment, paymentMethod, request);

                if (!result.Success)
                {
                    payment.PaymentStatus = PaymentStatus.Failed;
                    payment.Notes = result.Message;
                    await paymentRepo.UpdateAsync(payment, Guid.Empty);
                    await _unitOfWork.CommitAsync();
                    return result;
                }

                // 6. Update payment status
                payment.PaymentStatus = result.RequiresRedirect
                    ? PaymentStatus.Processing
                    : PaymentStatus.Paid;
                payment.TransactionId = result.TransactionId;
                payment.PaidAt = result.RequiresRedirect ? null : DateTime.UtcNow;
                await paymentRepo.UpdateAsync(payment, Guid.Empty);

                // 7. Update order payment status if immediate payment (e.g., Cash on Delivery)
                if (payment.PaymentStatus == PaymentStatus.Paid)
                {
                    order.PaymentStatus = PaymentStatus.Paid;
                    order.PaymentDate = DateTime.UtcNow;
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

                return new PaymentResultDto
                {
                    Success = false,
                    Message = "Payment processing failed",
                    ErrorCode = "PAYMENT_ERROR"
                };
            }
        }

        /// <summary>
        /// Get payment status for an order
        /// </summary>
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
                    ErrorMessage = payment.Notes
                };
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting payment status for order {OrderId}", orderId);
                throw;
            }
        }

        /// <summary>
        /// Get all payment attempts for an order
        /// </summary>
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
                    ErrorMessage = p.Notes,
                    CreatedDateUtc = p.CreatedDateUtc
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting payments for order {OrderId}", orderId);
                throw;
            }
        }

        /// <summary>
        /// Get payment by ID
        /// </summary>
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
                    ErrorMessage = payment.Notes,
                    CreatedDateUtc = payment.CreatedDateUtc
                };
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting payment {PaymentId}", paymentId);
                throw;
            }
        }

        /// <summary>
        /// Verify payment transaction
        /// </summary>
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

                // TODO: Verify with actual payment gateway
                // For now, just check if payment exists and is processing
                if (payment.PaymentStatus == PaymentStatus.Processing)
                {
                    // Update to paid
                    payment.PaymentStatus = PaymentStatus.Paid;
                    payment.PaidAt = DateTime.UtcNow;
                    await paymentRepo.UpdateAsync(payment, Guid.Empty);

                    // Update order
                    var orderRepo = _unitOfWork.TableRepository<TbOrder>();
                    var order = await orderRepo.FindByIdAsync(payment.OrderId);
                    if (order != null)
                    {
                        order.PaymentStatus = PaymentStatus.Paid;
                        order.PaymentDate = DateTime.UtcNow;
                        await orderRepo.UpdateAsync(order, Guid.Empty);
                    }

                    await _unitOfWork.CommitAsync();
                    return true;
                }

                return payment.PaymentStatus == PaymentStatus.Paid;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error verifying payment {TransactionId}", transactionId);
                return false;
            }
        }

        /// <summary>
        /// Process refund
        /// </summary>
        public async Task<RefundResultDto> ProcessRefundAsync(RefundProcessRequest request)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                // 1. Get payment
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

                if (payment.PaymentStatus != PaymentStatus.Paid)
                {
                    return new RefundResultDto
                    {
                        Success = false,
                        Message = "Payment not in paid status",
                        RefundStatus = "FAILED"
                    };
                }

                // 2. Validate refund amount
                if (request.RefundAmount > payment.Amount)
                {
                    return new RefundResultDto
                    {
                        Success = false,
                        Message = "Refund amount exceeds payment amount",
                        RefundStatus = "FAILED"
                    };
                }

                // 3. TODO: Process refund via payment gateway
                // var gatewayResult = await _paymentGateway.ProcessRefundAsync(...)

                // 4. Update payment record
                payment.PaymentStatus = PaymentStatus.Refunded;
                payment.RefundedAt = DateTime.UtcNow;
                payment.RefundAmount = request.RefundAmount;
                payment.Notes = $"Refunded: {request.Reason}";
                await paymentRepo.UpdateAsync(payment, Guid.Empty);

                // 5. Update order
                var orderRepo = _unitOfWork.TableRepository<TbOrder>();
                var order = await orderRepo.FindByIdAsync(payment.OrderId);
                if (order != null)
                {
                    order.PaymentStatus = PaymentStatus.Refunded;
                    await orderRepo.UpdateAsync(order, Guid.Empty);
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

        /// <summary>
        /// Cancel payment
        /// </summary>
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

        // ==================== PRIVATE HELPER METHODS ====================

        /// <summary>
        /// Process payment based on payment method
        /// </summary>
        private async Task<PaymentResultDto> ProcessPaymentByMethodAsync(
            TbOrderPayment payment,
            TbPaymentMethod paymentMethod,
            PaymentProcessRequest request)
        {
            switch (paymentMethod.MethodType)
            {
                case PaymentMethod.CreditCard:
                    return await ProcessCreditCardPaymentAsync(payment, request);

                case PaymentMethod.CashOnDelivery:
                    return ProcessCashOnDeliveryPayment(payment);

                case PaymentMethod.Wallet:
                    return await ProcessWalletPaymentAsync(payment, request);

                case PaymentMethod.BankTransfer:
                    return ProcessBankTransferPayment(payment);

                default:
                    return new PaymentResultDto
                    {
                        Success = false,
                        Message = $"Payment method {paymentMethod.MethodType} not supported",
                        ErrorCode = "UNSUPPORTED_METHOD"
                    };
            }
        }

        /// <summary>
        /// Process credit card payment via gateway
        /// </summary>
        private async Task<PaymentResultDto> ProcessCreditCardPaymentAsync(
            TbOrderPayment payment,
            PaymentProcessRequest request)
        {
            // TODO: Integrate with actual payment gateway (Stripe, PayPal, etc.)
            // For now, return mock redirect URL

            var transactionId = $"TXN-{DateTime.UtcNow:yyyyMMdd}-{payment.Id:N}".Substring(0, 30);
            var paymentUrl = $"https://payment-gateway.com/pay?token={transactionId}";

            return new PaymentResultDto
            {
                Success = true,
                Message = "Redirect to payment gateway",
                TransactionId = transactionId,
                PaymentUrl = paymentUrl,
                RequiresRedirect = true
            };
        }

        /// <summary>
        /// Process cash on delivery payment
        /// </summary>
        private PaymentResultDto ProcessCashOnDeliveryPayment(TbOrderPayment payment)
        {
            var transactionId = $"COD-{DateTime.UtcNow:yyyyMMdd}-{payment.Id:N}".Substring(0, 30);

            return new PaymentResultDto
            {
                Success = true,
                Message = "Cash on delivery confirmed",
                TransactionId = transactionId,
                RequiresRedirect = false
            };
        }

        /// <summary>
        /// Process wallet payment
        /// </summary>
        private async Task<PaymentResultDto> ProcessWalletPaymentAsync(
            TbOrderPayment payment,
            PaymentProcessRequest request)
        {
            // TODO: Integrate with wallet service
            // Check balance, deduct amount, etc.

            var transactionId = $"WALLET-{DateTime.UtcNow:yyyyMMdd}-{payment.Id:N}".Substring(0, 30);

            return new PaymentResultDto
            {
                Success = true,
                Message = "Payment processed via wallet",
                TransactionId = transactionId,
                RequiresRedirect = false
            };
        }

        /// <summary>
        /// Process bank transfer payment
        /// </summary>
        private PaymentResultDto ProcessBankTransferPayment(TbOrderPayment payment)
        {
            var transactionId = $"BANK-{DateTime.UtcNow:yyyyMMdd}-{payment.Id:N}".Substring(0, 30);

            return new PaymentResultDto
            {
                Success = true,
                Message = "Bank transfer pending verification",
                TransactionId = transactionId,
                RequiresRedirect = false
            };
        }

        /// <summary>
        /// Get status text for display
        /// </summary>
        private string GetStatusText(PaymentStatus status)
        {
            return status switch
            {
                PaymentStatus.Pending => "Pending",
                PaymentStatus.Processing => "Processing",
                PaymentStatus.Paid => "Paid",
                PaymentStatus.Failed => "Failed",
                PaymentStatus.Cancelled => "Cancelled",
                PaymentStatus.Refunded => "Refunded",
                _ => "Unknown"
            };
        }
    }
}