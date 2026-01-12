namespace Shared.DTOs.Order.Payment.PaymentProcessing;

/// <summary>
/// Payment result DTO
/// </summary>
public class PaymentResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public string? ErrorCode { get; set; }
    public Guid? PaymentId { get; set; }
    public string? TransactionId { get; set; }
    public string? PaymentUrl { get; set; }
    public bool RequiresRedirect { get; set; }
    public string? RedirectUrl { get; set; }
    public decimal? WalletAmount { get; set; }
    public decimal? CardAmount { get; set; }

    public static PaymentResult CreateSuccess(
        string transactionId,
        string? redirectUrl = null,
        bool requiresRedirect = false,
        string? message = null,
        decimal? walletAmount = null,
        decimal? cardAmount = null)
    {
        return new PaymentResult
        {
            Success = true,
            Message = message ?? "Payment processed successfully",
            TransactionId = transactionId,
            RedirectUrl = redirectUrl,
            RequiresRedirect = requiresRedirect,
            WalletAmount = walletAmount,
            CardAmount = cardAmount
        };
    }

    public static PaymentResult CreateFailure(
        string message,
        decimal? walletAmount = null)
    {
        return new PaymentResult
        {
            Success = false,
            Message = message,
            WalletAmount = walletAmount
        };
    }
}
