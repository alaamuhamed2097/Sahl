using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class cleanMigration2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                table: "AspNetRoleClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                table: "AspNetUserClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                table: "AspNetUserLogins");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_TbAttributeOptions_TbAttributes_AttributeId",
                table: "TbAttributeOptions");

            migrationBuilder.DropForeignKey(
                name: "FK_TbAuthorizedDistributors_AspNetUsers_VerifiedByUserId",
                table: "TbAuthorizedDistributors");

            migrationBuilder.DropForeignKey(
                name: "FK_TbAuthorizedDistributors_TbBrands_BrandId",
                table: "TbAuthorizedDistributors");

            migrationBuilder.DropForeignKey(
                name: "FK_TbAuthorizedDistributors_TbVendors_VendorId",
                table: "TbAuthorizedDistributors");

            migrationBuilder.DropForeignKey(
                name: "FK_TbBlockProducts_TbHomepageBlocks_HomepageBlockId",
                table: "TbBlockProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_TbBlockProducts_TbItems_ItemId",
                table: "TbBlockProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_TbBrandDocuments_AspNetUsers_VerifiedByUserId",
                table: "TbBrandDocuments");

            migrationBuilder.DropForeignKey(
                name: "FK_TbBrandDocuments_TbBrandRegistrationRequests_BrandRegistrationRequestId",
                table: "TbBrandDocuments");

            migrationBuilder.DropForeignKey(
                name: "FK_TbBrandRegistrationRequests_AspNetUsers_ReviewedByUserId",
                table: "TbBrandRegistrationRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_TbBrandRegistrationRequests_TbBrands_ApprovedBrandId",
                table: "TbBrandRegistrationRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_TbBrandRegistrationRequests_TbVendors_VendorId",
                table: "TbBrandRegistrationRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_TbBuyBoxCalculations_TbItems_ItemId",
                table: "TbBuyBoxCalculations");

            migrationBuilder.DropForeignKey(
                name: "FK_TbBuyBoxCalculations_TbOffers_WinningOfferId",
                table: "TbBuyBoxCalculations");

            migrationBuilder.DropForeignKey(
                name: "FK_TbBuyBoxHistories_TbItems_ItemId",
                table: "TbBuyBoxHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_TbBuyBoxHistories_TbOffers_OfferId",
                table: "TbBuyBoxHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_TbBuyBoxHistories_TbVendors_VendorId",
                table: "TbBuyBoxHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_TbCampaignProducts_AspNetUsers_ApprovedByUserId",
                table: "TbCampaignProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_TbCampaignProducts_TbCampaigns_CampaignId",
                table: "TbCampaignProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_TbCampaignProducts_TbItems_ItemId",
                table: "TbCampaignProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_TbCampaignProducts_TbVendors_VendorId",
                table: "TbCampaignProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_TbCampaignVendors_AspNetUsers_ApprovedByUserId",
                table: "TbCampaignVendors");

            migrationBuilder.DropForeignKey(
                name: "FK_TbCampaignVendors_TbCampaigns_CampaignId",
                table: "TbCampaignVendors");

            migrationBuilder.DropForeignKey(
                name: "FK_TbCampaignVendors_TbVendors_VendorId",
                table: "TbCampaignVendors");

            migrationBuilder.DropForeignKey(
                name: "FK_TbCategoryAttributes_TbAttributes_AttributeId",
                table: "TbCategoryAttributes");

            migrationBuilder.DropForeignKey(
                name: "FK_TbCategoryAttributes_TbCategories_CategoryId",
                table: "TbCategoryAttributes");

            migrationBuilder.DropForeignKey(
                name: "FK_TbCities_TbStates_StateId",
                table: "TbCities");

            migrationBuilder.DropForeignKey(
                name: "FK_TbCombinationAttributes_TbItemCombinations_ItemCombinationId",
                table: "TbCombinationAttributes");

            migrationBuilder.DropForeignKey(
                name: "FK_TbCombinationAttributesValues_TbAttributes_AttributeId",
                table: "TbCombinationAttributesValues");

            migrationBuilder.DropForeignKey(
                name: "FK_TbCombinationAttributesValues_TbCombinationAttributes_CombinationAttributeId",
                table: "TbCombinationAttributesValues");

            migrationBuilder.DropForeignKey(
                name: "FK_TbCustomerLoyalties_TbCustomers_CustomerId",
                table: "TbCustomerLoyalties");

            migrationBuilder.DropForeignKey(
                name: "FK_TbCustomerLoyalties_TbLoyaltyTiers_LoyaltyTierId",
                table: "TbCustomerLoyalties");

            migrationBuilder.DropForeignKey(
                name: "FK_TbCustomerSegmentPricings_TbOffers_OfferId",
                table: "TbCustomerSegmentPricings");

            migrationBuilder.DropForeignKey(
                name: "FK_TbCustomerWallets_TbCurrencies_CurrencyId",
                table: "TbCustomerWallets");

            migrationBuilder.DropForeignKey(
                name: "FK_TbCustomerWallets_TbCustomers_CustomerId",
                table: "TbCustomerWallets");

            migrationBuilder.DropForeignKey(
                name: "FK_TbDisputeMessages_TbDisputes_DisputeID",
                table: "TbDisputeMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_TbFBMInventories_TbItems_ItemId",
                table: "TbFBMInventories");

            migrationBuilder.DropForeignKey(
                name: "FK_TbFBMInventories_TbVendors_VendorId",
                table: "TbFBMInventories");

            migrationBuilder.DropForeignKey(
                name: "FK_TbFBMInventories_TbWarehouses_WarehouseId",
                table: "TbFBMInventories");

            migrationBuilder.DropForeignKey(
                name: "FK_TbFBMShipments_TbOrders_OrderId",
                table: "TbFBMShipments");

            migrationBuilder.DropForeignKey(
                name: "FK_TbFBMShipments_TbShippingCompanies_ShippingCompanyId",
                table: "TbFBMShipments");

            migrationBuilder.DropForeignKey(
                name: "FK_TbFBMShipments_TbWarehouses_WarehouseId",
                table: "TbFBMShipments");

            migrationBuilder.DropForeignKey(
                name: "FK_TbFlashSaleProducts_TbFlashSales_FlashSaleId",
                table: "TbFlashSaleProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_TbFlashSaleProducts_TbItems_ItemId",
                table: "TbFlashSaleProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_TbFlashSaleProducts_TbVendors_VendorId",
                table: "TbFlashSaleProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_TbFulfillmentFees_TbFulfillmentMethods_FulfillmentMethodId",
                table: "TbFulfillmentFees");

            migrationBuilder.DropForeignKey(
                name: "FK_TbItemAttribute_TbAttributes_AttributeId",
                table: "TbItemAttribute");

            migrationBuilder.DropForeignKey(
                name: "FK_TbItemAttribute_TbItems_ItemId",
                table: "TbItemAttribute");

            migrationBuilder.DropForeignKey(
                name: "FK_TbItemAttributeCombinationPricings_TbItems_ItemId",
                table: "TbItemAttributeCombinationPricings");

            migrationBuilder.DropForeignKey(
                name: "FK_TbItemCombinations_TbItems_ItemId",
                table: "TbItemCombinations");

            migrationBuilder.DropForeignKey(
                name: "FK_TbItemImages_TbItems_ItemId",
                table: "TbItemImages");

            migrationBuilder.DropForeignKey(
                name: "FK_TbItems_TbBrands_BrandId",
                table: "TbItems");

            migrationBuilder.DropForeignKey(
                name: "FK_TbItems_TbCategories_CategoryId",
                table: "TbItems");

            migrationBuilder.DropForeignKey(
                name: "FK_TbItems_TbUnits_UnitId",
                table: "TbItems");

            migrationBuilder.DropForeignKey(
                name: "FK_TbLoyaltyPointsTransactions_TbCustomerLoyalties_CustomerLoyaltyId",
                table: "TbLoyaltyPointsTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_TbLoyaltyPointsTransactions_TbOrders_OrderId",
                table: "TbLoyaltyPointsTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_TbLoyaltyPointsTransactions_TbProductReviews_ReviewId",
                table: "TbLoyaltyPointsTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_TbMediaContents_TbContentAreas_ContentAreaId",
                table: "TbMediaContents");

            migrationBuilder.DropForeignKey(
                name: "FK_TbMoitems_AspNetUsers_UserId",
                table: "TbMoitems");

            migrationBuilder.DropForeignKey(
                name: "FK_TbMortems_AspNetUsers_UserId",
                table: "TbMortems");

            migrationBuilder.DropForeignKey(
                name: "FK_TbMortems_TbOrders_OrderId",
                table: "TbMortems");

            migrationBuilder.DropForeignKey(
                name: "FK_TbMovitemsdetails_TbItemAttributeCombinationPricings_ItemAttributeCombinationPricingId",
                table: "TbMovitemsdetails");

            migrationBuilder.DropForeignKey(
                name: "FK_TbMovitemsdetails_TbItems_ItemId",
                table: "TbMovitemsdetails");

            migrationBuilder.DropForeignKey(
                name: "FK_TbMovitemsdetails_TbMoitems_MoitemId",
                table: "TbMovitemsdetails");

            migrationBuilder.DropForeignKey(
                name: "FK_TbMovitemsdetails_TbMortems_MortemId",
                table: "TbMovitemsdetails");

            migrationBuilder.DropForeignKey(
                name: "FK_TbMovitemsdetails_TbWarehouses_WarehouseId",
                table: "TbMovitemsdetails");

            migrationBuilder.DropForeignKey(
                name: "FK_TbNotificationPreferences_AspNetUsers_UserId",
                table: "TbNotificationPreferences");

            migrationBuilder.DropForeignKey(
                name: "FK_TbOfferCombinationPricings_TbItemCombinations_ItemCombinationId",
                table: "TbOfferCombinationPricings");

            migrationBuilder.DropForeignKey(
                name: "FK_TbOfferCombinationPricings_TbOffers_OfferId",
                table: "TbOfferCombinationPricings");

            migrationBuilder.DropForeignKey(
                name: "FK_TbOffers_AspNetUsers_UserId",
                table: "TbOffers");

            migrationBuilder.DropForeignKey(
                name: "FK_TbOffers_TbItems_ItemId",
                table: "TbOffers");

            migrationBuilder.DropForeignKey(
                name: "FK_TbOffers_TbOfferConditions_TbOfferConditionId",
                table: "TbOffers");

            migrationBuilder.DropForeignKey(
                name: "FK_TbOffers_TbWarranties_WarrantyId",
                table: "TbOffers");

            migrationBuilder.DropForeignKey(
                name: "FK_TbOrderDetails_TbItems_ItemId",
                table: "TbOrderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_TbOrderDetails_TbOrders_OrderId",
                table: "TbOrderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_TbOrders_AspNetUsers_UserId",
                table: "TbOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_TbOrders_TbCouponCodes_CouponId",
                table: "TbOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_TbOrders_TbShippingCompanies_ShippingCompanyId",
                table: "TbOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_TbPlatformTreasuries_TbCurrencies_CurrencyId",
                table: "TbPlatformTreasuries");

            migrationBuilder.DropForeignKey(
                name: "FK_TbPriceHistories_AspNetUsers_ChangedByUserId",
                table: "TbPriceHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_TbPriceHistories_TbOffers_OfferId",
                table: "TbPriceHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_TbProductVisibilityRules_AspNetUsers_SuppressedByUserId",
                table: "TbProductVisibilityRules");

            migrationBuilder.DropForeignKey(
                name: "FK_TbProductVisibilityRules_TbItems_ItemId",
                table: "TbProductVisibilityRules");

            migrationBuilder.DropForeignKey(
                name: "FK_TbQuantityPricings_TbOffers_OfferId",
                table: "TbQuantityPricings");

            migrationBuilder.DropForeignKey(
                name: "FK_TbRefundRequests_AspNetUsers_AdminUserId",
                table: "TbRefundRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_TbRefundRequests_TbOrders_OrderId",
                table: "TbRefundRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_TbRequestComments_AspNetUsers_UserId",
                table: "TbRequestComments");

            migrationBuilder.DropForeignKey(
                name: "FK_TbRequestComments_TbSellerRequests_SellerRequestId",
                table: "TbRequestComments");

            migrationBuilder.DropForeignKey(
                name: "FK_TbRequestDocuments_AspNetUsers_UploadedByUserId",
                table: "TbRequestDocuments");

            migrationBuilder.DropForeignKey(
                name: "FK_TbRequestDocuments_TbSellerRequests_SellerRequestId",
                table: "TbRequestDocuments");

            migrationBuilder.DropForeignKey(
                name: "FK_TbReviewVotes_TbProductReviews_ReviewID",
                table: "TbReviewVotes");

            migrationBuilder.DropForeignKey(
                name: "FK_TbSellerPerformanceMetrics_TbVendors_VendorId",
                table: "TbSellerPerformanceMetrics");

            migrationBuilder.DropForeignKey(
                name: "FK_TbSellerRequests_AspNetUsers_ReviewedByUserId",
                table: "TbSellerRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_TbSellerRequests_TbVendors_VendorId",
                table: "TbSellerRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_TbSellerTierBenefits_TbSellerTiers_SellerTierId",
                table: "TbSellerTierBenefits");

            migrationBuilder.DropForeignKey(
                name: "FK_TbShippingDetails_TbCities_CityId",
                table: "TbShippingDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_TbShippingDetails_TbOffers_OfferId",
                table: "TbShippingDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_TbStates_TbCountries_CountryId",
                table: "TbStates");

            migrationBuilder.DropForeignKey(
                name: "FK_TbSupportTicketMessages_TbSupportTickets_SupportTicketId",
                table: "TbSupportTicketMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_TbSuppressionReasons_TbProductVisibilityRules_ProductVisibilityRuleId",
                table: "TbSuppressionReasons");

            migrationBuilder.DropForeignKey(
                name: "FK_TbUnitConversions_TbUnits_FromUnitId",
                table: "TbUnitConversions");

            migrationBuilder.DropForeignKey(
                name: "FK_TbUnitConversions_TbUnits_ToUnitId",
                table: "TbUnitConversions");

            migrationBuilder.DropForeignKey(
                name: "FK_TbUserNotifications_AspNetUsers_UserId",
                table: "TbUserNotifications");

            migrationBuilder.DropForeignKey(
                name: "FK_TbUserNotifications_TbNotifications_NotificationId",
                table: "TbUserNotifications");

            migrationBuilder.DropForeignKey(
                name: "FK_TbUserOfferRatings_AspNetUsers_UserId",
                table: "TbUserOfferRatings");

            migrationBuilder.DropForeignKey(
                name: "FK_TbUserOfferRatings_TbOffers_OfferId",
                table: "TbUserOfferRatings");

            migrationBuilder.DropForeignKey(
                name: "FK_TbVendorTierHistories_TbSellerTiers_SellerTierId",
                table: "TbVendorTierHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_TbVendorTierHistories_TbVendors_VendorId",
                table: "TbVendorTierHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_TbVendorWallets_TbCurrencies_CurrencyId",
                table: "TbVendorWallets");

            migrationBuilder.DropForeignKey(
                name: "FK_TbVendorWallets_TbVendors_VendorId",
                table: "TbVendorWallets");

            migrationBuilder.DropForeignKey(
                name: "FK_TbVisibilityLogs_AspNetUsers_ChangedByUserId",
                table: "TbVisibilityLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_TbVisibilityLogs_TbItems_ItemId",
                table: "TbVisibilityLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_TbWalletTransactions_AspNetUsers_ProcessedByUserId",
                table: "TbWalletTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_TbWalletTransactions_TbCustomerWallets_CustomerWalletId",
                table: "TbWalletTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_TbWalletTransactions_TbOrders_OrderId",
                table: "TbWalletTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_TbWalletTransactions_TbRefundRequests_RefundId",
                table: "TbWalletTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_TbWalletTransactions_TbVendorWallets_VendorWalletId",
                table: "TbWalletTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_TbWarranties_TbCities_CityId",
                table: "TbWarranties");

            migrationBuilder.AlterColumn<string>(
                name: "ProcessedByUserId",
                table: "TbWalletTransactions",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ChangedByUserId",
                table: "TbVisibilityLogs",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "TbUserOfferRatings",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "TbUserNotifications",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<string>(
                name: "ReviewedByUserId",
                table: "TbSellerRequests",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UploadedByUserId",
                table: "TbRequestDocuments",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "TbRequestComments",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<string>(
                name: "AdminUserId",
                table: "TbRefundRequests",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SuppressedByUserId",
                table: "TbProductVisibilityRules",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ChangedByUserId",
                table: "TbPriceHistories",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "TbOrders",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "TbOffers",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "TbNotificationPreferences",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "TbMortems",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "TbMoitems",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ApprovedByUserId",
                table: "TbCampaignVendors",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ApprovedByUserId",
                table: "TbCampaignProducts",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ReviewedByUserId",
                table: "TbBrandRegistrationRequests",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "VerifiedByUserId",
                table: "TbBrandDocuments",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "VerifiedByUserId",
                table: "TbAuthorizedDistributors",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "AspNetUserTokens",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "AspNetUsers",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<string>(
                name: "RoleId",
                table: "AspNetUserRoles",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "AspNetUserRoles",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "AspNetUserLogins",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "AspNetUserClaims",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "AspNetRoles",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<string>(
                name: "RoleId",
                table: "AspNetRoleClaims",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                table: "AspNetUserClaims",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                table: "AspNetUserLogins",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbAttributeOptions_TbAttributes_AttributeId",
                table: "TbAttributeOptions",
                column: "AttributeId",
                principalTable: "TbAttributes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbAuthorizedDistributors_AspNetUsers_VerifiedByUserId",
                table: "TbAuthorizedDistributors",
                column: "VerifiedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TbAuthorizedDistributors_TbBrands_BrandId",
                table: "TbAuthorizedDistributors",
                column: "BrandId",
                principalTable: "TbBrands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbAuthorizedDistributors_TbVendors_VendorId",
                table: "TbAuthorizedDistributors",
                column: "VendorId",
                principalTable: "TbVendors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbBlockProducts_TbHomepageBlocks_HomepageBlockId",
                table: "TbBlockProducts",
                column: "HomepageBlockId",
                principalTable: "TbHomepageBlocks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbBlockProducts_TbItems_ItemId",
                table: "TbBlockProducts",
                column: "ItemId",
                principalTable: "TbItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbBrandDocuments_AspNetUsers_VerifiedByUserId",
                table: "TbBrandDocuments",
                column: "VerifiedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TbBrandDocuments_TbBrandRegistrationRequests_BrandRegistrationRequestId",
                table: "TbBrandDocuments",
                column: "BrandRegistrationRequestId",
                principalTable: "TbBrandRegistrationRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbBrandRegistrationRequests_AspNetUsers_ReviewedByUserId",
                table: "TbBrandRegistrationRequests",
                column: "ReviewedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TbBrandRegistrationRequests_TbBrands_ApprovedBrandId",
                table: "TbBrandRegistrationRequests",
                column: "ApprovedBrandId",
                principalTable: "TbBrands",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TbBrandRegistrationRequests_TbVendors_VendorId",
                table: "TbBrandRegistrationRequests",
                column: "VendorId",
                principalTable: "TbVendors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbBuyBoxCalculations_TbItems_ItemId",
                table: "TbBuyBoxCalculations",
                column: "ItemId",
                principalTable: "TbItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbBuyBoxCalculations_TbOffers_WinningOfferId",
                table: "TbBuyBoxCalculations",
                column: "WinningOfferId",
                principalTable: "TbOffers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbBuyBoxHistories_TbItems_ItemId",
                table: "TbBuyBoxHistories",
                column: "ItemId",
                principalTable: "TbItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbBuyBoxHistories_TbOffers_OfferId",
                table: "TbBuyBoxHistories",
                column: "OfferId",
                principalTable: "TbOffers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbBuyBoxHistories_TbVendors_VendorId",
                table: "TbBuyBoxHistories",
                column: "VendorId",
                principalTable: "TbVendors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbCampaignProducts_AspNetUsers_ApprovedByUserId",
                table: "TbCampaignProducts",
                column: "ApprovedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TbCampaignProducts_TbCampaigns_CampaignId",
                table: "TbCampaignProducts",
                column: "CampaignId",
                principalTable: "TbCampaigns",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbCampaignProducts_TbItems_ItemId",
                table: "TbCampaignProducts",
                column: "ItemId",
                principalTable: "TbItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbCampaignProducts_TbVendors_VendorId",
                table: "TbCampaignProducts",
                column: "VendorId",
                principalTable: "TbVendors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbCampaignVendors_AspNetUsers_ApprovedByUserId",
                table: "TbCampaignVendors",
                column: "ApprovedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TbCampaignVendors_TbCampaigns_CampaignId",
                table: "TbCampaignVendors",
                column: "CampaignId",
                principalTable: "TbCampaigns",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbCampaignVendors_TbVendors_VendorId",
                table: "TbCampaignVendors",
                column: "VendorId",
                principalTable: "TbVendors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbCategoryAttributes_TbAttributes_AttributeId",
                table: "TbCategoryAttributes",
                column: "AttributeId",
                principalTable: "TbAttributes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbCategoryAttributes_TbCategories_CategoryId",
                table: "TbCategoryAttributes",
                column: "CategoryId",
                principalTable: "TbCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbCities_TbStates_StateId",
                table: "TbCities",
                column: "StateId",
                principalTable: "TbStates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbCombinationAttributes_TbItemCombinations_ItemCombinationId",
                table: "TbCombinationAttributes",
                column: "ItemCombinationId",
                principalTable: "TbItemCombinations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbCombinationAttributesValues_TbAttributes_AttributeId",
                table: "TbCombinationAttributesValues",
                column: "AttributeId",
                principalTable: "TbAttributes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbCombinationAttributesValues_TbCombinationAttributes_CombinationAttributeId",
                table: "TbCombinationAttributesValues",
                column: "CombinationAttributeId",
                principalTable: "TbCombinationAttributes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbCustomerLoyalties_TbCustomers_CustomerId",
                table: "TbCustomerLoyalties",
                column: "CustomerId",
                principalTable: "TbCustomers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbCustomerLoyalties_TbLoyaltyTiers_LoyaltyTierId",
                table: "TbCustomerLoyalties",
                column: "LoyaltyTierId",
                principalTable: "TbLoyaltyTiers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbCustomerSegmentPricings_TbOffers_OfferId",
                table: "TbCustomerSegmentPricings",
                column: "OfferId",
                principalTable: "TbOffers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbCustomerWallets_TbCurrencies_CurrencyId",
                table: "TbCustomerWallets",
                column: "CurrencyId",
                principalTable: "TbCurrencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbCustomerWallets_TbCustomers_CustomerId",
                table: "TbCustomerWallets",
                column: "CustomerId",
                principalTable: "TbCustomers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbDisputeMessages_TbDisputes_DisputeID",
                table: "TbDisputeMessages",
                column: "DisputeID",
                principalTable: "TbDisputes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbFBMInventories_TbItems_ItemId",
                table: "TbFBMInventories",
                column: "ItemId",
                principalTable: "TbItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbFBMInventories_TbVendors_VendorId",
                table: "TbFBMInventories",
                column: "VendorId",
                principalTable: "TbVendors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbFBMInventories_TbWarehouses_WarehouseId",
                table: "TbFBMInventories",
                column: "WarehouseId",
                principalTable: "TbWarehouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbFBMShipments_TbOrders_OrderId",
                table: "TbFBMShipments",
                column: "OrderId",
                principalTable: "TbOrders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbFBMShipments_TbShippingCompanies_ShippingCompanyId",
                table: "TbFBMShipments",
                column: "ShippingCompanyId",
                principalTable: "TbShippingCompanies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbFBMShipments_TbWarehouses_WarehouseId",
                table: "TbFBMShipments",
                column: "WarehouseId",
                principalTable: "TbWarehouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbFlashSaleProducts_TbFlashSales_FlashSaleId",
                table: "TbFlashSaleProducts",
                column: "FlashSaleId",
                principalTable: "TbFlashSales",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbFlashSaleProducts_TbItems_ItemId",
                table: "TbFlashSaleProducts",
                column: "ItemId",
                principalTable: "TbItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbFlashSaleProducts_TbVendors_VendorId",
                table: "TbFlashSaleProducts",
                column: "VendorId",
                principalTable: "TbVendors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbFulfillmentFees_TbFulfillmentMethods_FulfillmentMethodId",
                table: "TbFulfillmentFees",
                column: "FulfillmentMethodId",
                principalTable: "TbFulfillmentMethods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbItemAttribute_TbAttributes_AttributeId",
                table: "TbItemAttribute",
                column: "AttributeId",
                principalTable: "TbAttributes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbItemAttribute_TbItems_ItemId",
                table: "TbItemAttribute",
                column: "ItemId",
                principalTable: "TbItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbItemAttributeCombinationPricings_TbItems_ItemId",
                table: "TbItemAttributeCombinationPricings",
                column: "ItemId",
                principalTable: "TbItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbItemCombinations_TbItems_ItemId",
                table: "TbItemCombinations",
                column: "ItemId",
                principalTable: "TbItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbItemImages_TbItems_ItemId",
                table: "TbItemImages",
                column: "ItemId",
                principalTable: "TbItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbItems_TbBrands_BrandId",
                table: "TbItems",
                column: "BrandId",
                principalTable: "TbBrands",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TbItems_TbCategories_CategoryId",
                table: "TbItems",
                column: "CategoryId",
                principalTable: "TbCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbItems_TbUnits_UnitId",
                table: "TbItems",
                column: "UnitId",
                principalTable: "TbUnits",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TbLoyaltyPointsTransactions_TbCustomerLoyalties_CustomerLoyaltyId",
                table: "TbLoyaltyPointsTransactions",
                column: "CustomerLoyaltyId",
                principalTable: "TbCustomerLoyalties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbLoyaltyPointsTransactions_TbOrders_OrderId",
                table: "TbLoyaltyPointsTransactions",
                column: "OrderId",
                principalTable: "TbOrders",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TbLoyaltyPointsTransactions_TbProductReviews_ReviewId",
                table: "TbLoyaltyPointsTransactions",
                column: "ReviewId",
                principalTable: "TbProductReviews",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TbMediaContents_TbContentAreas_ContentAreaId",
                table: "TbMediaContents",
                column: "ContentAreaId",
                principalTable: "TbContentAreas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbMoitems_AspNetUsers_UserId",
                table: "TbMoitems",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TbMortems_AspNetUsers_UserId",
                table: "TbMortems",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TbMortems_TbOrders_OrderId",
                table: "TbMortems",
                column: "OrderId",
                principalTable: "TbOrders",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TbMovitemsdetails_TbItemAttributeCombinationPricings_ItemAttributeCombinationPricingId",
                table: "TbMovitemsdetails",
                column: "ItemAttributeCombinationPricingId",
                principalTable: "TbItemAttributeCombinationPricings",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TbMovitemsdetails_TbItems_ItemId",
                table: "TbMovitemsdetails",
                column: "ItemId",
                principalTable: "TbItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbMovitemsdetails_TbMoitems_MoitemId",
                table: "TbMovitemsdetails",
                column: "MoitemId",
                principalTable: "TbMoitems",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TbMovitemsdetails_TbMortems_MortemId",
                table: "TbMovitemsdetails",
                column: "MortemId",
                principalTable: "TbMortems",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TbMovitemsdetails_TbWarehouses_WarehouseId",
                table: "TbMovitemsdetails",
                column: "WarehouseId",
                principalTable: "TbWarehouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbNotificationPreferences_AspNetUsers_UserId",
                table: "TbNotificationPreferences",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbOfferCombinationPricings_TbItemCombinations_ItemCombinationId",
                table: "TbOfferCombinationPricings",
                column: "ItemCombinationId",
                principalTable: "TbItemCombinations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbOfferCombinationPricings_TbOffers_OfferId",
                table: "TbOfferCombinationPricings",
                column: "OfferId",
                principalTable: "TbOffers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbOffers_AspNetUsers_UserId",
                table: "TbOffers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbOffers_TbItems_ItemId",
                table: "TbOffers",
                column: "ItemId",
                principalTable: "TbItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbOffers_TbOfferConditions_TbOfferConditionId",
                table: "TbOffers",
                column: "TbOfferConditionId",
                principalTable: "TbOfferConditions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TbOffers_TbWarranties_WarrantyId",
                table: "TbOffers",
                column: "WarrantyId",
                principalTable: "TbWarranties",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TbOrderDetails_TbItems_ItemId",
                table: "TbOrderDetails",
                column: "ItemId",
                principalTable: "TbItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbOrderDetails_TbOrders_OrderId",
                table: "TbOrderDetails",
                column: "OrderId",
                principalTable: "TbOrders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbOrders_AspNetUsers_UserId",
                table: "TbOrders",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbOrders_TbCouponCodes_CouponId",
                table: "TbOrders",
                column: "CouponId",
                principalTable: "TbCouponCodes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TbOrders_TbShippingCompanies_ShippingCompanyId",
                table: "TbOrders",
                column: "ShippingCompanyId",
                principalTable: "TbShippingCompanies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TbPlatformTreasuries_TbCurrencies_CurrencyId",
                table: "TbPlatformTreasuries",
                column: "CurrencyId",
                principalTable: "TbCurrencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbPriceHistories_AspNetUsers_ChangedByUserId",
                table: "TbPriceHistories",
                column: "ChangedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TbPriceHistories_TbOffers_OfferId",
                table: "TbPriceHistories",
                column: "OfferId",
                principalTable: "TbOffers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbProductVisibilityRules_AspNetUsers_SuppressedByUserId",
                table: "TbProductVisibilityRules",
                column: "SuppressedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TbProductVisibilityRules_TbItems_ItemId",
                table: "TbProductVisibilityRules",
                column: "ItemId",
                principalTable: "TbItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbQuantityPricings_TbOffers_OfferId",
                table: "TbQuantityPricings",
                column: "OfferId",
                principalTable: "TbOffers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbRefundRequests_AspNetUsers_AdminUserId",
                table: "TbRefundRequests",
                column: "AdminUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TbRefundRequests_TbOrders_OrderId",
                table: "TbRefundRequests",
                column: "OrderId",
                principalTable: "TbOrders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbRequestComments_AspNetUsers_UserId",
                table: "TbRequestComments",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbRequestComments_TbSellerRequests_SellerRequestId",
                table: "TbRequestComments",
                column: "SellerRequestId",
                principalTable: "TbSellerRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbRequestDocuments_AspNetUsers_UploadedByUserId",
                table: "TbRequestDocuments",
                column: "UploadedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TbRequestDocuments_TbSellerRequests_SellerRequestId",
                table: "TbRequestDocuments",
                column: "SellerRequestId",
                principalTable: "TbSellerRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbReviewVotes_TbProductReviews_ReviewID",
                table: "TbReviewVotes",
                column: "ReviewID",
                principalTable: "TbProductReviews",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbSellerPerformanceMetrics_TbVendors_VendorId",
                table: "TbSellerPerformanceMetrics",
                column: "VendorId",
                principalTable: "TbVendors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbSellerRequests_AspNetUsers_ReviewedByUserId",
                table: "TbSellerRequests",
                column: "ReviewedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TbSellerRequests_TbVendors_VendorId",
                table: "TbSellerRequests",
                column: "VendorId",
                principalTable: "TbVendors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbSellerTierBenefits_TbSellerTiers_SellerTierId",
                table: "TbSellerTierBenefits",
                column: "SellerTierId",
                principalTable: "TbSellerTiers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbShippingDetails_TbCities_CityId",
                table: "TbShippingDetails",
                column: "CityId",
                principalTable: "TbCities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbShippingDetails_TbOffers_OfferId",
                table: "TbShippingDetails",
                column: "OfferId",
                principalTable: "TbOffers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbStates_TbCountries_CountryId",
                table: "TbStates",
                column: "CountryId",
                principalTable: "TbCountries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbSupportTicketMessages_TbSupportTickets_SupportTicketId",
                table: "TbSupportTicketMessages",
                column: "SupportTicketId",
                principalTable: "TbSupportTickets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbSuppressionReasons_TbProductVisibilityRules_ProductVisibilityRuleId",
                table: "TbSuppressionReasons",
                column: "ProductVisibilityRuleId",
                principalTable: "TbProductVisibilityRules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbUnitConversions_TbUnits_FromUnitId",
                table: "TbUnitConversions",
                column: "FromUnitId",
                principalTable: "TbUnits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbUnitConversions_TbUnits_ToUnitId",
                table: "TbUnitConversions",
                column: "ToUnitId",
                principalTable: "TbUnits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbUserNotifications_AspNetUsers_UserId",
                table: "TbUserNotifications",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbUserNotifications_TbNotifications_NotificationId",
                table: "TbUserNotifications",
                column: "NotificationId",
                principalTable: "TbNotifications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbUserOfferRatings_AspNetUsers_UserId",
                table: "TbUserOfferRatings",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbUserOfferRatings_TbOffers_OfferId",
                table: "TbUserOfferRatings",
                column: "OfferId",
                principalTable: "TbOffers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbVendorTierHistories_TbSellerTiers_SellerTierId",
                table: "TbVendorTierHistories",
                column: "SellerTierId",
                principalTable: "TbSellerTiers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbVendorTierHistories_TbVendors_VendorId",
                table: "TbVendorTierHistories",
                column: "VendorId",
                principalTable: "TbVendors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbVendorWallets_TbCurrencies_CurrencyId",
                table: "TbVendorWallets",
                column: "CurrencyId",
                principalTable: "TbCurrencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbVendorWallets_TbVendors_VendorId",
                table: "TbVendorWallets",
                column: "VendorId",
                principalTable: "TbVendors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbVisibilityLogs_AspNetUsers_ChangedByUserId",
                table: "TbVisibilityLogs",
                column: "ChangedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TbVisibilityLogs_TbItems_ItemId",
                table: "TbVisibilityLogs",
                column: "ItemId",
                principalTable: "TbItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbWalletTransactions_AspNetUsers_ProcessedByUserId",
                table: "TbWalletTransactions",
                column: "ProcessedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TbWalletTransactions_TbCustomerWallets_CustomerWalletId",
                table: "TbWalletTransactions",
                column: "CustomerWalletId",
                principalTable: "TbCustomerWallets",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TbWalletTransactions_TbOrders_OrderId",
                table: "TbWalletTransactions",
                column: "OrderId",
                principalTable: "TbOrders",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TbWalletTransactions_TbRefundRequests_RefundId",
                table: "TbWalletTransactions",
                column: "RefundId",
                principalTable: "TbRefundRequests",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TbWalletTransactions_TbVendorWallets_VendorWalletId",
                table: "TbWalletTransactions",
                column: "VendorWalletId",
                principalTable: "TbVendorWallets",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TbWarranties_TbCities_CityId",
                table: "TbWarranties",
                column: "CityId",
                principalTable: "TbCities",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                table: "AspNetRoleClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                table: "AspNetUserClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                table: "AspNetUserLogins");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_TbAttributeOptions_TbAttributes_AttributeId",
                table: "TbAttributeOptions");

            migrationBuilder.DropForeignKey(
                name: "FK_TbAuthorizedDistributors_AspNetUsers_VerifiedByUserId",
                table: "TbAuthorizedDistributors");

            migrationBuilder.DropForeignKey(
                name: "FK_TbAuthorizedDistributors_TbBrands_BrandId",
                table: "TbAuthorizedDistributors");

            migrationBuilder.DropForeignKey(
                name: "FK_TbAuthorizedDistributors_TbVendors_VendorId",
                table: "TbAuthorizedDistributors");

            migrationBuilder.DropForeignKey(
                name: "FK_TbBlockProducts_TbHomepageBlocks_HomepageBlockId",
                table: "TbBlockProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_TbBlockProducts_TbItems_ItemId",
                table: "TbBlockProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_TbBrandDocuments_AspNetUsers_VerifiedByUserId",
                table: "TbBrandDocuments");

            migrationBuilder.DropForeignKey(
                name: "FK_TbBrandDocuments_TbBrandRegistrationRequests_BrandRegistrationRequestId",
                table: "TbBrandDocuments");

            migrationBuilder.DropForeignKey(
                name: "FK_TbBrandRegistrationRequests_AspNetUsers_ReviewedByUserId",
                table: "TbBrandRegistrationRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_TbBrandRegistrationRequests_TbBrands_ApprovedBrandId",
                table: "TbBrandRegistrationRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_TbBrandRegistrationRequests_TbVendors_VendorId",
                table: "TbBrandRegistrationRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_TbBuyBoxCalculations_TbItems_ItemId",
                table: "TbBuyBoxCalculations");

            migrationBuilder.DropForeignKey(
                name: "FK_TbBuyBoxCalculations_TbOffers_WinningOfferId",
                table: "TbBuyBoxCalculations");

            migrationBuilder.DropForeignKey(
                name: "FK_TbBuyBoxHistories_TbItems_ItemId",
                table: "TbBuyBoxHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_TbBuyBoxHistories_TbOffers_OfferId",
                table: "TbBuyBoxHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_TbBuyBoxHistories_TbVendors_VendorId",
                table: "TbBuyBoxHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_TbCampaignProducts_AspNetUsers_ApprovedByUserId",
                table: "TbCampaignProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_TbCampaignProducts_TbCampaigns_CampaignId",
                table: "TbCampaignProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_TbCampaignProducts_TbItems_ItemId",
                table: "TbCampaignProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_TbCampaignProducts_TbVendors_VendorId",
                table: "TbCampaignProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_TbCampaignVendors_AspNetUsers_ApprovedByUserId",
                table: "TbCampaignVendors");

            migrationBuilder.DropForeignKey(
                name: "FK_TbCampaignVendors_TbCampaigns_CampaignId",
                table: "TbCampaignVendors");

            migrationBuilder.DropForeignKey(
                name: "FK_TbCampaignVendors_TbVendors_VendorId",
                table: "TbCampaignVendors");

            migrationBuilder.DropForeignKey(
                name: "FK_TbCategoryAttributes_TbAttributes_AttributeId",
                table: "TbCategoryAttributes");

            migrationBuilder.DropForeignKey(
                name: "FK_TbCategoryAttributes_TbCategories_CategoryId",
                table: "TbCategoryAttributes");

            migrationBuilder.DropForeignKey(
                name: "FK_TbCities_TbStates_StateId",
                table: "TbCities");

            migrationBuilder.DropForeignKey(
                name: "FK_TbCombinationAttributes_TbItemCombinations_ItemCombinationId",
                table: "TbCombinationAttributes");

            migrationBuilder.DropForeignKey(
                name: "FK_TbCombinationAttributesValues_TbAttributes_AttributeId",
                table: "TbCombinationAttributesValues");

            migrationBuilder.DropForeignKey(
                name: "FK_TbCombinationAttributesValues_TbCombinationAttributes_CombinationAttributeId",
                table: "TbCombinationAttributesValues");

            migrationBuilder.DropForeignKey(
                name: "FK_TbCustomerLoyalties_TbCustomers_CustomerId",
                table: "TbCustomerLoyalties");

            migrationBuilder.DropForeignKey(
                name: "FK_TbCustomerLoyalties_TbLoyaltyTiers_LoyaltyTierId",
                table: "TbCustomerLoyalties");

            migrationBuilder.DropForeignKey(
                name: "FK_TbCustomerSegmentPricings_TbOffers_OfferId",
                table: "TbCustomerSegmentPricings");

            migrationBuilder.DropForeignKey(
                name: "FK_TbCustomerWallets_TbCurrencies_CurrencyId",
                table: "TbCustomerWallets");

            migrationBuilder.DropForeignKey(
                name: "FK_TbCustomerWallets_TbCustomers_CustomerId",
                table: "TbCustomerWallets");

            migrationBuilder.DropForeignKey(
                name: "FK_TbDisputeMessages_TbDisputes_DisputeID",
                table: "TbDisputeMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_TbFBMInventories_TbItems_ItemId",
                table: "TbFBMInventories");

            migrationBuilder.DropForeignKey(
                name: "FK_TbFBMInventories_TbVendors_VendorId",
                table: "TbFBMInventories");

            migrationBuilder.DropForeignKey(
                name: "FK_TbFBMInventories_TbWarehouses_WarehouseId",
                table: "TbFBMInventories");

            migrationBuilder.DropForeignKey(
                name: "FK_TbFBMShipments_TbOrders_OrderId",
                table: "TbFBMShipments");

            migrationBuilder.DropForeignKey(
                name: "FK_TbFBMShipments_TbShippingCompanies_ShippingCompanyId",
                table: "TbFBMShipments");

            migrationBuilder.DropForeignKey(
                name: "FK_TbFBMShipments_TbWarehouses_WarehouseId",
                table: "TbFBMShipments");

            migrationBuilder.DropForeignKey(
                name: "FK_TbFlashSaleProducts_TbFlashSales_FlashSaleId",
                table: "TbFlashSaleProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_TbFlashSaleProducts_TbItems_ItemId",
                table: "TbFlashSaleProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_TbFlashSaleProducts_TbVendors_VendorId",
                table: "TbFlashSaleProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_TbFulfillmentFees_TbFulfillmentMethods_FulfillmentMethodId",
                table: "TbFulfillmentFees");

            migrationBuilder.DropForeignKey(
                name: "FK_TbItemAttribute_TbAttributes_AttributeId",
                table: "TbItemAttribute");

            migrationBuilder.DropForeignKey(
                name: "FK_TbItemAttribute_TbItems_ItemId",
                table: "TbItemAttribute");

            migrationBuilder.DropForeignKey(
                name: "FK_TbItemAttributeCombinationPricings_TbItems_ItemId",
                table: "TbItemAttributeCombinationPricings");

            migrationBuilder.DropForeignKey(
                name: "FK_TbItemCombinations_TbItems_ItemId",
                table: "TbItemCombinations");

            migrationBuilder.DropForeignKey(
                name: "FK_TbItemImages_TbItems_ItemId",
                table: "TbItemImages");

            migrationBuilder.DropForeignKey(
                name: "FK_TbItems_TbBrands_BrandId",
                table: "TbItems");

            migrationBuilder.DropForeignKey(
                name: "FK_TbItems_TbCategories_CategoryId",
                table: "TbItems");

            migrationBuilder.DropForeignKey(
                name: "FK_TbItems_TbUnits_UnitId",
                table: "TbItems");

            migrationBuilder.DropForeignKey(
                name: "FK_TbLoyaltyPointsTransactions_TbCustomerLoyalties_CustomerLoyaltyId",
                table: "TbLoyaltyPointsTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_TbLoyaltyPointsTransactions_TbOrders_OrderId",
                table: "TbLoyaltyPointsTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_TbLoyaltyPointsTransactions_TbProductReviews_ReviewId",
                table: "TbLoyaltyPointsTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_TbMediaContents_TbContentAreas_ContentAreaId",
                table: "TbMediaContents");

            migrationBuilder.DropForeignKey(
                name: "FK_TbMoitems_AspNetUsers_UserId",
                table: "TbMoitems");

            migrationBuilder.DropForeignKey(
                name: "FK_TbMortems_AspNetUsers_UserId",
                table: "TbMortems");

            migrationBuilder.DropForeignKey(
                name: "FK_TbMortems_TbOrders_OrderId",
                table: "TbMortems");

            migrationBuilder.DropForeignKey(
                name: "FK_TbMovitemsdetails_TbItemAttributeCombinationPricings_ItemAttributeCombinationPricingId",
                table: "TbMovitemsdetails");

            migrationBuilder.DropForeignKey(
                name: "FK_TbMovitemsdetails_TbItems_ItemId",
                table: "TbMovitemsdetails");

            migrationBuilder.DropForeignKey(
                name: "FK_TbMovitemsdetails_TbMoitems_MoitemId",
                table: "TbMovitemsdetails");

            migrationBuilder.DropForeignKey(
                name: "FK_TbMovitemsdetails_TbMortems_MortemId",
                table: "TbMovitemsdetails");

            migrationBuilder.DropForeignKey(
                name: "FK_TbMovitemsdetails_TbWarehouses_WarehouseId",
                table: "TbMovitemsdetails");

            migrationBuilder.DropForeignKey(
                name: "FK_TbNotificationPreferences_AspNetUsers_UserId",
                table: "TbNotificationPreferences");

            migrationBuilder.DropForeignKey(
                name: "FK_TbOfferCombinationPricings_TbItemCombinations_ItemCombinationId",
                table: "TbOfferCombinationPricings");

            migrationBuilder.DropForeignKey(
                name: "FK_TbOfferCombinationPricings_TbOffers_OfferId",
                table: "TbOfferCombinationPricings");

            migrationBuilder.DropForeignKey(
                name: "FK_TbOffers_AspNetUsers_UserId",
                table: "TbOffers");

            migrationBuilder.DropForeignKey(
                name: "FK_TbOffers_TbItems_ItemId",
                table: "TbOffers");

            migrationBuilder.DropForeignKey(
                name: "FK_TbOffers_TbOfferConditions_TbOfferConditionId",
                table: "TbOffers");

            migrationBuilder.DropForeignKey(
                name: "FK_TbOffers_TbWarranties_WarrantyId",
                table: "TbOffers");

            migrationBuilder.DropForeignKey(
                name: "FK_TbOrderDetails_TbItems_ItemId",
                table: "TbOrderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_TbOrderDetails_TbOrders_OrderId",
                table: "TbOrderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_TbOrders_AspNetUsers_UserId",
                table: "TbOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_TbOrders_TbCouponCodes_CouponId",
                table: "TbOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_TbOrders_TbShippingCompanies_ShippingCompanyId",
                table: "TbOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_TbPlatformTreasuries_TbCurrencies_CurrencyId",
                table: "TbPlatformTreasuries");

            migrationBuilder.DropForeignKey(
                name: "FK_TbPriceHistories_AspNetUsers_ChangedByUserId",
                table: "TbPriceHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_TbPriceHistories_TbOffers_OfferId",
                table: "TbPriceHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_TbProductVisibilityRules_AspNetUsers_SuppressedByUserId",
                table: "TbProductVisibilityRules");

            migrationBuilder.DropForeignKey(
                name: "FK_TbProductVisibilityRules_TbItems_ItemId",
                table: "TbProductVisibilityRules");

            migrationBuilder.DropForeignKey(
                name: "FK_TbQuantityPricings_TbOffers_OfferId",
                table: "TbQuantityPricings");

            migrationBuilder.DropForeignKey(
                name: "FK_TbRefundRequests_AspNetUsers_AdminUserId",
                table: "TbRefundRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_TbRefundRequests_TbOrders_OrderId",
                table: "TbRefundRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_TbRequestComments_AspNetUsers_UserId",
                table: "TbRequestComments");

            migrationBuilder.DropForeignKey(
                name: "FK_TbRequestComments_TbSellerRequests_SellerRequestId",
                table: "TbRequestComments");

            migrationBuilder.DropForeignKey(
                name: "FK_TbRequestDocuments_AspNetUsers_UploadedByUserId",
                table: "TbRequestDocuments");

            migrationBuilder.DropForeignKey(
                name: "FK_TbRequestDocuments_TbSellerRequests_SellerRequestId",
                table: "TbRequestDocuments");

            migrationBuilder.DropForeignKey(
                name: "FK_TbReviewVotes_TbProductReviews_ReviewID",
                table: "TbReviewVotes");

            migrationBuilder.DropForeignKey(
                name: "FK_TbSellerPerformanceMetrics_TbVendors_VendorId",
                table: "TbSellerPerformanceMetrics");

            migrationBuilder.DropForeignKey(
                name: "FK_TbSellerRequests_AspNetUsers_ReviewedByUserId",
                table: "TbSellerRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_TbSellerRequests_TbVendors_VendorId",
                table: "TbSellerRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_TbSellerTierBenefits_TbSellerTiers_SellerTierId",
                table: "TbSellerTierBenefits");

            migrationBuilder.DropForeignKey(
                name: "FK_TbShippingDetails_TbCities_CityId",
                table: "TbShippingDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_TbShippingDetails_TbOffers_OfferId",
                table: "TbShippingDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_TbStates_TbCountries_CountryId",
                table: "TbStates");

            migrationBuilder.DropForeignKey(
                name: "FK_TbSupportTicketMessages_TbSupportTickets_SupportTicketId",
                table: "TbSupportTicketMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_TbSuppressionReasons_TbProductVisibilityRules_ProductVisibilityRuleId",
                table: "TbSuppressionReasons");

            migrationBuilder.DropForeignKey(
                name: "FK_TbUnitConversions_TbUnits_FromUnitId",
                table: "TbUnitConversions");

            migrationBuilder.DropForeignKey(
                name: "FK_TbUnitConversions_TbUnits_ToUnitId",
                table: "TbUnitConversions");

            migrationBuilder.DropForeignKey(
                name: "FK_TbUserNotifications_AspNetUsers_UserId",
                table: "TbUserNotifications");

            migrationBuilder.DropForeignKey(
                name: "FK_TbUserNotifications_TbNotifications_NotificationId",
                table: "TbUserNotifications");

            migrationBuilder.DropForeignKey(
                name: "FK_TbUserOfferRatings_AspNetUsers_UserId",
                table: "TbUserOfferRatings");

            migrationBuilder.DropForeignKey(
                name: "FK_TbUserOfferRatings_TbOffers_OfferId",
                table: "TbUserOfferRatings");

            migrationBuilder.DropForeignKey(
                name: "FK_TbVendorTierHistories_TbSellerTiers_SellerTierId",
                table: "TbVendorTierHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_TbVendorTierHistories_TbVendors_VendorId",
                table: "TbVendorTierHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_TbVendorWallets_TbCurrencies_CurrencyId",
                table: "TbVendorWallets");

            migrationBuilder.DropForeignKey(
                name: "FK_TbVendorWallets_TbVendors_VendorId",
                table: "TbVendorWallets");

            migrationBuilder.DropForeignKey(
                name: "FK_TbVisibilityLogs_AspNetUsers_ChangedByUserId",
                table: "TbVisibilityLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_TbVisibilityLogs_TbItems_ItemId",
                table: "TbVisibilityLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_TbWalletTransactions_AspNetUsers_ProcessedByUserId",
                table: "TbWalletTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_TbWalletTransactions_TbCustomerWallets_CustomerWalletId",
                table: "TbWalletTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_TbWalletTransactions_TbOrders_OrderId",
                table: "TbWalletTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_TbWalletTransactions_TbRefundRequests_RefundId",
                table: "TbWalletTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_TbWalletTransactions_TbVendorWallets_VendorWalletId",
                table: "TbWalletTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_TbWarranties_TbCities_CityId",
                table: "TbWarranties");

            migrationBuilder.AlterColumn<Guid>(
                name: "ProcessedByUserId",
                table: "TbWalletTransactions",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "ChangedByUserId",
                table: "TbVisibilityLogs",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "TbUserOfferRatings",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "TbUserNotifications",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<Guid>(
                name: "ReviewedByUserId",
                table: "TbSellerRequests",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "UploadedByUserId",
                table: "TbRequestDocuments",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "TbRequestComments",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<Guid>(
                name: "AdminUserId",
                table: "TbRefundRequests",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "SuppressedByUserId",
                table: "TbProductVisibilityRules",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "ChangedByUserId",
                table: "TbPriceHistories",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "TbOrders",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "TbOffers",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "TbNotificationPreferences",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "TbMortems",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "TbMoitems",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "ApprovedByUserId",
                table: "TbCampaignVendors",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "ApprovedByUserId",
                table: "TbCampaignProducts",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "ReviewedByUserId",
                table: "TbBrandRegistrationRequests",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "VerifiedByUserId",
                table: "TbBrandDocuments",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "VerifiedByUserId",
                table: "TbAuthorizedDistributors",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "AspNetUserTokens",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<Guid>(
                name: "RoleId",
                table: "AspNetUserRoles",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "AspNetUserRoles",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "AspNetUserLogins",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "AspNetUserClaims",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "AspNetRoles",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<Guid>(
                name: "RoleId",
                table: "AspNetRoleClaims",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                table: "AspNetUserClaims",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                table: "AspNetUserLogins",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbAttributeOptions_TbAttributes_AttributeId",
                table: "TbAttributeOptions",
                column: "AttributeId",
                principalTable: "TbAttributes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbAuthorizedDistributors_AspNetUsers_VerifiedByUserId",
                table: "TbAuthorizedDistributors",
                column: "VerifiedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbAuthorizedDistributors_TbBrands_BrandId",
                table: "TbAuthorizedDistributors",
                column: "BrandId",
                principalTable: "TbBrands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbAuthorizedDistributors_TbVendors_VendorId",
                table: "TbAuthorizedDistributors",
                column: "VendorId",
                principalTable: "TbVendors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbBlockProducts_TbHomepageBlocks_HomepageBlockId",
                table: "TbBlockProducts",
                column: "HomepageBlockId",
                principalTable: "TbHomepageBlocks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbBlockProducts_TbItems_ItemId",
                table: "TbBlockProducts",
                column: "ItemId",
                principalTable: "TbItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbBrandDocuments_AspNetUsers_VerifiedByUserId",
                table: "TbBrandDocuments",
                column: "VerifiedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbBrandDocuments_TbBrandRegistrationRequests_BrandRegistrationRequestId",
                table: "TbBrandDocuments",
                column: "BrandRegistrationRequestId",
                principalTable: "TbBrandRegistrationRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbBrandRegistrationRequests_AspNetUsers_ReviewedByUserId",
                table: "TbBrandRegistrationRequests",
                column: "ReviewedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbBrandRegistrationRequests_TbBrands_ApprovedBrandId",
                table: "TbBrandRegistrationRequests",
                column: "ApprovedBrandId",
                principalTable: "TbBrands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbBrandRegistrationRequests_TbVendors_VendorId",
                table: "TbBrandRegistrationRequests",
                column: "VendorId",
                principalTable: "TbVendors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbBuyBoxCalculations_TbItems_ItemId",
                table: "TbBuyBoxCalculations",
                column: "ItemId",
                principalTable: "TbItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbBuyBoxCalculations_TbOffers_WinningOfferId",
                table: "TbBuyBoxCalculations",
                column: "WinningOfferId",
                principalTable: "TbOffers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbBuyBoxHistories_TbItems_ItemId",
                table: "TbBuyBoxHistories",
                column: "ItemId",
                principalTable: "TbItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbBuyBoxHistories_TbOffers_OfferId",
                table: "TbBuyBoxHistories",
                column: "OfferId",
                principalTable: "TbOffers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbBuyBoxHistories_TbVendors_VendorId",
                table: "TbBuyBoxHistories",
                column: "VendorId",
                principalTable: "TbVendors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbCampaignProducts_AspNetUsers_ApprovedByUserId",
                table: "TbCampaignProducts",
                column: "ApprovedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbCampaignProducts_TbCampaigns_CampaignId",
                table: "TbCampaignProducts",
                column: "CampaignId",
                principalTable: "TbCampaigns",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbCampaignProducts_TbItems_ItemId",
                table: "TbCampaignProducts",
                column: "ItemId",
                principalTable: "TbItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbCampaignProducts_TbVendors_VendorId",
                table: "TbCampaignProducts",
                column: "VendorId",
                principalTable: "TbVendors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbCampaignVendors_AspNetUsers_ApprovedByUserId",
                table: "TbCampaignVendors",
                column: "ApprovedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbCampaignVendors_TbCampaigns_CampaignId",
                table: "TbCampaignVendors",
                column: "CampaignId",
                principalTable: "TbCampaigns",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbCampaignVendors_TbVendors_VendorId",
                table: "TbCampaignVendors",
                column: "VendorId",
                principalTable: "TbVendors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbCategoryAttributes_TbAttributes_AttributeId",
                table: "TbCategoryAttributes",
                column: "AttributeId",
                principalTable: "TbAttributes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbCategoryAttributes_TbCategories_CategoryId",
                table: "TbCategoryAttributes",
                column: "CategoryId",
                principalTable: "TbCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbCities_TbStates_StateId",
                table: "TbCities",
                column: "StateId",
                principalTable: "TbStates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbCombinationAttributes_TbItemCombinations_ItemCombinationId",
                table: "TbCombinationAttributes",
                column: "ItemCombinationId",
                principalTable: "TbItemCombinations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbCombinationAttributesValues_TbAttributes_AttributeId",
                table: "TbCombinationAttributesValues",
                column: "AttributeId",
                principalTable: "TbAttributes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbCombinationAttributesValues_TbCombinationAttributes_CombinationAttributeId",
                table: "TbCombinationAttributesValues",
                column: "CombinationAttributeId",
                principalTable: "TbCombinationAttributes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbCustomerLoyalties_TbCustomers_CustomerId",
                table: "TbCustomerLoyalties",
                column: "CustomerId",
                principalTable: "TbCustomers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbCustomerLoyalties_TbLoyaltyTiers_LoyaltyTierId",
                table: "TbCustomerLoyalties",
                column: "LoyaltyTierId",
                principalTable: "TbLoyaltyTiers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbCustomerSegmentPricings_TbOffers_OfferId",
                table: "TbCustomerSegmentPricings",
                column: "OfferId",
                principalTable: "TbOffers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbCustomerWallets_TbCurrencies_CurrencyId",
                table: "TbCustomerWallets",
                column: "CurrencyId",
                principalTable: "TbCurrencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbCustomerWallets_TbCustomers_CustomerId",
                table: "TbCustomerWallets",
                column: "CustomerId",
                principalTable: "TbCustomers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbDisputeMessages_TbDisputes_DisputeID",
                table: "TbDisputeMessages",
                column: "DisputeID",
                principalTable: "TbDisputes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbFBMInventories_TbItems_ItemId",
                table: "TbFBMInventories",
                column: "ItemId",
                principalTable: "TbItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbFBMInventories_TbVendors_VendorId",
                table: "TbFBMInventories",
                column: "VendorId",
                principalTable: "TbVendors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbFBMInventories_TbWarehouses_WarehouseId",
                table: "TbFBMInventories",
                column: "WarehouseId",
                principalTable: "TbWarehouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbFBMShipments_TbOrders_OrderId",
                table: "TbFBMShipments",
                column: "OrderId",
                principalTable: "TbOrders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbFBMShipments_TbShippingCompanies_ShippingCompanyId",
                table: "TbFBMShipments",
                column: "ShippingCompanyId",
                principalTable: "TbShippingCompanies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbFBMShipments_TbWarehouses_WarehouseId",
                table: "TbFBMShipments",
                column: "WarehouseId",
                principalTable: "TbWarehouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbFlashSaleProducts_TbFlashSales_FlashSaleId",
                table: "TbFlashSaleProducts",
                column: "FlashSaleId",
                principalTable: "TbFlashSales",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbFlashSaleProducts_TbItems_ItemId",
                table: "TbFlashSaleProducts",
                column: "ItemId",
                principalTable: "TbItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbFlashSaleProducts_TbVendors_VendorId",
                table: "TbFlashSaleProducts",
                column: "VendorId",
                principalTable: "TbVendors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbFulfillmentFees_TbFulfillmentMethods_FulfillmentMethodId",
                table: "TbFulfillmentFees",
                column: "FulfillmentMethodId",
                principalTable: "TbFulfillmentMethods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbItemAttribute_TbAttributes_AttributeId",
                table: "TbItemAttribute",
                column: "AttributeId",
                principalTable: "TbAttributes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbItemAttribute_TbItems_ItemId",
                table: "TbItemAttribute",
                column: "ItemId",
                principalTable: "TbItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbItemAttributeCombinationPricings_TbItems_ItemId",
                table: "TbItemAttributeCombinationPricings",
                column: "ItemId",
                principalTable: "TbItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbItemCombinations_TbItems_ItemId",
                table: "TbItemCombinations",
                column: "ItemId",
                principalTable: "TbItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbItemImages_TbItems_ItemId",
                table: "TbItemImages",
                column: "ItemId",
                principalTable: "TbItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbItems_TbBrands_BrandId",
                table: "TbItems",
                column: "BrandId",
                principalTable: "TbBrands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbItems_TbCategories_CategoryId",
                table: "TbItems",
                column: "CategoryId",
                principalTable: "TbCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbItems_TbUnits_UnitId",
                table: "TbItems",
                column: "UnitId",
                principalTable: "TbUnits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbLoyaltyPointsTransactions_TbCustomerLoyalties_CustomerLoyaltyId",
                table: "TbLoyaltyPointsTransactions",
                column: "CustomerLoyaltyId",
                principalTable: "TbCustomerLoyalties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbLoyaltyPointsTransactions_TbOrders_OrderId",
                table: "TbLoyaltyPointsTransactions",
                column: "OrderId",
                principalTable: "TbOrders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbLoyaltyPointsTransactions_TbProductReviews_ReviewId",
                table: "TbLoyaltyPointsTransactions",
                column: "ReviewId",
                principalTable: "TbProductReviews",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbMediaContents_TbContentAreas_ContentAreaId",
                table: "TbMediaContents",
                column: "ContentAreaId",
                principalTable: "TbContentAreas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbMoitems_AspNetUsers_UserId",
                table: "TbMoitems",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbMortems_AspNetUsers_UserId",
                table: "TbMortems",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbMortems_TbOrders_OrderId",
                table: "TbMortems",
                column: "OrderId",
                principalTable: "TbOrders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbMovitemsdetails_TbItemAttributeCombinationPricings_ItemAttributeCombinationPricingId",
                table: "TbMovitemsdetails",
                column: "ItemAttributeCombinationPricingId",
                principalTable: "TbItemAttributeCombinationPricings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbMovitemsdetails_TbItems_ItemId",
                table: "TbMovitemsdetails",
                column: "ItemId",
                principalTable: "TbItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbMovitemsdetails_TbMoitems_MoitemId",
                table: "TbMovitemsdetails",
                column: "MoitemId",
                principalTable: "TbMoitems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbMovitemsdetails_TbMortems_MortemId",
                table: "TbMovitemsdetails",
                column: "MortemId",
                principalTable: "TbMortems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbMovitemsdetails_TbWarehouses_WarehouseId",
                table: "TbMovitemsdetails",
                column: "WarehouseId",
                principalTable: "TbWarehouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbNotificationPreferences_AspNetUsers_UserId",
                table: "TbNotificationPreferences",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbOfferCombinationPricings_TbItemCombinations_ItemCombinationId",
                table: "TbOfferCombinationPricings",
                column: "ItemCombinationId",
                principalTable: "TbItemCombinations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbOfferCombinationPricings_TbOffers_OfferId",
                table: "TbOfferCombinationPricings",
                column: "OfferId",
                principalTable: "TbOffers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbOffers_AspNetUsers_UserId",
                table: "TbOffers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbOffers_TbItems_ItemId",
                table: "TbOffers",
                column: "ItemId",
                principalTable: "TbItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbOffers_TbOfferConditions_TbOfferConditionId",
                table: "TbOffers",
                column: "TbOfferConditionId",
                principalTable: "TbOfferConditions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbOffers_TbWarranties_WarrantyId",
                table: "TbOffers",
                column: "WarrantyId",
                principalTable: "TbWarranties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbOrderDetails_TbItems_ItemId",
                table: "TbOrderDetails",
                column: "ItemId",
                principalTable: "TbItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbOrderDetails_TbOrders_OrderId",
                table: "TbOrderDetails",
                column: "OrderId",
                principalTable: "TbOrders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbOrders_AspNetUsers_UserId",
                table: "TbOrders",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbOrders_TbCouponCodes_CouponId",
                table: "TbOrders",
                column: "CouponId",
                principalTable: "TbCouponCodes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbOrders_TbShippingCompanies_ShippingCompanyId",
                table: "TbOrders",
                column: "ShippingCompanyId",
                principalTable: "TbShippingCompanies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbPlatformTreasuries_TbCurrencies_CurrencyId",
                table: "TbPlatformTreasuries",
                column: "CurrencyId",
                principalTable: "TbCurrencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbPriceHistories_AspNetUsers_ChangedByUserId",
                table: "TbPriceHistories",
                column: "ChangedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbPriceHistories_TbOffers_OfferId",
                table: "TbPriceHistories",
                column: "OfferId",
                principalTable: "TbOffers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbProductVisibilityRules_AspNetUsers_SuppressedByUserId",
                table: "TbProductVisibilityRules",
                column: "SuppressedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbProductVisibilityRules_TbItems_ItemId",
                table: "TbProductVisibilityRules",
                column: "ItemId",
                principalTable: "TbItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbQuantityPricings_TbOffers_OfferId",
                table: "TbQuantityPricings",
                column: "OfferId",
                principalTable: "TbOffers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbRefundRequests_AspNetUsers_AdminUserId",
                table: "TbRefundRequests",
                column: "AdminUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbRefundRequests_TbOrders_OrderId",
                table: "TbRefundRequests",
                column: "OrderId",
                principalTable: "TbOrders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbRequestComments_AspNetUsers_UserId",
                table: "TbRequestComments",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbRequestComments_TbSellerRequests_SellerRequestId",
                table: "TbRequestComments",
                column: "SellerRequestId",
                principalTable: "TbSellerRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbRequestDocuments_AspNetUsers_UploadedByUserId",
                table: "TbRequestDocuments",
                column: "UploadedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbRequestDocuments_TbSellerRequests_SellerRequestId",
                table: "TbRequestDocuments",
                column: "SellerRequestId",
                principalTable: "TbSellerRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbReviewVotes_TbProductReviews_ReviewID",
                table: "TbReviewVotes",
                column: "ReviewID",
                principalTable: "TbProductReviews",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbSellerPerformanceMetrics_TbVendors_VendorId",
                table: "TbSellerPerformanceMetrics",
                column: "VendorId",
                principalTable: "TbVendors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbSellerRequests_AspNetUsers_ReviewedByUserId",
                table: "TbSellerRequests",
                column: "ReviewedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbSellerRequests_TbVendors_VendorId",
                table: "TbSellerRequests",
                column: "VendorId",
                principalTable: "TbVendors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbSellerTierBenefits_TbSellerTiers_SellerTierId",
                table: "TbSellerTierBenefits",
                column: "SellerTierId",
                principalTable: "TbSellerTiers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbShippingDetails_TbCities_CityId",
                table: "TbShippingDetails",
                column: "CityId",
                principalTable: "TbCities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbShippingDetails_TbOffers_OfferId",
                table: "TbShippingDetails",
                column: "OfferId",
                principalTable: "TbOffers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbStates_TbCountries_CountryId",
                table: "TbStates",
                column: "CountryId",
                principalTable: "TbCountries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbSupportTicketMessages_TbSupportTickets_SupportTicketId",
                table: "TbSupportTicketMessages",
                column: "SupportTicketId",
                principalTable: "TbSupportTickets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbSuppressionReasons_TbProductVisibilityRules_ProductVisibilityRuleId",
                table: "TbSuppressionReasons",
                column: "ProductVisibilityRuleId",
                principalTable: "TbProductVisibilityRules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbUnitConversions_TbUnits_FromUnitId",
                table: "TbUnitConversions",
                column: "FromUnitId",
                principalTable: "TbUnits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbUnitConversions_TbUnits_ToUnitId",
                table: "TbUnitConversions",
                column: "ToUnitId",
                principalTable: "TbUnits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbUserNotifications_AspNetUsers_UserId",
                table: "TbUserNotifications",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbUserNotifications_TbNotifications_NotificationId",
                table: "TbUserNotifications",
                column: "NotificationId",
                principalTable: "TbNotifications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbUserOfferRatings_AspNetUsers_UserId",
                table: "TbUserOfferRatings",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbUserOfferRatings_TbOffers_OfferId",
                table: "TbUserOfferRatings",
                column: "OfferId",
                principalTable: "TbOffers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbVendorTierHistories_TbSellerTiers_SellerTierId",
                table: "TbVendorTierHistories",
                column: "SellerTierId",
                principalTable: "TbSellerTiers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbVendorTierHistories_TbVendors_VendorId",
                table: "TbVendorTierHistories",
                column: "VendorId",
                principalTable: "TbVendors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbVendorWallets_TbCurrencies_CurrencyId",
                table: "TbVendorWallets",
                column: "CurrencyId",
                principalTable: "TbCurrencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbVendorWallets_TbVendors_VendorId",
                table: "TbVendorWallets",
                column: "VendorId",
                principalTable: "TbVendors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbVisibilityLogs_AspNetUsers_ChangedByUserId",
                table: "TbVisibilityLogs",
                column: "ChangedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbVisibilityLogs_TbItems_ItemId",
                table: "TbVisibilityLogs",
                column: "ItemId",
                principalTable: "TbItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbWalletTransactions_AspNetUsers_ProcessedByUserId",
                table: "TbWalletTransactions",
                column: "ProcessedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbWalletTransactions_TbCustomerWallets_CustomerWalletId",
                table: "TbWalletTransactions",
                column: "CustomerWalletId",
                principalTable: "TbCustomerWallets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbWalletTransactions_TbOrders_OrderId",
                table: "TbWalletTransactions",
                column: "OrderId",
                principalTable: "TbOrders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbWalletTransactions_TbRefundRequests_RefundId",
                table: "TbWalletTransactions",
                column: "RefundId",
                principalTable: "TbRefundRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbWalletTransactions_TbVendorWallets_VendorWalletId",
                table: "TbWalletTransactions",
                column: "VendorWalletId",
                principalTable: "TbVendorWallets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbWarranties_TbCities_CityId",
                table: "TbWarranties",
                column: "CityId",
                principalTable: "TbCities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
