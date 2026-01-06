namespace Shared.DTOs.Wallet.Customer
{
    public class ChargingCompletionDto
    {
        public string GatewayTransactionId { get; set; } = string.Empty;
        public bool IsSuccess { get; set; }
        public string? FailureReason { get; set; }
    }
}
