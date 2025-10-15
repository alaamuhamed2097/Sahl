using Domains.Entities.Base;

namespace Domains.Entities.Wallet
{
    public class TbWallet : BaseEntity
    {
        public Guid MarketerId { get; set; }
        public decimal AvailableBalanace { get; set; }
        public decimal PendingBalanace { get; set; }
        public bool IsLooked { get; set; }
        //public AccountPosition AccountPosition { get; set; }

        //public virtual TbMarketer? Marketer { get; set; }

        public virtual ICollection<TbWalletTransaction> WalletTransactions { get; set; } = new HashSet<TbWalletTransaction>();
    }
}
