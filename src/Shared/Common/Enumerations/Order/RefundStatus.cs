using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Enumerations.Order
{
    public enum RefundStatus
    {
        NotActive=0,
        Pending = 1,
        Approved = 2,
        Rejected = 3
    }
}
