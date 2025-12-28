namespace Common.Enumerations.Order
{
    /// <summary>
    /// Refund status enum
    /// </summary>
    public enum RefundStatus
    {
        Pending = 0,
        Approved = 1,
        Rejected = 2,
        Completed = 3,
        Failed = 4  // NEW - for payment gateway failures
    }
}
