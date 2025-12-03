using Common.Enumerations.Payment;
using Domains.Entities.Shipping;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Entities.Order
{
    public class TbShipmentPayment : BaseEntity
    {
        [ForeignKey("Shipment")]
        public Guid ShipmentId { get; set; }

        [ForeignKey("Order")]
        public Guid OrderId { get; set; }

        public PaymentStatus PaymentStatus { get; set; }

        public decimal Amount { get; set; }

        public string? TransactionId { get; set; }

        public DateTime? PaidAt { get; set; }

        [MaxLength(500)]
        public string? Notes { get; set; }

        public virtual TbOrderShipment Shipment { get; set; } = null!;
        public virtual TbOrder Order { get; set; } = null!;
    }
}
