using Domains.Entities.Catalog.Item;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Entities.Merchandising
{
    /// <summary>
    /// Tracks user product views for personalization
    /// </summary>
    public class TbUserItemView : BaseEntity
    {
        [Required]
        public string UserId { get; set; } = string.Empty;

        [Required]
        [ForeignKey("Item")]
        public Guid ItemId { get; set; }

        [Required]
        public DateTime ViewedAt { get; set; }

        public int ViewDurationSeconds { get; set; }

        [StringLength(100)]
        public string? SourceBlockType { get; set; }

        [StringLength(500)]
        public string? ReferrerUrl { get; set; }

        // === Relations ===

        public virtual ApplicationUser User { get; set; } = null!;

        public virtual TbItem Item { get; set; } = null!;
    }
}