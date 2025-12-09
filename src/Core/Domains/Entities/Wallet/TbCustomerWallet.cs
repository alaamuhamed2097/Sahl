using Domains.Entities.Currency;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Entities.Wallet
{
    public class TbCustomerWallet : BaseEntity
    {
        [Required]
        [ForeignKey("User")]
        public string UserId { get; set; }

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
        public decimal TotalSpent { get; set; } = 0m;

        [Required]
        [ForeignKey("Currency")]
        public Guid CurrencyId { get; set; }

        public DateTime? LastTransactionDate { get; set; }

        public virtual ApplicationUser User { get; set; } = null!;
        public virtual TbCurrency Currency { get; set; } = null!;
        public ICollection<TbWalletTransaction> Transactions { get; set; } = new HashSet<TbWalletTransaction>();
    }
}
