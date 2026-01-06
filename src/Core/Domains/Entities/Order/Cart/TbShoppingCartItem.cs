using Domains.Entities.Catalog.Item;
using Domains.Entities.Offer;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Entities.Order.Cart
{
    public class TbShoppingCartItem : BaseEntity
    {
        [ForeignKey("ShoppingCart")]
        public Guid ShoppingCartId { get; set; }

        [ForeignKey("Item")]
        public required Guid ItemId { get; set; }

        /// <summary>
        /// Stores the OfferCombinationPricingId - this is the exact pricing ID for a specific offer + item combination
        /// </summary>
        [ForeignKey("OfferCombinationPricing")]
        public Guid OfferCombinationPricingId { get; set; }

        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        public virtual TbShoppingCart ShoppingCart { get; set; } = null!;
        public virtual TbItem Item { get; set; } = null!;
        public virtual TbOfferCombinationPricing OfferCombinationPricing { get; set; } = null!;
    }
}

