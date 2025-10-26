using Common.Enumerations.Wallet;
using Domains.Entities.Base;

namespace Domains.Entities.Wallet
{
    public class TbWalletTransaction : BaseEntity
    {
        public Guid WalletId { get; set; }
        public decimal Amount { get; set; }
        public decimal FeeAmount { get; set; } = 0;
        public WalletTransactionType TransactionType { get; set; }  //Withdraw - Transfer - Commision - Gift - Business points
        public WalletTransactionStatus TransactionStatus { get; set; }

        public virtual TbWallet? Wallet { get; set; }
        public virtual ICollection<TbWithdrawalRequest> WithdrawalRequests { get; set; } = new HashSet<TbWithdrawalRequest>();
        public virtual ICollection<TbWalletEarning> WalletEarnings { get; set; } = new HashSet<TbWalletEarning>();
    }
}
