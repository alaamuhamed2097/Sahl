using Common.Enumerations.Payment;

namespace Shared.DTOs.Order.Payment.PaymentProcessing;

/// <summary>
/// Result of order creation
/// </summary>
public class CreateOrderResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public Guid? OrderId { get; set; }
    public string? OrderNumber { get; set; }
    public decimal TotalAmount { get; set; }
    public PaymentStatus PaymentStatus { get; set; }
    public PaymentResult? PaymentResult { get; set; }

    public static CreateOrderResult CreateSuccess(
        Guid orderId,
        string orderNumber,
        decimal totalAmount,
        PaymentResult paymentResult)
    {
        return new CreateOrderResult
        {
            Success = true,
            Message = "Order created successfully",
            OrderId = orderId,
            OrderNumber = orderNumber,
            TotalAmount = totalAmount,
            PaymentStatus = paymentResult.RequiresRedirect ? PaymentStatus.Pending : PaymentStatus.Completed,
            PaymentResult = paymentResult
        };
    }

    public static CreateOrderResult CreateFailure(
        string message,
        Guid? orderId = null,
        string? orderNumber = null)
    {
        return new CreateOrderResult
        {
            Success = false,
            Message = message,
            OrderId = orderId,
            OrderNumber = orderNumber
        };
    }
}