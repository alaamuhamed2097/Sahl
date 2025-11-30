namespace Common.Enumerations.Campaign
{
    /// <summary>
    /// Types of campaigns
    /// </summary>
    public enum CampaignType
    {
        /// <summary>
        /// White Friday sale
        /// </summary>
        WhiteFriday = 1,

        /// <summary>
        /// Ramadan campaign
        /// </summary>
        Ramadan = 2,

        /// <summary>
        /// Back to school campaign
        /// </summary>
        BackToSchool = 3,

        /// <summary>
        /// Summer sale
        /// </summary>
        SummerSale = 4,

        /// <summary>
        /// Winter sale
        /// </summary>
        WinterSale = 5,

        /// <summary>
        /// New year campaign
        /// </summary>
        NewYear = 6,

        /// <summary>
        /// Mother's Day
        /// </summary>
        MothersDay = 7,

        /// <summary>
        /// Valentine's Day
        /// </summary>
        ValentinesDay = 8,

        /// <summary>
        /// Clearance sale
        /// </summary>
        ClearanceSale = 9,

        /// <summary>
        /// Grand opening campaign
        /// </summary>
        GrandOpening = 10,

        /// <summary>
        /// Anniversary sale
        /// </summary>
        Anniversary = 11,

        /// <summary>
        /// Custom seasonal campaign
        /// </summary>
        Custom = 12
    }

    /// <summary>
    /// Campaign funding models
    /// </summary>
    public enum CampaignFundingModel
    {
        /// <summary>
        /// 100% funded by seller
        /// </summary>
        SellerFunded = 1,

        /// <summary>
        /// 100% funded by platform
        /// </summary>
        PlatformFunded = 2,

        /// <summary>
        /// Shared funding between platform and seller
        /// </summary>
        CoFunded = 3
    }

    /// <summary>
    /// Campaign status used by business logic
    /// </summary>
    public enum CampaignStatus
    {
        /// <summary>
        /// Draft status
        /// </summary>
        Draft = 0,

        /// <summary>
        /// Active status
        /// </summary>
        Active = 1,

        /// <summary>
        /// Paused status
        /// </summary>
        Paused = 2,

        /// <summary>
        /// Completed status
        /// </summary>
        Completed = 3,

        /// <summary>
        /// Cancelled status
        /// </summary>
        Cancelled = 4
    }
}
