using Common.Enumerations.Payment;
using Domains.Entities.Order.Shipping;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Entities.Order
{
    /// <summary>
    /// Shipment payment entity - ONLY for Cash on Delivery (COD)
    /// For online payments (wallet/card), use TbOrderPayment instead
    /// Collection details are handled by shipping company
    /// FINAL VERSION - Simplified
    /// </summary>
    public class TbShipmentPayment : BaseEntity
    {
        [ForeignKey("Shipment")]
        public Guid ShipmentId { get; set; }

        [ForeignKey("Order")]
        public Guid OrderId { get; set; }

        // Payment status for this specific shipment
        public PaymentStatus PaymentStatus { get; set; }

        // Amount to be collected for this shipment (includes tax, shipping, discount)
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        // Internal transaction reference
        [MaxLength(100)]
        public string? TransactionId { get; set; }

        // Date when payment was collected (from shipping company report)
        public DateTime? PaidAt { get; set; }

        // Additional notes (e.g., shipping company reference, external transaction ID)
        [MaxLength(1000)]
        public string? Notes { get; set; }

        // Navigation Properties
        public virtual TbOrderShipment Shipment { get; set; } = null!;
        public virtual TbOrder Order { get; set; } = null!;
    }
}