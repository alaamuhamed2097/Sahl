using Domains.Entities.Order.Refund;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Entities.Order.Returns
{
    // TbRefundItemVideo Entity
    public class TbRefundItemVideo : BaseEntity
    {
        [Required]
        public Guid RefundId { get; set; }

        [Required]
        [StringLength(500)]
        public string VideoUrl { get; set; } = null!;

        [Required]
        public int DisplayOrder { get; set; } = 0;

        // Navigation Property
        [ForeignKey("RefundId")]
        public virtual TbRefund Refund { get; set; } = null!;
    }
}
