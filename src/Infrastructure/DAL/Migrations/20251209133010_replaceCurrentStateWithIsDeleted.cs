usin






    Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class replaceCurrentStateWithIsDeleted : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TbWarranties_CurrentState",
                table: "TbWarranties");

            migrationBuilder.DropIndex(
                name: "IX_TbWarehouses_CurrentState",
                table: "TbWarehouses");

            migrationBuilder.DropIndex(
                name: "IX_TbWalletTransactions_CurrentState",
                table: "TbWalletTransactions");

            migrationBuilder.DropIndex(
                name: "IX_TbVisibilityLogs_CurrentState",
                table: "TbVisibilityLogs");

            migrationBuilder.DropIndex(
                name: "IX_TbVideoProviders_CurrentState",
                table: "TbVideoProviders");

            migrationBuilder.DropIndex(
                name: "IX_TbVendorWallets_CurrentState",
                table: "TbVendorWallets");

            migrationBuilder.DropIndex(
                name: "IX_TbVendorTierHistories_CurrentState",
                table: "TbVendorTierHistories");

            migrationBuilder.DropIndex(
                name: "IX_TbVendors_CurrentState",
                table: "TbVendors");

            migrationBuilder.DropIndex(
                name: "IX_TbUserOfferRatings_CurrentState",
                table: "TbUserOfferRatings");

            migrationBuilder.DropIndex(
                name: "IX_TbUserNotifications_CurrentState",
                table: "TbUserNotifications");

            migrationBuilder.DropIndex(
                name: "IX_TbUnits_CurrentState",
                table: "TbUnits");

            migrationBuilder.DropIndex(
                name: "IX_TbUnitConversions_CurrentState",
                table: "TbUnitConversions");

            migrationBuilder.DropIndex(
                name: "IX_TbSuppressionReasons_CurrentState",
                table: "TbSuppressionReasons");

            migrationBuilder.DropIndex(
                name: "IX_TbSupportTickets_CurrentState",
                table: "TbSupportTickets");

            migrationBuilder.DropIndex(
                name: "IX_TbSupportTicketMessages_CurrentState",
                table: "TbSupportTicketMessages");

            migrationBuilder.DropIndex(
                name: "IX_TbStates_CurrentState",
                table: "TbStates");

            migrationBuilder.DropIndex(
                name: "IX_TbShoppingCarts_CurrentState",
                table: "TbShoppingCarts");

            migrationBuilder.DropIndex(
                name: "IX_TbShoppingCartItems_CurrentState",
                table: "TbShoppingCartItems");

            migrationBuilder.DropIndex(
                name: "IX_TbShippingDetails_CurrentState",
                table: "TbShippingDetails");

            migrationBuilder.DropIndex(
                name: "IX_TbShippingCompanies_CurrentState",
                table: "TbShippingCompanies");

            migrationBuilder.DropIndex(
                name: "IX_TbSettings_CurrentState",
                table: "TbSettings");

            migrationBuilder.DropIndex(
                name: "IX_TbSellerTiers_CurrentState",
                table: "TbSellerTiers");

            migrationBuilder.DropIndex(
                name: "IX_TbSellerTierBenefits_CurrentState",
                table: "TbSellerTierBenefits");

            migrationBuilder.DropIndex(
                name: "IX_TbSellerRequests_CurrentState",
                table: "TbSellerRequests");

            migrationBuilder.DropIndex(
                name: "IX_TbSellerPerformanceMetrics_CurrentState",
                table: "TbSellerPerformanceMetrics");

            migrationBuilder.DropIndex(
                name: "IX_TbSalesReviews_CurrentState",
                table: "TbSalesReviews");

            migrationBuilder.DropIndex(
                name: "IX_TbReviewVotes_CurrentState",
                table: "TbReviewVotes");

            migrationBuilder.DropIndex(
                name: "IX_TbReviewReports_CurrentState",
                table: "TbReviewReports");

            migrationBuilder.DropIndex(
                name: "IX_TbRequestDocuments_CurrentState",
                table: "TbRequestDocuments");

            migrationBuilder.DropIndex(
                name: "IX_TbRequestComments_CurrentState",
                table: "TbRequestComments");

            migrationBuilder.DropIndex(
                name: "IX_TbRefundRequests_CurrentState",
                table: "TbRefundRequests");

            migrationBuilder.DropIndex(
                name: "IX_TbQuantityPricings_CurrentState",
                table: "TbQuantityPricings");

            migrationBuilder.DropIndex(
                name: "IX_TbProductVisibilityRules_CurrentState",
                table: "TbProductVisibilityRules");

            migrationBuilder.DropIndex(
                name: "IX_TbProductReviews_CurrentState",
                table: "TbProductReviews");

            migrationBuilder.DropIndex(
                name: "IX_TbPricingSystemSettings_CurrentState",
                table: "TbPricingSystemSettings");

            migrationBuilder.DropIndex(
                name: "IX_TbPriceHistories_CurrentState",
                table: "TbPriceHistories");

            migrationBuilder.DropIndex(
                name: "IX_TbPlatformTreasuries_CurrentState",
                table: "TbPlatformTreasuries");

            migrationBuilder.DropIndex(
                name: "IX_TbPaymentMethods_CurrentState",
                table: "TbPaymentMethods");

            migrationBuilder.DropIndex(
                name: "IX_TbPages_CurrentState",
                table: "TbPages");

            migrationBuilder.DropIndex(
                name: "IX_TbOrderShipments_CurrentState",
                table: "TbOrderShipments");

            migrationBuilder.DropIndex(
                name: "IX_TbOrderShipmentItems_CurrentState",
                table: "TbOrderShipmentItems");

            migrationBuilder.DropIndex(
                name: "IX_TbOrders_CurrentState",
                table: "TbOrders");

            migrationBuilder.DropIndex(
                name: "IX_TbOrderPayments_CurrentState",
                table: "TbOrderPayments");

            migrationBuilder.DropIndex(
                name: "IX_TbOrderDetails_CurrentState",
                table: "TbOrderDetails");

            migrationBuilder.DropIndex(
                name: "IX_TbOfferStatusHistories_CurrentState",
                table: "TbOfferStatusHistories");

            migrationBuilder.DropIndex(
                name: "IX_TbOffers_CurrentState",
                table: "TbOffers");

            migrationBuilder.DropIndex(
                name: "IX_TbOfferPriceHistories_CurrentState",
                table: "TbOfferPriceHistories");

            migrationBuilder.DropIndex(
                name: "IX_TbOfferConditions_CurrentState",
                table: "TbOfferConditions");

            migrationBuilder.DropIndex(
                name: "IX_TbOfferCombinationPricings_CurrentState",
                table: "TbOfferCombinationPricings");

            migrationBuilder.DropIndex(
                name: "IX_TbNotificationPreferences_CurrentState",
                table: "TbNotificationPreferences");

            migrationBuilder.DropIndex(
                name: "IX_TbNotificationChannels_CurrentState",
                table: "TbNotificationChannels");

            migrationBuilder.DropIndex(
                name: "IX_TbMediaContents_CurrentState",
                table: "TbMediaContents");

            migrationBuilder.DropIndex(
                name: "IX_TbLoyaltyTiers_CurrentState",
                table: "TbLoyaltyTiers");

            migrationBuilder.DropIndex(
                name: "IX_TbLoyaltyPointsTransactions_CurrentState",
                table: "TbLoyaltyPointsTransactions");

            migrationBuilder.DropIndex(
                name: "IX_TbItems_CurrentState",
                table: "TbItems");

            migrationBuilder.DropIndex(
                name: "IX_TbItemImages_CurrentState",
                table: "TbItemImages");

            migrationBuilder.DropIndex(
                name: "IX_TbItemCombinations_CurrentState",
                table: "TbItemCombinations");

            migrationBuilder.DropIndex(
                name: "IX_TbItemAttributes_CurrentState",
                table: "TbItemAttributes");

            migrationBuilder.DropIndex(
                name: "IX_TbHomepageBlocks_CurrentState",
                table: "TbHomepageBlocks");

            migrationBuilder.DropIndex(
                name: "IX_TbFlashSales_CurrentState",
                table: "TbFlashSales");

            migrationBuilder.DropIndex(
                name: "IX_TbFlashSaleProducts_CurrentState",
                table: "TbFlashSaleProducts");

            migrationBuilder.DropIndex(
                name: "IX_TbDisputes_CurrentState",
                table: "TbDisputes");

            migrationBuilder.DropIndex(
                name: "IX_TbDisputeMessages_CurrentState",
                table: "TbDisputeMessages");

            migrationBuilder.DropIndex(
                name: "IX_TbDeliveryReviews_CurrentState",
                table: "TbDeliveryReviews");

            migrationBuilder.DropIndex(
                name: "IX_TbCustomerWallets_CurrentState",
                table: "TbCustomerWallets");

            migrationBuilder.DropIndex(
                name: "IX_TbCustomerSegmentPricings_CurrentState",
                table: "TbCustomerSegmentPricings");

            migrationBuilder.DropIndex(
                name: "IX_TbCustomerLoyalties_CurrentState",
                table: "TbCustomerLoyalties");

            migrationBuilder.DropIndex(
                name: "IX_TbCustomerAddresses_CurrentState",
                table: "TbCustomerAddresses");

            migrationBuilder.DropIndex(
                name: "IX_TbCurrencies_CurrentState",
                table: "TbCurrencies");

            migrationBuilder.DropIndex(
                name: "IX_TbCouponCodes_CurrentState",
                table: "TbCouponCodes");

            migrationBuilder.DropIndex(
                name: "IX_TbCountries_CurrentState",
                table: "TbCountries");

            migrationBuilder.DropIndex(
                name: "IX_TbContentAreas_CurrentState",
                table: "TbContentAreas");

            migrationBuilder.DropIndex(
                name: "IX_TbCombinationAttributesValues_CurrentState",
                table: "TbCombinationAttributesValues");

            migrationBuilder.DropIndex(
                name: "IX_TbCombinationAttributes_CurrentState",
                table: "TbCombinationAttributes");

            migrationBuilder.DropIndex(
                name: "IX_TbCities_CurrentState",
                table: "TbCities");

            migrationBuilder.DropIndex(
                name: "IX_TbCategoryAttributes_CurrentState",
                table: "TbCategoryAttributes");

            migrationBuilder.DropIndex(
                name: "IX_TbCategories_CurrentState",
                table: "TbCategories");

            migrationBuilder.DropIndex(
                name: "IX_TbCampaignVendors_CurrentState",
                table: "TbCampaignVendors");

            migrationBuilder.DropIndex(
                name: "IX_TbCampaigns_CurrentState",
                table: "TbCampaigns");

            migrationBuilder.DropIndex(
                name: "IX_TbCampaignProducts_CurrentState",
                table: "TbCampaignProducts");

            migrationBuilder.DropIndex(
                name: "IX_TbBuyBoxHistories_CurrentState",
                table: "TbBuyBoxHistories");

            migrationBuilder.DropIndex(
                name: "IX_TbBuyBoxCalculations_CurrentState",
                table: "TbBuyBoxCalculations");

            migrationBuilder.DropIndex(
                name: "IX_TbBrands_CurrentState",
                table: "TbBrands");

            migrationBuilder.DropIndex(
                name: "IX_TbBrandRegistrationRequests_CurrentState",
                table: "TbBrandRegistrationRequests");

            migrationBuilder.DropIndex(
                name: "IX_TbBrandDocuments_CurrentState",
                table: "TbBrandDocuments");

            migrationBuilder.DropIndex(
                name: "IX_TbBlockProducts_CurrentState",
                table: "TbBlockProducts");

            migrationBuilder.DropIndex(
                name: "IX_TbAuthorizedDistributors_CurrentState",
                table: "TbAuthorizedDistributors");

            migrationBuilder.DropIndex(
                name: "IX_TbAttributeValuePriceModifiers_CurrentState",
                table: "TbAttributeValuePriceModifiers");

            migrationBuilder.DropIndex(
                name: "IX_TbAttributes_CurrentState",
                table: "TbAttributes");

            migrationBuilder.DropIndex(
                name: "IX_TbAttributeOptions_CurrentState",
                table: "TbAttributeOptions");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_CurrentState",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "CurrentState",
                table: "TbWarranties");

            migrationBuilder.DropColumn(
                name: "CurrentState",
                table: "TbWarehouses");

            migrationBuilder.DropColumn(
                name: "CurrentState",
                table: "TbWalletTransactions");

            migrationBuilder.DropColumn(
                name: "CurrentState",
                table: "TbVisibilityLogs");

            migrationBuilder.DropColumn(
                name: "CurrentState",
                table: "TbVideoProviders");

            migrationBuilder.DropColumn(
                name: "CurrentState",
                table: "TbVendorWallets");

            migrationBuilder.DropColumn(
                name: "CurrentState",
                table: "TbVendorTierHistories");

            migrationBuilder.DropColumn(
                name: "CurrentState",
                table: "TbVendors");

            migrationBuilder.DropColumn(
                name: "CurrentState",
                table: "TbUserOfferRatings");

            migrationBuilder.DropColumn(
                name: "CurrentState",
                table: "TbUserNotifications");

            migrationBuilder.DropColumn(
                name: "CurrentState",
                table: "TbUnits");

            migrationBuilder.DropColumn(
                name: "CurrentState",
                table: "TbUnitConversions");

            migrationBuilder.DropColumn(
                name: "CurrentState",
                table: "TbSuppressionReasons");

            migrationBuilder.DropColumn(
                name: "CurrentState",
                table: "TbSupportTickets");

            migrationBuilder.DropColumn(
                name: "CurrentState",
                table: "TbSupportTicketMessages");

            migrationBuilder.DropColumn(
                name: "CurrentState",
                table: "TbStates");

            migrationBuilder.DropColumn(
                name: "CurrentState",
                table: "TbShoppingCarts");

            migrationBuilder.DropColumn(
                name: "CurrentState",
                table: "TbShoppingCartItems");

            migrationBuilder.DropColumn(
                name: "CurrentState",
                table: "TbShippingDetails");

            migrationBuilder.DropColumn(
                name: "CurrentState",
                table: "TbShippingCompanies");

            migrationBuilder.DropColumn(
                name: "CurrentState",
                table: "TbSettings");

            migrationBuilder.DropColumn(
                name: "CurrentState",
                table: "TbSellerTiers");

            migrationBuilder.DropColumn(
                name: "CurrentState",
                table: "TbSellerTierBenefits");

            migrationBuilder.DropColumn(
                name: "CurrentState",
                table: "TbSellerRequests");

            migrationBuilder.DropColumn(
                name: "CurrentState",
                table: "TbSellerPerformanceMetrics");

            migrationBuilder.DropColumn(
                name: "CurrentState",
                table: "TbSalesReviews");

            migrationBuilder.DropColumn(
                name: "CurrentState",
                table: "TbReviewVotes");

            migrationBuilder.DropColumn(
                name: "CurrentState",
                table: "TbReviewReports");

            migrationBuilder.DropColumn(
                name: "CurrentState",
                table: "TbRequestDocuments");

            migrationBuilder.DropColumn(
                name: "CurrentState",
                table: "TbRequestComments");

            migrationBuilder.DropColumn(
                name: "CurrentState",
                table: "TbRefundRequests");

            migrationBuilder.DropColumn(
                name: "CurrentState",
                table: "TbQuantityPricings");

            migrationBuilder.DropColumn(
                name: "CurrentState",
                table: "TbProductVisibilityRules");

            migrationBuilder.DropColumn(
                name: "CurrentState",
                table: "TbProductReviews");

            migrationBuilder.DropColumn(
                name: "CurrentState",
                table: "TbPricingSystemSettings");

            migrationBuilder.DropColumn(
                name: "CurrentState",
                table: "TbPriceHistories");

            migrationBuilder.DropColumn(
                name: "CurrentState",
                table: "TbPlatformTreasuries");

            migrationBuilder.DropColumn(
                name: "CurrentState",
                table: "TbPaymentMethods");

            migrationBuilder.DropColumn(
                name: "CurrentState",
                table: "TbPages");

            migrationBuilder.DropColumn(
                name: "CurrentState",
                table: "TbOrderShipments");

            migrationBuilder.DropColumn(
                name: "CurrentState",
                table: "TbOrderShipmentItems");

            migrationBuilder.DropColumn(
                name: "CurrentState",
                table: "TbOrders");

            migrationBuilder.DropColumn(
                name: "CurrentState",
                table: "TbOrderPayments");

            migrationBuilder.DropColumn(
                name: "CurrentState",
                table: "TbOrderDetails");

            migrationBuilder.DropColumn(
                name: "CurrentState",
                table: "TbOfferStatusHistories");

            migrationBuilder.DropColumn(
                name: "CurrentState",
                table: "TbOffers");

            migrationBuilder.DropColumn(
                name: "CurrentState",
                table: "TbOfferPriceHistories");

            migrationBuilder.DropColumn(
                name: "CurrentState",
                table: "TbOfferConditions");

            migrationBuilder.DropColumn(
                name: "CurrentState",
                table: "TbOfferCombinationPricings");

            migrationBuilder.DropColumn(
                name: "CurrentState",
                table: "TbNotificationPreferences");

            migrationBuilder.DropColumn(
                name: "CurrentState",
                table: "TbNotificationChannels");

            migrationBuilder.DropColumn(
                name: "CurrentState",
                table: "TbMediaContents");

            migrationBuilder.DropColumn(
                name: "CurrentState",
                table: "TbLoyaltyTiers");

            migrationBuilder.DropColumn(
                name: "CurrentState",
                table: "TbLoyaltyPointsTransactions");

            migrationBuilder.DropColumn(
                name: "CurrentState",
                table: "TbItems");

            migrationBuilder.DropColumn(
                name: "CurrentState",
                table: "TbItemImages");

            migrationBuilder.DropColumn(
                name: "CurrentState",
                table: "TbItemCombinations");

            migrationBuilder.DropColumn(
                name: "CurrentState",
                table: "TbItemAttributes");

            migrationBuilder.DropColumn(
                name: "CurrentState",
                table: "TbHomepageBlocks");

            migrationBuilder.DropColumn(
                name: "CurrentState",
                table: "TbFlashSales");

            migrationBuilder.DropColumn(
                name: "CurrentState",
                table: "TbFlashSaleProducts");

            migrationBuilder.DropColumn(
                name: "CurrentState",
                table: "TbDisputes");

            migrationBuilder.DropColumn(
                name: "CurrentState",
                table: "TbDisputeMessages");

            migrationBuilder.DropColumn(
                name: "CurrentState",
                table: "TbDeliveryReviews");

            migrationBuilder.DropColumn(
                name: "CurrentState",
                table: "TbCustomerWallets");

            migrationBuilder.DropColumn(
                name: "CurrentState",
                table: "TbCustomerSegmentPricings");

            migrationBuilder.DropColumn(
                name: "CurrentState",
                table: "TbCustomerLoyalties");

            migrationBuilder.DropColumn(
                name: "CurrentState",
                table: "TbCustomerAddresses");

            migrationBuilder.DropColumn(
                name: "CurrentState",
                table: "TbCurrencies");

            migrationBuilder.DropColumn(
                name: "CurrentState",
                table: "TbCouponCodes");

            migrationBuilder.DropColumn(
                name: "CurrentState",
                table: "TbCountries");

            migrationBuilder.DropColumn(
                name: "CurrentState",
                table: "TbContentAreas");

            migrationBuilder.DropColumn(
                name: "CurrentState",
                table: "TbCombinationAttributesValues");

            migrationBuilder.DropColumn(
                name: "CurrentState",
                table: "TbCombinationAttributes");

            migrationBuilder.DropColumn(
                name: "CurrentState",
                table: "TbCities");

            migrationBuilder.DropColumn(
                name: "CurrentState",
                table: "TbCategoryAttributes");

            migrationBuilder.DropColumn(
                name: "CurrentState",
                table: "TbCategories");

            migrationBuilder.DropColumn(
                name: "CurrentState",
                table: "TbCampaignVendors");

            migrationBuilder.DropColumn(
                name: "CurrentState",
                table: "TbCampaigns");

            migrationBuilder.DropColumn(
                name: "CurrentState",
                table: "TbCampaignProducts");

            migrationBuilder.DropColumn(
                name: "CurrentState",
                table: "TbBuyBoxHistories");

            migrationBuilder.DropColumn(
                name: "CurrentState",
                table: "TbBuyBoxCalculations");

            migrationBuilder.DropColumn(
                name: "CurrentState",
                table: "TbBrands");

            migrationBuilder.DropColumn(
                name: "CurrentState",
                table: "TbBrandRegistrationRequests");

            migrationBuilder.DropColumn(
                name: "CurrentState",
                table: "TbBrandDocuments");

            migrationBuilder.DropColumn(
                name: "CurrentState",
                table: "TbBlockProducts");

            migrationBuilder.DropColumn(
                name: "CurrentState",
                table: "TbAuthorizedDistributors");

            migrationBuilder.DropColumn(
                name: "CurrentState",
                table: "TbAttributeValuePriceModifiers");

            migrationBuilder.DropColumn(
                name: "CurrentState",
                table: "TbAttributes");

            migrationBuilder.DropColumn(
                name: "CurrentState",
                table: "TbAttributeOptions");

            migrationBuilder.DropColumn(
                name: "CurrentState",
                table: "Notifications");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TbWarranties",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TbWarehouses",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TbWalletTransactions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TbVisibilityLogs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TbVideoProviders",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TbVendorWallets",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TbVendorTierHistories",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TbVendors",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TbUserOfferRatings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TbUserNotifications",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TbUnits",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TbUnitConversions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TbSuppressionReasons",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TbSupportTickets",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TbSupportTicketMessages",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TbStates",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TbShoppingCarts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TbShoppingCartItems",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TbShippingDetails",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TbShippingCompanies",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TbSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TbSellerTiers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TbSellerTierBenefits",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TbSellerRequests",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TbSellerPerformanceMetrics",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TbSalesReviews",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TbReviewVotes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TbReviewReports",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TbRequestDocuments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TbRequestComments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TbRefundRequests",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TbQuantityPricings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TbProductVisibilityRules",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TbProductReviews",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TbPricingSystemSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TbPriceHistories",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TbPlatformTreasuries",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TbPaymentMethods",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TbPages",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TbOrderShipments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TbOrderShipmentItems",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TbOrders",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TbOrderPayments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TbOrderDetails",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TbOfferStatusHistories",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TbOffers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TbOfferPriceHistories",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TbOfferConditions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TbOfferCombinationPricings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TbNotificationPreferences",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TbNotificationChannels",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TbMediaContents",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TbLoyaltyTiers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TbLoyaltyPointsTransactions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TbItems",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TbItemImages",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TbItemCombinations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TbItemAttributes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TbHomepageBlocks",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TbFlashSales",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TbFlashSaleProducts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TbDisputes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TbDisputeMessages",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TbDeliveryReviews",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TbCustomerWallets",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TbCustomerSegmentPricings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TbCustomerLoyalties",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TbCustomerAddresses",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TbCurrencies",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TbCouponCodes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TbCountries",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TbContentAreas",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TbCombinationAttributesValues",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TbCombinationAttributes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TbCities",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TbCategoryAttributes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TbCategories",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TbCampaignVendors",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TbCampaigns",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TbCampaignProducts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TbBuyBoxHistories",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TbBuyBoxCalculations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TbBrands",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TbBrandRegistrationRequests",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TbBrandDocuments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TbBlockProducts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TbAuthorizedDistributors",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TbAttributeValuePriceModifiers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TbAttributes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TbAttributeOptions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Notifications",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_TbWarranties_IsDeleted",
                table: "TbWarranties",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbWarehouses_IsDeleted",
                table: "TbWarehouses",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbWalletTransactions_IsDeleted",
                table: "TbWalletTransactions",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbVisibilityLogs_IsDeleted",
                table: "TbVisibilityLogs",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbVideoProviders_IsDeleted",
                table: "TbVideoProviders",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbVendorWallets_IsDeleted",
                table: "TbVendorWallets",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbVendorTierHistories_IsDeleted",
                table: "TbVendorTierHistories",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbVendors_IsDeleted",
                table: "TbVendors",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbUserOfferRatings_IsDeleted",
                table: "TbUserOfferRatings",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbUserNotifications_IsDeleted",
                table: "TbUserNotifications",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbUnits_IsDeleted",
                table: "TbUnits",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbUnitConversions_IsDeleted",
                table: "TbUnitConversions",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbSuppressionReasons_IsDeleted",
                table: "TbSuppressionReasons",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbSupportTickets_IsDeleted",
                table: "TbSupportTickets",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbSupportTicketMessages_IsDeleted",
                table: "TbSupportTicketMessages",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbStates_IsDeleted",
                table: "TbStates",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbShoppingCarts_IsDeleted",
                table: "TbShoppingCarts",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbShoppingCartItems_IsDeleted",
                table: "TbShoppingCartItems",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbShippingDetails_IsDeleted",
                table: "TbShippingDetails",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbShippingCompanies_IsDeleted",
                table: "TbShippingCompanies",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbSettings_IsDeleted",
                table: "TbSettings",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbSellerTiers_IsDeleted",
                table: "TbSellerTiers",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbSellerTierBenefits_IsDeleted",
                table: "TbSellerTierBenefits",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbSellerRequests_IsDeleted",
                table: "TbSellerRequests",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbSellerPerformanceMetrics_IsDeleted",
                table: "TbSellerPerformanceMetrics",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbSalesReviews_IsDeleted",
                table: "TbSalesReviews",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbReviewVotes_IsDeleted",
                table: "TbReviewVotes",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbReviewReports_IsDeleted",
                table: "TbReviewReports",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbRequestDocuments_IsDeleted",
                table: "TbRequestDocuments",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbRequestComments_IsDeleted",
                table: "TbRequestComments",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbRefundRequests_IsDeleted",
                table: "TbRefundRequests",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbQuantityPricings_IsDeleted",
                table: "TbQuantityPricings",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbProductVisibilityRules_IsDeleted",
                table: "TbProductVisibilityRules",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbProductReviews_IsDeleted",
                table: "TbProductReviews",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbPricingSystemSettings_IsDeleted",
                table: "TbPricingSystemSettings",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbPriceHistories_IsDeleted",
                table: "TbPriceHistories",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbPlatformTreasuries_IsDeleted",
                table: "TbPlatformTreasuries",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbPaymentMethods_IsDeleted",
                table: "TbPaymentMethods",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbPages_IsDeleted",
                table: "TbPages",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbOrderShipments_IsDeleted",
                table: "TbOrderShipments",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbOrderShipmentItems_IsDeleted",
                table: "TbOrderShipmentItems",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbOrders_IsDeleted",
                table: "TbOrders",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbOrderPayments_IsDeleted",
                table: "TbOrderPayments",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbOrderDetails_IsDeleted",
                table: "TbOrderDetails",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbOfferStatusHistories_IsDeleted",
                table: "TbOfferStatusHistories",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbOffers_IsDeleted",
                table: "TbOffers",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbOfferPriceHistories_IsDeleted",
                table: "TbOfferPriceHistories",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbOfferConditions_IsDeleted",
                table: "TbOfferConditions",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbOfferCombinationPricings_IsDeleted",
                table: "TbOfferCombinationPricings",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbNotificationPreferences_IsDeleted",
                table: "TbNotificationPreferences",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbNotificationChannels_IsDeleted",
                table: "TbNotificationChannels",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbMediaContents_IsDeleted",
                table: "TbMediaContents",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbLoyaltyTiers_IsDeleted",
                table: "TbLoyaltyTiers",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbLoyaltyPointsTransactions_IsDeleted",
                table: "TbLoyaltyPointsTransactions",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbItems_IsDeleted",
                table: "TbItems",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbItemImages_IsDeleted",
                table: "TbItemImages",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbItemCombinations_IsDeleted",
                table: "TbItemCombinations",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbItemAttributes_IsDeleted",
                table: "TbItemAttributes",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbHomepageBlocks_IsDeleted",
                table: "TbHomepageBlocks",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbFlashSales_IsDeleted",
                table: "TbFlashSales",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbFlashSaleProducts_IsDeleted",
                table: "TbFlashSaleProducts",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbDisputes_IsDeleted",
                table: "TbDisputes",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbDisputeMessages_IsDeleted",
                table: "TbDisputeMessages",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbDeliveryReviews_IsDeleted",
                table: "TbDeliveryReviews",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbCustomerWallets_IsDeleted",
                table: "TbCustomerWallets",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbCustomerSegmentPricings_IsDeleted",
                table: "TbCustomerSegmentPricings",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbCustomerLoyalties_IsDeleted",
                table: "TbCustomerLoyalties",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbCustomerAddresses_IsDeleted",
                table: "TbCustomerAddresses",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbCurrencies_IsDeleted",
                table: "TbCurrencies",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbCouponCodes_IsDeleted",
                table: "TbCouponCodes",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbCountries_IsDeleted",
                table: "TbCountries",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbContentAreas_IsDeleted",
                table: "TbContentAreas",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbCombinationAttributesValues_IsDeleted",
                table: "TbCombinationAttributesValues",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbCombinationAttributes_IsDeleted",
                table: "TbCombinationAttributes",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbCities_IsDeleted",
                table: "TbCities",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbCategoryAttributes_IsDeleted",
                table: "TbCategoryAttributes",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbCategories_IsDeleted",
                table: "TbCategories",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbCampaignVendors_IsDeleted",
                table: "TbCampaignVendors",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbCampaigns_IsDeleted",
                table: "TbCampaigns",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbCampaignProducts_IsDeleted",
                table: "TbCampaignProducts",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbBuyBoxHistories_IsDeleted",
                table: "TbBuyBoxHistories",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbBuyBoxCalculations_IsDeleted",
                table: "TbBuyBoxCalculations",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbBrands_IsDeleted",
                table: "TbBrands",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbBrandRegistrationRequests_IsDeleted",
                table: "TbBrandRegistrationRequests",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbBrandDocuments_IsDeleted",
                table: "TbBrandDocuments",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbBlockProducts_IsDeleted",
                table: "TbBlockProducts",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbAuthorizedDistributors_IsDeleted",
                table: "TbAuthorizedDistributors",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbAttributeValuePriceModifiers_IsDeleted",
                table: "TbAttributeValuePriceModifiers",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbAttributes_IsDeleted",
                table: "TbAttributes",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbAttributeOptions_IsDeleted",
                table: "TbAttributeOptions",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_IsDeleted",
                table: "Notifications",
                column: "IsDeleted");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TbWarranties_IsDeleted",
                table: "TbWarranties");

            migrationBuilder.DropIndex(
                name: "IX_TbWarehouses_IsDeleted",
                table: "TbWarehouses");

            migrationBuilder.DropIndex(
                name: "IX_TbWalletTransactions_IsDeleted",
                table: "TbWalletTransactions");

            migrationBuilder.DropIndex(
                name: "IX_TbVisibilityLogs_IsDeleted",
                table: "TbVisibilityLogs");

            migrationBuilder.DropIndex(
                name: "IX_TbVideoProviders_IsDeleted",
                table: "TbVideoProviders");

            migrationBuilder.DropIndex(
                name: "IX_TbVendorWallets_IsDeleted",
                table: "TbVendorWallets");

            migrationBuilder.DropIndex(
                name: "IX_TbVendorTierHistories_IsDeleted",
                table: "TbVendorTierHistories");

            migrationBuilder.DropIndex(
                name: "IX_TbVendors_IsDeleted",
                table: "TbVendors");

            migrationBuilder.DropIndex(
                name: "IX_TbUserOfferRatings_IsDeleted",
                table: "TbUserOfferRatings");

            migrationBuilder.DropIndex(
                name: "IX_TbUserNotifications_IsDeleted",
                table: "TbUserNotifications");

            migrationBuilder.DropIndex(
                name: "IX_TbUnits_IsDeleted",
                table: "TbUnits");

            migrationBuilder.DropIndex(
                name: "IX_TbUnitConversions_IsDeleted",
                table: "TbUnitConversions");

            migrationBuilder.DropIndex(
                name: "IX_TbSuppressionReasons_IsDeleted",
                table: "TbSuppressionReasons");

            migrationBuilder.DropIndex(
                name: "IX_TbSupportTickets_IsDeleted",
                table: "TbSupportTickets");

            migrationBuilder.DropIndex(
                name: "IX_TbSupportTicketMessages_IsDeleted",
                table: "TbSupportTicketMessages");

            migrationBuilder.DropIndex(
                name: "IX_TbStates_IsDeleted",
                table: "TbStates");

            migrationBuilder.DropIndex(
                name: "IX_TbShoppingCarts_IsDeleted",
                table: "TbShoppingCarts");

            migrationBuilder.DropIndex(
                name: "IX_TbShoppingCartItems_IsDeleted",
                table: "TbShoppingCartItems");

            migrationBuilder.DropIndex(
                name: "IX_TbShippingDetails_IsDeleted",
                table: "TbShippingDetails");

            migrationBuilder.DropIndex(
                name: "IX_TbShippingCompanies_IsDeleted",
                table: "TbShippingCompanies");

            migrationBuilder.DropIndex(
                name: "IX_TbSettings_IsDeleted",
                table: "TbSettings");

            migrationBuilder.DropIndex(
                name: "IX_TbSellerTiers_IsDeleted",
                table: "TbSellerTiers");

            migrationBuilder.DropIndex(
                name: "IX_TbSellerTierBenefits_IsDeleted",
                table: "TbSellerTierBenefits");

            migrationBuilder.DropIndex(
                name: "IX_TbSellerRequests_IsDeleted",
                table: "TbSellerRequests");

            migrationBuilder.DropIndex(
                name: "IX_TbSellerPerformanceMetrics_IsDeleted",
                table: "TbSellerPerformanceMetrics");

            migrationBuilder.DropIndex(
                name: "IX_TbSalesReviews_IsDeleted",
                table: "TbSalesReviews");

            migrationBuilder.DropIndex(
                name: "IX_TbReviewVotes_IsDeleted",
                table: "TbReviewVotes");

            migrationBuilder.DropIndex(
                name: "IX_TbReviewReports_IsDeleted",
                table: "TbReviewReports");

            migrationBuilder.DropIndex(
                name: "IX_TbRequestDocuments_IsDeleted",
                table: "TbRequestDocuments");

            migrationBuilder.DropIndex(
                name: "IX_TbRequestComments_IsDeleted",
                table: "TbRequestComments");

            migrationBuilder.DropIndex(
                name: "IX_TbRefundRequests_IsDeleted",
                table: "TbRefundRequests");

            migrationBuilder.DropIndex(
                name: "IX_TbQuantityPricings_IsDeleted",
                table: "TbQuantityPricings");

            migrationBuilder.DropIndex(
                name: "IX_TbProductVisibilityRules_IsDeleted",
                table: "TbProductVisibilityRules");

            migrationBuilder.DropIndex(
                name: "IX_TbProductReviews_IsDeleted",
                table: "TbProductReviews");

            migrationBuilder.DropIndex(
                name: "IX_TbPricingSystemSettings_IsDeleted",
                table: "TbPricingSystemSettings");

            migrationBuilder.DropIndex(
                name: "IX_TbPriceHistories_IsDeleted",
                table: "TbPriceHistories");

            migrationBuilder.DropIndex(
                name: "IX_TbPlatformTreasuries_IsDeleted",
                table: "TbPlatformTreasuries");

            migrationBuilder.DropIndex(
                name: "IX_TbPaymentMethods_IsDeleted",
                table: "TbPaymentMethods");

            migrationBuilder.DropIndex(
                name: "IX_TbPages_IsDeleted",
                table: "TbPages");

            migrationBuilder.DropIndex(
                name: "IX_TbOrderShipments_IsDeleted",
                table: "TbOrderShipments");

            migrationBuilder.DropIndex(
                name: "IX_TbOrderShipmentItems_IsDeleted",
                table: "TbOrderShipmentItems");

            migrationBuilder.DropIndex(
                name: "IX_TbOrders_IsDeleted",
                table: "TbOrders");

            migrationBuilder.DropIndex(
                name: "IX_TbOrderPayments_IsDeleted",
                table: "TbOrderPayments");

            migrationBuilder.DropIndex(
                name: "IX_TbOrderDetails_IsDeleted",
                table: "TbOrderDetails");

            migrationBuilder.DropIndex(
                name: "IX_TbOfferStatusHistories_IsDeleted",
                table: "TbOfferStatusHistories");

            migrationBuilder.DropIndex(
                name: "IX_TbOffers_IsDeleted",
                table: "TbOffers");

            migrationBuilder.DropIndex(
                name: "IX_TbOfferPriceHistories_IsDeleted",
                table: "TbOfferPriceHistories");

            migrationBuilder.DropIndex(
                name: "IX_TbOfferConditions_IsDeleted",
                table: "TbOfferConditions");

            migrationBuilder.DropIndex(
                name: "IX_TbOfferCombinationPricings_IsDeleted",
                table: "TbOfferCombinationPricings");

            migrationBuilder.DropIndex(
                name: "IX_TbNotificationPreferences_IsDeleted",
                table: "TbNotificationPreferences");

            migrationBuilder.DropIndex(
                name: "IX_TbNotificationChannels_IsDeleted",
                table: "TbNotificationChannels");

            migrationBuilder.DropIndex(
                name: "IX_TbMediaContents_IsDeleted",
                table: "TbMediaContents");

            migrationBuilder.DropIndex(
                name: "IX_TbLoyaltyTiers_IsDeleted",
                table: "TbLoyaltyTiers");

            migrationBuilder.DropIndex(
                name: "IX_TbLoyaltyPointsTransactions_IsDeleted",
                table: "TbLoyaltyPointsTransactions");

            migrationBuilder.DropIndex(
                name: "IX_TbItems_IsDeleted",
                table: "TbItems");

            migrationBuilder.DropIndex(
                name: "IX_TbItemImages_IsDeleted",
                table: "TbItemImages");

            migrationBuilder.DropIndex(
                name: "IX_TbItemCombinations_IsDeleted",
                table: "TbItemCombinations");

            migrationBuilder.DropIndex(
                name: "IX_TbItemAttributes_IsDeleted",
                table: "TbItemAttributes");

            migrationBuilder.DropIndex(
                name: "IX_TbHomepageBlocks_IsDeleted",
                table: "TbHomepageBlocks");

            migrationBuilder.DropIndex(
                name: "IX_TbFlashSales_IsDeleted",
                table: "TbFlashSales");

            migrationBuilder.DropIndex(
                name: "IX_TbFlashSaleProducts_IsDeleted",
                table: "TbFlashSaleProducts");

            migrationBuilder.DropIndex(
                name: "IX_TbDisputes_IsDeleted",
                table: "TbDisputes");

            migrationBuilder.DropIndex(
                name: "IX_TbDisputeMessages_IsDeleted",
                table: "TbDisputeMessages");

            migrationBuilder.DropIndex(
                name: "IX_TbDeliveryReviews_IsDeleted",
                table: "TbDeliveryReviews");

            migrationBuilder.DropIndex(
                name: "IX_TbCustomerWallets_IsDeleted",
                table: "TbCustomerWallets");

            migrationBuilder.DropIndex(
                name: "IX_TbCustomerSegmentPricings_IsDeleted",
                table: "TbCustomerSegmentPricings");

            migrationBuilder.DropIndex(
                name: "IX_TbCustomerLoyalties_IsDeleted",
                table: "TbCustomerLoyalties");

            migrationBuilder.DropIndex(
                name: "IX_TbCustomerAddresses_IsDeleted",
                table: "TbCustomerAddresses");

            migrationBuilder.DropIndex(
                name: "IX_TbCurrencies_IsDeleted",
                table: "TbCurrencies");

            migrationBuilder.DropIndex(
                name: "IX_TbCouponCodes_IsDeleted",
                table: "TbCouponCodes");

            migrationBuilder.DropIndex(
                name: "IX_TbCountries_IsDeleted",
                table: "TbCountries");

            migrationBuilder.DropIndex(
                name: "IX_TbContentAreas_IsDeleted",
                table: "TbContentAreas");

            migrationBuilder.DropIndex(
                name: "IX_TbCombinationAttributesValues_IsDeleted",
                table: "TbCombinationAttributesValues");

            migrationBuilder.DropIndex(
                name: "IX_TbCombinationAttributes_IsDeleted",
                table: "TbCombinationAttributes");

            migrationBuilder.DropIndex(
                name: "IX_TbCities_IsDeleted",
                table: "TbCities");

            migrationBuilder.DropIndex(
                name: "IX_TbCategoryAttributes_IsDeleted",
                table: "TbCategoryAttributes");

            migrationBuilder.DropIndex(
                name: "IX_TbCategories_IsDeleted",
                table: "TbCategories");

            migrationBuilder.DropIndex(
                name: "IX_TbCampaignVendors_IsDeleted",
                table: "TbCampaignVendors");

            migrationBuilder.DropIndex(
                name: "IX_TbCampaigns_IsDeleted",
                table: "TbCampaigns");

            migrationBuilder.DropIndex(
                name: "IX_TbCampaignProducts_IsDeleted",
                table: "TbCampaignProducts");

            migrationBuilder.DropIndex(
                name: "IX_TbBuyBoxHistories_IsDeleted",
                table: "TbBuyBoxHistories");

            migrationBuilder.DropIndex(
                name: "IX_TbBuyBoxCalculations_IsDeleted",
                table: "TbBuyBoxCalculations");

            migrationBuilder.DropIndex(
                name: "IX_TbBrands_IsDeleted",
                table: "TbBrands");

            migrationBuilder.DropIndex(
                name: "IX_TbBrandRegistrationRequests_IsDeleted",
                table: "TbBrandRegistrationRequests");

            migrationBuilder.DropIndex(
                name: "IX_TbBrandDocuments_IsDeleted",
                table: "TbBrandDocuments");

            migrationBuilder.DropIndex(
                name: "IX_TbBlockProducts_IsDeleted",
                table: "TbBlockProducts");

            migrationBuilder.DropIndex(
                name: "IX_TbAuthorizedDistributors_IsDeleted",
                table: "TbAuthorizedDistributors");

            migrationBuilder.DropIndex(
                name: "IX_TbAttributeValuePriceModifiers_IsDeleted",
                table: "TbAttributeValuePriceModifiers");

            migrationBuilder.DropIndex(
                name: "IX_TbAttributes_IsDeleted",
                table: "TbAttributes");

            migrationBuilder.DropIndex(
                name: "IX_TbAttributeOptions_IsDeleted",
                table: "TbAttributeOptions");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_IsDeleted",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TbWarranties");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TbWarehouses");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TbWalletTransactions");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TbVisibilityLogs");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TbVideoProviders");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TbVendorWallets");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TbVendorTierHistories");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TbVendors");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TbUserOfferRatings");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TbUserNotifications");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TbUnits");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TbUnitConversions");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TbSuppressionReasons");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TbSupportTickets");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TbSupportTicketMessages");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TbStates");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TbShoppingCarts");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TbShoppingCartItems");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TbShippingDetails");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TbShippingCompanies");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TbSettings");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TbSellerTiers");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TbSellerTierBenefits");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TbSellerRequests");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TbSellerPerformanceMetrics");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TbSalesReviews");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TbReviewVotes");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TbReviewReports");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TbRequestDocuments");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TbRequestComments");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TbRefundRequests");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TbQuantityPricings");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TbProductVisibilityRules");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TbProductReviews");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TbPricingSystemSettings");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TbPriceHistories");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TbPlatformTreasuries");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TbPaymentMethods");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TbPages");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TbOrderShipments");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TbOrderShipmentItems");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TbOrders");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TbOrderPayments");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TbOrderDetails");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TbOfferStatusHistories");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TbOffers");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TbOfferPriceHistories");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TbOfferConditions");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TbOfferCombinationPricings");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TbNotificationPreferences");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TbNotificationChannels");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TbMediaContents");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TbLoyaltyTiers");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TbLoyaltyPointsTransactions");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TbItems");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TbItemImages");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TbItemCombinations");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TbItemAttributes");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TbHomepageBlocks");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TbFlashSales");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TbFlashSaleProducts");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TbDisputes");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TbDisputeMessages");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TbDeliveryReviews");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TbCustomerWallets");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TbCustomerSegmentPricings");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TbCustomerLoyalties");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TbCustomerAddresses");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TbCurrencies");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TbCouponCodes");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TbCountries");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TbContentAreas");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TbCombinationAttributesValues");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TbCombinationAttributes");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TbCities");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TbCategoryAttributes");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TbCategories");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TbCampaignVendors");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TbCampaigns");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TbCampaignProducts");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TbBuyBoxHistories");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TbBuyBoxCalculations");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TbBrands");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TbBrandRegistrationRequests");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TbBrandDocuments");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TbBlockProducts");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TbAuthorizedDistributors");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TbAttributeValuePriceModifiers");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TbAttributes");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TbAttributeOptions");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Notifications");

            migrationBuilder.AddColumn<int>(
                name: "CurrentState",
                table: "TbWarranties",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "CurrentState",
                table: "TbWarehouses",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "CurrentState",
                table: "TbWalletTransactions",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "CurrentState",
                table: "TbVisibilityLogs",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "CurrentState",
                table: "TbVideoProviders",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "CurrentState",
                table: "TbVendorWallets",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "CurrentState",
                table: "TbVendorTierHistories",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "CurrentState",
                table: "TbVendors",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "CurrentState",
                table: "TbUserOfferRatings",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "CurrentState",
                table: "TbUserNotifications",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "CurrentState",
                table: "TbUnits",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "CurrentState",
                table: "TbUnitConversions",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "CurrentState",
                table: "TbSuppressionReasons",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "CurrentState",
                table: "TbSupportTickets",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "CurrentState",
                table: "TbSupportTicketMessages",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "CurrentState",
                table: "TbStates",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "CurrentState",
                table: "TbShoppingCarts",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "CurrentState",
                table: "TbShoppingCartItems",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "CurrentState",
                table: "TbShippingDetails",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "CurrentState",
                table: "TbShippingCompanies",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "CurrentState",
                table: "TbSettings",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "CurrentState",
                table: "TbSellerTiers",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "CurrentState",
                table: "TbSellerTierBenefits",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "CurrentState",
                table: "TbSellerRequests",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "CurrentState",
                table: "TbSellerPerformanceMetrics",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "CurrentState",
                table: "TbSalesReviews",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "CurrentState",
                table: "TbReviewVotes",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "CurrentState",
                table: "TbReviewReports",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "CurrentState",
                table: "TbRequestDocuments",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "CurrentState",
                table: "TbRequestComments",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "CurrentState",
                table: "TbRefundRequests",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "CurrentState",
                table: "TbQuantityPricings",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "CurrentState",
                table: "TbProductVisibilityRules",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "CurrentState",
                table: "TbProductReviews",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "CurrentState",
                table: "TbPricingSystemSettings",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "CurrentState",
                table: "TbPriceHistories",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "CurrentState",
                table: "TbPlatformTreasuries",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "CurrentState",
                table: "TbPaymentMethods",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "CurrentState",
                table: "TbPages",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "CurrentState",
                table: "TbOrderShipments",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "CurrentState",
                table: "TbOrderShipmentItems",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "CurrentState",
                table: "TbOrders",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "CurrentState",
                table: "TbOrderPayments",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "CurrentState",
                table: "TbOrderDetails",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "CurrentState",
                table: "TbOfferStatusHistories",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "CurrentState",
                table: "TbOffers",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "CurrentState",
                table: "TbOfferPriceHistories",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "CurrentState",
                table: "TbOfferConditions",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "CurrentState",
                table: "TbOfferCombinationPricings",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "CurrentState",
                table: "TbNotificationPreferences",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "CurrentState",
                table: "TbNotificationChannels",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "CurrentState",
                table: "TbMediaContents",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "CurrentState",
                table: "TbLoyaltyTiers",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "CurrentState",
                table: "TbLoyaltyPointsTransactions",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "CurrentState",
                table: "TbItems",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "CurrentState",
                table: "TbItemImages",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "CurrentState",
                table: "TbItemCombinations",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "CurrentState",
                table: "TbItemAttributes",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "CurrentState",
                table: "TbHomepageBlocks",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "CurrentState",
                table: "TbFlashSales",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "CurrentState",
                table: "TbFlashSaleProducts",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "CurrentState",
                table: "TbDisputes",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "CurrentState",
                table: "TbDisputeMessages",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "CurrentState",
                table: "TbDeliveryReviews",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "CurrentState",
                table: "TbCustomerWallets",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "CurrentState",
                table: "TbCustomerSegmentPricings",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "CurrentState",
                table: "TbCustomerLoyalties",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "CurrentState",
                table: "TbCustomerAddresses",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "CurrentState",
                table: "TbCurrencies",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "CurrentState",
                table: "TbCouponCodes",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "CurrentState",
                table: "TbCountries",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "CurrentState",
                table: "TbContentAreas",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "CurrentState",
                table: "TbCombinationAttributesValues",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "CurrentState",
                table: "TbCombinationAttributes",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "CurrentState",
                table: "TbCities",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "CurrentState",
                table: "TbCategoryAttributes",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "CurrentState",
                table: "TbCategories",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "CurrentState",
                table: "TbCampaignVendors",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "CurrentState",
                table: "TbCampaigns",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "CurrentState",
                table: "TbCampaignProducts",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "CurrentState",
                table: "TbBuyBoxHistories",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "CurrentState",
                table: "TbBuyBoxCalculations",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "CurrentState",
                table: "TbBrands",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "CurrentState",
                table: "TbBrandRegistrationRequests",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "CurrentState",
                table: "TbBrandDocuments",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "CurrentState",
                table: "TbBlockProducts",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "CurrentState",
                table: "TbAuthorizedDistributors",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "CurrentState",
                table: "TbAttributeValuePriceModifiers",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "CurrentState",
                table: "TbAttributes",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "CurrentState",
                table: "TbAttributeOptions",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "CurrentState",
                table: "Notifications",
                type: "int",
                nullable: false,
                defaultValue: 1);


            migrationBuilder.CreateIndex(
                name: "IX_TbWarranties_CurrentState",
                table: "TbWarranties",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbWarehouses_CurrentState",
                table: "TbWarehouses",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbWalletTransactions_CurrentState",
                table: "TbWalletTransactions",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbVisibilityLogs_CurrentState",
                table: "TbVisibilityLogs",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbVideoProviders_CurrentState",
                table: "TbVideoProviders",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbVendorWallets_CurrentState",
                table: "TbVendorWallets",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbVendorTierHistories_CurrentState",
                table: "TbVendorTierHistories",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbVendors_CurrentState",
                table: "TbVendors",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbUserOfferRatings_CurrentState",
                table: "TbUserOfferRatings",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbUserNotifications_CurrentState",
                table: "TbUserNotifications",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbUnits_CurrentState",
                table: "TbUnits",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbUnitConversions_CurrentState",
                table: "TbUnitConversions",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbSuppressionReasons_CurrentState",
                table: "TbSuppressionReasons",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbSupportTickets_CurrentState",
                table: "TbSupportTickets",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbSupportTicketMessages_CurrentState",
                table: "TbSupportTicketMessages",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbStates_CurrentState",
                table: "TbStates",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbShoppingCarts_CurrentState",
                table: "TbShoppingCarts",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbShoppingCartItems_CurrentState",
                table: "TbShoppingCartItems",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbShippingDetails_CurrentState",
                table: "TbShippingDetails",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbShippingCompanies_CurrentState",
                table: "TbShippingCompanies",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbSettings_CurrentState",
                table: "TbSettings",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbSellerTiers_CurrentState",
                table: "TbSellerTiers",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbSellerTierBenefits_CurrentState",
                table: "TbSellerTierBenefits",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbSellerRequests_CurrentState",
                table: "TbSellerRequests",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbSellerPerformanceMetrics_CurrentState",
                table: "TbSellerPerformanceMetrics",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbSalesReviews_CurrentState",
                table: "TbSalesReviews",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbReviewVotes_CurrentState",
                table: "TbReviewVotes",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbReviewReports_CurrentState",
                table: "TbReviewReports",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbRequestDocuments_CurrentState",
                table: "TbRequestDocuments",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbRequestComments_CurrentState",
                table: "TbRequestComments",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbRefundRequests_CurrentState",
                table: "TbRefundRequests",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbQuantityPricings_CurrentState",
                table: "TbQuantityPricings",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbProductVisibilityRules_CurrentState",
                table: "TbProductVisibilityRules",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbProductReviews_CurrentState",
                table: "TbProductReviews",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbPricingSystemSettings_CurrentState",
                table: "TbPricingSystemSettings",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbPriceHistories_CurrentState",
                table: "TbPriceHistories",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbPlatformTreasuries_CurrentState",
                table: "TbPlatformTreasuries",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbPaymentMethods_CurrentState",
                table: "TbPaymentMethods",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbPages_CurrentState",
                table: "TbPages",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbOrderShipments_CurrentState",
                table: "TbOrderShipments",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbOrderShipmentItems_CurrentState",
                table: "TbOrderShipmentItems",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbOrders_CurrentState",
                table: "TbOrders",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbOrderPayments_CurrentState",
                table: "TbOrderPayments",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbOrderDetails_CurrentState",
                table: "TbOrderDetails",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbOfferStatusHistories_CurrentState",
                table: "TbOfferStatusHistories",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbOffers_CurrentState",
                table: "TbOffers",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbOfferPriceHistories_CurrentState",
                table: "TbOfferPriceHistories",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbOfferConditions_CurrentState",
                table: "TbOfferConditions",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbOfferCombinationPricings_CurrentState",
                table: "TbOfferCombinationPricings",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbNotificationPreferences_CurrentState",
                table: "TbNotificationPreferences",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbNotificationChannels_CurrentState",
                table: "TbNotificationChannels",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbMediaContents_CurrentState",
                table: "TbMediaContents",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbLoyaltyTiers_CurrentState",
                table: "TbLoyaltyTiers",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbLoyaltyPointsTransactions_CurrentState",
                table: "TbLoyaltyPointsTransactions",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbItems_CurrentState",
                table: "TbItems",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbItemImages_CurrentState",
                table: "TbItemImages",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbItemCombinations_CurrentState",
                table: "TbItemCombinations",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbItemAttributes_CurrentState",
                table: "TbItemAttributes",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbHomepageBlocks_CurrentState",
                table: "TbHomepageBlocks",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbFlashSales_CurrentState",
                table: "TbFlashSales",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbFlashSaleProducts_CurrentState",
                table: "TbFlashSaleProducts",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbDisputes_CurrentState",
                table: "TbDisputes",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbDisputeMessages_CurrentState",
                table: "TbDisputeMessages",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbDeliveryReviews_CurrentState",
                table: "TbDeliveryReviews",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbCustomerWallets_CurrentState",
                table: "TbCustomerWallets",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbCustomerSegmentPricings_CurrentState",
                table: "TbCustomerSegmentPricings",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbCustomerLoyalties_CurrentState",
                table: "TbCustomerLoyalties",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbCustomerAddresses_CurrentState",
                table: "TbCustomerAddresses",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbCurrencies_CurrentState",
                table: "TbCurrencies",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbCouponCodes_CurrentState",
                table: "TbCouponCodes",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbCountries_CurrentState",
                table: "TbCountries",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbContentAreas_CurrentState",
                table: "TbContentAreas",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbCombinationAttributesValues_CurrentState",
                table: "TbCombinationAttributesValues",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbCombinationAttributes_CurrentState",
                table: "TbCombinationAttributes",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbCities_CurrentState",
                table: "TbCities",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbCategoryAttributes_CurrentState",
                table: "TbCategoryAttributes",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbCategories_CurrentState",
                table: "TbCategories",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbCampaignVendors_CurrentState",
                table: "TbCampaignVendors",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbCampaigns_CurrentState",
                table: "TbCampaigns",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbCampaignProducts_CurrentState",
                table: "TbCampaignProducts",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbBuyBoxHistories_CurrentState",
                table: "TbBuyBoxHistories",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbBuyBoxCalculations_CurrentState",
                table: "TbBuyBoxCalculations",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbBrands_CurrentState",
                table: "TbBrands",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbBrandRegistrationRequests_CurrentState",
                table: "TbBrandRegistrationRequests",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbBrandDocuments_CurrentState",
                table: "TbBrandDocuments",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbBlockProducts_CurrentState",
                table: "TbBlockProducts",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbAuthorizedDistributors_CurrentState",
                table: "TbAuthorizedDistributors",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbAttributeValuePriceModifiers_CurrentState",
                table: "TbAttributeValuePriceModifiers",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbAttributes_CurrentState",
                table: "TbAttributes",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbAttributeOptions_CurrentState",
                table: "TbAttributeOptions",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_CurrentState",
                table: "Notifications",
                column: "CurrentState");
        }
    }
}
