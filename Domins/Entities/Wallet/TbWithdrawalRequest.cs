using Domains.Entities.Base;
using Domains.Entities.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domains.Entities.Wallet
{
    public class TbWithdrawalRequest : BaseEntity
    {
        public Guid TransactionId { get; set; }
        public Guid PaymentMethodId { get; set; }
        public string? RejectionReason { get; set; }

        public virtual TbWalletTransaction? WalletTransaction { get; set; }
        public virtual TbUserPaymentMethod? UserPaymentMethod { get; set; }
    }
}
