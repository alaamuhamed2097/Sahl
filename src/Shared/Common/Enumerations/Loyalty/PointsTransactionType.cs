namespace Common.Enumerations.Loyalty
{
    /// <summary>
    /// Points transaction types
    /// </summary>
    public enum PointsTransactionType
    {
        /// <summary>
        /// Earned from purchase
        /// </summary>
        EarnedFromPurchase = 1,

        /// <summary>
        /// Earned from review
        /// </summary>
        EarnedFromReview = 2,

        /// <summary>
        /// Earned from referral
        /// </summary>
        EarnedFromReferral = 3,

        /// <summary>
        /// Earned from profile completion
        /// </summary>
        EarnedFromProfileCompletion = 4,

        /// <summary>
        /// Earned from campaign
        /// </summary>
        EarnedFromCampaign = 5,

        /// <summary>
        /// Redeemed for discount
        /// </summary>
        RedeemedForDiscount = 6,

        /// <summary>
        /// Redeemed for product
        /// </summary>
        RedeemedForProduct = 7,

        /// <summary>
        /// Expired
        /// </summary>
        Expired = 8,

        /// <summary>
        /// Adjusted by admin
        /// </summary>
        AdminAdjustment = 9,

        /// <summary>
        /// Bonus points
        /// </summary>
        Bonus = 10
    }
}
