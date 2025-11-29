using Common.Enumerations.Visibility;
using Domains.Entities.Catalog.Item;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Entities.Visibility
{
    public class TbProductVisibilityRule : BaseEntity
    {
        [Required]
        [ForeignKey("Item")]
        public Guid ItemId { get; set; }

        [Required]
        public ProductVisibilityStatus VisibilityStatus { get; set; }

        public bool IsVisible { get; set; } = true;

        public bool HasActiveOffers { get; set; }

        public bool HasStock { get; set; }

        public bool IsApproved { get; set; }

        public bool HasValidCategory { get; set; }

        public bool AllSellersActive { get; set; }

        public DateTime? LastCheckedAt { get; set; }

        public DateTime? SuppressedAt { get; set; }

        [ForeignKey("SuppressedByUser")]
        public string? SuppressedByUserId { get; set; }

        public virtual TbItem Item { get; set; } = null!;
        public virtual ApplicationUser? SuppressedByUser { get; set; }
        public ICollection<TbSuppressionReason> SuppressionReasons { get; set; } = new HashSet<TbSuppressionReason>();
    }
}
