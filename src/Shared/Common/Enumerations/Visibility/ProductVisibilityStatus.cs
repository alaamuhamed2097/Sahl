namespace Common.Enumerations.Visibility
{
    /// <summary>
    /// Product visibility status
    /// </summary>
    public enum ProductVisibilityStatus
    {
        /// <summary>
        /// Product is visible to customers
        /// </summary>
        Visible = 1,

        /// <summary>
        /// Product is hidden from search and browse
        /// </summary>
        Hidden = 2,

        /// <summary>
        /// Product is suppressed due to policy violation
        /// </summary>
        Suppressed = 3,

        /// <summary>
        /// Product is pending approval
        /// </summary>
        PendingApproval = 4,

        /// <summary>
        /// Product is temporarily unavailable
        /// </summary>
        TemporarilyUnavailable = 5
    }
}
