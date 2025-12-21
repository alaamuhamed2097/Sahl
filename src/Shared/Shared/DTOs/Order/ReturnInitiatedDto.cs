using Shared.DTOs.Base;

namespace Shared.DTOs.Order
{
    /// <summary>
    /// Return/Refund Response
    /// </summary>
    public class ReturnInitiatedDto : BaseDto
    {
        public Guid ReturnId { get; set; }
        public Guid ShipmentId { get; set; }
        public string ShipmentNumber { get; set; } = null!;
        public string Reason { get; set; } = null!;
        public string Status { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public string Message { get; set; } = null!;
    }
}
