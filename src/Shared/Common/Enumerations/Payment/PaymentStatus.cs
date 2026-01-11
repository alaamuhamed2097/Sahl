namespace Common.Enumerations.Payment;

/// <summary>
/// Payment status enum
/// CORRECTED: Removed duplicate Failed value, fixed numbering
/// </summary>
public enum PaymentStatus
{
    /// <summary>
    /// Payment is awaiting processing
    /// </summary>
    Pending = 1,

    /// <summary>
    /// Payment is being processed
    /// </summary>
    Processing = 2,

    /// <summary>
    /// Payment completed successfully
    /// </summary>
    Completed = 3,

    /// <summary>
    /// Payment failed
    /// </summary>
    Failed = 4,

    /// <summary>
    /// Payment was cancelled
    /// </summary>
    Cancelled = 5,

    /// <summary>
    /// Payment was fully refunded
    /// </summary>
    Refunded = 6,

    /// <summary>
    /// Payment was partially refunded
    /// </summary>
    PartiallyRefunded = 7,

    /// <summary>
    /// Payment partially completed (for mixed payment scenarios)
    /// </summary>
    PartiallyPaid = 8
}