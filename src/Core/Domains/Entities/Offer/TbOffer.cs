using Common.Enumerations.Fulfillment;
using Common.Enumerations.Offer;
using Domains.Entities.Catalog.Item;
using Domains.Entities.ECommerceSystem.Vendor;
using Domains.Entities.Offer.Rating;
using Domains.Entities.Offer.Warranty;
using Domains.Entities.Order.Shipping;
using Domains.Entities.Warehouse;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Entities.Offer
{
    public class TbOffer : BaseEntity
    {
        // Required properties
        public Guid ItemId { get; set; }
        public Guid VendorId { get; set; }
        public Guid WarehouseId { get; set; }

        // Filtering and search optimization
        public int? EstimatedDeliveryDays { get; set; }
        public bool IsFreeShipping { get; set; } = false;
        public OfferVisibilityScope VisibilityScope { get; set; }
        public FulfillmentType FulfillmentType { get; set; } = FulfillmentType.Seller;

        // Optional properties
        public Guid? WarrantyId { get; set; }


        // Navigation properties
        [ForeignKey("ItemId")]
        public virtual TbItem Item { get; set; }
        [ForeignKey("VendorId")]
        public virtual TbVendor Vendor { get; set; }
        [ForeignKey("WarrantyId")]
        public virtual TbWarranty Warranty { get; set; }
        [ForeignKey("WarehouseId")]
        public virtual TbWarehouse Warehouse { get; set; }

        // Collections
        public virtual ICollection<TbUserOfferRating> UserOfferRatings { get; set; }
        public virtual ICollection<TbShippingDetail> ShippingDetails { get; set; }
        public virtual ICollection<TbOfferCombinationPricing> OfferCombinationPricings { get; set; }
        public virtual ICollection<TbOfferStatusHistory> OfferStatusHistories { get; set; }
    }
}