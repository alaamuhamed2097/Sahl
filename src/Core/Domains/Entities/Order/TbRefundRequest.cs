using Common.Enumerations.Order;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Entities.Order
{
    /// <summary>
    /// UPDATED TbRefundRequest entity with all required fields
    /// </summary>
    public class TbRefundRequest : BaseEntity
    {
        // Order Reference
        [Required]
        public Guid OrderId { get; set; }

        [Required]
        [StringLength(450)]
        public string UserId { get; set; } = string.Empty;

        // Refund Reason
        [Required]
        [StringLength(200)]
        public string RefundReason { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? RefundReasonDetails { get; set; }

        // Amounts
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal RequestedAmount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal RefundAmount { get; set; }

        // Status
        [Required]
        public RefundStatus RefundStatus { get; set; } = RefundStatus.Pending;

        // Notes
        [StringLength(1000)]
        public string? CustomerNotes { get; set; }

        [StringLength(450)]
        public string? AdminUserId { get; set; }

        [StringLength(1000)]
        public string? AdminNotes { get; set; }

        // Transaction Info
        [StringLength(200)]
        public string? RefundTransactionId { get; set; }

        [StringLength(500)]
        public string? RefundFailureReason { get; set; }

        // Dates
        public DateTime? ProcessedDate { get; set; }
        public DateTime? RefundCompletedDate { get; set; }

        // Navigation Properties
        [ForeignKey("OrderId")]
        public virtual TbOrder Order { get; set; } = null!;
    }
}
