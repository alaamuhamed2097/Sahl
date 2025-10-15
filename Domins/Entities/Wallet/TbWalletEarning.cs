using Common.Enumerations.Wallet;
using Domains.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domains.Entities.Wallet
{
    public class TbWalletEarning : BaseEntity
    {
        public Guid TransactionId { get; set; }
        public Guid SourceId { get; set; } // CommissionId - GiftyId  - BusinessPointsId
        public WalletEarningType Type { get; set; }

        public virtual TbWalletTransaction? WalletTransaction { get; set; }
    }
}
