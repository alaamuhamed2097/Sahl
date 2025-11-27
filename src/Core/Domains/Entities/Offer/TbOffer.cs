using Domains.Entities.Catalog.Item;
using Domains.Entities.Offer.Rating;
using Domains.Entities.Offer.Warranty;
using Domains.Entities.Shipping;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Entities.Offer
{
    public class TbOffer : BaseEntity
    {
        [Required]
        public Guid ItemId { get; set; }

        [Required]
        public string UserId { get; set; }

        public Guid? OfferConditionId { get; set; }

        public Guid? WarrantyId { get; set; }

        public int StorageLocation { get; set; }

        public int HandlingTimeDays { get; set; }

        public bool IsProtectingCorrections { get; set; }

        public int VisibilityScope { get; set; }

        [ForeignKey("ItemId")]
        public virtual TbItem Item { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }

        [ForeignKey("OfferConditionId")]
        public virtual TbOfferCondition OfferCondition { get; set; }

        [ForeignKey("WarrantyId")]
        public virtual TbWarranty Warranty { get; set; }

        public virtual ICollection<TbUserOfferRating> UserOfferRatings { get; set; }
        public virtual ICollection<TbShippingDetail> ShippingDetails { get; set; }
        public virtual ICollection<TbOfferCombinationPricing> OfferCombinationPricings { get; set; }
    }
}
