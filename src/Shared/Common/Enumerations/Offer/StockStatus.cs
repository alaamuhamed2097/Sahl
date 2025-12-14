using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Enumerations.Offer
{
    public enum StockStatus
    {
        Unknown = 0,
        InStock = 1,
        OutOfStock = 2,
        LimitedStock = 3,
        ComingSoon = 4,
        preOrder = 5,
        madeToOrder = 6
    }
}
