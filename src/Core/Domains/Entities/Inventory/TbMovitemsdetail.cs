using Domains.Entities.Catalog.Item;
using Domains.Entities.Catalog.Item.ItemAttributes;
using Domains.Entities.Warehouse;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Entities.Inventory
{
    public class TbMovitemsdetail : BaseEntity
    {
        public Guid? MoitemId { get; set; }

        public Guid? MortemId { get; set; }

        public Guid ItemId { get; set; }

        public Guid WarehouseId { get; set; }

        public Guid? ItemCombinationId { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal TotalPrice { get; set; }

        [ForeignKey("MoitemId")]
        public virtual TbMoitem? Moitem { get; set; }
        [ForeignKey("MortemId")]
        public virtual TbMortem? Mortem { get; set; }
        [ForeignKey("ItemId")]
        public virtual TbItem Item { get; set; } = null!;
        [ForeignKey("WarehouseId")]
        public virtual TbWarehouse Warehouse { get; set; } = null!;
        [ForeignKey("ItemCombinationId")]
        public virtual TbItemCombination? ItemCombination { get; set; }
    }
}
