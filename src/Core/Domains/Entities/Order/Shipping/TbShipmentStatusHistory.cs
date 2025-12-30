using Common.Enumerations.Shipping;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Entities.Order.Shipping
{
    public class TbShipmentStatusHistory : BaseEntity
    {
        [ForeignKey("Shipment")]
        public Guid ShipmentId { get; set; }

        public ShipmentStatus Status { get; set; }

        public DateTime StatusDate { get; set; }

        [MaxLength(500)]
        public string? Location { get; set; }

        [MaxLength(1000)]
        public string? Notes { get; set; }

        public virtual TbOrderShipment Shipment { get; set; } = null!;
    }
}
