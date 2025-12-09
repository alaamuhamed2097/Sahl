using Common.Enumerations.Fulfillment;
using Common.Enumerations.Offer;
using Domains.Entities.BuyBox;
using Domains.Entities.Catalog.Item;
using Domains.Entities.ECommerceSystem.Vendor;
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
        public Guid VendorId { get; set; }
        public StorgeLocation StorgeLocation { get; set; }
        public int HandlingTimeInDays { get; set; }
        public OfferVisibilityScope VisibilityScope { get; set; }
        
        // Fulfillment type (Marketplace/FBS or Seller/FBM)
        public FulfillmentType FulfillmentType { get; set; } = FulfillmentType.Seller;

        // Optional properties
        public Guid? WarrantyId { get; set; }
        public Guid? OfferConditionId { get; set; }

        // Navigation properties
        [ForeignKey("ItemId")]
        public virtual TbItem Item { get; set; }
        [ForeignKey("VendorId")]
        public virtual TbVendor Vendor { get; set; }
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