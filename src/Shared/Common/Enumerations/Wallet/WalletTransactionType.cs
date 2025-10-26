using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Enumerations.Wallet
{
    public enum WalletTransactionType
    {
        Withdraw = 1,
        Transfer = 2,
        Commission = 3,
        Gift = 4,
        Points = 5
    }
}
