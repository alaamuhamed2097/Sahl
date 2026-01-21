using Common.Enumerations.Wallet.Customer;
using Domains.Entities.Order.Payment;
using Shared.Common.Enumerations.Wallet;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Entities.Wallet.Customer
{
    public class TbCustomerWalletTransaction : BaseEntity
    {
        [ForeignKey("Wallet")]
        public Guid WalletId { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal FeeAmount { get; set; } = 0m;

        public WalletTransactionDirection Direction { get; set; }
        public WalletTransactionType TransactionType { get; set; }
        public WalletTransactionStatus TransactionStatus { get; set; } // Pending, Completed, Failed

        // Reference External Entities
        public Guid ReferenceId { get; set; }
        public string ReferenceType { get; set; } = string.Empty; // "Order", "Refund", "Deposit"

        public virtual TbCustomerWallet Wallet { get; set; } = null!;
        public virtual TbOrderPayment? WalletTransaction { get; set; }
    }
}
