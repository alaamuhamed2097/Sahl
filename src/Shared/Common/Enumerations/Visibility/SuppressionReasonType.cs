namespace Common.Enumerations.Visibility
{

    /// <summary>
    /// Reasons for product suppression
    /// </summary>
    public enum SuppressionReasonType
    {
        /// <summary>
        /// All offers are out of stock
        /// </summary>
        NoStock = 1,

        /// <summary>
        /// No active offers available
        /// </summary>
        NoActiveOffers = 2,

        /// <summary>
        /// All sellers are suspended
        /// </summary>
        AllSellersSuspended = 3,

        /// <summary>
        /// Policy violation detected
        /// </summary>
        PolicyViolation = 4,

        /// <summary>
        /// Quality issues reported
        /// </summary>
        QualityIssues = 5,

        /// <summary>
        /// Duplicate content detected
        /// </summary>
        DuplicateContent = 6,

        /// <summary>
        /// Missing required information
        /// </summary>
        MissingRequiredInfo = 7,

        /// <summary>
        /// Invalid or incomplete category
        /// </summary>
        InvalidCategory = 8,

        /// <summary>
        /// Product not approved yet
        /// </summary>
        NotApproved = 9,

        /// <summary>
        /// Copyright infringement
        /// </summary>
        CopyrightIssue = 10,

        /// <summary>
        /// Prohibited item
        /// </summary>
        ProhibitedItem = 11,

        /// <summary>
        /// Manual suppression by admin
        /// </summary>
        ManualSuppression = 12
    }
}
