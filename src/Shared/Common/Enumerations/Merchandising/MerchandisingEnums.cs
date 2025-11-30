namespace Common.Enumerations.Merchandising
{
    /// <summary>
    /// Homepage block types for merchandising
    /// </summary>
    public enum HomepageBlockType
    {
        /// <summary>
        /// Flash deals - time-limited deep discounts
        /// </summary>
        FlashDeals = 1,

        /// <summary>
        /// Seasonal campaigns (Ramadan, White Friday, etc.)
        /// </summary>
        SeasonalCampaign = 2,

        /// <summary>
        /// Price-based filtering (Under 200, 500-1000, Over 1000)
        /// </summary>
        PriceBased = 3,

        /// <summary>
        /// Category-based (Electronics, Fashion, Home)
        /// </summary>
        CategoryBased = 4,

        /// <summary>
        /// Trending products (most viewed, most sold)
        /// </summary>
        Trending = 5,

        /// <summary>
        /// New arrivals - recently added products
        /// </summary>
        NewArrivals = 6,

        /// <summary>
        /// Recommended for you - personalized suggestions
        /// </summary>
        Recommended = 7,

        /// <summary>
        /// Top rated - 4.5+ stars
        /// </summary>
        TopRated = 8,

        /// <summary>
        /// Best sellers
        /// </summary>
        BestSellers = 9,

        /// <summary>
        /// Deals of the day
        /// </summary>
        DealsOfTheDay = 10,

        /// <summary>
        /// Brand showcase
        /// </summary>
        BrandShowcase = 11,

        /// <summary>
        /// Recently viewed by customer
        /// </summary>
        RecentlyViewed = 12,

        /// <summary>
        /// Frequently bought together
        /// </summary>
        FrequentlyBoughtTogether = 13,

        /// <summary>
        /// Custom curated collection
        /// </summary>
        CustomCollection = 14
    }
}
