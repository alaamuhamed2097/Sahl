using Common.Enumerations.Fulfillment;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Entities.Fulfillment
{
    public class TbFulfillmentFee : BaseEntity
    {
        [Required]
        [ForeignKey("FulfillmentMethod")]
        public Guid FulfillmentMethodId { get; set; }

        [Required]
        public FulfillmentFeeType FeeType { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal BaseFee { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal? PercentageFee { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? MinimumFee { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? MaximumFee { get; set; }

        [Column(TypeName = "decimal(10,3)")]
        public decimal? WeightBasedFeePerKg { get; set; }

        [Column(TypeName = "decimal(10,3)")]
        public decimal? VolumeBasedFeePerCubicMeter { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime? EffectiveFrom { get; set; }

        public DateTime? EffectiveTo { get; set; }

        [StringLength(500)]
        public string? DescriptionEn { get; set; }

        [StringLength(500)]
        public string? DescriptionAr { get; set; }

        public virtual TbFulfillmentMethod FulfillmentMethod { get; set; } = null!;
    }
}
