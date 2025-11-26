using Common.Enumerations.Wallet;
using Domains.Entities.Base;
using Domains.Entities.Currency;
using Domains.Entities.ECommerceSystem.Vendor;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Entities.Wallet
{
    public class TbVendorWallet : BaseEntity
    {
        [Required]
        [ForeignKey("Vendor")]
        public Guid VendorId { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal AvailableBalance { get; set; } = 0m;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PendingBalance { get; set; } = 0m;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalEarned { get; set; } = 0m;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalWithdrawn { get; set; } = 0m;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalCommissionPaid { get; set; } = 0m;

        [Required]
        [ForeignKey("Currency")]
        public Guid CurrencyId { get; set; }

        public DateTime? LastWithdrawalDate { get; set; }

        public DateTime? LastTransactionDate { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal WithdrawalFeePercentage { get; set; } = 2.5m;

        public virtual TbVendor Vendor { get; set; } = null!;
        public virtual TbCurrency Currency { get; set; } = null!;
        public ICollection<TbWalletTransaction> Transactions { get; set; } = new HashSet<TbWalletTransaction>();
    }
}
