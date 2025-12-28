using Shared.DTOs.Order.Payment.PaymentProcessing;

namespace BL.Contracts.Service.Order.OrderProcessing;

/// <summary>
/// Interface for Order Purchase Service
/// </summary>
public interface IOrderPurchaseService
{
    /// <summary>
    /// Processes order purchase
    /// </summary>
    Task<PaymentTransactionResult> PurchaseAsync(
        OrderPurchaseDto purchaseDto,
        string customerId);
}
