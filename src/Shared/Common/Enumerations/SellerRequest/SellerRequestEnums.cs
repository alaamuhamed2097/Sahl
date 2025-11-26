namespace Common.Enumerations.SellerRequest
{
    /// <summary>
    /// Types of seller requests
    /// </summary>
    public enum SellerRequestType
    {
        /// <summary>
        /// Campaign participation request
        /// </summary>
        CampaignParticipation = 1,

        /// <summary>
        /// Promo code participation request
        /// </summary>
        PromoCodeParticipation = 2,

        /// <summary>
        /// New product creation request
        /// </summary>
        NewProductCreation = 3,

        /// <summary>
        /// Product content edit request
        /// </summary>
        ProductContentEdit = 4,

        /// <summary>
        /// Wallet withdrawal request
        /// </summary>
        WalletWithdrawal = 5,

        /// <summary>
        /// Selling limit increase request
        /// </summary>
        SellingLimitIncrease = 6,

        /// <summary>
        /// Suspension appeal request
        /// </summary>
        SuspensionAppeal = 7,

        /// <summary>
        /// Quality check request
        /// </summary>
        QualityCheck = 8,

        /// <summary>
        /// Brand registration request
        /// </summary>
        BrandRegistration = 9,

        /// <summary>
        /// FBM enrollment request
        /// </summary>
        FBMEnrollment = 10,

        /// <summary>
        /// Tier upgrade request
        /// </summary>
        TierUpgrade = 11,

        /// <summary>
        /// Featured placement request
        /// </summary>
        FeaturedPlacement = 12,

        /// <summary>
        /// General support request
        /// </summary>
        GeneralSupport = 13
    }

    /// <summary>
    /// Status of seller request
    /// </summary>
    public enum SellerRequestStatus
    {
        /// <summary>
        /// Draft - not submitted yet
        /// </summary>
        Draft = 1,

        /// <summary>
        /// Pending review
        /// </summary>
        Pending = 2,

        /// <summary>
        /// Under review by admin
        /// </summary>
        UnderReview = 3,

        /// <summary>
        /// Needs more information
        /// </summary>
        NeedsMoreInfo = 4,

        /// <summary>
        /// Approved
        /// </summary>
        Approved = 5,

        /// <summary>
        /// Rejected
        /// </summary>
        Rejected = 6,

        /// <summary>
        /// Completed successfully
        /// </summary>
        Completed = 7,

        /// <summary>
        /// Cancelled by seller
        /// </summary>
        Cancelled = 8
    }
}
