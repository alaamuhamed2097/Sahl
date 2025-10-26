using Domains.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Common.Enumerations;
using Common.Enumerations.Payment;
using Domains.Identity;

namespace Domains.Entities.Order
{
    public class TbRefundRequest : BaseEntity
    {
        [Column(TypeName = "decimal(10,2)")]
        public decimal? RefundAmount { get; set; }

        [Required]
        [MaxLength(50)]
        public string UserName { get; set; } = null!;

        [Required]
        [MaxLength(500)]
        public string RefundReason { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string? AdminComments { get; set; }

        public DateTime? ProcessedDate { get; set; }

        public PaymentGatewayMethod? RefundMethod { get; set; }

        [ForeignKey("Admin")]
        public string? AdminUserId { get; set; }

        [ForeignKey("Order")]
        public Guid OrderId { get; set; }

        public virtual TbOrder Order { get; set; } = null!;
        public virtual ApplicationUser Admin { get; set; } = null!;
    }
}
