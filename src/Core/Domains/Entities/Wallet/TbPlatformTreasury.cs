using Domains.Entities.Base;
using Domains.Entities.Currency;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Entities.Wallet
{
    public class TbPlatformTreasury : BaseEntity
    {
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalBalance { get; set; } = 0m;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal CustomerWalletsTotal { get; set; } = 0m;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal VendorWalletsTotal { get; set; } = 0m;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PendingCommissions { get; set; } = 0m;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal CollectedCommissions { get; set; } = 0m;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PendingPayouts { get; set; } = 0m;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal ProcessedPayouts { get; set; } = 0m;

        // New properties expected by WalletService
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalRevenue { get; set; } = 0m;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalCommissions { get; set; } = 0m;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalRefunds { get; set; } = 0m;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPayouts { get; set; } = 0m;

        [Required]
        public DateTime LastUpdatedUtc { get; set; } = DateTime.UtcNow;

        [Required]
        [ForeignKey("Currency")]
        public Guid CurrencyId { get; set; }

        public DateTime? LastReconciliationDate { get; set; }

        [StringLength(500)]
        public string? Notes { get; set; }

        public virtual TbCurrency Currency { get; set; } = null!;
    }
}
