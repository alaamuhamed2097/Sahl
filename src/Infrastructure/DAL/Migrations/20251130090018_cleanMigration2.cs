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
                name: "FK_TbAuthorizedDistributors_TbVendors_VendorId",
                table: "TbAuthorizedDistributors");

            migrationBuilder.DropForeignKey(
                name: "FK_TbBlockProducts_TbHomepageBlocks_HomepageBlockId",
                table: "TbBlockProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_TbBrandDocuments_TbBrandRegistrationRequests_BrandRegistrationRequestId",
                table: "TbBrandDocuments");

            migrationBuilder.DropForeignKey(
                name: "FK_TbBrandRegistrationRequests_TbVendors_VendorId",
                table: "TbBrandRegistrationRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_TbBuyBoxHistories_TbItems_ItemId",
                table: "TbBuyBoxHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_TbCampaignProducts_TbCampaigns_CampaignId",
                table: "TbCampaignProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_TbCampaignVendors_TbCampaigns_CampaignId",
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
                name: "FK_TbCustomerSegmentPricings_TbOffers_OfferId",
                table: "TbCustomerSegmentPricings");

            migrationBuilder.DropForeignKey(
                name: "FK_TbCustomerWallets_TbCustomers_CustomerId",
                table: "TbCustomerWallets");

            migrationBuilder.DropForeignKey(
                name: "FK_TbDisputeMessages_TbDisputes_DisputeID",
                table: "TbDisputeMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_TbFlashSaleProducts_TbFlashSales_FlashSaleId",
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
                name: "FK_TbOffers_TbOfferConditions_TbOfferConditionId",
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
                name: "FK_TbPriceHistories_TbOffers_OfferId",
                table: "TbPriceHistories");

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
                name: "FK_TbRequestComments_TbSellerRequests_SellerRequestId",
                table: "TbRequestComments");

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
                name: "FK_TbSellerRequests_TbVendors_VendorId",
                table: "TbSellerRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_TbSellerTierBenefits_TbSellerTiers_SellerTierId",
                table: "TbSellerTierBenefits");

            migrationBuilder.DropForeignKey(
                name: "FK_TbShippingDetails_TbCities_CityId",
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
                name: "FK_TbVendorTierHistories_TbVendors_VendorId",
                table: "TbVendorTierHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_TbVendorWallets_TbVendors_VendorId",
                table: "TbVendorWallets");

            migrationBuilder.DropForeignKey(
                name: "FK_TbVisibilityLogs_TbItems_ItemId",
                table: "TbVisibilityLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_TbWalletTransactions_TbCustomerWallets_CustomerWalletId",
                table: "TbWalletTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_TbWalletTransactions_TbVendorWallets_VendorWalletId",
                table: "TbWalletTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_TbWarranties_TbCities_CityId",
                table: "TbWarranties");

            migrationBuilder.DropIndex(
                name: "IX_TbVendorWallets_VendorId",
                table: "TbVendorWallets");

            migrationBuilder.DropIndex(
                name: "IX_TbSupportTicketMessages_SupportTicketId",
                table: "TbSupportTicketMessages");

            migrationBuilder.DropIndex(
                name: "IX_TbProductVisibilityRules_ItemId",
                table: "TbProductVisibilityRules");

            migrationBuilder.DropIndex(
                name: "IX_TbCustomerWallets_CustomerId",
                table: "TbCustomerWallets");

            migrationBuilder.DropIndex(
                name: "IX_TbCustomerLoyalties_CustomerId",
                table: "TbCustomerLoyalties");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TbNotifications",
                table: "TbNotifications");

            migrationBuilder.DropColumn(
                name: "SupportTicketId",
                table: "TbSupportTicketMessages");

            migrationBuilder.RenameTable(
                name: "TbNotifications",
                newName: "Notifications");

            migrationBuilder.RenameIndex(
                name: "IX_TbNotifications_CurrentState",
                table: "Notifications",
                newName: "IX_Notifications_CurrentState");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "TbWarehouses",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ProcessedDate",
                table: "TbWalletTransactions",
                type: "datetime2(2)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsAutomatic",
                table: "TbVisibilityLogs",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ChangedAt",
                table: "TbVisibilityLogs",
                type: "datetime2(2)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<decimal>(
                name: "WithdrawalFeePercentage",
                table: "TbVendorWallets",
                type: "decimal(5,2)",
                nullable: false,
                defaultValue: 2.5m,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalWithdrawn",
                table: "TbVendorWallets",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalEarned",
                table: "TbVendorWallets",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalCommissionPaid",
                table: "TbVendorWallets",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "PendingBalance",
                table: "TbVendorWallets",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastWithdrawalDate",
                table: "TbVendorWallets",
                type: "datetime2(2)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastTransactionDate",
                table: "TbVendorWallets",
                type: "datetime2(2)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "AvailableBalance",
                table: "TbVendorWallets",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<bool>(
                name: "IsAutomatic",
                table: "TbVendorTierHistories",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndedAt",
                table: "TbVendorTierHistories",
                type: "datetime2(2)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "AchievedAt",
                table: "TbVendorTierHistories",
                type: "datetime2(2)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<bool>(
                name: "IsRead",
                table: "TbUserNotifications",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ResolvedAt",
                table: "TbSuppressionReasons",
                type: "datetime2(2)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsResolved",
                table: "TbSuppressionReasons",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DetectedAt",
                table: "TbSuppressionReasons",
                type: "datetime2(2)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "UserType",
                table: "TbSupportTickets",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "UserID",
                table: "TbSupportTickets",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "TicketCreatedDate",
                table: "TbSupportTickets",
                type: "datetime2(2)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Subject",
                table: "TbSupportTickets",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "TbSupportTickets",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Priority",
                table: "TbSupportTickets",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Category",
                table: "TbSupportTickets",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "AssignedTo",
                table: "TbSupportTickets",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AssignedTeam",
                table: "TbSupportTickets",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserType",
                table: "TbSupportTicketMessages",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "SentDate",
                table: "TbSupportTicketMessages",
                type: "datetime2(2)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SenderID",
                table: "TbSupportTicketMessages",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "TitleEn",
                table: "TbStates",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "TitleAr",
                table: "TbStates",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "TbShippingCompanies",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneCode",
                table: "TbShippingCompanies",
                type: "nvarchar(4)",
                maxLength: 4,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "TbShippingCompanies",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "LogoImagePath",
                table: "TbShippingCompanies",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "WhatsAppNumber",
                table: "TbSettings",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "TbSellerTiers",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "HasPrioritySupport",
                table: "TbSellerTiers",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "HasFeaturedListings",
                table: "TbSellerTiers",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "HasDedicatedAccountManager",
                table: "TbSellerTiers",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "HasBuyBoxBoost",
                table: "TbSellerTiers",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "HasAdvancedAnalytics",
                table: "TbSellerTiers",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<int>(
                name: "DisplayOrder",
                table: "TbSellerTiers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "CommissionReductionPercentage",
                table: "TbSellerTiers",
                type: "decimal(5,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "TbSellerTierBenefits",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<int>(
                name: "DisplayOrder",
                table: "TbSellerTierBenefits",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "SubmittedAt",
                table: "TbSellerRequests",
                type: "datetime2(2)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ReviewedAt",
                table: "TbSellerRequests",
                type: "datetime2(2)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ProcessedAt",
                table: "TbSellerRequests",
                type: "datetime2(2)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Priority",
                table: "TbSellerRequests",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<bool>(
                name: "UsesFBM",
                table: "TbSellerPerformanceMetrics",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<int>(
                name: "TotalReviews",
                table: "TbSellerPerformanceMetrics",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "ReturnRate",
                table: "TbSellerPerformanceMetrics",
                type: "decimal(5,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "ResponseRate",
                table: "TbSellerPerformanceMetrics",
                type: "decimal(5,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "OrderCompletionRate",
                table: "TbSellerPerformanceMetrics",
                type: "decimal(5,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "OnTimeShippingRate",
                table: "TbSellerPerformanceMetrics",
                type: "decimal(5,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastUpdated",
                table: "TbSellerPerformanceMetrics",
                type: "datetime2(2)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "CancellationRate",
                table: "TbSellerPerformanceMetrics",
                type: "decimal(5,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CalculatedForPeriodStart",
                table: "TbSellerPerformanceMetrics",
                type: "datetime2(2)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CalculatedForPeriodEnd",
                table: "TbSellerPerformanceMetrics",
                type: "datetime2(2)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<int>(
                name: "BuyBoxWins",
                table: "TbSellerPerformanceMetrics",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "BuyBoxWinRate",
                table: "TbSellerPerformanceMetrics",
                type: "decimal(5,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)");

            migrationBuilder.AlterColumn<int>(
                name: "AverageResponseTimeInHours",
                table: "TbSellerPerformanceMetrics",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "AverageRating",
                table: "TbSellerPerformanceMetrics",
                type: "decimal(3,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(3,2)");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "TbSalesReviews",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<decimal>(
                name: "ShippingSpeedRating",
                table: "TbSalesReviews",
                type: "decimal(2,1)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ServiceRating",
                table: "TbSalesReviews",
                type: "decimal(2,1)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ReviewDate",
                table: "TbSalesReviews",
                type: "datetime2(2)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ProductAccuracyRating",
                table: "TbSalesReviews",
                type: "decimal(2,1)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "OverallRating",
                table: "TbSalesReviews",
                type: "decimal(2,1)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "CommunicationRating",
                table: "TbSalesReviews",
                type: "decimal(2,1)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "WithType",
                table: "TbReviewVotes",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "VoteValue",
                table: "TbReviewVotes",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "VoteType",
                table: "TbReviewVotes",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UploadedAt",
                table: "TbRequestDocuments",
                type: "datetime2(2)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsInternal",
                table: "TbRequestComments",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "TbQuantityPricings",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<int>(
                name: "DisplayOrder",
                table: "TbQuantityPricings",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "SuppressedAt",
                table: "TbProductVisibilityRules",
                type: "datetime2(2)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastCheckedAt",
                table: "TbProductVisibilityRules",
                type: "datetime2(2)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsVisible",
                table: "TbProductVisibilityRules",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "IsApproved",
                table: "TbProductVisibilityRules",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "HasValidCategory",
                table: "TbProductVisibilityRules",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "HasStock",
                table: "TbProductVisibilityRules",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "HasActiveOffers",
                table: "TbProductVisibilityRules",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "AllSellersActive",
                table: "TbProductVisibilityRules",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<DateTime>(
                name: "VendorResponseDate",
                table: "TbProductReviews",
                type: "datetime2(2)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "TbProductReviews",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ReviewTitle",
                table: "TbProductReviews",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ReviewDate",
                table: "TbProductReviews",
                type: "datetime2(2)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Rating",
                table: "TbProductReviews",
                type: "decimal(2,1)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "NotHelpfulCount",
                table: "TbProductReviews",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<bool>(
                name: "IsVerifiedPurchase",
                table: "TbProductReviews",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<int>(
                name: "HelpfulCount",
                table: "TbProductReviews",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ApprovedDate",
                table: "TbProductReviews",
                type: "datetime2(2)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsAutomatic",
                table: "TbPriceHistories",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ChangedAt",
                table: "TbPriceHistories",
                type: "datetime2(2)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<decimal>(
                name: "VendorWalletsTotal",
                table: "TbPlatformTreasuries",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalBalance",
                table: "TbPlatformTreasuries",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "ProcessedPayouts",
                table: "TbPlatformTreasuries",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "PendingPayouts",
                table: "TbPlatformTreasuries",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "PendingCommissions",
                table: "TbPlatformTreasuries",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastReconciliationDate",
                table: "TbPlatformTreasuries",
                type: "datetime2(2)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "CustomerWalletsTotal",
                table: "TbPlatformTreasuries",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "CollectedCommissions",
                table: "TbPlatformTreasuries",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "ShippingCost",
                table: "TbOffers",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "SellerRating",
                table: "TbOffers",
                type: "decimal(3,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Quantity",
                table: "TbOffers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "TbOffers",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<string>(
                name: "Condition",
                table: "TbOffers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<bool>(
                name: "EnableSMS",
                table: "TbNotificationPreferences",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "EnablePush",
                table: "TbNotificationPreferences",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "EnableInApp",
                table: "TbNotificationPreferences",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "EnableEmail",
                table: "TbNotificationPreferences",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "TbNotificationChannels",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "TbMortems",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DocumentDate",
                table: "TbMortems",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DocumentDate",
                table: "TbMoitems",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "MediaType",
                table: "TbMediaContents",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "Image",
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "TbMediaContents",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<int>(
                name: "DisplayOrder",
                table: "TbMediaContents",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "PointsMultiplier",
                table: "TbLoyaltyTiers",
                type: "decimal(5,2)",
                nullable: false,
                defaultValue: 1.0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<bool>(
                name: "HasPrioritySupport",
                table: "TbLoyaltyTiers",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "HasFreeShipping",
                table: "TbLoyaltyTiers",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<int>(
                name: "DisplayOrder",
                table: "TbLoyaltyTiers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "CashbackPercentage",
                table: "TbLoyaltyTiers",
                type: "decimal(5,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<bool>(
                name: "IsExpired",
                table: "TbLoyaltyPointsTransactions",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ExpiryDate",
                table: "TbLoyaltyPointsTransactions",
                type: "datetime2(2)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsNewArrival",
                table: "TbItems",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "ShowViewAllLink",
                table: "TbHomepageBlocks",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "IsVisible",
                table: "TbHomepageBlocks",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "TbHomepageBlocks",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ActiveTo",
                table: "TbHomepageBlocks",
                type: "datetime2(2)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ActiveFrom",
                table: "TbHomepageBlocks",
                type: "datetime2(2)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "RequiresWarehouse",
                table: "TbFulfillmentMethods",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "TbFulfillmentMethods",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<int>(
                name: "DisplayOrder",
                table: "TbFulfillmentMethods",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "BuyBoxPriorityBoost",
                table: "TbFulfillmentMethods",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "TbFulfillmentFees",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EffectiveTo",
                table: "TbFulfillmentFees",
                type: "datetime2(2)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "EffectiveFrom",
                table: "TbFulfillmentFees",
                type: "datetime2(2)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                table: "TbFlashSales",
                type: "datetime2(2)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<bool>(
                name: "ShowCountdownTimer",
                table: "TbFlashSales",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<decimal>(
                name: "MinimumSellerRating",
                table: "TbFlashSales",
                type: "decimal(3,2)",
                nullable: false,
                defaultValue: 4.0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(3,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "MinimumDiscountPercentage",
                table: "TbFlashSales",
                type: "decimal(5,2)",
                nullable: false,
                defaultValue: 20m,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "TbFlashSales",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                table: "TbFlashSales",
                type: "datetime2(2)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<int>(
                name: "DisplayOrder",
                table: "TbFlashSales",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ViewCount",
                table: "TbFlashSaleProducts",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "SoldQuantity",
                table: "TbFlashSaleProducts",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "TbFlashSaleProducts",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<int>(
                name: "DisplayOrder",
                table: "TbFlashSaleProducts",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "AddToCartCount",
                table: "TbFlashSaleProducts",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ShippedDate",
                table: "TbFBMShipments",
                type: "datetime2(2)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "PickupDate",
                table: "TbFBMShipments",
                type: "datetime2(2)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DeliveredDate",
                table: "TbFBMShipments",
                type: "datetime2(2)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ReservedQuantity",
                table: "TbFBMInventories",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastSyncDate",
                table: "TbFBMInventories",
                type: "datetime2(2)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "InTransitQuantity",
                table: "TbFBMInventories",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ExpiryDate",
                table: "TbFBMInventories",
                type: "datetime2(2)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DamagedQuantity",
                table: "TbFBMInventories",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "AvailableQuantity",
                table: "TbFBMInventories",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "TbDisputes",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "SenderType",
                table: "TbDisputes",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "SenderID",
                table: "TbDisputes",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Parties",
                table: "TbDisputes",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "AssignedAdminID",
                table: "TbDisputes",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "SentDate",
                table: "TbDisputeMessages",
                type: "datetime2(2)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "SenderType",
                table: "TbDisputeMessages",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "SenderID",
                table: "TbDisputeMessages",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "TbDeliveryReviews",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ReviewDate",
                table: "TbDeliveryReviews",
                type: "datetime2(2)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "PackagingRating",
                table: "TbDeliveryReviews",
                type: "decimal(2,1)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "OverallRating",
                table: "TbDeliveryReviews",
                type: "decimal(2,1)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "CourierDeliveryRating",
                table: "TbDeliveryReviews",
                type: "decimal(2,1)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalSpent",
                table: "TbCustomerWallets",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalEarned",
                table: "TbCustomerWallets",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "PendingBalance",
                table: "TbCustomerWallets",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastTransactionDate",
                table: "TbCustomerWallets",
                type: "datetime2(2)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "AvailableBalance",
                table: "TbCustomerWallets",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<int>(
                name: "MinimumOrderQuantity",
                table: "TbCustomerSegmentPricings",
                type: "int",
                nullable: false,
                defaultValue: 1,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "TbCustomerSegmentPricings",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EffectiveTo",
                table: "TbCustomerSegmentPricings",
                type: "datetime2(2)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "EffectiveFrom",
                table: "TbCustomerSegmentPricings",
                type: "datetime2(2)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "UsedPoints",
                table: "TbCustomerLoyalties",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalSpentThisYear",
                table: "TbCustomerLoyalties",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalPoints",
                table: "TbCustomerLoyalties",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<int>(
                name: "TotalOrdersThisYear",
                table: "TbCustomerLoyalties",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "NextTierEligibilityDate",
                table: "TbCustomerLoyalties",
                type: "datetime2(2)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastTierUpgradeDate",
                table: "TbCustomerLoyalties",
                type: "datetime2(2)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ExpiredPoints",
                table: "TbCustomerLoyalties",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "AvailablePoints",
                table: "TbCustomerLoyalties",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "ExchangeRate",
                table: "TbCurrencies",
                type: "decimal(18,6)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<string>(
                name: "CountryCode",
                table: "TbCurrencies",
                type: "nvarchar(3)",
                maxLength: 3,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "UsageCount",
                table: "TbCouponCodes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "TitleEN",
                table: "TbCouponCodes",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "TitleAR",
                table: "TbCouponCodes",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDateUTC",
                table: "TbCouponCodes",
                type: "datetime2(2)",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDateUTC",
                table: "TbCouponCodes",
                type: "datetime2(2)",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "TbCouponCodes",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "TitleEn",
                table: "TbCountries",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "TitleAr",
                table: "TbCountries",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "TbContentAreas",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<int>(
                name: "DisplayOrder",
                table: "TbContentAreas",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "TitleEn",
                table: "TbCities",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "TitleAr",
                table: "TbCities",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalSales",
                table: "TbCampaignVendors",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<int>(
                name: "TotalProductsSubmitted",
                table: "TbCampaignVendors",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "TotalProductsApproved",
                table: "TbCampaignVendors",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalCommissionPaid",
                table: "TbCampaignVendors",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<bool>(
                name: "IsApproved",
                table: "TbCampaignVendors",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ApprovedAt",
                table: "TbCampaignVendors",
                type: "datetime2(2)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "AppliedAt",
                table: "TbCampaignVendors",
                type: "datetime2(2)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                table: "TbCampaigns",
                type: "datetime2(2)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<decimal>(
                name: "MinimumDiscountPercentage",
                table: "TbCampaigns",
                type: "decimal(5,2)",
                nullable: false,
                defaultValue: 20m,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)");

            migrationBuilder.AlterColumn<bool>(
                name: "IsFeatured",
                table: "TbCampaigns",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "TbCampaigns",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                table: "TbCampaigns",
                type: "datetime2(2)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<int>(
                name: "DisplayOrder",
                table: "TbCampaigns",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "SoldQuantity",
                table: "TbCampaignProducts",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "TbCampaignProducts",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<int>(
                name: "DisplayOrder",
                table: "TbCampaignProducts",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ApprovedAt",
                table: "TbCampaignProducts",
                type: "datetime2(2)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "WonAt",
                table: "TbBuyBoxHistories",
                type: "datetime2(2)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LostAt",
                table: "TbBuyBoxHistories",
                type: "datetime2(2)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DurationInMinutes",
                table: "TbBuyBoxHistories",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ExpiresAt",
                table: "TbBuyBoxCalculations",
                type: "datetime2(2)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CalculatedAt",
                table: "TbBuyBoxCalculations",
                type: "datetime2(2)",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<Guid>(
                name: "TbOfferId",
                table: "TbBuyBoxCalculations",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DisplayOrder",
                table: "TbBrands",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "TrademarkExpiryDate",
                table: "TbBrandRegistrationRequests",
                type: "datetime2(2)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "SubmittedAt",
                table: "TbBrandRegistrationRequests",
                type: "datetime2(2)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ReviewedAt",
                table: "TbBrandRegistrationRequests",
                type: "datetime2(2)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "VerifiedAt",
                table: "TbBrandDocuments",
                type: "datetime2(2)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UploadedAt",
                table: "TbBrandDocuments",
                type: "datetime2(2)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<bool>(
                name: "IsVerified",
                table: "TbBrandDocuments",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "IsFeatured",
                table: "TbBlockProducts",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "TbBlockProducts",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FeaturedTo",
                table: "TbBlockProducts",
                type: "datetime2(2)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "FeaturedFrom",
                table: "TbBlockProducts",
                type: "datetime2(2)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DisplayOrder",
                table: "TbBlockProducts",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "VerifiedAt",
                table: "TbAuthorizedDistributors",
                type: "datetime2(2)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "TbAuthorizedDistributors",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<DateTime>(
                name: "AuthorizationStartDate",
                table: "TbAuthorizedDistributors",
                type: "datetime2(2)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "AuthorizationEndDate",
                table: "TbAuthorizedDistributors",
                type: "datetime2(2)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Severity",
                table: "Notifications",
                type: "int",
                nullable: false,
                defaultValue: 2,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "SentDate",
                table: "Notifications",
                type: "datetime2(2)",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ReadDate",
                table: "Notifications",
                type: "datetime2(2)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Message",
                table: "Notifications",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<bool>(
                name: "IsRead",
                table: "Notifications",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<int>(
                name: "DeliveryStatus",
                table: "Notifications",
                type: "int",
                nullable: false,
                defaultValue: 1,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Notifications",
                table: "Notifications",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_TbWarehouses_IsActive",
                table: "TbWarehouses",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_TbWalletTransactions_CreatedDateUtc",
                table: "TbWalletTransactions",
                column: "CreatedDateUtc");

            migrationBuilder.CreateIndex(
                name: "IX_TbWalletTransactions_CustomerWalletId_Status",
                table: "TbWalletTransactions",
                columns: new[] { "CustomerWalletId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_TbWalletTransactions_ReferenceNumber",
                table: "TbWalletTransactions",
                column: "ReferenceNumber",
                unique: true,
                filter: "[ReferenceNumber] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_TbWalletTransactions_Status",
                table: "TbWalletTransactions",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_TbWalletTransactions_TransactionType",
                table: "TbWalletTransactions",
                column: "TransactionType");

            migrationBuilder.CreateIndex(
                name: "IX_TbWalletTransactions_VendorWalletId_Status",
                table: "TbWalletTransactions",
                columns: new[] { "VendorWalletId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_TbVisibilityLogs_ChangedAt",
                table: "TbVisibilityLogs",
                column: "ChangedAt");

            migrationBuilder.CreateIndex(
                name: "IX_TbVisibilityLogs_IsAutomatic",
                table: "TbVisibilityLogs",
                column: "IsAutomatic");

            migrationBuilder.CreateIndex(
                name: "IX_TbVisibilityLogs_ItemId_ChangedAt",
                table: "TbVisibilityLogs",
                columns: new[] { "ItemId", "ChangedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_TbVendorWallets_AvailableBalance",
                table: "TbVendorWallets",
                column: "AvailableBalance");

            migrationBuilder.CreateIndex(
                name: "IX_TbVendorWallets_VendorId",
                table: "TbVendorWallets",
                column: "VendorId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TbVendorTierHistories_AchievedAt",
                table: "TbVendorTierHistories",
                column: "AchievedAt");

            migrationBuilder.CreateIndex(
                name: "IX_TbVendorTierHistories_EndedAt",
                table: "TbVendorTierHistories",
                column: "EndedAt");

            migrationBuilder.CreateIndex(
                name: "IX_TbVendorTierHistories_VendorId_AchievedAt",
                table: "TbVendorTierHistories",
                columns: new[] { "VendorId", "AchievedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_TbUserNotifications_IsRead",
                table: "TbUserNotifications",
                column: "IsRead");

            migrationBuilder.CreateIndex(
                name: "IX_TbSuppressionReasons_DetectedAt",
                table: "TbSuppressionReasons",
                column: "DetectedAt");

            migrationBuilder.CreateIndex(
                name: "IX_TbSuppressionReasons_IsResolved",
                table: "TbSuppressionReasons",
                column: "IsResolved");

            migrationBuilder.CreateIndex(
                name: "IX_TbSuppressionReasons_IsResolved_DetectedAt",
                table: "TbSuppressionReasons",
                columns: new[] { "IsResolved", "DetectedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_TbSuppressionReasons_ProductVisibilityRuleId_ReasonType",
                table: "TbSuppressionReasons",
                columns: new[] { "ProductVisibilityRuleId", "ReasonType" });

            migrationBuilder.CreateIndex(
                name: "IX_TbSuppressionReasons_ReasonType",
                table: "TbSuppressionReasons",
                column: "ReasonType");

            migrationBuilder.CreateIndex(
                name: "IX_TbSupportTickets_AssignedTo",
                table: "TbSupportTickets",
                column: "AssignedTo");

            migrationBuilder.CreateIndex(
                name: "IX_TbSupportTickets_Category",
                table: "TbSupportTickets",
                column: "Category");

            migrationBuilder.CreateIndex(
                name: "IX_TbSupportTickets_CreatedDateUtc",
                table: "TbSupportTickets",
                column: "CreatedDateUtc");

            migrationBuilder.CreateIndex(
                name: "IX_TbSupportTickets_Priority",
                table: "TbSupportTickets",
                column: "Priority");

            migrationBuilder.CreateIndex(
                name: "IX_TbSupportTickets_Status",
                table: "TbSupportTickets",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_TbSupportTickets_TicketNumber",
                table: "TbSupportTickets",
                column: "TicketNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TbSupportTickets_UserID",
                table: "TbSupportTickets",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_TbSupportTicketMessages_SenderID",
                table: "TbSupportTicketMessages",
                column: "SenderID");

            migrationBuilder.CreateIndex(
                name: "IX_TbSupportTicketMessages_SentDate",
                table: "TbSupportTicketMessages",
                column: "SentDate");

            migrationBuilder.CreateIndex(
                name: "IX_TbSupportTicketMessages_TicketID",
                table: "TbSupportTicketMessages",
                column: "TicketID");

            migrationBuilder.CreateIndex(
                name: "IX_TbSupportTicketMessages_TicketID_MessageNumber",
                table: "TbSupportTicketMessages",
                columns: new[] { "TicketID", "MessageNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TbSellerTiers_DisplayOrder",
                table: "TbSellerTiers",
                column: "DisplayOrder");

            migrationBuilder.CreateIndex(
                name: "IX_TbSellerTiers_IsActive",
                table: "TbSellerTiers",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_TbSellerTiers_MinimumOrders_MaximumOrders",
                table: "TbSellerTiers",
                columns: new[] { "MinimumOrders", "MaximumOrders" });

            migrationBuilder.CreateIndex(
                name: "IX_TbSellerTiers_TierCode",
                table: "TbSellerTiers",
                column: "TierCode",
                unique: true);

            migrationBuilder.AddCheckConstraint(
                name: "CK_SellerTier_Orders",
                table: "TbSellerTiers",
                sql: "[MaximumOrders] IS NULL OR [MaximumOrders] > [MinimumOrders]");

            migrationBuilder.CreateIndex(
                name: "IX_TbSellerTierBenefits_DisplayOrder",
                table: "TbSellerTierBenefits",
                column: "DisplayOrder");

            migrationBuilder.CreateIndex(
                name: "IX_TbSellerTierBenefits_IsActive",
                table: "TbSellerTierBenefits",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_TbSellerRequests_Priority",
                table: "TbSellerRequests",
                column: "Priority");

            migrationBuilder.CreateIndex(
                name: "IX_TbSellerRequests_RequestType",
                table: "TbSellerRequests",
                column: "RequestType");

            migrationBuilder.CreateIndex(
                name: "IX_TbSellerRequests_RequestType_Status",
                table: "TbSellerRequests",
                columns: new[] { "RequestType", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_TbSellerRequests_Status",
                table: "TbSellerRequests",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_TbSellerRequests_SubmittedAt",
                table: "TbSellerRequests",
                column: "SubmittedAt");

            migrationBuilder.CreateIndex(
                name: "IX_TbSellerRequests_VendorId_Status",
                table: "TbSellerRequests",
                columns: new[] { "VendorId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_TbSellerPerformanceMetrics_AverageRating",
                table: "TbSellerPerformanceMetrics",
                column: "AverageRating");

            migrationBuilder.CreateIndex(
                name: "IX_TbSellerPerformanceMetrics_BuyBoxWinRate",
                table: "TbSellerPerformanceMetrics",
                column: "BuyBoxWinRate");

            migrationBuilder.CreateIndex(
                name: "IX_TbSellerPerformanceMetrics_OrderCompletionRate",
                table: "TbSellerPerformanceMetrics",
                column: "OrderCompletionRate");

            migrationBuilder.CreateIndex(
                name: "IX_TbSellerPerformanceMetrics_UsesFBM",
                table: "TbSellerPerformanceMetrics",
                column: "UsesFBM");

            migrationBuilder.CreateIndex(
                name: "IX_TbSellerPerformanceMetrics_VendorId_CalculatedForPeriodStart",
                table: "TbSellerPerformanceMetrics",
                columns: new[] { "VendorId", "CalculatedForPeriodStart" });

            migrationBuilder.CreateIndex(
                name: "IX_TbSalesReviews_CustomerID",
                table: "TbSalesReviews",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_TbSalesReviews_OrderItemID",
                table: "TbSalesReviews",
                column: "OrderItemID");

            migrationBuilder.CreateIndex(
                name: "IX_TbSalesReviews_OverallRating",
                table: "TbSalesReviews",
                column: "OverallRating");

            migrationBuilder.CreateIndex(
                name: "IX_TbSalesReviews_ReviewDate",
                table: "TbSalesReviews",
                column: "ReviewDate");

            migrationBuilder.CreateIndex(
                name: "IX_TbSalesReviews_ReviewNumber",
                table: "TbSalesReviews",
                column: "ReviewNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TbSalesReviews_Status",
                table: "TbSalesReviews",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_TbSalesReviews_VendorID",
                table: "TbSalesReviews",
                column: "VendorID");

            migrationBuilder.CreateIndex(
                name: "IX_TbSalesReviews_VendorID_CustomerID",
                table: "TbSalesReviews",
                columns: new[] { "VendorID", "CustomerID" });

            migrationBuilder.CreateIndex(
                name: "IX_TbReviewVotes_CustomerID",
                table: "TbReviewVotes",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_TbReviewVotes_ReviewID_CustomerID_VoteType",
                table: "TbReviewVotes",
                columns: new[] { "ReviewID", "CustomerID", "VoteType" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TbReviewVotes_VoteType",
                table: "TbReviewVotes",
                column: "VoteType");

            migrationBuilder.CreateIndex(
                name: "IX_TbReviewVotes_VoteValue",
                table: "TbReviewVotes",
                column: "VoteValue");

            migrationBuilder.CreateIndex(
                name: "IX_TbRequestDocuments_DocumentType",
                table: "TbRequestDocuments",
                column: "DocumentType");

            migrationBuilder.CreateIndex(
                name: "IX_TbRequestDocuments_UploadedAt",
                table: "TbRequestDocuments",
                column: "UploadedAt");

            migrationBuilder.CreateIndex(
                name: "IX_TbRequestComments_CreatedDateUtc",
                table: "TbRequestComments",
                column: "CreatedDateUtc");

            migrationBuilder.CreateIndex(
                name: "IX_TbRequestComments_SellerRequestId_CreatedDateUtc",
                table: "TbRequestComments",
                columns: new[] { "SellerRequestId", "CreatedDateUtc" });

            migrationBuilder.CreateIndex(
                name: "IX_TbQuantityPricings_IsActive",
                table: "TbQuantityPricings",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_TbQuantityPricings_MinimumQuantity",
                table: "TbQuantityPricings",
                column: "MinimumQuantity");

            migrationBuilder.CreateIndex(
                name: "IX_TbQuantityPricings_OfferId_MinimumQuantity",
                table: "TbQuantityPricings",
                columns: new[] { "OfferId", "MinimumQuantity" },
                unique: true);

            migrationBuilder.AddCheckConstraint(
                name: "CK_QuantityPricing_Quantities",
                table: "TbQuantityPricings",
                sql: "[MaximumQuantity] IS NULL OR [MaximumQuantity] > [MinimumQuantity]");

            migrationBuilder.CreateIndex(
                name: "IX_TbProductVisibilityRules_IsVisible",
                table: "TbProductVisibilityRules",
                column: "IsVisible");

            migrationBuilder.CreateIndex(
                name: "IX_TbProductVisibilityRules_IsVisible_VisibilityStatus",
                table: "TbProductVisibilityRules",
                columns: new[] { "IsVisible", "VisibilityStatus" });

            migrationBuilder.CreateIndex(
                name: "IX_TbProductVisibilityRules_ItemId",
                table: "TbProductVisibilityRules",
                column: "ItemId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TbProductVisibilityRules_LastCheckedAt",
                table: "TbProductVisibilityRules",
                column: "LastCheckedAt");

            migrationBuilder.CreateIndex(
                name: "IX_TbProductVisibilityRules_VisibilityStatus",
                table: "TbProductVisibilityRules",
                column: "VisibilityStatus");

            migrationBuilder.CreateIndex(
                name: "IX_TbProductReviews_CustomerID",
                table: "TbProductReviews",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_TbProductReviews_IsVerifiedPurchase",
                table: "TbProductReviews",
                column: "IsVerifiedPurchase");

            migrationBuilder.CreateIndex(
                name: "IX_TbProductReviews_OrderItemID",
                table: "TbProductReviews",
                column: "OrderItemID");

            migrationBuilder.CreateIndex(
                name: "IX_TbProductReviews_ProductID",
                table: "TbProductReviews",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_TbProductReviews_ProductID_CustomerID",
                table: "TbProductReviews",
                columns: new[] { "ProductID", "CustomerID" });

            migrationBuilder.CreateIndex(
                name: "IX_TbProductReviews_Rating",
                table: "TbProductReviews",
                column: "Rating");

            migrationBuilder.CreateIndex(
                name: "IX_TbProductReviews_ReviewDate",
                table: "TbProductReviews",
                column: "ReviewDate");

            migrationBuilder.CreateIndex(
                name: "IX_TbProductReviews_ReviewNumber",
                table: "TbProductReviews",
                column: "ReviewNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TbProductReviews_Status",
                table: "TbProductReviews",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_TbPriceHistories_ChangedAt",
                table: "TbPriceHistories",
                column: "ChangedAt");

            migrationBuilder.CreateIndex(
                name: "IX_TbPriceHistories_IsAutomatic",
                table: "TbPriceHistories",
                column: "IsAutomatic");

            migrationBuilder.CreateIndex(
                name: "IX_TbPriceHistories_OfferId_ChangedAt",
                table: "TbPriceHistories",
                columns: new[] { "OfferId", "ChangedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_TbPlatformTreasuries_LastReconciliationDate",
                table: "TbPlatformTreasuries",
                column: "LastReconciliationDate");

            migrationBuilder.CreateIndex(
                name: "IX_TbOffers_IsActive",
                table: "TbOffers",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_TbOffers_Item_Active_Price",
                table: "TbOffers",
                columns: new[] { "ItemId", "IsActive", "Price" });

            migrationBuilder.CreateIndex(
                name: "IX_TbOffers_Price",
                table: "TbOffers",
                column: "Price");

            migrationBuilder.CreateIndex(
                name: "IX_TbNotificationPreferences_NotificationType",
                table: "TbNotificationPreferences",
                column: "NotificationType");

            migrationBuilder.CreateIndex(
                name: "IX_TbNotificationPreferences_UserId_UserType",
                table: "TbNotificationPreferences",
                columns: new[] { "UserId", "UserType" });

            migrationBuilder.CreateIndex(
                name: "IX_TbNotificationPreferences_UserId_UserType_NotificationType",
                table: "TbNotificationPreferences",
                columns: new[] { "UserId", "UserType", "NotificationType" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TbNotificationChannels_Channel",
                table: "TbNotificationChannels",
                column: "Channel");

            migrationBuilder.CreateIndex(
                name: "IX_TbNotificationChannels_IsActive",
                table: "TbNotificationChannels",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_TbMortems_DocumentNumber",
                table: "TbMortems",
                column: "DocumentNumber");

            migrationBuilder.CreateIndex(
                name: "IX_TbMortems_Status",
                table: "TbMortems",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_TbMoitems_DocumentDate",
                table: "TbMoitems",
                column: "DocumentDate");

            migrationBuilder.CreateIndex(
                name: "IX_TbMoitems_DocumentNumber",
                table: "TbMoitems",
                column: "DocumentNumber");

            migrationBuilder.CreateIndex(
                name: "IX_TbMediaContents_DisplayOrder",
                table: "TbMediaContents",
                column: "DisplayOrder");

            migrationBuilder.CreateIndex(
                name: "IX_TbMediaContents_IsActive",
                table: "TbMediaContents",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_TbMediaContents_MediaType",
                table: "TbMediaContents",
                column: "MediaType");

            migrationBuilder.CreateIndex(
                name: "IX_TbLoyaltyTiers_DisplayOrder",
                table: "TbLoyaltyTiers",
                column: "DisplayOrder");

            migrationBuilder.CreateIndex(
                name: "IX_TbLoyaltyTiers_MinimumOrdersPerYear_MaximumOrdersPerYear",
                table: "TbLoyaltyTiers",
                columns: new[] { "MinimumOrdersPerYear", "MaximumOrdersPerYear" });

            migrationBuilder.CreateIndex(
                name: "IX_TbLoyaltyTiers_TierNameEn",
                table: "TbLoyaltyTiers",
                column: "TierNameEn",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TbLoyaltyPointsTransactions_CreatedDateUtc",
                table: "TbLoyaltyPointsTransactions",
                column: "CreatedDateUtc");

            migrationBuilder.CreateIndex(
                name: "IX_TbLoyaltyPointsTransactions_CustomerLoyaltyId_TransactionType",
                table: "TbLoyaltyPointsTransactions",
                columns: new[] { "CustomerLoyaltyId", "TransactionType" });

            migrationBuilder.CreateIndex(
                name: "IX_TbLoyaltyPointsTransactions_IsExpired",
                table: "TbLoyaltyPointsTransactions",
                column: "IsExpired");

            migrationBuilder.CreateIndex(
                name: "IX_TbLoyaltyPointsTransactions_TransactionType",
                table: "TbLoyaltyPointsTransactions",
                column: "TransactionType");

            migrationBuilder.CreateIndex(
                name: "IX_TbItems_IsNewArrival",
                table: "TbItems",
                column: "IsNewArrival");

            migrationBuilder.CreateIndex(
                name: "IX_TbItemImages_Order",
                table: "TbItemImages",
                column: "Order");

            migrationBuilder.CreateIndex(
                name: "IX_TbItemAttributeCombinationPricings_AttributeIds",
                table: "TbItemAttributeCombinationPricings",
                column: "AttributeIds");

            migrationBuilder.CreateIndex(
                name: "IX_TbItemAttributeCombinationPricings_Price",
                table: "TbItemAttributeCombinationPricings",
                column: "Price");

            migrationBuilder.CreateIndex(
                name: "IX_TbHomepageBlocks_ActiveFrom_ActiveTo",
                table: "TbHomepageBlocks",
                columns: new[] { "ActiveFrom", "ActiveTo" });

            migrationBuilder.CreateIndex(
                name: "IX_TbHomepageBlocks_BlockType",
                table: "TbHomepageBlocks",
                column: "BlockType");

            migrationBuilder.CreateIndex(
                name: "IX_TbHomepageBlocks_DisplayOrder",
                table: "TbHomepageBlocks",
                column: "DisplayOrder");

            migrationBuilder.CreateIndex(
                name: "IX_TbHomepageBlocks_IsActive",
                table: "TbHomepageBlocks",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_TbHomepageBlocks_IsActive_DisplayOrder",
                table: "TbHomepageBlocks",
                columns: new[] { "IsActive", "DisplayOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_TbHomepageBlocks_IsVisible",
                table: "TbHomepageBlocks",
                column: "IsVisible");

            migrationBuilder.CreateIndex(
                name: "IX_TbFulfillmentMethods_Code",
                table: "TbFulfillmentMethods",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TbFulfillmentMethods_DisplayOrder",
                table: "TbFulfillmentMethods",
                column: "DisplayOrder");

            migrationBuilder.CreateIndex(
                name: "IX_TbFulfillmentMethods_IsActive",
                table: "TbFulfillmentMethods",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_TbFulfillmentFees_EffectiveFrom_EffectiveTo",
                table: "TbFulfillmentFees",
                columns: new[] { "EffectiveFrom", "EffectiveTo" });

            migrationBuilder.CreateIndex(
                name: "IX_TbFulfillmentFees_FeeType",
                table: "TbFulfillmentFees",
                column: "FeeType");

            migrationBuilder.CreateIndex(
                name: "IX_TbFulfillmentFees_FulfillmentMethodId_FeeType",
                table: "TbFulfillmentFees",
                columns: new[] { "FulfillmentMethodId", "FeeType" });

            migrationBuilder.CreateIndex(
                name: "IX_TbFulfillmentFees_IsActive",
                table: "TbFulfillmentFees",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_TbFlashSales_EndDate",
                table: "TbFlashSales",
                column: "EndDate");

            migrationBuilder.CreateIndex(
                name: "IX_TbFlashSales_IsActive",
                table: "TbFlashSales",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_TbFlashSales_StartDate",
                table: "TbFlashSales",
                column: "StartDate");

            migrationBuilder.CreateIndex(
                name: "IX_TbFlashSales_StartDate_EndDate_IsActive",
                table: "TbFlashSales",
                columns: new[] { "StartDate", "EndDate", "IsActive" });

            migrationBuilder.AddCheckConstraint(
                name: "CK_FlashSale_DurationInHours",
                table: "TbFlashSales",
                sql: "[DurationInHours] >= 6 AND [DurationInHours] <= 48");

            migrationBuilder.CreateIndex(
                name: "IX_TbFlashSaleProducts_DisplayOrder",
                table: "TbFlashSaleProducts",
                column: "DisplayOrder");

            migrationBuilder.CreateIndex(
                name: "IX_TbFlashSaleProducts_FlashSaleId_ItemId",
                table: "TbFlashSaleProducts",
                columns: new[] { "FlashSaleId", "ItemId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TbFlashSaleProducts_FlashSaleId_SoldQuantity",
                table: "TbFlashSaleProducts",
                columns: new[] { "FlashSaleId", "SoldQuantity" });

            migrationBuilder.CreateIndex(
                name: "IX_TbFlashSaleProducts_IsActive",
                table: "TbFlashSaleProducts",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_TbFBMShipments_ShipmentNumber",
                table: "TbFBMShipments",
                column: "ShipmentNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TbFBMShipments_ShippedDate",
                table: "TbFBMShipments",
                column: "ShippedDate");

            migrationBuilder.CreateIndex(
                name: "IX_TbFBMShipments_Status",
                table: "TbFBMShipments",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_TbFBMShipments_Status_ShippedDate",
                table: "TbFBMShipments",
                columns: new[] { "Status", "ShippedDate" });

            migrationBuilder.CreateIndex(
                name: "IX_TbFBMShipments_TrackingNumber",
                table: "TbFBMShipments",
                column: "TrackingNumber");

            migrationBuilder.CreateIndex(
                name: "IX_TbFBMInventories_ItemId_WarehouseId_VendorId",
                table: "TbFBMInventories",
                columns: new[] { "ItemId", "WarehouseId", "VendorId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TbFBMInventories_SKU",
                table: "TbFBMInventories",
                column: "SKU");

            migrationBuilder.CreateIndex(
                name: "IX_TbFBMInventories_WarehouseId_AvailableQuantity",
                table: "TbFBMInventories",
                columns: new[] { "WarehouseId", "AvailableQuantity" });

            migrationBuilder.CreateIndex(
                name: "IX_TbDisputes_CreatedDateUtc",
                table: "TbDisputes",
                column: "CreatedDateUtc");

            migrationBuilder.CreateIndex(
                name: "IX_TbDisputes_DisputeNumber",
                table: "TbDisputes",
                column: "DisputeNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TbDisputes_OrderID",
                table: "TbDisputes",
                column: "OrderID");

            migrationBuilder.CreateIndex(
                name: "IX_TbDisputes_SenderID",
                table: "TbDisputes",
                column: "SenderID");

            migrationBuilder.CreateIndex(
                name: "IX_TbDisputes_Status",
                table: "TbDisputes",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_TbDisputeMessages_DisputeID_MessageNumber",
                table: "TbDisputeMessages",
                columns: new[] { "DisputeID", "MessageNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TbDisputeMessages_SenderID",
                table: "TbDisputeMessages",
                column: "SenderID");

            migrationBuilder.CreateIndex(
                name: "IX_TbDisputeMessages_SentDate",
                table: "TbDisputeMessages",
                column: "SentDate");

            migrationBuilder.CreateIndex(
                name: "IX_TbDeliveryReviews_CustomerID",
                table: "TbDeliveryReviews",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_TbDeliveryReviews_OrderID",
                table: "TbDeliveryReviews",
                column: "OrderID");

            migrationBuilder.CreateIndex(
                name: "IX_TbDeliveryReviews_OrderID_CustomerID",
                table: "TbDeliveryReviews",
                columns: new[] { "OrderID", "CustomerID" });

            migrationBuilder.CreateIndex(
                name: "IX_TbDeliveryReviews_OverallRating",
                table: "TbDeliveryReviews",
                column: "OverallRating");

            migrationBuilder.CreateIndex(
                name: "IX_TbDeliveryReviews_ReviewDate",
                table: "TbDeliveryReviews",
                column: "ReviewDate");

            migrationBuilder.CreateIndex(
                name: "IX_TbDeliveryReviews_ReviewNumber",
                table: "TbDeliveryReviews",
                column: "ReviewNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TbDeliveryReviews_Status",
                table: "TbDeliveryReviews",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_TbCustomerWallets_AvailableBalance",
                table: "TbCustomerWallets",
                column: "AvailableBalance");

            migrationBuilder.CreateIndex(
                name: "IX_TbCustomerWallets_CustomerId",
                table: "TbCustomerWallets",
                column: "CustomerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TbCustomerSegmentPricings_EffectiveFrom_EffectiveTo",
                table: "TbCustomerSegmentPricings",
                columns: new[] { "EffectiveFrom", "EffectiveTo" });

            migrationBuilder.CreateIndex(
                name: "IX_TbCustomerSegmentPricings_IsActive",
                table: "TbCustomerSegmentPricings",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_TbCustomerSegmentPricings_OfferId_SegmentType",
                table: "TbCustomerSegmentPricings",
                columns: new[] { "OfferId", "SegmentType" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TbCustomerSegmentPricings_SegmentType",
                table: "TbCustomerSegmentPricings",
                column: "SegmentType");

            migrationBuilder.CreateIndex(
                name: "IX_TbCustomerLoyalties_AvailablePoints",
                table: "TbCustomerLoyalties",
                column: "AvailablePoints");

            migrationBuilder.CreateIndex(
                name: "IX_TbCustomerLoyalties_CustomerId",
                table: "TbCustomerLoyalties",
                column: "CustomerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TbCurrencies_Code",
                table: "TbCurrencies",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TbCurrencies_IsActive",
                table: "TbCurrencies",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_TbCurrencies_IsBaseCurrency",
                table: "TbCurrencies",
                column: "IsBaseCurrency");

            migrationBuilder.CreateIndex(
                name: "IX_TbCouponCodes_Code",
                table: "TbCouponCodes",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TbContentAreas_AreaCode",
                table: "TbContentAreas",
                column: "AreaCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TbContentAreas_DisplayOrder",
                table: "TbContentAreas",
                column: "DisplayOrder");

            migrationBuilder.CreateIndex(
                name: "IX_TbContentAreas_IsActive",
                table: "TbContentAreas",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_TbCampaignVendors_CampaignId_VendorId",
                table: "TbCampaignVendors",
                columns: new[] { "CampaignId", "VendorId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TbCampaignVendors_IsApproved",
                table: "TbCampaignVendors",
                column: "IsApproved");

            migrationBuilder.CreateIndex(
                name: "IX_TbCampaigns_CampaignType",
                table: "TbCampaigns",
                column: "CampaignType");

            migrationBuilder.CreateIndex(
                name: "IX_TbCampaigns_EndDate",
                table: "TbCampaigns",
                column: "EndDate");

            migrationBuilder.CreateIndex(
                name: "IX_TbCampaigns_IsActive",
                table: "TbCampaigns",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_TbCampaigns_IsFeatured",
                table: "TbCampaigns",
                column: "IsFeatured");

            migrationBuilder.CreateIndex(
                name: "IX_TbCampaigns_SlugEn",
                table: "TbCampaigns",
                column: "SlugEn",
                unique: true,
                filter: "[SlugEn] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_TbCampaigns_StartDate",
                table: "TbCampaigns",
                column: "StartDate");

            migrationBuilder.CreateIndex(
                name: "IX_TbCampaigns_StartDate_EndDate_IsActive",
                table: "TbCampaigns",
                columns: new[] { "StartDate", "EndDate", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_TbCampaignProducts_CampaignId_IsActive",
                table: "TbCampaignProducts",
                columns: new[] { "CampaignId", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_TbCampaignProducts_CampaignId_ItemId",
                table: "TbCampaignProducts",
                columns: new[] { "CampaignId", "ItemId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TbCampaignProducts_DisplayOrder",
                table: "TbCampaignProducts",
                column: "DisplayOrder");

            migrationBuilder.CreateIndex(
                name: "IX_TbCampaignProducts_IsActive",
                table: "TbCampaignProducts",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_TbBuyBoxHistories_ItemId_WonAt",
                table: "TbBuyBoxHistories",
                columns: new[] { "ItemId", "WonAt" });

            migrationBuilder.CreateIndex(
                name: "IX_TbBuyBoxHistories_LostAt",
                table: "TbBuyBoxHistories",
                column: "LostAt");

            migrationBuilder.CreateIndex(
                name: "IX_TbBuyBoxHistories_VendorId_WonAt",
                table: "TbBuyBoxHistories",
                columns: new[] { "VendorId", "WonAt" });

            migrationBuilder.CreateIndex(
                name: "IX_TbBuyBoxHistories_WonAt",
                table: "TbBuyBoxHistories",
                column: "WonAt");

            migrationBuilder.CreateIndex(
                name: "IX_TbBuyBoxCalculations_CalculatedAt",
                table: "TbBuyBoxCalculations",
                column: "CalculatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_TbBuyBoxCalculations_ExpiresAt",
                table: "TbBuyBoxCalculations",
                column: "ExpiresAt");

            migrationBuilder.CreateIndex(
                name: "IX_TbBuyBoxCalculations_ItemId_CalculatedAt",
                table: "TbBuyBoxCalculations",
                columns: new[] { "ItemId", "CalculatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_TbBuyBoxCalculations_TbOfferId",
                table: "TbBuyBoxCalculations",
                column: "TbOfferId");

            migrationBuilder.CreateIndex(
                name: "IX_TbBuyBoxCalculations_TotalScore",
                table: "TbBuyBoxCalculations",
                column: "TotalScore");

            migrationBuilder.CreateIndex(
                name: "IX_TbBrands_DisplayOrder",
                table: "TbBrands",
                column: "DisplayOrder");

            migrationBuilder.CreateIndex(
                name: "IX_TbBrandRegistrationRequests_BrandNameEn_VendorId",
                table: "TbBrandRegistrationRequests",
                columns: new[] { "BrandNameEn", "VendorId" });

            migrationBuilder.CreateIndex(
                name: "IX_TbBrandRegistrationRequests_BrandType",
                table: "TbBrandRegistrationRequests",
                column: "BrandType");

            migrationBuilder.CreateIndex(
                name: "IX_TbBrandRegistrationRequests_Status",
                table: "TbBrandRegistrationRequests",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_TbBrandRegistrationRequests_SubmittedAt",
                table: "TbBrandRegistrationRequests",
                column: "SubmittedAt");

            migrationBuilder.CreateIndex(
                name: "IX_TbBrandRegistrationRequests_TrademarkNumber",
                table: "TbBrandRegistrationRequests",
                column: "TrademarkNumber");

            migrationBuilder.CreateIndex(
                name: "IX_TbBrandRegistrationRequests_VendorId_Status",
                table: "TbBrandRegistrationRequests",
                columns: new[] { "VendorId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_TbBrandDocuments_BrandRegistrationRequestId_DocumentType",
                table: "TbBrandDocuments",
                columns: new[] { "BrandRegistrationRequestId", "DocumentType" });

            migrationBuilder.CreateIndex(
                name: "IX_TbBrandDocuments_DocumentType",
                table: "TbBrandDocuments",
                column: "DocumentType");

            migrationBuilder.CreateIndex(
                name: "IX_TbBrandDocuments_IsVerified",
                table: "TbBrandDocuments",
                column: "IsVerified");

            migrationBuilder.CreateIndex(
                name: "IX_TbBrandDocuments_UploadedAt",
                table: "TbBrandDocuments",
                column: "UploadedAt");

            migrationBuilder.CreateIndex(
                name: "IX_TbBlockProducts_DisplayOrder",
                table: "TbBlockProducts",
                column: "DisplayOrder");

            migrationBuilder.CreateIndex(
                name: "IX_TbBlockProducts_HomepageBlockId_DisplayOrder",
                table: "TbBlockProducts",
                columns: new[] { "HomepageBlockId", "DisplayOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_TbBlockProducts_HomepageBlockId_ItemId",
                table: "TbBlockProducts",
                columns: new[] { "HomepageBlockId", "ItemId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TbBlockProducts_IsActive",
                table: "TbBlockProducts",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_TbBlockProducts_IsFeatured",
                table: "TbBlockProducts",
                column: "IsFeatured");

            migrationBuilder.CreateIndex(
                name: "IX_TbAuthorizedDistributors_AuthorizationNumber",
                table: "TbAuthorizedDistributors",
                column: "AuthorizationNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TbAuthorizedDistributors_AuthorizationStartDate_AuthorizationEndDate",
                table: "TbAuthorizedDistributors",
                columns: new[] { "AuthorizationStartDate", "AuthorizationEndDate" });

            migrationBuilder.CreateIndex(
                name: "IX_TbAuthorizedDistributors_BrandId_VendorId",
                table: "TbAuthorizedDistributors",
                columns: new[] { "BrandId", "VendorId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TbAuthorizedDistributors_IsActive",
                table: "TbAuthorizedDistributors",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_IsRead",
                table: "Notifications",
                column: "IsRead");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_NotificationType",
                table: "Notifications",
                column: "NotificationType");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_RecipientType_RecipientID",
                table: "Notifications",
                columns: new[] { "RecipientType", "RecipientID" });

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_SentDate",
                table: "Notifications",
                column: "SentDate");

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
                name: "FK_TbBrandDocuments_TbBrandRegistrationRequests_BrandRegistrationRequestId",
                table: "TbBrandDocuments",
                column: "BrandRegistrationRequestId",
                principalTable: "TbBrandRegistrationRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbBrandRegistrationRequests_TbVendors_VendorId",
                table: "TbBrandRegistrationRequests",
                column: "VendorId",
                principalTable: "TbVendors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbBuyBoxCalculations_TbOffers_TbOfferId",
                table: "TbBuyBoxCalculations",
                column: "TbOfferId",
                principalTable: "TbOffers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TbBuyBoxHistories_TbItems_ItemId",
                table: "TbBuyBoxHistories",
                column: "ItemId",
                principalTable: "TbItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbCampaignProducts_TbCampaigns_CampaignId",
                table: "TbCampaignProducts",
                column: "CampaignId",
                principalTable: "TbCampaigns",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbCampaignVendors_TbCampaigns_CampaignId",
                table: "TbCampaignVendors",
                column: "CampaignId",
                principalTable: "TbCampaigns",
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
                name: "FK_TbCustomerSegmentPricings_TbOffers_OfferId",
                table: "TbCustomerSegmentPricings",
                column: "OfferId",
                principalTable: "TbOffers",
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
                name: "FK_TbFlashSaleProducts_TbFlashSales_FlashSaleId",
                table: "TbFlashSaleProducts",
                column: "FlashSaleId",
                principalTable: "TbFlashSales",
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
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbLoyaltyPointsTransactions_TbCustomerLoyalties_CustomerLoyaltyId",
                table: "TbLoyaltyPointsTransactions",
                column: "CustomerLoyaltyId",
                principalTable: "TbCustomerLoyalties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TbMovitemsdetails_TbMoitems_MoitemId",
                table: "TbMovitemsdetails",
                column: "MoitemId",
                principalTable: "TbMoitems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbMovitemsdetails_TbMortems_MortemId",
                table: "TbMovitemsdetails",
                column: "MortemId",
                principalTable: "TbMortems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbMovitemsdetails_TbWarehouses_WarehouseId",
                table: "TbMovitemsdetails",
                column: "WarehouseId",
                principalTable: "TbWarehouses",
                principalColumn: "Id");

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
                name: "FK_TbOffers_TbOfferConditions_TbOfferConditionId",
                table: "TbOffers",
                column: "TbOfferConditionId",
                principalTable: "TbOfferConditions",
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
                name: "FK_TbPriceHistories_TbOffers_OfferId",
                table: "TbPriceHistories",
                column: "OfferId",
                principalTable: "TbOffers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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
                name: "FK_TbRequestComments_TbSellerRequests_SellerRequestId",
                table: "TbRequestComments",
                column: "SellerRequestId",
                principalTable: "TbSellerRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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
                name: "FK_TbStates_TbCountries_CountryId",
                table: "TbStates",
                column: "CountryId",
                principalTable: "TbCountries",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TbSupportTicketMessages_TbSupportTickets_TicketID",
                table: "TbSupportTicketMessages",
                column: "TicketID",
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
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TbUnitConversions_TbUnits_ToUnitId",
                table: "TbUnitConversions",
                column: "ToUnitId",
                principalTable: "TbUnits",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TbUserNotifications_AspNetUsers_UserId",
                table: "TbUserNotifications",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbUserNotifications_Notifications_NotificationId",
                table: "TbUserNotifications",
                column: "NotificationId",
                principalTable: "Notifications",
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
                name: "FK_TbVendorTierHistories_TbVendors_VendorId",
                table: "TbVendorTierHistories",
                column: "VendorId",
                principalTable: "TbVendors",
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
                name: "FK_TbVisibilityLogs_TbItems_ItemId",
                table: "TbVisibilityLogs",
                column: "ItemId",
                principalTable: "TbItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbWalletTransactions_TbCustomerWallets_CustomerWalletId",
                table: "TbWalletTransactions",
                column: "CustomerWalletId",
                principalTable: "TbCustomerWallets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbWalletTransactions_TbVendorWallets_VendorWalletId",
                table: "TbWalletTransactions",
                column: "VendorWalletId",
                principalTable: "TbVendorWallets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbWarranties_TbCities_CityId",
                table: "TbWarranties",
                column: "CityId",
                principalTable: "TbCities",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
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
                name: "FK_TbAuthorizedDistributors_TbVendors_VendorId",
                table: "TbAuthorizedDistributors");

            migrationBuilder.DropForeignKey(
                name: "FK_TbBlockProducts_TbHomepageBlocks_HomepageBlockId",
                table: "TbBlockProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_TbBrandDocuments_TbBrandRegistrationRequests_BrandRegistrationRequestId",
                table: "TbBrandDocuments");

            migrationBuilder.DropForeignKey(
                name: "FK_TbBrandRegistrationRequests_TbVendors_VendorId",
                table: "TbBrandRegistrationRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_TbBuyBoxCalculations_TbOffers_TbOfferId",
                table: "TbBuyBoxCalculations");

            migrationBuilder.DropForeignKey(
                name: "FK_TbBuyBoxHistories_TbItems_ItemId",
                table: "TbBuyBoxHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_TbCampaignProducts_TbCampaigns_CampaignId",
                table: "TbCampaignProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_TbCampaignVendors_TbCampaigns_CampaignId",
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
                name: "FK_TbCustomerSegmentPricings_TbOffers_OfferId",
                table: "TbCustomerSegmentPricings");

            migrationBuilder.DropForeignKey(
                name: "FK_TbCustomerWallets_TbCustomers_CustomerId",
                table: "TbCustomerWallets");

            migrationBuilder.DropForeignKey(
                name: "FK_TbDisputeMessages_TbDisputes_DisputeID",
                table: "TbDisputeMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_TbFlashSaleProducts_TbFlashSales_FlashSaleId",
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
                name: "FK_TbOffers_TbOfferConditions_TbOfferConditionId",
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
                name: "FK_TbPriceHistories_TbOffers_OfferId",
                table: "TbPriceHistories");

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
                name: "FK_TbRequestComments_TbSellerRequests_SellerRequestId",
                table: "TbRequestComments");

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
                name: "FK_TbSellerRequests_TbVendors_VendorId",
                table: "TbSellerRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_TbSellerTierBenefits_TbSellerTiers_SellerTierId",
                table: "TbSellerTierBenefits");

            migrationBuilder.DropForeignKey(
                name: "FK_TbShippingDetails_TbCities_CityId",
                table: "TbShippingDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_TbStates_TbCountries_CountryId",
                table: "TbStates");

            migrationBuilder.DropForeignKey(
                name: "FK_TbSupportTicketMessages_TbSupportTickets_TicketID",
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
                name: "FK_TbUserNotifications_Notifications_NotificationId",
                table: "TbUserNotifications");

            migrationBuilder.DropForeignKey(
                name: "FK_TbUserOfferRatings_AspNetUsers_UserId",
                table: "TbUserOfferRatings");

            migrationBuilder.DropForeignKey(
                name: "FK_TbVendorTierHistories_TbVendors_VendorId",
                table: "TbVendorTierHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_TbVendorWallets_TbVendors_VendorId",
                table: "TbVendorWallets");

            migrationBuilder.DropForeignKey(
                name: "FK_TbVisibilityLogs_TbItems_ItemId",
                table: "TbVisibilityLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_TbWalletTransactions_TbCustomerWallets_CustomerWalletId",
                table: "TbWalletTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_TbWalletTransactions_TbVendorWallets_VendorWalletId",
                table: "TbWalletTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_TbWarranties_TbCities_CityId",
                table: "TbWarranties");

            migrationBuilder.DropIndex(
                name: "IX_TbWarehouses_IsActive",
                table: "TbWarehouses");

            migrationBuilder.DropIndex(
                name: "IX_TbWalletTransactions_CreatedDateUtc",
                table: "TbWalletTransactions");

            migrationBuilder.DropIndex(
                name: "IX_TbWalletTransactions_CustomerWalletId_Status",
                table: "TbWalletTransactions");

            migrationBuilder.DropIndex(
                name: "IX_TbWalletTransactions_ReferenceNumber",
                table: "TbWalletTransactions");

            migrationBuilder.DropIndex(
                name: "IX_TbWalletTransactions_Status",
                table: "TbWalletTransactions");

            migrationBuilder.DropIndex(
                name: "IX_TbWalletTransactions_TransactionType",
                table: "TbWalletTransactions");

            migrationBuilder.DropIndex(
                name: "IX_TbWalletTransactions_VendorWalletId_Status",
                table: "TbWalletTransactions");

            migrationBuilder.DropIndex(
                name: "IX_TbVisibilityLogs_ChangedAt",
                table: "TbVisibilityLogs");

            migrationBuilder.DropIndex(
                name: "IX_TbVisibilityLogs_IsAutomatic",
                table: "TbVisibilityLogs");

            migrationBuilder.DropIndex(
                name: "IX_TbVisibilityLogs_ItemId_ChangedAt",
                table: "TbVisibilityLogs");

            migrationBuilder.DropIndex(
                name: "IX_TbVendorWallets_AvailableBalance",
                table: "TbVendorWallets");

            migrationBuilder.DropIndex(
                name: "IX_TbVendorWallets_VendorId",
                table: "TbVendorWallets");

            migrationBuilder.DropIndex(
                name: "IX_TbVendorTierHistories_AchievedAt",
                table: "TbVendorTierHistories");

            migrationBuilder.DropIndex(
                name: "IX_TbVendorTierHistories_EndedAt",
                table: "TbVendorTierHistories");

            migrationBuilder.DropIndex(
                name: "IX_TbVendorTierHistories_VendorId_AchievedAt",
                table: "TbVendorTierHistories");

            migrationBuilder.DropIndex(
                name: "IX_TbUserNotifications_IsRead",
                table: "TbUserNotifications");

            migrationBuilder.DropIndex(
                name: "IX_TbSuppressionReasons_DetectedAt",
                table: "TbSuppressionReasons");

            migrationBuilder.DropIndex(
                name: "IX_TbSuppressionReasons_IsResolved",
                table: "TbSuppressionReasons");

            migrationBuilder.DropIndex(
                name: "IX_TbSuppressionReasons_IsResolved_DetectedAt",
                table: "TbSuppressionReasons");

            migrationBuilder.DropIndex(
                name: "IX_TbSuppressionReasons_ProductVisibilityRuleId_ReasonType",
                table: "TbSuppressionReasons");

            migrationBuilder.DropIndex(
                name: "IX_TbSuppressionReasons_ReasonType",
                table: "TbSuppressionReasons");

            migrationBuilder.DropIndex(
                name: "IX_TbSupportTickets_AssignedTo",
                table: "TbSupportTickets");

            migrationBuilder.DropIndex(
                name: "IX_TbSupportTickets_Category",
                table: "TbSupportTickets");

            migrationBuilder.DropIndex(
                name: "IX_TbSupportTickets_CreatedDateUtc",
                table: "TbSupportTickets");

            migrationBuilder.DropIndex(
                name: "IX_TbSupportTickets_Priority",
                table: "TbSupportTickets");

            migrationBuilder.DropIndex(
                name: "IX_TbSupportTickets_Status",
                table: "TbSupportTickets");

            migrationBuilder.DropIndex(
                name: "IX_TbSupportTickets_TicketNumber",
                table: "TbSupportTickets");

            migrationBuilder.DropIndex(
                name: "IX_TbSupportTickets_UserID",
                table: "TbSupportTickets");

            migrationBuilder.DropIndex(
                name: "IX_TbSupportTicketMessages_SenderID",
                table: "TbSupportTicketMessages");

            migrationBuilder.DropIndex(
                name: "IX_TbSupportTicketMessages_SentDate",
                table: "TbSupportTicketMessages");

            migrationBuilder.DropIndex(
                name: "IX_TbSupportTicketMessages_TicketID",
                table: "TbSupportTicketMessages");

            migrationBuilder.DropIndex(
                name: "IX_TbSupportTicketMessages_TicketID_MessageNumber",
                table: "TbSupportTicketMessages");

            migrationBuilder.DropIndex(
                name: "IX_TbSellerTiers_DisplayOrder",
                table: "TbSellerTiers");

            migrationBuilder.DropIndex(
                name: "IX_TbSellerTiers_IsActive",
                table: "TbSellerTiers");

            migrationBuilder.DropIndex(
                name: "IX_TbSellerTiers_MinimumOrders_MaximumOrders",
                table: "TbSellerTiers");

            migrationBuilder.DropIndex(
                name: "IX_TbSellerTiers_TierCode",
                table: "TbSellerTiers");

            migrationBuilder.DropCheckConstraint(
                name: "CK_SellerTier_Orders",
                table: "TbSellerTiers");

            migrationBuilder.DropIndex(
                name: "IX_TbSellerTierBenefits_DisplayOrder",
                table: "TbSellerTierBenefits");

            migrationBuilder.DropIndex(
                name: "IX_TbSellerTierBenefits_IsActive",
                table: "TbSellerTierBenefits");

            migrationBuilder.DropIndex(
                name: "IX_TbSellerRequests_Priority",
                table: "TbSellerRequests");

            migrationBuilder.DropIndex(
                name: "IX_TbSellerRequests_RequestType",
                table: "TbSellerRequests");

            migrationBuilder.DropIndex(
                name: "IX_TbSellerRequests_RequestType_Status",
                table: "TbSellerRequests");

            migrationBuilder.DropIndex(
                name: "IX_TbSellerRequests_Status",
                table: "TbSellerRequests");

            migrationBuilder.DropIndex(
                name: "IX_TbSellerRequests_SubmittedAt",
                table: "TbSellerRequests");

            migrationBuilder.DropIndex(
                name: "IX_TbSellerRequests_VendorId_Status",
                table: "TbSellerRequests");

            migrationBuilder.DropIndex(
                name: "IX_TbSellerPerformanceMetrics_AverageRating",
                table: "TbSellerPerformanceMetrics");

            migrationBuilder.DropIndex(
                name: "IX_TbSellerPerformanceMetrics_BuyBoxWinRate",
                table: "TbSellerPerformanceMetrics");

            migrationBuilder.DropIndex(
                name: "IX_TbSellerPerformanceMetrics_OrderCompletionRate",
                table: "TbSellerPerformanceMetrics");

            migrationBuilder.DropIndex(
                name: "IX_TbSellerPerformanceMetrics_UsesFBM",
                table: "TbSellerPerformanceMetrics");

            migrationBuilder.DropIndex(
                name: "IX_TbSellerPerformanceMetrics_VendorId_CalculatedForPeriodStart",
                table: "TbSellerPerformanceMetrics");

            migrationBuilder.DropIndex(
                name: "IX_TbSalesReviews_CustomerID",
                table: "TbSalesReviews");

            migrationBuilder.DropIndex(
                name: "IX_TbSalesReviews_OrderItemID",
                table: "TbSalesReviews");

            migrationBuilder.DropIndex(
                name: "IX_TbSalesReviews_OverallRating",
                table: "TbSalesReviews");

            migrationBuilder.DropIndex(
                name: "IX_TbSalesReviews_ReviewDate",
                table: "TbSalesReviews");

            migrationBuilder.DropIndex(
                name: "IX_TbSalesReviews_ReviewNumber",
                table: "TbSalesReviews");

            migrationBuilder.DropIndex(
                name: "IX_TbSalesReviews_Status",
                table: "TbSalesReviews");

            migrationBuilder.DropIndex(
                name: "IX_TbSalesReviews_VendorID",
                table: "TbSalesReviews");

            migrationBuilder.DropIndex(
                name: "IX_TbSalesReviews_VendorID_CustomerID",
                table: "TbSalesReviews");

            migrationBuilder.DropIndex(
                name: "IX_TbReviewVotes_CustomerID",
                table: "TbReviewVotes");

            migrationBuilder.DropIndex(
                name: "IX_TbReviewVotes_ReviewID_CustomerID_VoteType",
                table: "TbReviewVotes");

            migrationBuilder.DropIndex(
                name: "IX_TbReviewVotes_VoteType",
                table: "TbReviewVotes");

            migrationBuilder.DropIndex(
                name: "IX_TbReviewVotes_VoteValue",
                table: "TbReviewVotes");

            migrationBuilder.DropIndex(
                name: "IX_TbRequestDocuments_DocumentType",
                table: "TbRequestDocuments");

            migrationBuilder.DropIndex(
                name: "IX_TbRequestDocuments_UploadedAt",
                table: "TbRequestDocuments");

            migrationBuilder.DropIndex(
                name: "IX_TbRequestComments_CreatedDateUtc",
                table: "TbRequestComments");

            migrationBuilder.DropIndex(
                name: "IX_TbRequestComments_SellerRequestId_CreatedDateUtc",
                table: "TbRequestComments");

            migrationBuilder.DropIndex(
                name: "IX_TbQuantityPricings_IsActive",
                table: "TbQuantityPricings");

            migrationBuilder.DropIndex(
                name: "IX_TbQuantityPricings_MinimumQuantity",
                table: "TbQuantityPricings");

            migrationBuilder.DropIndex(
                name: "IX_TbQuantityPricings_OfferId_MinimumQuantity",
                table: "TbQuantityPricings");

            migrationBuilder.DropCheckConstraint(
                name: "CK_QuantityPricing_Quantities",
                table: "TbQuantityPricings");

            migrationBuilder.DropIndex(
                name: "IX_TbProductVisibilityRules_IsVisible",
                table: "TbProductVisibilityRules");

            migrationBuilder.DropIndex(
                name: "IX_TbProductVisibilityRules_IsVisible_VisibilityStatus",
                table: "TbProductVisibilityRules");

            migrationBuilder.DropIndex(
                name: "IX_TbProductVisibilityRules_ItemId",
                table: "TbProductVisibilityRules");

            migrationBuilder.DropIndex(
                name: "IX_TbProductVisibilityRules_LastCheckedAt",
                table: "TbProductVisibilityRules");

            migrationBuilder.DropIndex(
                name: "IX_TbProductVisibilityRules_VisibilityStatus",
                table: "TbProductVisibilityRules");

            migrationBuilder.DropIndex(
                name: "IX_TbProductReviews_CustomerID",
                table: "TbProductReviews");

            migrationBuilder.DropIndex(
                name: "IX_TbProductReviews_IsVerifiedPurchase",
                table: "TbProductReviews");

            migrationBuilder.DropIndex(
                name: "IX_TbProductReviews_OrderItemID",
                table: "TbProductReviews");

            migrationBuilder.DropIndex(
                name: "IX_TbProductReviews_ProductID",
                table: "TbProductReviews");

            migrationBuilder.DropIndex(
                name: "IX_TbProductReviews_ProductID_CustomerID",
                table: "TbProductReviews");

            migrationBuilder.DropIndex(
                name: "IX_TbProductReviews_Rating",
                table: "TbProductReviews");

            migrationBuilder.DropIndex(
                name: "IX_TbProductReviews_ReviewDate",
                table: "TbProductReviews");

            migrationBuilder.DropIndex(
                name: "IX_TbProductReviews_ReviewNumber",
                table: "TbProductReviews");

            migrationBuilder.DropIndex(
                name: "IX_TbProductReviews_Status",
                table: "TbProductReviews");

            migrationBuilder.DropIndex(
                name: "IX_TbPriceHistories_ChangedAt",
                table: "TbPriceHistories");

            migrationBuilder.DropIndex(
                name: "IX_TbPriceHistories_IsAutomatic",
                table: "TbPriceHistories");

            migrationBuilder.DropIndex(
                name: "IX_TbPriceHistories_OfferId_ChangedAt",
                table: "TbPriceHistories");

            migrationBuilder.DropIndex(
                name: "IX_TbPlatformTreasuries_LastReconciliationDate",
                table: "TbPlatformTreasuries");

            migrationBuilder.DropIndex(
                name: "IX_TbOffers_IsActive",
                table: "TbOffers");

            migrationBuilder.DropIndex(
                name: "IX_TbOffers_Item_Active_Price",
                table: "TbOffers");

            migrationBuilder.DropIndex(
                name: "IX_TbOffers_Price",
                table: "TbOffers");

            migrationBuilder.DropIndex(
                name: "IX_TbNotificationPreferences_NotificationType",
                table: "TbNotificationPreferences");

            migrationBuilder.DropIndex(
                name: "IX_TbNotificationPreferences_UserId_UserType",
                table: "TbNotificationPreferences");

            migrationBuilder.DropIndex(
                name: "IX_TbNotificationPreferences_UserId_UserType_NotificationType",
                table: "TbNotificationPreferences");

            migrationBuilder.DropIndex(
                name: "IX_TbNotificationChannels_Channel",
                table: "TbNotificationChannels");

            migrationBuilder.DropIndex(
                name: "IX_TbNotificationChannels_IsActive",
                table: "TbNotificationChannels");

            migrationBuilder.DropIndex(
                name: "IX_TbMortems_DocumentNumber",
                table: "TbMortems");

            migrationBuilder.DropIndex(
                name: "IX_TbMortems_Status",
                table: "TbMortems");

            migrationBuilder.DropIndex(
                name: "IX_TbMoitems_DocumentDate",
                table: "TbMoitems");

            migrationBuilder.DropIndex(
                name: "IX_TbMoitems_DocumentNumber",
                table: "TbMoitems");

            migrationBuilder.DropIndex(
                name: "IX_TbMediaContents_DisplayOrder",
                table: "TbMediaContents");

            migrationBuilder.DropIndex(
                name: "IX_TbMediaContents_IsActive",
                table: "TbMediaContents");

            migrationBuilder.DropIndex(
                name: "IX_TbMediaContents_MediaType",
                table: "TbMediaContents");

            migrationBuilder.DropIndex(
                name: "IX_TbLoyaltyTiers_DisplayOrder",
                table: "TbLoyaltyTiers");

            migrationBuilder.DropIndex(
                name: "IX_TbLoyaltyTiers_MinimumOrdersPerYear_MaximumOrdersPerYear",
                table: "TbLoyaltyTiers");

            migrationBuilder.DropIndex(
                name: "IX_TbLoyaltyTiers_TierNameEn",
                table: "TbLoyaltyTiers");

            migrationBuilder.DropIndex(
                name: "IX_TbLoyaltyPointsTransactions_CreatedDateUtc",
                table: "TbLoyaltyPointsTransactions");

            migrationBuilder.DropIndex(
                name: "IX_TbLoyaltyPointsTransactions_CustomerLoyaltyId_TransactionType",
                table: "TbLoyaltyPointsTransactions");

            migrationBuilder.DropIndex(
                name: "IX_TbLoyaltyPointsTransactions_IsExpired",
                table: "TbLoyaltyPointsTransactions");

            migrationBuilder.DropIndex(
                name: "IX_TbLoyaltyPointsTransactions_TransactionType",
                table: "TbLoyaltyPointsTransactions");

            migrationBuilder.DropIndex(
                name: "IX_TbItems_IsNewArrival",
                table: "TbItems");

            migrationBuilder.DropIndex(
                name: "IX_TbItemImages_Order",
                table: "TbItemImages");

            migrationBuilder.DropIndex(
                name: "IX_TbItemAttributeCombinationPricings_AttributeIds",
                table: "TbItemAttributeCombinationPricings");

            migrationBuilder.DropIndex(
                name: "IX_TbItemAttributeCombinationPricings_Price",
                table: "TbItemAttributeCombinationPricings");

            migrationBuilder.DropIndex(
                name: "IX_TbHomepageBlocks_ActiveFrom_ActiveTo",
                table: "TbHomepageBlocks");

            migrationBuilder.DropIndex(
                name: "IX_TbHomepageBlocks_BlockType",
                table: "TbHomepageBlocks");

            migrationBuilder.DropIndex(
                name: "IX_TbHomepageBlocks_DisplayOrder",
                table: "TbHomepageBlocks");

            migrationBuilder.DropIndex(
                name: "IX_TbHomepageBlocks_IsActive",
                table: "TbHomepageBlocks");

            migrationBuilder.DropIndex(
                name: "IX_TbHomepageBlocks_IsActive_DisplayOrder",
                table: "TbHomepageBlocks");

            migrationBuilder.DropIndex(
                name: "IX_TbHomepageBlocks_IsVisible",
                table: "TbHomepageBlocks");

            migrationBuilder.DropIndex(
                name: "IX_TbFulfillmentMethods_Code",
                table: "TbFulfillmentMethods");

            migrationBuilder.DropIndex(
                name: "IX_TbFulfillmentMethods_DisplayOrder",
                table: "TbFulfillmentMethods");

            migrationBuilder.DropIndex(
                name: "IX_TbFulfillmentMethods_IsActive",
                table: "TbFulfillmentMethods");

            migrationBuilder.DropIndex(
                name: "IX_TbFulfillmentFees_EffectiveFrom_EffectiveTo",
                table: "TbFulfillmentFees");

            migrationBuilder.DropIndex(
                name: "IX_TbFulfillmentFees_FeeType",
                table: "TbFulfillmentFees");

            migrationBuilder.DropIndex(
                name: "IX_TbFulfillmentFees_FulfillmentMethodId_FeeType",
                table: "TbFulfillmentFees");

            migrationBuilder.DropIndex(
                name: "IX_TbFulfillmentFees_IsActive",
                table: "TbFulfillmentFees");

            migrationBuilder.DropIndex(
                name: "IX_TbFlashSales_EndDate",
                table: "TbFlashSales");

            migrationBuilder.DropIndex(
                name: "IX_TbFlashSales_IsActive",
                table: "TbFlashSales");

            migrationBuilder.DropIndex(
                name: "IX_TbFlashSales_StartDate",
                table: "TbFlashSales");

            migrationBuilder.DropIndex(
                name: "IX_TbFlashSales_StartDate_EndDate_IsActive",
                table: "TbFlashSales");

            migrationBuilder.DropCheckConstraint(
                name: "CK_FlashSale_DurationInHours",
                table: "TbFlashSales");

            migrationBuilder.DropIndex(
                name: "IX_TbFlashSaleProducts_DisplayOrder",
                table: "TbFlashSaleProducts");

            migrationBuilder.DropIndex(
                name: "IX_TbFlashSaleProducts_FlashSaleId_ItemId",
                table: "TbFlashSaleProducts");

            migrationBuilder.DropIndex(
                name: "IX_TbFlashSaleProducts_FlashSaleId_SoldQuantity",
                table: "TbFlashSaleProducts");

            migrationBuilder.DropIndex(
                name: "IX_TbFlashSaleProducts_IsActive",
                table: "TbFlashSaleProducts");

            migrationBuilder.DropIndex(
                name: "IX_TbFBMShipments_ShipmentNumber",
                table: "TbFBMShipments");

            migrationBuilder.DropIndex(
                name: "IX_TbFBMShipments_ShippedDate",
                table: "TbFBMShipments");

            migrationBuilder.DropIndex(
                name: "IX_TbFBMShipments_Status",
                table: "TbFBMShipments");

            migrationBuilder.DropIndex(
                name: "IX_TbFBMShipments_Status_ShippedDate",
                table: "TbFBMShipments");

            migrationBuilder.DropIndex(
                name: "IX_TbFBMShipments_TrackingNumber",
                table: "TbFBMShipments");

            migrationBuilder.DropIndex(
                name: "IX_TbFBMInventories_ItemId_WarehouseId_VendorId",
                table: "TbFBMInventories");

            migrationBuilder.DropIndex(
                name: "IX_TbFBMInventories_SKU",
                table: "TbFBMInventories");

            migrationBuilder.DropIndex(
                name: "IX_TbFBMInventories_WarehouseId_AvailableQuantity",
                table: "TbFBMInventories");

            migrationBuilder.DropIndex(
                name: "IX_TbDisputes_CreatedDateUtc",
                table: "TbDisputes");

            migrationBuilder.DropIndex(
                name: "IX_TbDisputes_DisputeNumber",
                table: "TbDisputes");

            migrationBuilder.DropIndex(
                name: "IX_TbDisputes_OrderID",
                table: "TbDisputes");

            migrationBuilder.DropIndex(
                name: "IX_TbDisputes_SenderID",
                table: "TbDisputes");

            migrationBuilder.DropIndex(
                name: "IX_TbDisputes_Status",
                table: "TbDisputes");

            migrationBuilder.DropIndex(
                name: "IX_TbDisputeMessages_DisputeID_MessageNumber",
                table: "TbDisputeMessages");

            migrationBuilder.DropIndex(
                name: "IX_TbDisputeMessages_SenderID",
                table: "TbDisputeMessages");

            migrationBuilder.DropIndex(
                name: "IX_TbDisputeMessages_SentDate",
                table: "TbDisputeMessages");

            migrationBuilder.DropIndex(
                name: "IX_TbDeliveryReviews_CustomerID",
                table: "TbDeliveryReviews");

            migrationBuilder.DropIndex(
                name: "IX_TbDeliveryReviews_OrderID",
                table: "TbDeliveryReviews");

            migrationBuilder.DropIndex(
                name: "IX_TbDeliveryReviews_OrderID_CustomerID",
                table: "TbDeliveryReviews");

            migrationBuilder.DropIndex(
                name: "IX_TbDeliveryReviews_OverallRating",
                table: "TbDeliveryReviews");

            migrationBuilder.DropIndex(
                name: "IX_TbDeliveryReviews_ReviewDate",
                table: "TbDeliveryReviews");

            migrationBuilder.DropIndex(
                name: "IX_TbDeliveryReviews_ReviewNumber",
                table: "TbDeliveryReviews");

            migrationBuilder.DropIndex(
                name: "IX_TbDeliveryReviews_Status",
                table: "TbDeliveryReviews");

            migrationBuilder.DropIndex(
                name: "IX_TbCustomerWallets_AvailableBalance",
                table: "TbCustomerWallets");

            migrationBuilder.DropIndex(
                name: "IX_TbCustomerWallets_CustomerId",
                table: "TbCustomerWallets");

            migrationBuilder.DropIndex(
                name: "IX_TbCustomerSegmentPricings_EffectiveFrom_EffectiveTo",
                table: "TbCustomerSegmentPricings");

            migrationBuilder.DropIndex(
                name: "IX_TbCustomerSegmentPricings_IsActive",
                table: "TbCustomerSegmentPricings");

            migrationBuilder.DropIndex(
                name: "IX_TbCustomerSegmentPricings_OfferId_SegmentType",
                table: "TbCustomerSegmentPricings");

            migrationBuilder.DropIndex(
                name: "IX_TbCustomerSegmentPricings_SegmentType",
                table: "TbCustomerSegmentPricings");

            migrationBuilder.DropIndex(
                name: "IX_TbCustomerLoyalties_AvailablePoints",
                table: "TbCustomerLoyalties");

            migrationBuilder.DropIndex(
                name: "IX_TbCustomerLoyalties_CustomerId",
                table: "TbCustomerLoyalties");

            migrationBuilder.DropIndex(
                name: "IX_TbCurrencies_Code",
                table: "TbCurrencies");

            migrationBuilder.DropIndex(
                name: "IX_TbCurrencies_IsActive",
                table: "TbCurrencies");

            migrationBuilder.DropIndex(
                name: "IX_TbCurrencies_IsBaseCurrency",
                table: "TbCurrencies");

            migrationBuilder.DropIndex(
                name: "IX_TbCouponCodes_Code",
                table: "TbCouponCodes");

            migrationBuilder.DropIndex(
                name: "IX_TbContentAreas_AreaCode",
                table: "TbContentAreas");

            migrationBuilder.DropIndex(
                name: "IX_TbContentAreas_DisplayOrder",
                table: "TbContentAreas");

            migrationBuilder.DropIndex(
                name: "IX_TbContentAreas_IsActive",
                table: "TbContentAreas");

            migrationBuilder.DropIndex(
                name: "IX_TbCampaignVendors_CampaignId_VendorId",
                table: "TbCampaignVendors");

            migrationBuilder.DropIndex(
                name: "IX_TbCampaignVendors_IsApproved",
                table: "TbCampaignVendors");

            migrationBuilder.DropIndex(
                name: "IX_TbCampaigns_CampaignType",
                table: "TbCampaigns");

            migrationBuilder.DropIndex(
                name: "IX_TbCampaigns_EndDate",
                table: "TbCampaigns");

            migrationBuilder.DropIndex(
                name: "IX_TbCampaigns_IsActive",
                table: "TbCampaigns");

            migrationBuilder.DropIndex(
                name: "IX_TbCampaigns_IsFeatured",
                table: "TbCampaigns");

            migrationBuilder.DropIndex(
                name: "IX_TbCampaigns_SlugEn",
                table: "TbCampaigns");

            migrationBuilder.DropIndex(
                name: "IX_TbCampaigns_StartDate",
                table: "TbCampaigns");

            migrationBuilder.DropIndex(
                name: "IX_TbCampaigns_StartDate_EndDate_IsActive",
                table: "TbCampaigns");

            migrationBuilder.DropIndex(
                name: "IX_TbCampaignProducts_CampaignId_IsActive",
                table: "TbCampaignProducts");

            migrationBuilder.DropIndex(
                name: "IX_TbCampaignProducts_CampaignId_ItemId",
                table: "TbCampaignProducts");

            migrationBuilder.DropIndex(
                name: "IX_TbCampaignProducts_DisplayOrder",
                table: "TbCampaignProducts");

            migrationBuilder.DropIndex(
                name: "IX_TbCampaignProducts_IsActive",
                table: "TbCampaignProducts");

            migrationBuilder.DropIndex(
                name: "IX_TbBuyBoxHistories_ItemId_WonAt",
                table: "TbBuyBoxHistories");

            migrationBuilder.DropIndex(
                name: "IX_TbBuyBoxHistories_LostAt",
                table: "TbBuyBoxHistories");

            migrationBuilder.DropIndex(
                name: "IX_TbBuyBoxHistories_VendorId_WonAt",
                table: "TbBuyBoxHistories");

            migrationBuilder.DropIndex(
                name: "IX_TbBuyBoxHistories_WonAt",
                table: "TbBuyBoxHistories");

            migrationBuilder.DropIndex(
                name: "IX_TbBuyBoxCalculations_CalculatedAt",
                table: "TbBuyBoxCalculations");

            migrationBuilder.DropIndex(
                name: "IX_TbBuyBoxCalculations_ExpiresAt",
                table: "TbBuyBoxCalculations");

            migrationBuilder.DropIndex(
                name: "IX_TbBuyBoxCalculations_ItemId_CalculatedAt",
                table: "TbBuyBoxCalculations");

            migrationBuilder.DropIndex(
                name: "IX_TbBuyBoxCalculations_TbOfferId",
                table: "TbBuyBoxCalculations");

            migrationBuilder.DropIndex(
                name: "IX_TbBuyBoxCalculations_TotalScore",
                table: "TbBuyBoxCalculations");

            migrationBuilder.DropIndex(
                name: "IX_TbBrands_DisplayOrder",
                table: "TbBrands");

            migrationBuilder.DropIndex(
                name: "IX_TbBrandRegistrationRequests_BrandNameEn_VendorId",
                table: "TbBrandRegistrationRequests");

            migrationBuilder.DropIndex(
                name: "IX_TbBrandRegistrationRequests_BrandType",
                table: "TbBrandRegistrationRequests");

            migrationBuilder.DropIndex(
                name: "IX_TbBrandRegistrationRequests_Status",
                table: "TbBrandRegistrationRequests");

            migrationBuilder.DropIndex(
                name: "IX_TbBrandRegistrationRequests_SubmittedAt",
                table: "TbBrandRegistrationRequests");

            migrationBuilder.DropIndex(
                name: "IX_TbBrandRegistrationRequests_TrademarkNumber",
                table: "TbBrandRegistrationRequests");

            migrationBuilder.DropIndex(
                name: "IX_TbBrandRegistrationRequests_VendorId_Status",
                table: "TbBrandRegistrationRequests");

            migrationBuilder.DropIndex(
                name: "IX_TbBrandDocuments_BrandRegistrationRequestId_DocumentType",
                table: "TbBrandDocuments");

            migrationBuilder.DropIndex(
                name: "IX_TbBrandDocuments_DocumentType",
                table: "TbBrandDocuments");

            migrationBuilder.DropIndex(
                name: "IX_TbBrandDocuments_IsVerified",
                table: "TbBrandDocuments");

            migrationBuilder.DropIndex(
                name: "IX_TbBrandDocuments_UploadedAt",
                table: "TbBrandDocuments");

            migrationBuilder.DropIndex(
                name: "IX_TbBlockProducts_DisplayOrder",
                table: "TbBlockProducts");

            migrationBuilder.DropIndex(
                name: "IX_TbBlockProducts_HomepageBlockId_DisplayOrder",
                table: "TbBlockProducts");

            migrationBuilder.DropIndex(
                name: "IX_TbBlockProducts_HomepageBlockId_ItemId",
                table: "TbBlockProducts");

            migrationBuilder.DropIndex(
                name: "IX_TbBlockProducts_IsActive",
                table: "TbBlockProducts");

            migrationBuilder.DropIndex(
                name: "IX_TbBlockProducts_IsFeatured",
                table: "TbBlockProducts");

            migrationBuilder.DropIndex(
                name: "IX_TbAuthorizedDistributors_AuthorizationNumber",
                table: "TbAuthorizedDistributors");

            migrationBuilder.DropIndex(
                name: "IX_TbAuthorizedDistributors_AuthorizationStartDate_AuthorizationEndDate",
                table: "TbAuthorizedDistributors");

            migrationBuilder.DropIndex(
                name: "IX_TbAuthorizedDistributors_BrandId_VendorId",
                table: "TbAuthorizedDistributors");

            migrationBuilder.DropIndex(
                name: "IX_TbAuthorizedDistributors_IsActive",
                table: "TbAuthorizedDistributors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Notifications",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_IsRead",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_NotificationType",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_RecipientType_RecipientID",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_SentDate",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "TbOfferId",
                table: "TbBuyBoxCalculations");

            migrationBuilder.RenameTable(
                name: "Notifications",
                newName: "TbNotifications");

            migrationBuilder.RenameIndex(
                name: "IX_Notifications_CurrentState",
                table: "TbNotifications",
                newName: "IX_TbNotifications_CurrentState");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "TbWarehouses",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ProcessedDate",
                table: "TbWalletTransactions",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2(2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsAutomatic",
                table: "TbVisibilityLogs",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ChangedAt",
                table: "TbVisibilityLogs",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2(2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "WithdrawalFeePercentage",
                table: "TbVendorWallets",
                type: "decimal(5,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)",
                oldDefaultValue: 2.5m);

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalWithdrawn",
                table: "TbVendorWallets",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldDefaultValue: 0m);

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalEarned",
                table: "TbVendorWallets",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldDefaultValue: 0m);

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalCommissionPaid",
                table: "TbVendorWallets",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldDefaultValue: 0m);

            migrationBuilder.AlterColumn<decimal>(
                name: "PendingBalance",
                table: "TbVendorWallets",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldDefaultValue: 0m);

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastWithdrawalDate",
                table: "TbVendorWallets",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2(2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastTransactionDate",
                table: "TbVendorWallets",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2(2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "AvailableBalance",
                table: "TbVendorWallets",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldDefaultValue: 0m);

            migrationBuilder.AlterColumn<bool>(
                name: "IsAutomatic",
                table: "TbVendorTierHistories",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndedAt",
                table: "TbVendorTierHistories",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2(2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "AchievedAt",
                table: "TbVendorTierHistories",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2(2)");

            migrationBuilder.AlterColumn<bool>(
                name: "IsRead",
                table: "TbUserNotifications",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ResolvedAt",
                table: "TbSuppressionReasons",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2(2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsResolved",
                table: "TbSuppressionReasons",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DetectedAt",
                table: "TbSuppressionReasons",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2(2)");

            migrationBuilder.AlterColumn<string>(
                name: "UserType",
                table: "TbSupportTickets",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "UserID",
                table: "TbSupportTickets",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldMaxLength: 450);

            migrationBuilder.AlterColumn<DateTime>(
                name: "TicketCreatedDate",
                table: "TbSupportTickets",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2(2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Subject",
                table: "TbSupportTickets",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "TbSupportTickets",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Priority",
                table: "TbSupportTickets",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Category",
                table: "TbSupportTickets",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "AssignedTo",
                table: "TbSupportTickets",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldMaxLength: 450,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AssignedTeam",
                table: "TbSupportTickets",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserType",
                table: "TbSupportTicketMessages",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<DateTime>(
                name: "SentDate",
                table: "TbSupportTicketMessages",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2(2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SenderID",
                table: "TbSupportTicketMessages",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldMaxLength: 450);

            migrationBuilder.AddColumn<Guid>(
                name: "SupportTicketId",
                table: "TbSupportTicketMessages",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<string>(
                name: "TitleEn",
                table: "TbStates",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "TitleAr",
                table: "TbStates",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "TbShippingCompanies",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(15)",
                oldMaxLength: 15);

            migrationBuilder.AlterColumn<string>(
                name: "PhoneCode",
                table: "TbShippingCompanies",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(4)",
                oldMaxLength: 4);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "TbShippingCompanies",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "LogoImagePath",
                table: "TbShippingCompanies",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(300)",
                oldMaxLength: 300);

            migrationBuilder.AlterColumn<string>(
                name: "WhatsAppNumber",
                table: "TbSettings",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "TbSellerTiers",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<bool>(
                name: "HasPrioritySupport",
                table: "TbSellerTiers",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "HasFeaturedListings",
                table: "TbSellerTiers",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "HasDedicatedAccountManager",
                table: "TbSellerTiers",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "HasBuyBoxBoost",
                table: "TbSellerTiers",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "HasAdvancedAnalytics",
                table: "TbSellerTiers",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<int>(
                name: "DisplayOrder",
                table: "TbSellerTiers",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<decimal>(
                name: "CommissionReductionPercentage",
                table: "TbSellerTiers",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)",
                oldDefaultValue: 0m);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "TbSellerTierBenefits",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<int>(
                name: "DisplayOrder",
                table: "TbSellerTierBenefits",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "SubmittedAt",
                table: "TbSellerRequests",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2(2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ReviewedAt",
                table: "TbSellerRequests",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2(2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ProcessedAt",
                table: "TbSellerRequests",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2(2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Priority",
                table: "TbSellerRequests",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<bool>(
                name: "UsesFBM",
                table: "TbSellerPerformanceMetrics",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<int>(
                name: "TotalReviews",
                table: "TbSellerPerformanceMetrics",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<decimal>(
                name: "ReturnRate",
                table: "TbSellerPerformanceMetrics",
                type: "decimal(5,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)",
                oldDefaultValue: 0m);

            migrationBuilder.AlterColumn<decimal>(
                name: "ResponseRate",
                table: "TbSellerPerformanceMetrics",
                type: "decimal(5,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)",
                oldDefaultValue: 0m);

            migrationBuilder.AlterColumn<decimal>(
                name: "OrderCompletionRate",
                table: "TbSellerPerformanceMetrics",
                type: "decimal(5,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)",
                oldDefaultValue: 0m);

            migrationBuilder.AlterColumn<decimal>(
                name: "OnTimeShippingRate",
                table: "TbSellerPerformanceMetrics",
                type: "decimal(5,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)",
                oldDefaultValue: 0m);

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastUpdated",
                table: "TbSellerPerformanceMetrics",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2(2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "CancellationRate",
                table: "TbSellerPerformanceMetrics",
                type: "decimal(5,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)",
                oldDefaultValue: 0m);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CalculatedForPeriodStart",
                table: "TbSellerPerformanceMetrics",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2(2)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CalculatedForPeriodEnd",
                table: "TbSellerPerformanceMetrics",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2(2)");

            migrationBuilder.AlterColumn<int>(
                name: "BuyBoxWins",
                table: "TbSellerPerformanceMetrics",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<decimal>(
                name: "BuyBoxWinRate",
                table: "TbSellerPerformanceMetrics",
                type: "decimal(5,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)",
                oldDefaultValue: 0m);

            migrationBuilder.AlterColumn<int>(
                name: "AverageResponseTimeInHours",
                table: "TbSellerPerformanceMetrics",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<decimal>(
                name: "AverageRating",
                table: "TbSellerPerformanceMetrics",
                type: "decimal(3,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(3,2)",
                oldDefaultValue: 0m);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "TbSalesReviews",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<decimal>(
                name: "ShippingSpeedRating",
                table: "TbSalesReviews",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(2,1)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ServiceRating",
                table: "TbSalesReviews",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(2,1)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ReviewDate",
                table: "TbSalesReviews",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2(2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ProductAccuracyRating",
                table: "TbSalesReviews",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(2,1)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "OverallRating",
                table: "TbSalesReviews",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(2,1)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "CommunicationRating",
                table: "TbSalesReviews",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(2,1)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "WithType",
                table: "TbReviewVotes",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "VoteValue",
                table: "TbReviewVotes",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "VoteType",
                table: "TbReviewVotes",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UploadedAt",
                table: "TbRequestDocuments",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2(2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsInternal",
                table: "TbRequestComments",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "TbQuantityPricings",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<int>(
                name: "DisplayOrder",
                table: "TbQuantityPricings",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "SuppressedAt",
                table: "TbProductVisibilityRules",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2(2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastCheckedAt",
                table: "TbProductVisibilityRules",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2(2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsVisible",
                table: "TbProductVisibilityRules",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsApproved",
                table: "TbProductVisibilityRules",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "HasValidCategory",
                table: "TbProductVisibilityRules",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "HasStock",
                table: "TbProductVisibilityRules",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "HasActiveOffers",
                table: "TbProductVisibilityRules",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "AllSellersActive",
                table: "TbProductVisibilityRules",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "VendorResponseDate",
                table: "TbProductReviews",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2(2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "TbProductReviews",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "ReviewTitle",
                table: "TbProductReviews",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ReviewDate",
                table: "TbProductReviews",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2(2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Rating",
                table: "TbProductReviews",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(2,1)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "NotHelpfulCount",
                table: "TbProductReviews",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<bool>(
                name: "IsVerifiedPurchase",
                table: "TbProductReviews",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<int>(
                name: "HelpfulCount",
                table: "TbProductReviews",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ApprovedDate",
                table: "TbProductReviews",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2(2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsAutomatic",
                table: "TbPriceHistories",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ChangedAt",
                table: "TbPriceHistories",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2(2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "VendorWalletsTotal",
                table: "TbPlatformTreasuries",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldDefaultValue: 0m);

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalBalance",
                table: "TbPlatformTreasuries",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldDefaultValue: 0m);

            migrationBuilder.AlterColumn<decimal>(
                name: "ProcessedPayouts",
                table: "TbPlatformTreasuries",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldDefaultValue: 0m);

            migrationBuilder.AlterColumn<decimal>(
                name: "PendingPayouts",
                table: "TbPlatformTreasuries",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldDefaultValue: 0m);

            migrationBuilder.AlterColumn<decimal>(
                name: "PendingCommissions",
                table: "TbPlatformTreasuries",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldDefaultValue: 0m);

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastReconciliationDate",
                table: "TbPlatformTreasuries",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2(2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "CustomerWalletsTotal",
                table: "TbPlatformTreasuries",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldDefaultValue: 0m);

            migrationBuilder.AlterColumn<decimal>(
                name: "CollectedCommissions",
                table: "TbPlatformTreasuries",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldDefaultValue: 0m);

            migrationBuilder.AlterColumn<decimal>(
                name: "ShippingCost",
                table: "TbOffers",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldDefaultValue: 0m);

            migrationBuilder.AlterColumn<decimal>(
                name: "SellerRating",
                table: "TbOffers",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(3,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Quantity",
                table: "TbOffers",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "TbOffers",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<string>(
                name: "Condition",
                table: "TbOffers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<bool>(
                name: "EnableSMS",
                table: "TbNotificationPreferences",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "EnablePush",
                table: "TbNotificationPreferences",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<bool>(
                name: "EnableInApp",
                table: "TbNotificationPreferences",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<bool>(
                name: "EnableEmail",
                table: "TbNotificationPreferences",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "TbNotificationChannels",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "TbMortems",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DocumentDate",
                table: "TbMortems",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DocumentDate",
                table: "TbMoitems",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.AlterColumn<string>(
                name: "MediaType",
                table: "TbMediaContents",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldDefaultValue: "Image");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "TbMediaContents",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<int>(
                name: "DisplayOrder",
                table: "TbMediaContents",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<decimal>(
                name: "PointsMultiplier",
                table: "TbLoyaltyTiers",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)",
                oldDefaultValue: 1.0m);

            migrationBuilder.AlterColumn<bool>(
                name: "HasPrioritySupport",
                table: "TbLoyaltyTiers",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "HasFreeShipping",
                table: "TbLoyaltyTiers",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<int>(
                name: "DisplayOrder",
                table: "TbLoyaltyTiers",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<decimal>(
                name: "CashbackPercentage",
                table: "TbLoyaltyTiers",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)",
                oldDefaultValue: 0m);

            migrationBuilder.AlterColumn<bool>(
                name: "IsExpired",
                table: "TbLoyaltyPointsTransactions",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ExpiryDate",
                table: "TbLoyaltyPointsTransactions",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2(2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsNewArrival",
                table: "TbItems",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "ShowViewAllLink",
                table: "TbHomepageBlocks",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsVisible",
                table: "TbHomepageBlocks",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "TbHomepageBlocks",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ActiveTo",
                table: "TbHomepageBlocks",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2(2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ActiveFrom",
                table: "TbHomepageBlocks",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2(2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "RequiresWarehouse",
                table: "TbFulfillmentMethods",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "TbFulfillmentMethods",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<int>(
                name: "DisplayOrder",
                table: "TbFulfillmentMethods",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "BuyBoxPriorityBoost",
                table: "TbFulfillmentMethods",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "TbFulfillmentFees",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "EffectiveTo",
                table: "TbFulfillmentFees",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2(2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "EffectiveFrom",
                table: "TbFulfillmentFees",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2(2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                table: "TbFlashSales",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2(2)");

            migrationBuilder.AlterColumn<bool>(
                name: "ShowCountdownTimer",
                table: "TbFlashSales",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "MinimumSellerRating",
                table: "TbFlashSales",
                type: "decimal(3,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(3,2)",
                oldDefaultValue: 4.0m);

            migrationBuilder.AlterColumn<decimal>(
                name: "MinimumDiscountPercentage",
                table: "TbFlashSales",
                type: "decimal(5,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)",
                oldDefaultValue: 20m);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "TbFlashSales",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                table: "TbFlashSales",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2(2)");

            migrationBuilder.AlterColumn<int>(
                name: "DisplayOrder",
                table: "TbFlashSales",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "ViewCount",
                table: "TbFlashSaleProducts",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "SoldQuantity",
                table: "TbFlashSaleProducts",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "TbFlashSaleProducts",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<int>(
                name: "DisplayOrder",
                table: "TbFlashSaleProducts",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "AddToCartCount",
                table: "TbFlashSaleProducts",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ShippedDate",
                table: "TbFBMShipments",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2(2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "PickupDate",
                table: "TbFBMShipments",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2(2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DeliveredDate",
                table: "TbFBMShipments",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2(2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ReservedQuantity",
                table: "TbFBMInventories",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastSyncDate",
                table: "TbFBMInventories",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2(2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "InTransitQuantity",
                table: "TbFBMInventories",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ExpiryDate",
                table: "TbFBMInventories",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2(2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DamagedQuantity",
                table: "TbFBMInventories",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "AvailableQuantity",
                table: "TbFBMInventories",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "TbDisputes",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "SenderType",
                table: "TbDisputes",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "SenderID",
                table: "TbDisputes",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldMaxLength: 450);

            migrationBuilder.AlterColumn<string>(
                name: "Parties",
                table: "TbDisputes",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "AssignedAdminID",
                table: "TbDisputes",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldMaxLength: 450,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "SentDate",
                table: "TbDisputeMessages",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2(2)");

            migrationBuilder.AlterColumn<string>(
                name: "SenderType",
                table: "TbDisputeMessages",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "SenderID",
                table: "TbDisputeMessages",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldMaxLength: 450);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "TbDeliveryReviews",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ReviewDate",
                table: "TbDeliveryReviews",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2(2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "PackagingRating",
                table: "TbDeliveryReviews",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(2,1)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "OverallRating",
                table: "TbDeliveryReviews",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(2,1)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "CourierDeliveryRating",
                table: "TbDeliveryReviews",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(2,1)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalSpent",
                table: "TbCustomerWallets",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldDefaultValue: 0m);

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalEarned",
                table: "TbCustomerWallets",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldDefaultValue: 0m);

            migrationBuilder.AlterColumn<decimal>(
                name: "PendingBalance",
                table: "TbCustomerWallets",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldDefaultValue: 0m);

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastTransactionDate",
                table: "TbCustomerWallets",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2(2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "AvailableBalance",
                table: "TbCustomerWallets",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldDefaultValue: 0m);

            migrationBuilder.AlterColumn<int>(
                name: "MinimumOrderQuantity",
                table: "TbCustomerSegmentPricings",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 1);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "TbCustomerSegmentPricings",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "EffectiveTo",
                table: "TbCustomerSegmentPricings",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2(2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "EffectiveFrom",
                table: "TbCustomerSegmentPricings",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2(2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "UsedPoints",
                table: "TbCustomerLoyalties",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldDefaultValue: 0m);

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalSpentThisYear",
                table: "TbCustomerLoyalties",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldDefaultValue: 0m);

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalPoints",
                table: "TbCustomerLoyalties",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldDefaultValue: 0m);

            migrationBuilder.AlterColumn<int>(
                name: "TotalOrdersThisYear",
                table: "TbCustomerLoyalties",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "NextTierEligibilityDate",
                table: "TbCustomerLoyalties",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2(2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastTierUpgradeDate",
                table: "TbCustomerLoyalties",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2(2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ExpiredPoints",
                table: "TbCustomerLoyalties",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldDefaultValue: 0m);

            migrationBuilder.AlterColumn<decimal>(
                name: "AvailablePoints",
                table: "TbCustomerLoyalties",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldDefaultValue: 0m);

            migrationBuilder.AlterColumn<decimal>(
                name: "ExchangeRate",
                table: "TbCurrencies",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,6)");

            migrationBuilder.AlterColumn<string>(
                name: "CountryCode",
                table: "TbCurrencies",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(3)",
                oldMaxLength: 3);

            migrationBuilder.AlterColumn<int>(
                name: "UsageCount",
                table: "TbCouponCodes",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "TitleEN",
                table: "TbCouponCodes",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "TitleAR",
                table: "TbCouponCodes",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDateUTC",
                table: "TbCouponCodes",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2(2)",
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDateUTC",
                table: "TbCouponCodes",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2(2)",
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "TbCouponCodes",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "TitleEn",
                table: "TbCountries",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "TitleAr",
                table: "TbCountries",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldDefaultValue: "");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "TbContentAreas",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<int>(
                name: "DisplayOrder",
                table: "TbContentAreas",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "TitleEn",
                table: "TbCities",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "TitleAr",
                table: "TbCities",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldDefaultValue: "");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalSales",
                table: "TbCampaignVendors",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldDefaultValue: 0m);

            migrationBuilder.AlterColumn<int>(
                name: "TotalProductsSubmitted",
                table: "TbCampaignVendors",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "TotalProductsApproved",
                table: "TbCampaignVendors",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalCommissionPaid",
                table: "TbCampaignVendors",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldDefaultValue: 0m);

            migrationBuilder.AlterColumn<bool>(
                name: "IsApproved",
                table: "TbCampaignVendors",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ApprovedAt",
                table: "TbCampaignVendors",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2(2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "AppliedAt",
                table: "TbCampaignVendors",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2(2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                table: "TbCampaigns",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2(2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "MinimumDiscountPercentage",
                table: "TbCampaigns",
                type: "decimal(5,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)",
                oldDefaultValue: 20m);

            migrationBuilder.AlterColumn<bool>(
                name: "IsFeatured",
                table: "TbCampaigns",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "TbCampaigns",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                table: "TbCampaigns",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2(2)");

            migrationBuilder.AlterColumn<int>(
                name: "DisplayOrder",
                table: "TbCampaigns",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "SoldQuantity",
                table: "TbCampaignProducts",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "TbCampaignProducts",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<int>(
                name: "DisplayOrder",
                table: "TbCampaignProducts",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ApprovedAt",
                table: "TbCampaignProducts",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2(2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "WonAt",
                table: "TbBuyBoxHistories",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2(2)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LostAt",
                table: "TbBuyBoxHistories",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2(2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DurationInMinutes",
                table: "TbBuyBoxHistories",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ExpiresAt",
                table: "TbBuyBoxCalculations",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2(2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CalculatedAt",
                table: "TbBuyBoxCalculations",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2(2)",
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.AlterColumn<int>(
                name: "DisplayOrder",
                table: "TbBrands",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "TrademarkExpiryDate",
                table: "TbBrandRegistrationRequests",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2(2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "SubmittedAt",
                table: "TbBrandRegistrationRequests",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2(2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ReviewedAt",
                table: "TbBrandRegistrationRequests",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2(2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "VerifiedAt",
                table: "TbBrandDocuments",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2(2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UploadedAt",
                table: "TbBrandDocuments",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2(2)");

            migrationBuilder.AlterColumn<bool>(
                name: "IsVerified",
                table: "TbBrandDocuments",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsFeatured",
                table: "TbBlockProducts",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "TbBlockProducts",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "FeaturedTo",
                table: "TbBlockProducts",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2(2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "FeaturedFrom",
                table: "TbBlockProducts",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2(2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DisplayOrder",
                table: "TbBlockProducts",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "VerifiedAt",
                table: "TbAuthorizedDistributors",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2(2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "TbAuthorizedDistributors",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "AuthorizationStartDate",
                table: "TbAuthorizedDistributors",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2(2)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "AuthorizationEndDate",
                table: "TbAuthorizedDistributors",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2(2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Severity",
                table: "TbNotifications",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 2);

            migrationBuilder.AlterColumn<DateTime>(
                name: "SentDate",
                table: "TbNotifications",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2(2)",
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ReadDate",
                table: "TbNotifications",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2(2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Message",
                table: "TbNotifications",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AlterColumn<bool>(
                name: "IsRead",
                table: "TbNotifications",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<int>(
                name: "DeliveryStatus",
                table: "TbNotifications",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 1);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TbNotifications",
                table: "TbNotifications",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_TbVendorWallets_VendorId",
                table: "TbVendorWallets",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_TbSupportTicketMessages_SupportTicketId",
                table: "TbSupportTicketMessages",
                column: "SupportTicketId");

            migrationBuilder.CreateIndex(
                name: "IX_TbProductVisibilityRules_ItemId",
                table: "TbProductVisibilityRules",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_TbCustomerWallets_CustomerId",
                table: "TbCustomerWallets",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_TbCustomerLoyalties_CustomerId",
                table: "TbCustomerLoyalties",
                column: "CustomerId");

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
                name: "FK_TbBrandDocuments_TbBrandRegistrationRequests_BrandRegistrationRequestId",
                table: "TbBrandDocuments",
                column: "BrandRegistrationRequestId",
                principalTable: "TbBrandRegistrationRequests",
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
                name: "FK_TbBuyBoxHistories_TbItems_ItemId",
                table: "TbBuyBoxHistories",
                column: "ItemId",
                principalTable: "TbItems",
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
                name: "FK_TbCampaignVendors_TbCampaigns_CampaignId",
                table: "TbCampaignVendors",
                column: "CampaignId",
                principalTable: "TbCampaigns",
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
                name: "FK_TbCustomerSegmentPricings_TbOffers_OfferId",
                table: "TbCustomerSegmentPricings",
                column: "OfferId",
                principalTable: "TbOffers",
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
                name: "FK_TbFlashSaleProducts_TbFlashSales_FlashSaleId",
                table: "TbFlashSaleProducts",
                column: "FlashSaleId",
                principalTable: "TbFlashSales",
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
                name: "FK_TbOffers_TbOfferConditions_TbOfferConditionId",
                table: "TbOffers",
                column: "TbOfferConditionId",
                principalTable: "TbOfferConditions",
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
                name: "FK_TbPriceHistories_TbOffers_OfferId",
                table: "TbPriceHistories",
                column: "OfferId",
                principalTable: "TbOffers",
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
                name: "FK_TbRequestComments_TbSellerRequests_SellerRequestId",
                table: "TbRequestComments",
                column: "SellerRequestId",
                principalTable: "TbSellerRequests",
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
                name: "FK_TbVendorTierHistories_TbVendors_VendorId",
                table: "TbVendorTierHistories",
                column: "VendorId",
                principalTable: "TbVendors",
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
                name: "FK_TbVisibilityLogs_TbItems_ItemId",
                table: "TbVisibilityLogs",
                column: "ItemId",
                principalTable: "TbItems",
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
