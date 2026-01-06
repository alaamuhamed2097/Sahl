using Common.Enumerations.Order;
using Domains.Entities.Order.Refund;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Entities.Order.Returns
{
    /// <summary>
    /// TbRefundStatusHistory Entity
    /// </summary>
    public class TbRefundStatusHistory : BaseEntity
    {
        [Required]
        public Guid RefundId { get; set; }

        [Required]
        public RefundStatus OldStatus { get; set; } 

        [Required]
        public RefundStatus NewStatus { get; set; } 

        [StringLength(1000)]
        public string? Notes { get; set; } 

        // Navigation Property
        [ForeignKey("RefundId")]
        public virtual TbRefund Refund { get; set; } = null!;
    }

}
