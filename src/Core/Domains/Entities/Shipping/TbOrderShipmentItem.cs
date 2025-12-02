using Domains.Entities.Order;
using Domains.Entities.Catalog.Item;

namespace Domains.Entities.Shipping
{
    public class TbOrderShipmentItem : BaseEntity
    {
        public Guid ShipmentId { get; set; }
        public Guid OrderDetailId { get; set; }
        public Guid ItemId { get; set; }
        public Guid? ItemCombinationId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal SubTotal { get; set; }

        public virtual TbOrderShipment Shipment { get; set; } = null!;
        public virtual TbOrderDetail OrderDetail { get; set; } = null!;
        public virtual TbItem Item { get; set; } = null!;
    }
}
