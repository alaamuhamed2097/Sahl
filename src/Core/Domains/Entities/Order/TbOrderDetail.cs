using Domains.Entities.Catalog.Item;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Entities.Order
{
    public class TbOrderDetail : BaseEntity
    {
        public int Quantity { get; set; } = 1;
        public decimal UnitPrice { get; set; }
        public int UnitPVs { get; set; }
        public decimal SubTotal { get; set; }

        [ForeignKey("Order")]
        public Guid OrderId { get; set; }
        [ForeignKey("Item")]
        public Guid ItemId { get; set; }

        // New fields
        public Guid? ItemCombinationId { get; set; }
        public Guid? OfferId { get; set; }
        public Guid? VendorId { get; set; }
        public Guid? WarehouseId { get; set; }
        public decimal DiscountAmount { get; set; } = 0m;
        public decimal TaxAmount { get; set; } = 0m;

        public virtual TbOrder Order { get; set; }
        public virtual TbItem Item { get; set; } = null!;
    }
}