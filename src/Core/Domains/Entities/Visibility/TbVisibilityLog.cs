using Domains.Entities.Catalog.Item;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Entities.Visibility
{
    public class TbVisibilityLog : BaseEntity
    {
        [Required]
        [ForeignKey("Item")]
        public Guid ItemId { get; set; }

        [Required]
        public bool WasVisible { get; set; }

        [Required]
        public bool IsVisible { get; set; }

        [Required]
        public DateTime ChangedAt { get; set; }

        [StringLength(500)]
        public string? ChangeReason { get; set; }

        public bool IsAutomatic { get; set; }

        [ForeignKey("ChangedByUser")]
        public string? ChangedByUserId { get; set; }

        public virtual TbItem Item { get; set; } = null!;
        public virtual ApplicationUser? ChangedByUser { get; set; }
    }
}
