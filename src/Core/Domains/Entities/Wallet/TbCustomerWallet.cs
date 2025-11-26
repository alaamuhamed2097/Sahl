using Common.Enumerations.Wallet;
using Domains.Entities.Base;
using Domains.Entities.Currency;
using Domains.Entities.ECommerceSystem.Customer;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Entities.Wallet
{
    public class TbCustomerWallet : BaseEntity
    {
        [Required]
        [ForeignKey("Customer")]
        public Guid CustomerId { get; set; }

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

        public virtual TbCustomer Customer { get; set; } = null!;
        public virtual TbCurrency Currency { get; set; } = null!;
        public ICollection<TbWalletTransaction> Transactions { get; set; } = new HashSet<TbWalletTransaction>();
    }
}
