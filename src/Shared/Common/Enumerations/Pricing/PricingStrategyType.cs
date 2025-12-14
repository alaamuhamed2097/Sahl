namespace Common.Enumerations.Pricing
{
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
