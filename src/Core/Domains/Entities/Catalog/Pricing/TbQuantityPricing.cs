using Domains.Entities.Offer;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Entities.Catalog.Pricing
{
    public class TbQuantityPricing : BaseEntity
    {
        [Required]
        [ForeignKey("Offer")]
        public Guid OfferId { get; set; }

        [Required]
        public int MinimumQuantity { get; set; }

        public int? MaximumQuantity { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal DiscountPercentage { get; set; }

        public bool IsActive { get; set; } = true;

        public int DisplayOrder { get; set; }

        public virtual TbOffer Offer { get; set; } = null!;
    }
}
