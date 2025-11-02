using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Enumerations.Payment
{
    public enum PaymentGatewayMethod
    {
        CreditCard=1,
        BankTransfer=2,
        Cash=3,
        MobileWallet = 4
    }
}
