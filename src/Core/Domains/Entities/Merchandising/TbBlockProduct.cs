using Domains.Entities.Base;
using Domains.Entities.Catalog.Item;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Entities.Merchandising
{
    public class TbBlockProduct : BaseEntity
    {
        [Required]
        [ForeignKey("HomepageBlock")]
        public Guid HomepageBlockId { get; set; }

        [Required]
        [ForeignKey("Item")]
        public Guid ItemId { get; set; }

        public int DisplayOrder { get; set; }

        public bool IsActive { get; set; } = true;

        public bool IsFeatured { get; set; }

        [StringLength(100)]
        public string? BadgeText { get; set; }

        [StringLength(50)]
        public string? BadgeColor { get; set; }

        public DateTime? FeaturedFrom { get; set; }

        public DateTime? FeaturedTo { get; set; }

        public virtual TbHomepageBlock HomepageBlock { get; set; } = null!;
        public virtual TbItem Item { get; set; } = null!;
    }
}
