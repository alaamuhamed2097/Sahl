namespace Shared.DTOs.Wallet.Customer
{
    public class ChargingInitiationResultDto
    {
        public Guid ChargingRequestId { get; set; }
        public string? PaymentUrl { get; set; }
        public string? GatewayTransactionId { get; set; }
        public bool Success { get; set; }
        public string? Message { get; set; }
    }
}
