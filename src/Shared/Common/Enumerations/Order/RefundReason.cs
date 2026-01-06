namespace Common.Enumerations.Order
{
    public enum RefundReason
    {
        DefectiveProduct = 1,
        WrongItemShipped = 2,
        ItemNotAsDescribed = 3,
        DamagedDuringShipping = 4,
        ChangedMind = 5,
        OrderedByMistake = 6,
        BetterPriceFound = 7,
        LateDelivery = 8,
        MissingParts = 9,
        QualityNotSatisfactory = 10,
        Other = 99
    }
}