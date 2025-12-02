using Domains.Entities.Catalog.Item;
using Domains.Entities.Offer;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Entities.ECommerceSystem.Cart
{
    public class TbShoppingCartItem : BaseEntity
    {
        [ForeignKey("ShoppingCart")]
        public Guid ShoppingCartId { get; set; }

        [ForeignKey("Item")]
        public Guid ItemId { get; set; }

        [ForeignKey("Offer")]
        public Guid OfferId { get; set; }

        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        public virtual TbShoppingCart ShoppingCart { get; set; } = null!;
        public virtual TbItem Item { get; set; } = null!;
        public virtual TbOffer Offer { get; set; } = null!;
    }
}
