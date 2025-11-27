using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Entities.Inventory
{
    public class TbMortem : BaseEntity
    {
        [Required]
        [MaxLength(50)]
        public string DocumentNumber { get; set; } = string.Empty;

        public DateTime DocumentDate { get; set; } = DateTime.UtcNow;

        [MaxLength(500)]
        public string? Reason { get; set; }

        [ForeignKey("Order")]
        public Guid? OrderId { get; set; }

        [ForeignKey("User")]
        public Guid? UserId { get; set; }

        public decimal TotalAmount { get; set; }

        public int Status { get; set; } // 0: Pending, 1: Approved, 2: Rejected

        public virtual Order.TbOrder? Order { get; set; }
        public virtual ApplicationUser? User { get; set; }
        public virtual ICollection<TbMovitemsdetail>? MovitemsDetails { get; set; }
    }
}
