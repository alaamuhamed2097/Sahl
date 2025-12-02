using Common.Enumerations.Offer;
using Domains.Entities.BuyBox;
using Domains.Entities.Catalog.Item;
using Domains.Entities.Offer.Rating;
using Domains.Entities.Offer.Warranty;
using Domains.Entities.Shipping;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Entities.Offer
{
    public class TbOffer : BaseEntity
    {
        // Required properties
        public Guid ItemId { get; set; }
        public string UserId { get; set; } = null!;
        public StorgeLocation StorgeLocation { get; set; }
        public int HandlingTimeInDays { get; set; }
        public OfferVisibilityScope VisibilityScope { get; set; }

        // Optional properties
        public Guid? WarrantyId { get; set; }
        public Guid? OfferConditionId { get; set; }

        // Navigation properties
        [ForeignKey("ItemId")]
        public virtual TbItem Item { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }
        [ForeignKey("WarrantyId")]
        public virtual TbWarranty Warranty { get; set; }
        [ForeignKey("OfferConditionId")]
        public virtual TbOfferCondition OfferCondition { get; set; }

        // Collections
        public virtual ICollection<TbUserOfferRating> UserOfferRatings { get; set; }
        public virtual ICollection<TbShippingDetail> ShippingDetails { get; set; }
        public virtual ICollection<TbOfferCombinationPricing> OfferCombinationPricings { get; set; }
        public virtual ICollection<TbOfferStatusHistory> OfferStatusHistories { get; set; }
        public virtual ICollection<TbBuyBoxCalculation> BuyBoxCalculations { get; set; }
    }
}