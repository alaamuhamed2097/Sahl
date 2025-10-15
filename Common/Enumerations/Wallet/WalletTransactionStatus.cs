using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Enumerations.Wallet
{
    public enum WalletTransactionStatus
    {
        Pending = 1,
        Rejected = 2,
        Accepted = 3,
        Received = 4,
    }
}
