using Domains.Entities.Catalog.Item;
using Domains.Entities.ECommerceSystem.Review;
using Domains.Entities.ECommerceSystem.Vendor;
using Domains.Entities.Offer;
using Domains.Entities.Order.Refund;
using Domains.Entities.Order.Returns;
using Domains.Entities.Order.Shipping;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Entities.Order
{
    public class TbOrderDetail : BaseEntity
    {
        public int Quantity { get; set; } = 1;
        public decimal UnitPrice { get; set; }
        public decimal SubTotal { get; set; }

        // Foreign Keys
        [ForeignKey("Order")]
        public Guid OrderId { get; set; }

        [ForeignKey("Item")]
        public Guid ItemId { get; set; }

        [ForeignKey("OfferCombinationPricing")]
        public Guid OfferCombinationPricingId { get; set; }

        [ForeignKey("Vendor")]
        public Guid VendorId { get; set; }

        public Guid WarehouseId { get; set; }

        // Price breakdowns
        public decimal DiscountAmount { get; set; } = 0m;
        public decimal TaxAmount { get; set; } = 0m;

        // Navigation Properties
        public virtual TbOrder Order { get; set; } = null!;
        public virtual TbItem Item { get; set; } = null!;
        public virtual TbOfferCombinationPricing OfferCombinationPricing { get; set; } = null!;
        public virtual TbVendor Vendor { get; set; } = null!;
        public virtual TbOrderShipmentItem? OrderShipmentItem { get; set; }
        public virtual ICollection<TbVendorReview> ItemReviews { get; set; } = new List<TbVendorReview>();
        public virtual ICollection<TbRefund> Refunds { get; set; } = new List<TbRefund>();
    }
}