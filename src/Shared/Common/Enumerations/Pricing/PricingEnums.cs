namespace Common.Enumerations.Pricing
{
    /// <summary>
    /// Customer segment types for pricing
    /// </summary>
    public enum CustomerSegmentType
    {
        /// <summary>
        /// Business to Consumer (regular customers)
        /// </summary>
        B2C = 1,

        /// <summary>
        /// Business to Business (wholesale customers)
        /// </summary>
        B2B = 2,

        /// <summary>
        /// VIP customers
        /// </summary>
        VIP = 3,

        /// <summary>
        /// Loyalty program members
        /// </summary>
        LoyaltyMember = 4,

        /// <summary>
        /// First-time buyers
        /// </summary>
        FirstTimeBuyer = 5,

        /// <summary>
        /// Bulk buyers
        /// </summary>
        BulkBuyer = 6
    }
}
