namespace Common.Enumerations.Order
{
    /// <summary>
    /// Order progress status stored on TbOrder.OrderStatus column
    /// Values intentionally start at 0 to allow DB default 0 = Pending
    /// </summary>
    public enum OrderProgressStatus
    {
        Pending = 0,
        Processing = 1,
        Shipped = 2,
        Delivered = 3,
        Cancelled = 4
    }
}
