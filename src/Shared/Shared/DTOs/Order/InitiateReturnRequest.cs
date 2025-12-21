namespace Shared.DTOs.Order
{
    /// <summary>
    /// Return/Refund Request
    /// </summary>
    public class InitiateReturnRequest
    {
        public Guid ShipmentId { get; set; }
        public string Reason { get; set; } = null!;
        public string? Description { get; set; }
        public List<string>? IssueImages { get; set; }
    }
}
