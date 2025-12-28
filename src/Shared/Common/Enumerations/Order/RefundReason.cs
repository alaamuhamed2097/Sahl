namespace Common.Enumerations.Order
{
    /// <summary>
    /// Refund reason enum
    /// </summary>
    public enum RefundReason
    {
        DefectiveProduct = 1,
        WrongItemReceived = 2,
        ItemNotAsDescribed = 3,
        ChangedMind = 4,
        OrderCancellation = 5,
        LateDelivery = 6,
        DamagedInShipping = 7,
        Other = 99
    }
}
