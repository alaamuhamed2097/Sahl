using Domains.Entities.Catalog.Item;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Entities.ECommerceSystem.Cart
{
    public class TbShoppingCartItem : BaseEntity
    {
        [ForeignKey("ShoppingCart")]
        public Guid ShoppingCartId { get; set; }

        [ForeignKey("Item")]
        public Guid ItemId { get; set; }

        public Guid? ItemCombinationId { get; set; }

        public Guid OfferId { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public bool IsAvailable { get; set; } = true;

        public virtual TbShoppingCart ShoppingCart { get; set; } = null!;
        public virtual TbItem Item { get; set; } = null!;
    }
}
