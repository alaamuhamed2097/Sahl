using Domains.Entities.BuyBox;
using Domains.Entities.Catalog.Item;
using Domains.Entities.Offer.Rating;
using Domains.Entities.Offer.Warranty;
using Domains.Entities.Shipping;

namespace Domains.Entities.Offer
{
    public class TbOffer : BaseEntity
    {
        // Required properties
        public Guid ItemId { get; set; }
        public Guid UserId { get; set; } 
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string Condition { get; set; }
        public bool IsActive { get; set; } = true;

        // Optional properties
        public decimal? OriginalPrice { get; set; }
        public decimal ShippingCost { get; set; }
        public decimal? SellerRating { get; set; }
        public Guid? WarrantyId { get; set; }
        public Guid? OfferConditionId { get; set; }

        // Navigation properties
        public virtual TbItem Item { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual TbWarranty Warranty { get; set; }

        // Collections
        public virtual ICollection<TbUserOfferRating> UserOfferRatings { get; set; }
        public virtual ICollection<TbShippingDetail> ShippingDetails { get; set; }
        public virtual ICollection<TbOfferCombinationPricing> OfferCombinationPricings { get; set; }
        public virtual ICollection<TbBuyBoxCalculation> BuyBoxCalculations { get; set; }
    }
}