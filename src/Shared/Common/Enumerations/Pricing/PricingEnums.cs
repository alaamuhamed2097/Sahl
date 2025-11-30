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

    /// <summary>
    /// Category-level pricing strategies supported by the platform
    /// </summary>
    public enum PricingStrategyType
    {
        /// <summary>
        /// Simple fixed pricing (single price + quantity)
        /// </summary>
        Simple = 1,

        /// <summary>
        /// Combination-based pricing (attribute/variant based)
        /// </summary>
        CombinationBased = 2,

        /// <summary>
        /// Quantity-based (tiered) pricing
        /// </summary>
        QuantityBased = 3,

        /// <summary>
        /// Hybrid: combination + quantity tiers
        /// </summary>
        Hybrid = 4
    }
}
