using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Entities.Wallet.Customer
{
    public class TbCustomerWallet : BaseEntity
    {
        [Required]
        [ForeignKey("User")]
        public string UserId { get; set; } = null!;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Balance { get; set; } = 0m;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal LockedBalance { get; set; } = 0m;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PendingBalance { get; set; } = 0m;

        public DateTime? LastTransactionDate { get; set; } = DateTime.UtcNow;

        public virtual ApplicationUser User { get; set; } = null!;
        public virtual ICollection<TbCustomerWalletTransaction> Transactions { get; set; } = new HashSet<TbCustomerWalletTransaction>();
    }
}
