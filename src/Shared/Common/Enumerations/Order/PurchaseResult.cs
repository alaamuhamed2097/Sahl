namespace Common.Enumerations.Order
{
    public enum PurchaseResult
    {
        Success,
        OrderNotFound,
        PaymentFailed,
        OrderNotAdded,
        InvalidPrice,
        InvalidItemQuantity,
        InvalidItemId,
        InvalidDirectSaleLinkCode,
        InvalidPromoCode,
        SavingOrderFailed
    }
}
