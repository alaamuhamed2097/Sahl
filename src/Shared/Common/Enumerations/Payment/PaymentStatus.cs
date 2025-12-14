namespace Common.Enumerations.Payment
{
    public enum PaymentStatus
    {
        Pending = 1,
        Completed = 2,  // Note: Previously called "Paid", renamed to "Completed"
        Failed = 3,
        Cancelled = 4,
        Refunded = 5,
        PartiallyRefunded = 6,
        Disputed = 7,
        
        // Backward compatibility aliases (deprecated, will be removed in v2)
        Paid = 2  // Maps to Completed
    }
}