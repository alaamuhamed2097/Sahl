using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Entities.Inventory
{
    public class TbMoitem : BaseEntity
    {
        [Required]
        [MaxLength(50)]
        public string DocumentNumber { get; set; } = string.Empty;

        public DateTime DocumentDate { get; set; } = DateTime.UtcNow;

        [Required]
        [MaxLength(20)]
        public string MovementType { get; set; } = string.Empty; // IN, OUT, TRANSFER

        [MaxLength(500)]
        public string? Notes { get; set; }

        [ForeignKey("User")]
        public Guid? UserId { get; set; }

        public decimal TotalAmount { get; set; }

        public virtual ApplicationUser? User { get; set; }
        public virtual ICollection<TbMovitemsdetail>? MovitemsDetails { get; set; }
    }
}
