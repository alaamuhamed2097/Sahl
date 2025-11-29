using Common.Enumerations.Wallet;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Entities.Wallet
{
    public class TbWalletTransaction : BaseEntity
    {
        [ForeignKey("CustomerWallet")]
        public Guid? CustomerWalletId { get; set; }

        [ForeignKey("VendorWallet")]
        public Guid? VendorWalletId { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Required]
        public WalletTransactionType TransactionType { get; set; }

        [Required]
        public WalletTransactionStatus Status { get; set; }

        [Required]
        [StringLength(200)]
        public string DescriptionEn { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string DescriptionAr { get; set; } = string.Empty;

        [ForeignKey("Order")]
        public Guid? OrderId { get; set; }

        [ForeignKey("Refund")]
        public Guid? RefundId { get; set; }

        [StringLength(100)]
        public string? ReferenceNumber { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? BalanceBefore { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? BalanceAfter { get; set; }

        [StringLength(500)]
        public string? Notes { get; set; }

        public DateTime? ProcessedDate { get; set; }

        [ForeignKey("ProcessedByUser")]
        public string? ProcessedByUserId { get; set; }

        public virtual TbCustomerWallet? CustomerWallet { get; set; }
        public virtual TbVendorWallet? VendorWallet { get; set; }
        public virtual TbOrder? Order { get; set; }
        public virtual TbRefundRequest? Refund { get; set; }
        public virtual ApplicationUser? ProcessedByUser { get; set; }
    }
}
