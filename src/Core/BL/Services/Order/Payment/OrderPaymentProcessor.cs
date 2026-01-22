using BL.Contracts.Service.Currency;
using BL.Contracts.Service.Order.Payment;
using BL.Contracts.Service.Wallet.Customer;
using Common.Enumerations.Payment;
using DAL.Contracts.Repositories.Order;
using Domains.Entities.Order.Payment;
using Serilog;
using Shared.DTOs.Order.Payment.PaymentProcessing;

namespace BL.Services.Order.Payment;

/// <summary>
/// Service for processing order payments
/// REFACTORED: Separated responsibilities for each payment method
/// Uses repositories, no Info logging
/// </summary>
public class OrderPaymentProcessor : IOrderPaymentProcessor
{
    private readonly IPaymentService _paymentService;
    private readonly ICustomerWalletService _walletService;
    private readonly IOrderRepository _orderRepository;
    private readonly IOrderPaymentRepository _paymentRepository;
    private readonly ICurrencyService _currencyService;
    private readonly IPaymentMethodService _paymentMethodService;
    private readonly ILogger _logger;

    public OrderPaymentProcessor(
        IPaymentService paymentService,
        ICustomerWalletService walletService,
        IOrderRepository orderRepository,
        IOrderPaymentRepository paymentRepository,
        ICurrencyService currencyService,
        IPaymentMethodService paymentMethodService,
        ILogger logger)
    {
        _paymentService = paymentService ?? throw new ArgumentNullException(nameof(paymentService));
        _walletService = walletService ?? throw new ArgumentNullException(nameof(walletService));
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        _paymentRepository = paymentRepository ?? throw new ArgumentNullException(nameof(paymentRepository));
        _currencyService = currencyService ?? throw new ArgumentNullException(nameof(currencyService));
        _paymentMethodService = paymentMethodService ?? throw new ArgumentNullException(nameof(paymentMethodService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Process Cash on Delivery payment
    /// </summary>
    public async Task<PaymentResult> ProcessCashOnDeliveryAsync(
        Guid orderId,
        decimal amount,
        CancellationToken cancellationToken = default)
    {
        try
        {
            // Get COD payment method ID by type
            var codPaymentMethod = await _paymentMethodService.GetPaymentMethodByTypeAsync(
                PaymentMethodType.CashOnDelivery);

            if (codPaymentMethod == null)
            {
                _logger.Error("Cash on Delivery payment method not found in system");
                return PaymentResult.CreateFailure("Cash on Delivery payment method is not configured");
            }

            // Create payment record with pending status
            var payment = new TbOrderPayment
            {
                Id = Guid.NewGuid(),
                OrderId = orderId,
                PaymentMethodId = codPaymentMethod.Id,
                PaymentMethodType = PaymentMethodType.CashOnDelivery,
                Amount = amount,
                PaymentStatus = PaymentStatus.Pending,
                TransactionId = $"COD-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid():N}"[..30],
                CreatedDateUtc = DateTime.UtcNow,
                CreatedBy = Guid.Empty,
                IsDeleted = false,
            };

            await _paymentRepository.CreateAsync(payment, cancellationToken);

            // Order stays pending until delivery
            return PaymentResult.CreateSuccess(
                payment.TransactionId!,
                message: "Order created. Payment will be collected on delivery.");
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Failed to process COD payment for order {OrderId}", orderId);
            return PaymentResult.CreateFailure("Failed to process cash on delivery payment");
        }
    }

    /// <summary>
    /// Process Wallet payment
    /// </summary>
    public async Task<PaymentResult> ProcessWalletPaymentAsync(
        Guid orderId,
        decimal amount,
        string customerId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            // Get wallet payment method ID by type
            var walletPaymentMethod = await _paymentMethodService.GetPaymentMethodByTypeAsync(
                PaymentMethodType.Wallet);

            if (walletPaymentMethod == null)
            {
                _logger.Error("Wallet payment method not found in system");
                return PaymentResult.CreateFailure("Wallet payment method is not configured");
            }

            // 1. Check wallet balance
            var balance = await _walletService.GetBalanceAsync(customerId);

            if (balance < amount)
            {
                return PaymentResult.CreateFailure(
                    $"Insufficient wallet balance. Required: {amount:C}, Available: {balance:C}");
            }

            // 2. Deduct from wallet
            var walletPaymentSuccess = await _walletService.PayOrderAsync(
                customerId,
                amount,
                orderId);

            if (!walletPaymentSuccess)
            {
                return PaymentResult.CreateFailure("Failed to deduct from wallet");
            }

            // 3. Create payment record
            var payment = new TbOrderPayment
            {
                Id = Guid.NewGuid(),
                OrderId = orderId,
                PaymentMethodId = walletPaymentMethod.Id,
                PaymentMethodType = PaymentMethodType.Wallet,
                Amount = amount,
                PaymentStatus = PaymentStatus.Completed,
                TransactionId = $"WALLET-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid():N}"[..30],
                PaidAt = DateTime.UtcNow,
                CreatedDateUtc = DateTime.UtcNow,
                CreatedBy = Guid.Empty,
                IsDeleted = false
            };

            await _paymentRepository.CreateAsync(payment, cancellationToken);

            // 4. Update order payment status
            await UpdateOrderPaymentStatusAsync(
                orderId,
                PaymentStatus.Completed,
                cancellationToken);

            return PaymentResult.CreateSuccess(
                payment.TransactionId!,
                message: "Payment completed successfully using wallet");
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Failed to process wallet payment for order {OrderId}", orderId);
            return PaymentResult.CreateFailure("Failed to process wallet payment");
        }
    }

    /// <summary>
    /// Process Card payment via payment gateway
    /// </summary>
    public async Task<PaymentResult> ProcessCardPaymentAsync(
        Guid orderId,
        decimal amount,
        string customerId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            // Get payment method ID by type
            var cardPaymentMethod = await _paymentMethodService.GetPaymentMethodByTypeAsync(
                PaymentMethodType.Card);

            if (cardPaymentMethod == null)
            {
                _logger.Error("Card payment method not found in system");
                return PaymentResult.CreateFailure("Card payment method is not configured");
            }

            // 1. Create pending payment record
            var payment = new TbOrderPayment
            {
                Id = Guid.NewGuid(),
                OrderId = orderId,
                PaymentMethodId = cardPaymentMethod.Id,
                PaymentMethodType = PaymentMethodType.Card,
                Amount = amount,
                PaymentStatus = PaymentStatus.Pending,
                CreatedDateUtc = DateTime.UtcNow,
                CreatedBy = Guid.Empty,
                IsDeleted = false
            };

            await _paymentRepository.CreateAsync(payment, cancellationToken);

            // 2. Process via payment gateway
            var gatewayResult = await _paymentService.ProcessPaymentAsync(
                new OrderPaymentProcessRequest
                {
                    OrderId = orderId,
                    PaymentMethodId = cardPaymentMethod.Id,
                    Amount = amount
                });

            if (!gatewayResult.Success)
            {
                // Update payment status to failed
                payment.PaymentStatus = PaymentStatus.Failed;
                payment.FailureReason = gatewayResult.Message;
                await _paymentRepository.UpdateAsync(payment, cancellationToken);

                return PaymentResult.CreateFailure(gatewayResult.Message);
            }

            // 3. Update payment with transaction details
            payment.TransactionId = gatewayResult.TransactionId;
            payment.GatewayTransactionId = gatewayResult.TransactionId;

            if (gatewayResult.RequiresRedirect)
            {
                // 3D Secure or redirect required
                payment.PaymentStatus = PaymentStatus.Processing;
                await _paymentRepository.UpdateAsync(payment, cancellationToken);

                return PaymentResult.CreateSuccess(
                    gatewayResult.TransactionId!,
                    gatewayResult.PaymentUrl,
                    requiresRedirect: true);
            }
            else
            {
                // Direct success
                payment.PaymentStatus = PaymentStatus.Completed;
                payment.PaidAt = DateTime.UtcNow;
                await _paymentRepository.UpdateAsync(payment, cancellationToken);

                // Update order
                await UpdateOrderPaymentStatusAsync(
                    orderId,
                    PaymentStatus.Completed,
                    cancellationToken);

                return PaymentResult.CreateSuccess(
                    gatewayResult.TransactionId!,
                    message: "Payment completed successfully");
            }
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Failed to process card payment for order {OrderId}", orderId);
            return PaymentResult.CreateFailure("Failed to process card payment");
        }
    }

    /// <summary>
    /// Process Mixed payment (Wallet first, then Card for remaining)
    /// </summary>
    public async Task<PaymentResult> ProcessMixedPaymentAsync(
        Guid orderId,
        decimal totalAmount,
        string customerId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            // Get wallet and card payment methods
            var walletPaymentMethod = await _paymentMethodService.GetPaymentMethodByTypeAsync(
                PaymentMethodType.Wallet);

            if (walletPaymentMethod == null)
            {
                _logger.Error("Wallet payment method not found in system for mixed payment");
                return PaymentResult.CreateFailure("Wallet payment method is not configured");
            }

            var cardPaymentMethod = await _paymentMethodService.GetPaymentMethodByTypeAsync(
                PaymentMethodType.Card);

            if (cardPaymentMethod == null)
            {
                _logger.Error("Card payment method not found in system for mixed payment");
                return PaymentResult.CreateFailure("Card payment method is not configured");
            }

            // 1. Get wallet balance
            var walletBalance = await _walletService.GetBalanceAsync(customerId);

            if (walletBalance <= 0)
            {
                // No wallet balance - process as full card payment
                return await ProcessCardPaymentAsync(
                    orderId,
                    totalAmount,
                    customerId,
                    cancellationToken);
            }

            // 2. Calculate split amounts
            var walletAmount = Math.Min(walletBalance, totalAmount);
            var cardAmount = totalAmount - walletAmount;

            // 3. Deduct from wallet
            var walletPaymentSuccess = await _walletService.PayOrderAsync(
                customerId,
                walletAmount,
                orderId);

            if (!walletPaymentSuccess)
            {
                return PaymentResult.CreateFailure("Failed to deduct from wallet");
            }

            // 4. Create wallet payment record
            var walletPayment = new TbOrderPayment
            {
                Id = Guid.NewGuid(),
                OrderId = orderId,
                PaymentMethodId = walletPaymentMethod.Id,
                PaymentMethodType = PaymentMethodType.Wallet,
                Amount = walletAmount,
                PaymentStatus = PaymentStatus.Completed,
                TransactionId = $"WALLET-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid():N}"[..30],
                PaidAt = DateTime.UtcNow,
                CreatedDateUtc = DateTime.UtcNow,
                CreatedBy = Guid.Empty,
                IsDeleted = false
            };

            await _paymentRepository.CreateAsync(walletPayment, cancellationToken);

            // 5. If full amount covered by wallet
            if (cardAmount <= 0)
            {
                await UpdateOrderPaymentStatusAsync(
                    orderId,
                    PaymentStatus.Completed,
                    cancellationToken);

                return PaymentResult.CreateSuccess(
                    walletPayment.TransactionId!,
                    message: $"Payment completed using wallet: {walletAmount:C}",
                    walletAmount: walletAmount);
            }

            // 6. Process remaining amount via card
            var cardPayment = new TbOrderPayment
            {
                Id = Guid.NewGuid(),
                OrderId = orderId,
                PaymentMethodId = cardPaymentMethod.Id,
                PaymentMethodType = PaymentMethodType.Card,
                Amount = cardAmount,
                PaymentStatus = PaymentStatus.Pending,
                CreatedDateUtc = DateTime.UtcNow,
                CreatedBy = Guid.Empty,
                IsDeleted = false
            };

            await _paymentRepository.CreateAsync(cardPayment, cancellationToken);

            // 7. Process card payment
            var gatewayResult = await _paymentService.ProcessPaymentAsync(
                new OrderPaymentProcessRequest
                {
                    OrderId = orderId,
                    PaymentMethodId = cardPaymentMethod.Id,
                    Amount = cardAmount
                });

            if (!gatewayResult.Success)
            {
                // Card payment failed - order is partially paid
                cardPayment.PaymentStatus = PaymentStatus.Failed;
                cardPayment.FailureReason = gatewayResult.Message;
                await _paymentRepository.UpdateAsync(cardPayment, cancellationToken);

                await UpdateOrderPaymentStatusAsync(
                    orderId,
                    PaymentStatus.PartiallyPaid,
                    cancellationToken);

                return PaymentResult.CreateFailure(
                    $"Wallet payment succeeded ({walletAmount:C}), but card payment failed: {gatewayResult.Message}",
                    walletAmount: walletAmount);
            }

            // 8. Update card payment
            cardPayment.TransactionId = gatewayResult.TransactionId;
            cardPayment.GatewayTransactionId = gatewayResult.TransactionId;

            if (gatewayResult.RequiresRedirect)
            {
                cardPayment.PaymentStatus = PaymentStatus.Processing;
                await _paymentRepository.UpdateAsync(cardPayment, cancellationToken);

                return PaymentResult.CreateSuccess(
                    gatewayResult.TransactionId!,
                    gatewayResult.PaymentUrl,
                    requiresRedirect: true,
                    walletAmount: walletAmount,
                    cardAmount: cardAmount);
            }
            else
            {
                cardPayment.PaymentStatus = PaymentStatus.Completed;
                cardPayment.PaidAt = DateTime.UtcNow;
                await _paymentRepository.UpdateAsync(cardPayment, cancellationToken);

                await UpdateOrderPaymentStatusAsync(
                    orderId,
                    PaymentStatus.Completed,
                    cancellationToken);

                return PaymentResult.CreateSuccess(
                    cardPayment.TransactionId!,
                    message: $"Payment completed - Wallet: {walletAmount:C}, Card: {cardAmount:C}",
                    walletAmount: walletAmount,
                    cardAmount: cardAmount);
            }
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Failed to process mixed payment for order {OrderId}", orderId);
            return PaymentResult.CreateFailure("Failed to process mixed payment");
        }
    }

    /// <summary>
    /// Verify payment callback from gateway
    /// </summary>
    public async Task<bool> VerifyPaymentCallbackAsync(
        string gatewayTransactionId,
        bool isSuccess,
        string? failureReason = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var payment = await _paymentRepository.GetByGatewayTransactionIdAsync(
                gatewayTransactionId,
                cancellationToken);

            if (payment == null)
            {
                _logger.Error("Payment not found for gateway transaction {TransactionId}", gatewayTransactionId);
                return false;
            }

            if (isSuccess)
            {
                payment.PaymentStatus = PaymentStatus.Completed;
                payment.PaidAt = DateTime.UtcNow;

                await UpdateOrderPaymentStatusAsync(
                    payment.OrderId,
                    PaymentStatus.Completed,
                    cancellationToken);
            }
            else
            {
                payment.PaymentStatus = PaymentStatus.Failed;
                payment.FailureReason = failureReason;
            }

            await _paymentRepository.UpdateAsync(payment, cancellationToken);

            return true;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Failed to verify payment callback for {TransactionId}", gatewayTransactionId);
            return false;
        }
    }

    #region Private Helper Methods

    /// <summary>
    /// Update order payment status
    /// </summary>
    private async Task UpdateOrderPaymentStatusAsync(
        Guid orderId,
        PaymentStatus status,
        CancellationToken cancellationToken)
    {
        var order = await _orderRepository.FindByIdAsync(orderId, cancellationToken);

        if (order != null)
        {
            order.PaymentStatus = status;

            if (status == PaymentStatus.Completed)
            {
                order.PaidAt = DateTime.UtcNow;
            }

            await _orderRepository.UpdateAsync(order, cancellationToken);
        }
    }

    #endregion
}