using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Entities.Inventory
{
    public class TbMovitemsdetail : BaseEntity
    {
        [ForeignKey("Moitem")]
        public Guid? MoitemId { get; set; }

        [ForeignKey("Mortem")]
        public Guid? MortemId { get; set; }

        [ForeignKey("Item")]
        public Guid ItemId { get; set; }

        [ForeignKey("Warehouse")]
        public Guid WarehouseId { get; set; }

        [ForeignKey("ItemAttributeCombinationPricing")]
        public Guid? ItemAttributeCombinationPricingId { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal TotalPrice { get; set; }

        public virtual TbMoitem? Moitem { get; set; }
        public virtual TbMortem? Mortem { get; set; }
        public virtual Catalog.Item.TbItem Item { get; set; } = null!;
        public virtual Warehouse.TbWarehouse Warehouse { get; set; } = null!;
        public virtual Catalog.Item.ItemAttributes.TbItemAttributeCombinationPricing? ItemAttributeCombinationPricing { get; set; }
    }
}
