namespace Common.Enumerations.Payment
{
    public enum PaymentStatus
    {
        Pending = 0,
        Processing = 1,    // ✅ NEW
        Paid = 2,
        Completed = 2,     // Alias
        Failed = 3,
        Cancelled = 4,
        Refunded = 5,
        PartiallyRefunded = 6,
        Disputed = 7
    }
}