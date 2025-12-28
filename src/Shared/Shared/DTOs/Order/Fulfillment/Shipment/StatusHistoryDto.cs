namespace Shared.DTOs.Order.Fulfillment.Shipment
{
    /// <summary>
    /// Status history DTO
    /// </summary>
    public class StatusHistoryDto
    {
        public Guid Id { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime StatusDate { get; set; }
        public string? Location { get; set; }
        public string? Notes { get; set; }
    }
}