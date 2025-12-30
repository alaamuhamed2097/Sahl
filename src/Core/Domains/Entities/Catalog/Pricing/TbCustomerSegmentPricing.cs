using Common.Enumerations.Pricing;
using Domains.Entities.Base;
using Domains.Entities.Offer;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Entities.Catalog.Pricing
{
    public class TbCustomerSegmentPricing : BaseEntity
    {
        [Required]
        [ForeignKey("Offer")]
        public Guid OfferId { get; set; }

        [Required]
        public CustomerSegmentType SegmentType { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal? DiscountPercentage { get; set; }

        [Required]
        public int MinimumOrderQuantity { get; set; } = 1;

        public bool IsActive { get; set; } = true;

        public DateTime? EffectiveFrom { get; set; }

        public DateTime? EffectiveTo { get; set; }

        public virtual TbOffer Offer { get; set; } = null!;
    }
}
