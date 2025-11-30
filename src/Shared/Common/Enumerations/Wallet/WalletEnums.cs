namespace Common.Enumerations.Wallet
{
    /// <summary>
    /// Wallet transaction types
    /// </summary>
    public enum WalletTransactionType
    {
        // Customer Wallet Transactions
        /// <summary>
        /// Refund from order
        /// </summary>
        CustomerRefund = 1,

        /// <summary>
        /// Cashback from purchase
        /// </summary>
        CustomerCashback = 2,

        /// <summary>
        /// Points converted to cash
        /// </summary>
        CustomerPointsConversion = 3,

        /// <summary>
        /// Promo credit
        /// </summary>
        CustomerPromoCredit = 4,

        /// <summary>
        /// Gift credit
        /// </summary>
        CustomerGiftCredit = 5,

        /// <summary>
        /// Manual deposit
        /// </summary>
        CustomerManualDeposit = 6,

        /// <summary>
        /// Payment for order
        /// </summary>
        CustomerPayment = 7,

        // Added aliases for existing code references
        /// <summary>
        /// Customer deposit (alias for manual deposit)
        /// </summary>
        CustomerDeposit = 6,

        /// <summary>
        /// Customer withdrawal (cash-out)
        /// </summary>
        CustomerWithdrawal = 8,

        // Vendor Wallet Transactions
        /// <summary>
        /// Sales revenue
        /// </summary>
        VendorSalesRevenue = 11,

        /// <summary>
        /// Platform commission deduction
        /// </summary>
        VendorCommissionDeduction = 12,

        /// <summary>
        /// Shipping fee deduction
        /// </summary>
        VendorShippingFeeDeduction = 13,

        /// <summary>
        /// FBM fee deduction
        /// </summary>
        VendorFBMFeeDeduction = 14,

        /// <summary>
        /// Withdrawal
        /// </summary>
        VendorWithdrawal = 15,

        /// <summary>
        /// Refund to customer
        /// </summary>
        VendorRefund = 16,

        /// <summary>
        /// Bonus credit
        /// </summary>
        VendorBonusCredit = 17,

        /// <summary>
        /// Campaign participation fee
        /// </summary>
        VendorCampaignFee = 18,

        /// <summary>
        /// Penalty fee
        /// </summary>
        VendorPenaltyFee = 19,

        /// <summary>
        /// Admin adjustment
        /// </summary>
        AdminAdjustment = 20
    }

    /// <summary>
    /// Wallet transaction status
    /// </summary>
    public enum WalletTransactionStatus
    {
        /// <summary>
        /// Pending processing
        /// </summary>
        Pending = 1,

        /// <summary>
        /// Completed successfully
        /// </summary>
        Completed = 2,

        /// <summary>
        /// Failed
        /// </summary>
        Failed = 3,

        /// <summary>
        /// Cancelled
        /// </summary>
        Cancelled = 4,

        /// <summary>
        /// Under review
        /// </summary>
        UnderReview = 5,

        /// <summary>
        /// Reversed
        /// </summary>
        Reversed = 6
    }
}
