using Domains.Entities.Base;
using Domains.Entities.Items;
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

        public virtual TbOrder Order { get; set; }
        public virtual TbItem Item { get; set; } = null!;
    }
}