using Domains.Entities.Order.Shipping;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Entities.Order
{
    public class TbReturnOrder : BaseEntity
    {
        [ForeignKey("Shipment")]
        public Guid ShipmentId { get; set; }

        public ReturnStatus Status { get; set; }

        [Required]
        [MaxLength(500)]
        public string Reason { get; set; } = null!;

        public DateTime RequestDate { get; set; }

        public DateTime? ApprovedDate { get; set; }

        [MaxLength(1000)]
        public string? AdminNotes { get; set; }

        public decimal ReturnShippingCost { get; set; } = 0m;

        public decimal RefundAmount { get; set; } = 0m;

        public virtual TbOrderShipment Shipment { get; set; } = null!;
    }

    public enum ReturnStatus
    {
        Pending = 1,
        Approved = 2,
        Rejected = 3,
        InTransit = 4,
        Received = 5,
        Refunded = 6,
        Cancelled = 7
    }
}
