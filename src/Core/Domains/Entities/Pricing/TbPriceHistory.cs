using Domains.Entities.Base;
using Domains.Entities.Offer;
using Domains.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Entities.Pricing
{
    public class TbPriceHistory : BaseEntity
    {
        [Required]
        [ForeignKey("Offer")]
        public Guid OfferId { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal OldPrice { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal NewPrice { get; set; }

        [Required]
        [Column(TypeName = "decimal(5,2)")]
        public decimal ChangedPercentage { get; set; }

        [Required]
        public DateTime ChangedAt { get; set; }

        [ForeignKey("ChangedByUser")]
        public string? ChangedByUserId { get; set; }

        [StringLength(500)]
        public string? Reason { get; set; }

        public bool IsAutomatic { get; set; }

        public virtual TbOffer Offer { get; set; } = null!;
        public virtual ApplicationUser? ChangedByUser { get; set; }
    }
}
