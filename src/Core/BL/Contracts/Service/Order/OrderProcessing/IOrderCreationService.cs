using Shared.DTOs.Order.Payment.PaymentProcessing;

namespace BL.Contracts.Service.Order.OrderProcessing;

/// <summary>
/// Interface for order creation service
/// </summary>
public interface IOrderCreationService
{
    /// <summary>
    /// Creates an order from the current cart with payment processing
    /// </summary>
    Task<CreateOrderResult> CreateOrderAsync(
        string customerId,
        CreateOrderRequest request,
        CancellationToken cancellationToken = default);
}