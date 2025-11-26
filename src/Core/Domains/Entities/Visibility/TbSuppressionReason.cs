using Common.Enumerations.Visibility;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Entities.Visibility
{
    public class TbSuppressionReason : BaseEntity
    {
        [Required]
        [ForeignKey("ProductVisibilityRule")]
        public Guid ProductVisibilityRuleId { get; set; }

        [Required]
        public SuppressionReasonType ReasonType { get; set; }

        [Required]
        [StringLength(500)]
        public string ReasonDescriptionEn { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string ReasonDescriptionAr { get; set; } = string.Empty;

        public DateTime DetectedAt { get; set; }

        public DateTime? ResolvedAt { get; set; }

        public bool IsResolved { get; set; }

        [StringLength(1000)]
        public string? ResolutionNotes { get; set; }

        public virtual TbProductVisibilityRule ProductVisibilityRule { get; set; } = null!;
    }
}
