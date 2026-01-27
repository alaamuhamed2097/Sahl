namespace Common.Enumerations.Order
{
    public enum OrderProgressStatus
    {
        Pending = 0,
        Confirmed = 1,
        Processing = 2,
        Shipped = 3,
        Delivered = 4,
        Completed = 5,
        Cancelled = 6,
        PaymentFailed = 7,
        RefundRequested = 8,
        Refunded = 9,
        Returned = 10
    }
}
