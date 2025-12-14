namespace Common.Enumerations.Pricing
{
    /// <summary>
    /// Pricing system types that can be enabled per category / globally
    /// Matches database values used in migrations/SQL (0 = Standard)
    /// </summary>
    public enum PricingSystemType
    {
        /// <summary>
        /// Standard pricing (price + quantity)
        /// </summary>
        Standard = 0,

        /// <summary>
        /// Combination-based pricing (variants / combinations only)
        /// </summary>
        Combination = 1,

        /// <summary>
        /// Quantity tiers (tiered pricing)
        /// </summary>
        Quantity = 2,

        /// <summary>
        /// Combination + Quantity (hybrid)
        /// </summary>
        CombinationWithQuantity = 3,

        /// <summary>
        /// Customer segment pricing (by customer group)
        /// </summary>
        CustomerSegmentPricing = 4
    }
}
