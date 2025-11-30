using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class addNewBusinessTables : Migration
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
                name: "FK_TbCategoryAttributes_TbAttributes_AttributeId",
                table: "TbCategoryAttributes");

            migrationBuilder.DropForeignKey(
                name: "FK_TbCategoryAttributes_TbCategories_CategoryId",
                table: "TbCategoryAttributes");

            migrationBuilder.DropForeignKey(
                name: "FK_TbCities_TbStates_StateId",
                table: "TbCities");

            migrationBuilder.DropForeignKey(
                name: "FK_TbCombinationAttribute_TbItemCombination_ItemCombinationId",
                table: "TbCombinationAttribute");

            migrationBuilder.DropForeignKey(
                name: "FK_TbCombinationAttributesValue_TbAttributes_AttributeId",
                table: "TbCombinationAttributesValue");

            migrationBuilder.DropForeignKey(
                name: "FK_TbCombinationAttributesValue_TbCombinationAttribute_CombinationAttributeId",
                table: "TbCombinationAttributesValue");

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
                name: "FK_TbItemCombination_TbItems_ItemId",
                table: "TbItemCombination");

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
                name: "FK_TbItems_TbVideoProviders_VideoProviderId",
                table: "TbItems");

            migrationBuilder.DropForeignKey(
                name: "FK_TbOrder_AspNetUsers_UserId",
                table: "TbOrder");

            migrationBuilder.DropForeignKey(
                name: "FK_TbOrder_TbCoupons_CouponId",
                table: "TbOrder");

            migrationBuilder.DropForeignKey(
                name: "FK_TbOrder_TbShippingCompanies_ShippingCompanyId",
                table: "TbOrder");

            migrationBuilder.DropForeignKey(
                name: "FK_TbOrderDetail_TbItems_ItemId",
                table: "TbOrderDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_TbOrderDetail_TbOrder_OrderId",
                table: "TbOrderDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_TbStates_TbCountries_CountryId",
                table: "TbStates");

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

            migrationBuilder.DropTable(
                name: "TbTestimonials");

            migrationBuilder.DropIndex(
                name: "IX_TbUserNotifications_IsRead",
                table: "TbUserNotifications");

            migrationBuilder.DropIndex(
                name: "IX_TbItems_IsNewArrival",
                table: "TbItems");

            migrationBuilder.DropIndex(
                name: "IX_TbItems_VideoProviderId",
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
                name: "IX_TbCurrencies_Code",
                table: "TbCurrencies");

            migrationBuilder.DropIndex(
                name: "IX_TbCurrencies_IsActive",
                table: "TbCurrencies");

            migrationBuilder.DropIndex(
                name: "IX_TbCurrencies_IsBaseCurrency",
                table: "TbCurrencies");

            migrationBuilder.DropIndex(
                name: "IX_TbBrands_DisplayOrder",
                table: "TbBrands");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TbOrderDetail",
                table: "TbOrderDetail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TbOrder",
                table: "TbOrder");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TbItemCombination",
                table: "TbItemCombination");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TbCoupons",
                table: "TbCoupons");

            migrationBuilder.DropIndex(
                name: "IX_TbCoupons_Code",
                table: "TbCoupons");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TbCombinationAttributesValue",
                table: "TbCombinationAttributesValue");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TbCombinationAttribute",
                table: "TbCombinationAttribute");

            migrationBuilder.RenameTable(
                name: "TbOrderDetail",
                newName: "TbOrderDetails");

            migrationBuilder.RenameTable(
                name: "TbOrder",
                newName: "TbOrders");

            migrationBuilder.RenameTable(
                name: "TbItemCombination",
                newName: "TbItemCombinations");

            migrationBuilder.RenameTable(
                name: "TbCoupons",
                newName: "TbCouponCodes");

            migrationBuilder.RenameTable(
                name: "TbCombinationAttributesValue",
                newName: "TbCombinationAttributesValues");

            migrationBuilder.RenameTable(
                name: "TbCombinationAttribute",
                newName: "TbCombinationAttributes");

            migrationBuilder.RenameColumn(
                name: "VideoLink",
                table: "TbItems",
                newName: "VideoUrl");

            migrationBuilder.RenameIndex(
                name: "IX_TbOrderDetail_OrderId",
                table: "TbOrderDetails",
                newName: "IX_TbOrderDetails_OrderId");

            migrationBuilder.RenameIndex(
                name: "IX_TbOrderDetail_ItemId",
                table: "TbOrderDetails",
                newName: "IX_TbOrderDetails_ItemId");

            migrationBuilder.RenameIndex(
                name: "IX_TbOrderDetail_CurrentState",
                table: "TbOrderDetails",
                newName: "IX_TbOrderDetails_CurrentState");

            migrationBuilder.RenameIndex(
                name: "IX_TbOrder_UserId",
                table: "TbOrders",
                newName: "IX_TbOrders_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_TbOrder_ShippingCompanyId",
                table: "TbOrders",
                newName: "IX_TbOrders_ShippingCompanyId");

            migrationBuilder.RenameIndex(
                name: "IX_TbOrder_CurrentState",
                table: "TbOrders",
                newName: "IX_TbOrders_CurrentState");

            migrationBuilder.RenameIndex(
                name: "IX_TbOrder_CouponId",
                table: "TbOrders",
                newName: "IX_TbOrders_CouponId");

            migrationBuilder.RenameIndex(
                name: "IX_TbItemCombination_ItemId",
                table: "TbItemCombinations",
                newName: "IX_TbItemCombinations_ItemId");

            migrationBuilder.RenameIndex(
                name: "IX_TbItemCombination_CurrentState",
                table: "TbItemCombinations",
                newName: "IX_TbItemCombinations_CurrentState");

            migrationBuilder.RenameIndex(
                name: "IX_TbCoupons_CurrentState",
                table: "TbCouponCodes",
                newName: "IX_TbCouponCodes_CurrentState");

            migrationBuilder.RenameIndex(
                name: "IX_TbCombinationAttributesValue_CurrentState",
                table: "TbCombinationAttributesValues",
                newName: "IX_TbCombinationAttributesValues_CurrentState");

            migrationBuilder.RenameIndex(
                name: "IX_TbCombinationAttributesValue_CombinationAttributeId",
                table: "TbCombinationAttributesValues",
                newName: "IX_TbCombinationAttributesValues_CombinationAttributeId");

            migrationBuilder.RenameIndex(
                name: "IX_TbCombinationAttributesValue_AttributeId",
                table: "TbCombinationAttributesValues",
                newName: "IX_TbCombinationAttributesValues_AttributeId");

            migrationBuilder.RenameIndex(
                name: "IX_TbCombinationAttribute_ItemCombinationId",
                table: "TbCombinationAttributes",
                newName: "IX_TbCombinationAttributes_ItemCombinationId");

            migrationBuilder.RenameIndex(
                name: "IX_TbCombinationAttribute_CurrentState",
                table: "TbCombinationAttributes",
                newName: "IX_TbCombinationAttributes_CurrentState");

            migrationBuilder.AlterColumn<bool>(
                name: "IsRead",
                table: "TbUserNotifications",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

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
                name: "IsNewArrival",
                table: "TbItems",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

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

            migrationBuilder.AlterColumn<int>(
                name: "DisplayOrder",
                table: "TbBrands",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

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

            migrationBuilder.AddPrimaryKey(
                name: "PK_TbOrderDetails",
                table: "TbOrderDetails",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TbOrders",
                table: "TbOrders",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TbItemCombinations",
                table: "TbItemCombinations",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TbCouponCodes",
                table: "TbCouponCodes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TbCombinationAttributesValues",
                table: "TbCombinationAttributesValues",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TbCombinationAttributes",
                table: "TbCombinationAttributes",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "TbNotificationPreferences",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserType = table.Column<int>(type: "int", nullable: false),
                    NotificationType = table.Column<int>(type: "int", nullable: false),
                    EnableEmail = table.Column<bool>(type: "bit", nullable: false),
                    EnableSMS = table.Column<bool>(type: "bit", nullable: false),
                    EnablePush = table.Column<bool>(type: "bit", nullable: false),
                    EnableInApp = table.Column<bool>(type: "bit", nullable: false),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationPreferences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotificationPreferences_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    RecipientID = table.Column<int>(type: "int", nullable: false),
                    RecipientType = table.Column<int>(type: "int", nullable: false),
                    NotificationType = table.Column<int>(type: "int", nullable: false),
                    Severity = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Data = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    ReadDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeliveryStatus = table.Column<int>(type: "int", nullable: false),
                    DeliveryChannel = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TbAuthorizedDistributors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    BrandId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VendorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AuthorizationNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    AuthorizationStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AuthorizationEndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    AuthorizationDocumentPath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    VerifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    VerifiedByUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbAuthorizedDistributors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbAuthorizedDistributors_AspNetUsers_VerifiedByUserId",
                        column: x => x.VerifiedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TbAuthorizedDistributors_TbBrands_BrandId",
                        column: x => x.BrandId,
                        principalTable: "TbBrands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TbAuthorizedDistributors_TbVendors_VendorId",
                        column: x => x.VendorId,
                        principalTable: "TbVendors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TbBrandRegistrationRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    VendorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BrandNameEn = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    BrandNameAr = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    BrandType = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    DescriptionEn = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DescriptionAr = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    OfficialWebsite = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    TrademarkNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TrademarkExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CommercialRegistrationNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SubmittedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReviewedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReviewedByUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ReviewNotes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    RejectionReason = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ApprovedBrandId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbBrandRegistrationRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbBrandRegistrationRequests_AspNetUsers_ReviewedByUserId",
                        column: x => x.ReviewedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TbBrandRegistrationRequests_TbBrands_ApprovedBrandId",
                        column: x => x.ApprovedBrandId,
                        principalTable: "TbBrands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TbBrandRegistrationRequests_TbVendors_VendorId",
                        column: x => x.VendorId,
                        principalTable: "TbVendors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TbCampaigns",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    TitleEn = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    TitleAr = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DescriptionEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CampaignType = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MinimumDiscountPercentage = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    FundingModel = table.Column<int>(type: "int", nullable: false),
                    PlatformFundingPercentage = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    SellerFundingPercentage = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    BannerImagePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ThumbnailImagePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ThemeColor = table.Column<string>(type: "nvarchar(7)", maxLength: 7, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsFeatured = table.Column<bool>(type: "bit", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    SlugEn = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SlugAr = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    MaxParticipatingProducts = table.Column<int>(type: "int", nullable: true),
                    MaxProductsPerVendor = table.Column<int>(type: "int", nullable: true),
                    MinimumOrderValue = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    BudgetLimit = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalSpent = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbCampaigns", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TbContentAreas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    TitleAr = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TitleEn = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    AreaCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DescriptionAr = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DescriptionEn = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbContentAreas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TbCustomerWallets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AvailableBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PendingBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalEarned = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalSpent = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CurrencyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastTransactionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbCustomerWallets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbCustomerWallets_TbCurrencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "TbCurrencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TbCustomerWallets_TbCustomers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "TbCustomers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TbDeliveryReviews",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    ReviewNumber = table.Column<int>(type: "int", nullable: false),
                    OrderID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PackagingRating = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CourierDeliveryRating = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    OverallRating = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ShippingAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ReviewDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbDeliveryReviews", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TbDisputes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    DisputeNumber = table.Column<int>(type: "int", nullable: false),
                    MessageID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SenderID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Parties = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SenderType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Details = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RequiredResolution = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RequestedRefund = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Evidence = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PlatformDecision = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Resolution = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ResolutionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ResolvedNotes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OpenedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ClosedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AssignedAdminID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbDisputes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TbFlashSales",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    TitleEn = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    TitleAr = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DescriptionEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DurationInHours = table.Column<int>(type: "int", nullable: false),
                    MinimumDiscountPercentage = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    MinimumSellerRating = table.Column<decimal>(type: "decimal(3,2)", nullable: false),
                    BannerImagePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ThemeColor = table.Column<string>(type: "nvarchar(7)", maxLength: 7, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    ShowCountdownTimer = table.Column<bool>(type: "bit", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    MaxProducts = table.Column<int>(type: "int", nullable: true),
                    TotalSales = table.Column<int>(type: "int", nullable: false),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbFlashSales", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TbFulfillmentMethods",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DescriptionEn = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DescriptionAr = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    BuyBoxPriorityBoost = table.Column<int>(type: "int", nullable: false),
                    RequiresWarehouse = table.Column<bool>(type: "bit", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbFulfillmentMethods", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TbHomepageBlocks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    TitleEn = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TitleAr = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DescriptionEn = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DescriptionAr = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    BlockType = table.Column<int>(type: "int", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsVisible = table.Column<bool>(type: "bit", nullable: false),
                    BackgroundColor = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BackgroundImagePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    TextColor = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CssClass = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    MaxItemsToDisplay = table.Column<int>(type: "int", nullable: true),
                    ShowViewAllLink = table.Column<bool>(type: "bit", nullable: false),
                    ViewAllLinkUrl = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ActiveFrom = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ActiveTo = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbHomepageBlocks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TbLoyaltyTiers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    TierNameEn = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TierNameAr = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MinimumOrdersPerYear = table.Column<int>(type: "int", nullable: false),
                    MaximumOrdersPerYear = table.Column<int>(type: "int", nullable: false),
                    PointsMultiplier = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CashbackPercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    HasFreeShipping = table.Column<bool>(type: "bit", nullable: false),
                    HasPrioritySupport = table.Column<bool>(type: "bit", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    DescriptionEn = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DescriptionAr = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    BadgeColor = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BadgeIconPath = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbLoyaltyTiers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TbMoitems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    DocumentNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DocumentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MovementType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbMoitems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbMoitems_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TbMortems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    DocumentNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DocumentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbMortems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbMortems_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TbMortems_TbOrders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "TbOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TbNotificationChannels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    TitleAr = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TitleEn = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Channel = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Configuration = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbNotificationChannels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TbOfferConditions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    NameAr = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsNew = table.Column<bool>(type: "bit", nullable: false),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbOfferConditions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TbPlatformTreasuries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    TotalBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CustomerWalletsTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    VendorWalletsTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PendingCommissions = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CollectedCommissions = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PendingPayouts = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ProcessedPayouts = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalRevenue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalCommissions = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalRefunds = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalPayouts = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LastUpdatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CurrencyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastReconciliationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbPlatformTreasuries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbPlatformTreasuries_TbCurrencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "TbCurrencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TbProductReviews",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    ReviewNumber = table.Column<int>(type: "int", nullable: false),
                    ProductID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderItemID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Rating = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ReviewTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReviewText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Images = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Videos = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsVerifiedPurchase = table.Column<bool>(type: "bit", nullable: false),
                    HelpfulCount = table.Column<int>(type: "int", nullable: false),
                    NotHelpfulCount = table.Column<int>(type: "int", nullable: false),
                    ReviewDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ApprovedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModerationNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VendorResponse = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VendorResponseDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbProductReviews", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TbProductVisibilityRules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    ItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VisibilityStatus = table.Column<int>(type: "int", nullable: false),
                    IsVisible = table.Column<bool>(type: "bit", nullable: false),
                    HasActiveOffers = table.Column<bool>(type: "bit", nullable: false),
                    HasStock = table.Column<bool>(type: "bit", nullable: false),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    HasValidCategory = table.Column<bool>(type: "bit", nullable: false),
                    AllSellersActive = table.Column<bool>(type: "bit", nullable: false),
                    LastCheckedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SuppressedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SuppressedByUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbProductVisibilityRules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbProductVisibilityRules_AspNetUsers_SuppressedByUserId",
                        column: x => x.SuppressedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TbProductVisibilityRules_TbItems_ItemId",
                        column: x => x.ItemId,
                        principalTable: "TbItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TbRefundRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    RefundAmount = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RefundReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    AdminComments = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ProcessedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RefundMethod = table.Column<int>(type: "int", nullable: true),
                    AdminUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbRefundRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbRefundRequests_AspNetUsers_AdminUserId",
                        column: x => x.AdminUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TbRefundRequests_TbOrders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "TbOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TbSalesReviews",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    ReviewNumber = table.Column<int>(type: "int", nullable: false),
                    OrderItemID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VendorID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductAccuracyRating = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ShippingSpeedRating = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CommunicationRating = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ServiceRating = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    OverallRating = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ReviewDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbSalesReviews", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TbSellerPerformanceMetrics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    VendorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AverageRating = table.Column<decimal>(type: "decimal(3,2)", nullable: false),
                    TotalReviews = table.Column<int>(type: "int", nullable: false),
                    OrderCompletionRate = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    OnTimeShippingRate = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    ReturnRate = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    CancellationRate = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    ResponseRate = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    AverageResponseTimeInHours = table.Column<int>(type: "int", nullable: false),
                    BuyBoxWins = table.Column<int>(type: "int", nullable: false),
                    BuyBoxWinRate = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    UsesFBM = table.Column<bool>(type: "bit", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CalculatedForPeriodStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CalculatedForPeriodEnd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbSellerPerformanceMetrics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbSellerPerformanceMetrics_TbVendors_VendorId",
                        column: x => x.VendorId,
                        principalTable: "TbVendors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TbSellerRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    VendorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RequestType = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    TitleEn = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    TitleAr = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DescriptionEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DescriptionAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RequestData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubmittedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReviewedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ProcessedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReviewedByUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ReviewNotes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    RejectionReason = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbSellerRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbSellerRequests_AspNetUsers_ReviewedByUserId",
                        column: x => x.ReviewedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TbSellerRequests_TbVendors_VendorId",
                        column: x => x.VendorId,
                        principalTable: "TbVendors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TbSellerTiers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    TierCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TierNameEn = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TierNameAr = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DescriptionEn = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DescriptionAr = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    MinimumOrders = table.Column<int>(type: "int", nullable: false),
                    MaximumOrders = table.Column<int>(type: "int", nullable: true),
                    CommissionReductionPercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    HasPrioritySupport = table.Column<bool>(type: "bit", nullable: false),
                    HasBuyBoxBoost = table.Column<bool>(type: "bit", nullable: false),
                    HasFeaturedListings = table.Column<bool>(type: "bit", nullable: false),
                    HasAdvancedAnalytics = table.Column<bool>(type: "bit", nullable: false),
                    HasDedicatedAccountManager = table.Column<bool>(type: "bit", nullable: false),
                    BadgeColor = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BadgeIconPath = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbSellerTiers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TbSupportTickets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    TicketNumber = table.Column<int>(type: "int", nullable: false),
                    UserID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Priority = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TicketCreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AssignedTo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AssignedTeam = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbSupportTickets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TbVendorWallets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    VendorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AvailableBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PendingBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalEarned = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalWithdrawn = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalCommissionPaid = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CurrencyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastWithdrawalDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastTransactionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    WithdrawalFeePercentage = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbVendorWallets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbVendorWallets_TbCurrencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "TbCurrencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TbVendorWallets_TbVendors_VendorId",
                        column: x => x.VendorId,
                        principalTable: "TbVendors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TbVisibilityLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    ItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WasVisible = table.Column<bool>(type: "bit", nullable: false),
                    IsVisible = table.Column<bool>(type: "bit", nullable: false),
                    ChangedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ChangeReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsAutomatic = table.Column<bool>(type: "bit", nullable: false),
                    ChangedByUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbVisibilityLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbVisibilityLogs_AspNetUsers_ChangedByUserId",
                        column: x => x.ChangedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TbVisibilityLogs_TbItems_ItemId",
                        column: x => x.ItemId,
                        principalTable: "TbItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TbWarehouses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    TitleAr = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    TitleEn = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    PhoneCode = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbWarehouses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TbWarranties",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    WarrantyType = table.Column<int>(type: "int", nullable: false),
                    WarrantyPeriodMonths = table.Column<int>(type: "int", nullable: false),
                    WarrantyPolicy = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    WarrantyServiceCenter = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CityId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbWarranties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbWarranties_TbCities_CityId",
                        column: x => x.CityId,
                        principalTable: "TbCities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TbBrandDocuments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    BrandRegistrationRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DocumentType = table.Column<int>(type: "int", nullable: false),
                    DocumentName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DocumentPath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    FileExtension = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    UploadedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsVerified = table.Column<bool>(type: "bit", nullable: false),
                    VerifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    VerifiedByUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    VerificationNotes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbBrandDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbBrandDocuments_AspNetUsers_VerifiedByUserId",
                        column: x => x.VerifiedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TbBrandDocuments_TbBrandRegistrationRequests_BrandRegistrationRequestId",
                        column: x => x.BrandRegistrationRequestId,
                        principalTable: "TbBrandRegistrationRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TbCampaignProducts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    CampaignId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VendorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OriginalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CampaignPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DiscountPercentage = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    PlatformContribution = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    VendorContribution = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    StockQuantity = table.Column<int>(type: "int", nullable: true),
                    SoldQuantity = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    ApprovedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ApprovedByUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbCampaignProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbCampaignProducts_AspNetUsers_ApprovedByUserId",
                        column: x => x.ApprovedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TbCampaignProducts_TbCampaigns_CampaignId",
                        column: x => x.CampaignId,
                        principalTable: "TbCampaigns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TbCampaignProducts_TbItems_ItemId",
                        column: x => x.ItemId,
                        principalTable: "TbItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TbCampaignProducts_TbVendors_VendorId",
                        column: x => x.VendorId,
                        principalTable: "TbVendors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TbCampaignVendors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    CampaignId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VendorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    AppliedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ApprovedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ApprovedByUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    TotalProductsSubmitted = table.Column<int>(type: "int", nullable: false),
                    TotalProductsApproved = table.Column<int>(type: "int", nullable: false),
                    TotalSales = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalCommissionPaid = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbCampaignVendors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbCampaignVendors_AspNetUsers_ApprovedByUserId",
                        column: x => x.ApprovedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TbCampaignVendors_TbCampaigns_CampaignId",
                        column: x => x.CampaignId,
                        principalTable: "TbCampaigns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TbCampaignVendors_TbVendors_VendorId",
                        column: x => x.VendorId,
                        principalTable: "TbVendors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TbMediaContents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    ContentAreaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TitleAr = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TitleEn = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DescriptionAr = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DescriptionEn = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    MediaType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    MediaPath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    LinkUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbMediaContents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbMediaContents_TbContentAreas_ContentAreaId",
                        column: x => x.ContentAreaId,
                        principalTable: "TbContentAreas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TbDisputeMessages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    DisputeID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MessageNumber = table.Column<int>(type: "int", nullable: false),
                    SenderID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SenderType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Attachments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbDisputeMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbDisputeMessages_TbDisputes_DisputeID",
                        column: x => x.DisputeID,
                        principalTable: "TbDisputes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TbFlashSaleProducts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    FlashSaleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VendorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OriginalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FlashSalePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DiscountPercentage = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    AvailableQuantity = table.Column<int>(type: "int", nullable: false),
                    SoldQuantity = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    ViewCount = table.Column<int>(type: "int", nullable: false),
                    AddToCartCount = table.Column<int>(type: "int", nullable: false),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbFlashSaleProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbFlashSaleProducts_TbFlashSales_FlashSaleId",
                        column: x => x.FlashSaleId,
                        principalTable: "TbFlashSales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TbFlashSaleProducts_TbItems_ItemId",
                        column: x => x.ItemId,
                        principalTable: "TbItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TbFlashSaleProducts_TbVendors_VendorId",
                        column: x => x.VendorId,
                        principalTable: "TbVendors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TbFulfillmentFees",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    FulfillmentMethodId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FeeType = table.Column<int>(type: "int", nullable: false),
                    BaseFee = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PercentageFee = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    MinimumFee = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    MaximumFee = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    WeightBasedFeePerKg = table.Column<decimal>(type: "decimal(10,3)", nullable: true),
                    VolumeBasedFeePerCubicMeter = table.Column<decimal>(type: "decimal(10,3)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    EffectiveFrom = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EffectiveTo = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DescriptionEn = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DescriptionAr = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbFulfillmentFees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbFulfillmentFees_TbFulfillmentMethods_FulfillmentMethodId",
                        column: x => x.FulfillmentMethodId,
                        principalTable: "TbFulfillmentMethods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TbBlockProducts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    HomepageBlockId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsFeatured = table.Column<bool>(type: "bit", nullable: false),
                    BadgeText = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    BadgeColor = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FeaturedFrom = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FeaturedTo = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbBlockProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbBlockProducts_TbHomepageBlocks_HomepageBlockId",
                        column: x => x.HomepageBlockId,
                        principalTable: "TbHomepageBlocks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TbBlockProducts_TbItems_ItemId",
                        column: x => x.ItemId,
                        principalTable: "TbItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TbCustomerLoyalties",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoyaltyTierId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TotalPoints = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AvailablePoints = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UsedPoints = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ExpiredPoints = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalOrdersThisYear = table.Column<int>(type: "int", nullable: false),
                    TotalSpentThisYear = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LastTierUpgradeDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NextTierEligibilityDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbCustomerLoyalties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbCustomerLoyalties_TbCustomers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "TbCustomers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TbCustomerLoyalties_TbLoyaltyTiers_LoyaltyTierId",
                        column: x => x.LoyaltyTierId,
                        principalTable: "TbLoyaltyTiers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TbReviewVotes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    ReviewID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VoteType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VoteValue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WithType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbReviewVotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbReviewVotes_TbProductReviews_ReviewID",
                        column: x => x.ReviewID,
                        principalTable: "TbProductReviews",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TbSuppressionReasons",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    ProductVisibilityRuleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReasonType = table.Column<int>(type: "int", nullable: false),
                    ReasonDescriptionEn = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ReasonDescriptionAr = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    DetectedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ResolvedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsResolved = table.Column<bool>(type: "bit", nullable: false),
                    ResolutionNotes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbSuppressionReasons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbSuppressionReasons_TbProductVisibilityRules_ProductVisibilityRuleId",
                        column: x => x.ProductVisibilityRuleId,
                        principalTable: "TbProductVisibilityRules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TbRequestComments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    SellerRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsInternal = table.Column<bool>(type: "bit", nullable: false),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbRequestComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbRequestComments_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TbRequestComments_TbSellerRequests_SellerRequestId",
                        column: x => x.SellerRequestId,
                        principalTable: "TbSellerRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TbRequestDocuments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    SellerRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DocumentName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DocumentPath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    DocumentType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    UploadedByUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    UploadedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbRequestDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbRequestDocuments_AspNetUsers_UploadedByUserId",
                        column: x => x.UploadedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TbRequestDocuments_TbSellerRequests_SellerRequestId",
                        column: x => x.SellerRequestId,
                        principalTable: "TbSellerRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TbSellerTierBenefits",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    SellerTierId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BenefitNameEn = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    BenefitNameAr = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DescriptionEn = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DescriptionAr = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IconPath = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbSellerTierBenefits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbSellerTierBenefits_TbSellerTiers_SellerTierId",
                        column: x => x.SellerTierId,
                        principalTable: "TbSellerTiers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TbVendorTierHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    VendorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SellerTierId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AchievedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TotalOrdersAtTime = table.Column<int>(type: "int", nullable: false),
                    TotalSalesAtTime = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsAutomatic = table.Column<bool>(type: "bit", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbVendorTierHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbVendorTierHistories_TbSellerTiers_SellerTierId",
                        column: x => x.SellerTierId,
                        principalTable: "TbSellerTiers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TbVendorTierHistories_TbVendors_VendorId",
                        column: x => x.VendorId,
                        principalTable: "TbVendors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TbSupportTicketMessages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    TicketID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MessageNumber = table.Column<int>(type: "int", nullable: false),
                    SenderID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Attachments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SentDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    InternalNote = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SupportTicketId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbSupportTicketMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbSupportTicketMessages_TbSupportTickets_SupportTicketId",
                        column: x => x.SupportTicketId,
                        principalTable: "TbSupportTickets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TbWalletTransactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    CustomerWalletId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    VendorWalletId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TransactionType = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    DescriptionEn = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DescriptionAr = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RefundId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ReferenceNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    BalanceBefore = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    BalanceAfter = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ProcessedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ProcessedByUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbWalletTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbWalletTransactions_AspNetUsers_ProcessedByUserId",
                        column: x => x.ProcessedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TbWalletTransactions_TbCustomerWallets_CustomerWalletId",
                        column: x => x.CustomerWalletId,
                        principalTable: "TbCustomerWallets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TbWalletTransactions_TbOrders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "TbOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TbWalletTransactions_TbRefundRequests_RefundId",
                        column: x => x.RefundId,
                        principalTable: "TbRefundRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TbWalletTransactions_TbVendorWallets_VendorWalletId",
                        column: x => x.VendorWalletId,
                        principalTable: "TbVendorWallets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TbFBMInventories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    ItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VendorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WarehouseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AvailableQuantity = table.Column<int>(type: "int", nullable: false),
                    ReservedQuantity = table.Column<int>(type: "int", nullable: false),
                    InTransitQuantity = table.Column<int>(type: "int", nullable: false),
                    DamagedQuantity = table.Column<int>(type: "int", nullable: false),
                    LastSyncDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SKU = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    LocationCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbFBMInventories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbFBMInventories_TbItems_ItemId",
                        column: x => x.ItemId,
                        principalTable: "TbItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TbFBMInventories_TbVendors_VendorId",
                        column: x => x.VendorId,
                        principalTable: "TbVendors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TbFBMInventories_TbWarehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "TbWarehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TbFBMShipments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    ShipmentNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WarehouseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ShippingCompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    TrackingNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PickupDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ShippedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeliveredDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ShippingCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ActualWeight = table.Column<decimal>(type: "decimal(10,3)", nullable: true),
                    VolumetricWeight = table.Column<decimal>(type: "decimal(10,3)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    DeliveryNotes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbFBMShipments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbFBMShipments_TbOrders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "TbOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TbFBMShipments_TbShippingCompanies_ShippingCompanyId",
                        column: x => x.ShippingCompanyId,
                        principalTable: "TbShippingCompanies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TbFBMShipments_TbWarehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "TbWarehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TbMovitemsdetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    MoitemId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MortemId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WarehouseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ItemAttributeCombinationPricingId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbMovitemsdetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbMovitemsdetails_TbItemAttributeCombinationPricings_ItemAttributeCombinationPricingId",
                        column: x => x.ItemAttributeCombinationPricingId,
                        principalTable: "TbItemAttributeCombinationPricings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TbMovitemsdetails_TbItems_ItemId",
                        column: x => x.ItemId,
                        principalTable: "TbItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TbMovitemsdetails_TbMoitems_MoitemId",
                        column: x => x.MoitemId,
                        principalTable: "TbMoitems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TbMovitemsdetails_TbMortems_MortemId",
                        column: x => x.MortemId,
                        principalTable: "TbMortems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TbMovitemsdetails_TbWarehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "TbWarehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TbOffers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    ItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Condition = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    OriginalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ShippingCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SellerRating = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    WarrantyId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OfferConditionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TbOfferConditionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbOffers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbOffers_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TbOffers_TbItems_ItemId",
                        column: x => x.ItemId,
                        principalTable: "TbItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TbOffers_TbOfferConditions_TbOfferConditionId",
                        column: x => x.TbOfferConditionId,
                        principalTable: "TbOfferConditions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TbOffers_TbWarranties_WarrantyId",
                        column: x => x.WarrantyId,
                        principalTable: "TbWarranties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TbLoyaltyPointsTransactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    CustomerLoyaltyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Points = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TransactionType = table.Column<int>(type: "int", nullable: false),
                    DescriptionEn = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DescriptionAr = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ReviewId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ReferralId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsExpired = table.Column<bool>(type: "bit", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbLoyaltyPointsTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbLoyaltyPointsTransactions_TbCustomerLoyalties_CustomerLoyaltyId",
                        column: x => x.CustomerLoyaltyId,
                        principalTable: "TbCustomerLoyalties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TbLoyaltyPointsTransactions_TbOrders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "TbOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TbLoyaltyPointsTransactions_TbProductReviews_ReviewId",
                        column: x => x.ReviewId,
                        principalTable: "TbProductReviews",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TbBuyBoxCalculations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    ItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WinningOfferId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PriceScore = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    SellerRatingScore = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    ShippingSpeedScore = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    FBMUsageScore = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    StockLevelScore = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    ReturnRateScore = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    TotalScore = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    CalculatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CalculationDetails = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbBuyBoxCalculations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbBuyBoxCalculations_TbItems_ItemId",
                        column: x => x.ItemId,
                        principalTable: "TbItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TbBuyBoxCalculations_TbOffers_WinningOfferId",
                        column: x => x.WinningOfferId,
                        principalTable: "TbOffers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TbBuyBoxHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    ItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OfferId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VendorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Score = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    WonAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LostAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DurationInMinutes = table.Column<int>(type: "int", nullable: false),
                    LossReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbBuyBoxHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbBuyBoxHistories_TbItems_ItemId",
                        column: x => x.ItemId,
                        principalTable: "TbItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TbBuyBoxHistories_TbOffers_OfferId",
                        column: x => x.OfferId,
                        principalTable: "TbOffers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TbBuyBoxHistories_TbVendors_VendorId",
                        column: x => x.VendorId,
                        principalTable: "TbVendors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TbCustomerSegmentPricings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    OfferId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SegmentType = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DiscountPercentage = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    MinimumOrderQuantity = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    EffectiveFrom = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EffectiveTo = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbCustomerSegmentPricings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbCustomerSegmentPricings_TbOffers_OfferId",
                        column: x => x.OfferId,
                        principalTable: "TbOffers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TbOfferCombinationPricings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    ItemCombinationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OfferId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SalesPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AvailableQuantity = table.Column<int>(type: "int", nullable: false),
                    ReservedQuantity = table.Column<int>(type: "int", nullable: false),
                    RefundedQuantity = table.Column<int>(type: "int", nullable: false),
                    DamagedQuantity = table.Column<int>(type: "int", nullable: false),
                    InTransitQuantity = table.Column<int>(type: "int", nullable: false),
                    ReturnedQuantity = table.Column<int>(type: "int", nullable: false),
                    LockedQuantity = table.Column<int>(type: "int", nullable: false),
                    StockStatus = table.Column<int>(type: "int", nullable: false),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbOfferCombinationPricings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbOfferCombinationPricings_TbItemCombinations_ItemCombinationId",
                        column: x => x.ItemCombinationId,
                        principalTable: "TbItemCombinations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TbOfferCombinationPricings_TbOffers_OfferId",
                        column: x => x.OfferId,
                        principalTable: "TbOffers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TbPriceHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    OfferId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OldPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NewPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ChangedPercentage = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    ChangedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ChangedByUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Reason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsAutomatic = table.Column<bool>(type: "bit", nullable: false),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbPriceHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbPriceHistories_AspNetUsers_ChangedByUserId",
                        column: x => x.ChangedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TbPriceHistories_TbOffers_OfferId",
                        column: x => x.OfferId,
                        principalTable: "TbOffers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TbQuantityPricings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    OfferId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MinimumQuantity = table.Column<int>(type: "int", nullable: false),
                    MaximumQuantity = table.Column<int>(type: "int", nullable: true),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DiscountPercentage = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbQuantityPricings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbQuantityPricings_TbOffers_OfferId",
                        column: x => x.OfferId,
                        principalTable: "TbOffers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TbShippingDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    OfferId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ShippingCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ShippingMethod = table.Column<int>(type: "int", nullable: false),
                    MinimumEstimatedDays = table.Column<int>(type: "int", nullable: false),
                    MaximumEstimatedDays = table.Column<int>(type: "int", nullable: false),
                    IsCODSupported = table.Column<bool>(type: "bit", nullable: false),
                    FulfillmentType = table.Column<int>(type: "int", nullable: false),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbShippingDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbShippingDetails_TbCities_CityId",
                        column: x => x.CityId,
                        principalTable: "TbCities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TbShippingDetails_TbOffers_OfferId",
                        column: x => x.OfferId,
                        principalTable: "TbOffers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TbUserOfferRatings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    OfferId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsVerifiedPurchase = table.Column<bool>(type: "bit", nullable: false),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbUserOfferRatings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbUserOfferRatings_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TbUserOfferRatings_TbOffers_OfferId",
                        column: x => x.OfferId,
                        principalTable: "TbOffers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NotificationPreferences_CurrentState",
                table: "TbNotificationPreferences",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationPreferences_UserId",
                table: "TbNotificationPreferences",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_CurrentState",
                table: "Notifications",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbAuthorizedDistributors_BrandId",
                table: "TbAuthorizedDistributors",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_TbAuthorizedDistributors_CurrentState",
                table: "TbAuthorizedDistributors",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbAuthorizedDistributors_VendorId",
                table: "TbAuthorizedDistributors",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_TbAuthorizedDistributors_VerifiedByUserId",
                table: "TbAuthorizedDistributors",
                column: "VerifiedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TbBlockProducts_CurrentState",
                table: "TbBlockProducts",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbBlockProducts_HomepageBlockId",
                table: "TbBlockProducts",
                column: "HomepageBlockId");

            migrationBuilder.CreateIndex(
                name: "IX_TbBlockProducts_ItemId",
                table: "TbBlockProducts",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_TbBrandDocuments_BrandRegistrationRequestId",
                table: "TbBrandDocuments",
                column: "BrandRegistrationRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_TbBrandDocuments_CurrentState",
                table: "TbBrandDocuments",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbBrandDocuments_VerifiedByUserId",
                table: "TbBrandDocuments",
                column: "VerifiedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TbBrandRegistrationRequests_ApprovedBrandId",
                table: "TbBrandRegistrationRequests",
                column: "ApprovedBrandId");

            migrationBuilder.CreateIndex(
                name: "IX_TbBrandRegistrationRequests_CurrentState",
                table: "TbBrandRegistrationRequests",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbBrandRegistrationRequests_ReviewedByUserId",
                table: "TbBrandRegistrationRequests",
                column: "ReviewedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TbBrandRegistrationRequests_VendorId",
                table: "TbBrandRegistrationRequests",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_TbBuyBoxCalculations_CurrentState",
                table: "TbBuyBoxCalculations",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbBuyBoxCalculations_ItemId",
                table: "TbBuyBoxCalculations",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_TbBuyBoxCalculations_WinningOfferId",
                table: "TbBuyBoxCalculations",
                column: "WinningOfferId");

            migrationBuilder.CreateIndex(
                name: "IX_TbBuyBoxHistories_CurrentState",
                table: "TbBuyBoxHistories",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbBuyBoxHistories_ItemId",
                table: "TbBuyBoxHistories",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_TbBuyBoxHistories_OfferId",
                table: "TbBuyBoxHistories",
                column: "OfferId");

            migrationBuilder.CreateIndex(
                name: "IX_TbBuyBoxHistories_VendorId",
                table: "TbBuyBoxHistories",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_TbCampaignProducts_ApprovedByUserId",
                table: "TbCampaignProducts",
                column: "ApprovedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TbCampaignProducts_CampaignId",
                table: "TbCampaignProducts",
                column: "CampaignId");

            migrationBuilder.CreateIndex(
                name: "IX_TbCampaignProducts_CurrentState",
                table: "TbCampaignProducts",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbCampaignProducts_ItemId",
                table: "TbCampaignProducts",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_TbCampaignProducts_VendorId",
                table: "TbCampaignProducts",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_TbCampaigns_CurrentState",
                table: "TbCampaigns",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbCampaignVendors_ApprovedByUserId",
                table: "TbCampaignVendors",
                column: "ApprovedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TbCampaignVendors_CampaignId",
                table: "TbCampaignVendors",
                column: "CampaignId");

            migrationBuilder.CreateIndex(
                name: "IX_TbCampaignVendors_CurrentState",
                table: "TbCampaignVendors",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbCampaignVendors_VendorId",
                table: "TbCampaignVendors",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_TbContentAreas_CurrentState",
                table: "TbContentAreas",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbCustomerLoyalties_CurrentState",
                table: "TbCustomerLoyalties",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbCustomerLoyalties_CustomerId",
                table: "TbCustomerLoyalties",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_TbCustomerLoyalties_LoyaltyTierId",
                table: "TbCustomerLoyalties",
                column: "LoyaltyTierId");

            migrationBuilder.CreateIndex(
                name: "IX_TbCustomerSegmentPricings_CurrentState",
                table: "TbCustomerSegmentPricings",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbCustomerSegmentPricings_OfferId",
                table: "TbCustomerSegmentPricings",
                column: "OfferId");

            migrationBuilder.CreateIndex(
                name: "IX_TbCustomerWallets_CurrencyId",
                table: "TbCustomerWallets",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_TbCustomerWallets_CurrentState",
                table: "TbCustomerWallets",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbCustomerWallets_CustomerId",
                table: "TbCustomerWallets",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_TbDeliveryReviews_CurrentState",
                table: "TbDeliveryReviews",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbDisputeMessages_CurrentState",
                table: "TbDisputeMessages",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbDisputeMessages_DisputeID",
                table: "TbDisputeMessages",
                column: "DisputeID");

            migrationBuilder.CreateIndex(
                name: "IX_TbDisputes_CurrentState",
                table: "TbDisputes",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbFBMInventories_CurrentState",
                table: "TbFBMInventories",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbFBMInventories_ItemId",
                table: "TbFBMInventories",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_TbFBMInventories_VendorId",
                table: "TbFBMInventories",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_TbFBMInventories_WarehouseId",
                table: "TbFBMInventories",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_TbFBMShipments_CurrentState",
                table: "TbFBMShipments",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbFBMShipments_OrderId",
                table: "TbFBMShipments",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_TbFBMShipments_ShippingCompanyId",
                table: "TbFBMShipments",
                column: "ShippingCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_TbFBMShipments_WarehouseId",
                table: "TbFBMShipments",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_TbFlashSaleProducts_CurrentState",
                table: "TbFlashSaleProducts",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbFlashSaleProducts_FlashSaleId",
                table: "TbFlashSaleProducts",
                column: "FlashSaleId");

            migrationBuilder.CreateIndex(
                name: "IX_TbFlashSaleProducts_ItemId",
                table: "TbFlashSaleProducts",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_TbFlashSaleProducts_VendorId",
                table: "TbFlashSaleProducts",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_TbFlashSales_CurrentState",
                table: "TbFlashSales",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbFulfillmentFees_CurrentState",
                table: "TbFulfillmentFees",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbFulfillmentFees_FulfillmentMethodId",
                table: "TbFulfillmentFees",
                column: "FulfillmentMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_TbFulfillmentMethods_CurrentState",
                table: "TbFulfillmentMethods",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbHomepageBlocks_CurrentState",
                table: "TbHomepageBlocks",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbLoyaltyPointsTransactions_CurrentState",
                table: "TbLoyaltyPointsTransactions",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbLoyaltyPointsTransactions_CustomerLoyaltyId",
                table: "TbLoyaltyPointsTransactions",
                column: "CustomerLoyaltyId");

            migrationBuilder.CreateIndex(
                name: "IX_TbLoyaltyPointsTransactions_OrderId",
                table: "TbLoyaltyPointsTransactions",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_TbLoyaltyPointsTransactions_ReviewId",
                table: "TbLoyaltyPointsTransactions",
                column: "ReviewId");

            migrationBuilder.CreateIndex(
                name: "IX_TbLoyaltyTiers_CurrentState",
                table: "TbLoyaltyTiers",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbMediaContents_ContentAreaId",
                table: "TbMediaContents",
                column: "ContentAreaId");

            migrationBuilder.CreateIndex(
                name: "IX_TbMediaContents_CurrentState",
                table: "TbMediaContents",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbMoitems_CurrentState",
                table: "TbMoitems",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbMoitems_UserId",
                table: "TbMoitems",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TbMortems_CurrentState",
                table: "TbMortems",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbMortems_OrderId",
                table: "TbMortems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_TbMortems_UserId",
                table: "TbMortems",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TbMovitemsdetails_CurrentState",
                table: "TbMovitemsdetails",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbMovitemsdetails_ItemAttributeCombinationPricingId",
                table: "TbMovitemsdetails",
                column: "ItemAttributeCombinationPricingId");

            migrationBuilder.CreateIndex(
                name: "IX_TbMovitemsdetails_ItemId",
                table: "TbMovitemsdetails",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_TbMovitemsdetails_MoitemId",
                table: "TbMovitemsdetails",
                column: "MoitemId");

            migrationBuilder.CreateIndex(
                name: "IX_TbMovitemsdetails_MortemId",
                table: "TbMovitemsdetails",
                column: "MortemId");

            migrationBuilder.CreateIndex(
                name: "IX_TbMovitemsdetails_WarehouseId",
                table: "TbMovitemsdetails",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_TbNotificationChannels_CurrentState",
                table: "TbNotificationChannels",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbOfferCombinationPricings_CurrentState",
                table: "TbOfferCombinationPricings",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbOfferCombinationPricings_ItemCombinationId",
                table: "TbOfferCombinationPricings",
                column: "ItemCombinationId");

            migrationBuilder.CreateIndex(
                name: "IX_TbOfferCombinationPricings_OfferId",
                table: "TbOfferCombinationPricings",
                column: "OfferId");

            migrationBuilder.CreateIndex(
                name: "IX_TbOfferConditions_CurrentState",
                table: "TbOfferConditions",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbOffers_CurrentState",
                table: "TbOffers",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbOffers_ItemId",
                table: "TbOffers",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_TbOffers_TbOfferConditionId",
                table: "TbOffers",
                column: "TbOfferConditionId");

            migrationBuilder.CreateIndex(
                name: "IX_TbOffers_UserId",
                table: "TbOffers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TbOffers_WarrantyId",
                table: "TbOffers",
                column: "WarrantyId");

            migrationBuilder.CreateIndex(
                name: "IX_TbPlatformTreasuries_CurrencyId",
                table: "TbPlatformTreasuries",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_TbPlatformTreasuries_CurrentState",
                table: "TbPlatformTreasuries",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbPriceHistories_ChangedByUserId",
                table: "TbPriceHistories",
                column: "ChangedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TbPriceHistories_CurrentState",
                table: "TbPriceHistories",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbPriceHistories_OfferId",
                table: "TbPriceHistories",
                column: "OfferId");

            migrationBuilder.CreateIndex(
                name: "IX_TbProductReviews_CurrentState",
                table: "TbProductReviews",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbProductVisibilityRules_CurrentState",
                table: "TbProductVisibilityRules",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbProductVisibilityRules_ItemId",
                table: "TbProductVisibilityRules",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_TbProductVisibilityRules_SuppressedByUserId",
                table: "TbProductVisibilityRules",
                column: "SuppressedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TbQuantityPricings_CurrentState",
                table: "TbQuantityPricings",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbQuantityPricings_OfferId",
                table: "TbQuantityPricings",
                column: "OfferId");

            migrationBuilder.CreateIndex(
                name: "IX_TbRefundRequests_AdminUserId",
                table: "TbRefundRequests",
                column: "AdminUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TbRefundRequests_CurrentState",
                table: "TbRefundRequests",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbRefundRequests_OrderId",
                table: "TbRefundRequests",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_TbRequestComments_CurrentState",
                table: "TbRequestComments",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbRequestComments_SellerRequestId",
                table: "TbRequestComments",
                column: "SellerRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_TbRequestComments_UserId",
                table: "TbRequestComments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TbRequestDocuments_CurrentState",
                table: "TbRequestDocuments",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbRequestDocuments_SellerRequestId",
                table: "TbRequestDocuments",
                column: "SellerRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_TbRequestDocuments_UploadedByUserId",
                table: "TbRequestDocuments",
                column: "UploadedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TbReviewVotes_CurrentState",
                table: "TbReviewVotes",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbReviewVotes_ReviewID",
                table: "TbReviewVotes",
                column: "ReviewID");

            migrationBuilder.CreateIndex(
                name: "IX_TbSalesReviews_CurrentState",
                table: "TbSalesReviews",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbSellerPerformanceMetrics_CurrentState",
                table: "TbSellerPerformanceMetrics",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbSellerPerformanceMetrics_VendorId",
                table: "TbSellerPerformanceMetrics",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_TbSellerRequests_CurrentState",
                table: "TbSellerRequests",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbSellerRequests_ReviewedByUserId",
                table: "TbSellerRequests",
                column: "ReviewedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TbSellerRequests_VendorId",
                table: "TbSellerRequests",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_TbSellerTierBenefits_CurrentState",
                table: "TbSellerTierBenefits",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbSellerTierBenefits_SellerTierId",
                table: "TbSellerTierBenefits",
                column: "SellerTierId");

            migrationBuilder.CreateIndex(
                name: "IX_TbSellerTiers_CurrentState",
                table: "TbSellerTiers",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbShippingDetails_CityId",
                table: "TbShippingDetails",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_TbShippingDetails_CurrentState",
                table: "TbShippingDetails",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbShippingDetails_OfferId",
                table: "TbShippingDetails",
                column: "OfferId");

            migrationBuilder.CreateIndex(
                name: "IX_TbSupportTicketMessages_CurrentState",
                table: "TbSupportTicketMessages",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbSupportTicketMessages_SupportTicketId",
                table: "TbSupportTicketMessages",
                column: "SupportTicketId");

            migrationBuilder.CreateIndex(
                name: "IX_TbSupportTickets_CurrentState",
                table: "TbSupportTickets",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbSuppressionReasons_CurrentState",
                table: "TbSuppressionReasons",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbSuppressionReasons_ProductVisibilityRuleId",
                table: "TbSuppressionReasons",
                column: "ProductVisibilityRuleId");

            migrationBuilder.CreateIndex(
                name: "IX_TbUserOfferRatings_CurrentState",
                table: "TbUserOfferRatings",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbUserOfferRatings_OfferId",
                table: "TbUserOfferRatings",
                column: "OfferId");

            migrationBuilder.CreateIndex(
                name: "IX_TbUserOfferRatings_UserId",
                table: "TbUserOfferRatings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TbVendorTierHistories_CurrentState",
                table: "TbVendorTierHistories",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbVendorTierHistories_SellerTierId",
                table: "TbVendorTierHistories",
                column: "SellerTierId");

            migrationBuilder.CreateIndex(
                name: "IX_TbVendorTierHistories_VendorId",
                table: "TbVendorTierHistories",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_TbVendorWallets_CurrencyId",
                table: "TbVendorWallets",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_TbVendorWallets_CurrentState",
                table: "TbVendorWallets",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbVendorWallets_VendorId",
                table: "TbVendorWallets",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_TbVisibilityLogs_ChangedByUserId",
                table: "TbVisibilityLogs",
                column: "ChangedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TbVisibilityLogs_CurrentState",
                table: "TbVisibilityLogs",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbVisibilityLogs_ItemId",
                table: "TbVisibilityLogs",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_TbWalletTransactions_CurrentState",
                table: "TbWalletTransactions",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbWalletTransactions_CustomerWalletId",
                table: "TbWalletTransactions",
                column: "CustomerWalletId");

            migrationBuilder.CreateIndex(
                name: "IX_TbWalletTransactions_OrderId",
                table: "TbWalletTransactions",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_TbWalletTransactions_ProcessedByUserId",
                table: "TbWalletTransactions",
                column: "ProcessedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TbWalletTransactions_RefundId",
                table: "TbWalletTransactions",
                column: "RefundId");

            migrationBuilder.CreateIndex(
                name: "IX_TbWalletTransactions_VendorWalletId",
                table: "TbWalletTransactions",
                column: "VendorWalletId");

            migrationBuilder.CreateIndex(
                name: "IX_TbWarehouses_CurrentState",
                table: "TbWarehouses",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbWarranties_CityId",
                table: "TbWarranties",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_TbWarranties_CurrentState",
                table: "TbWarranties",
                column: "CurrentState");

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
                name: "FK_TbStates_TbCountries_CountryId",
                table: "TbStates",
                column: "CountryId",
                principalTable: "TbCountries",
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
                name: "FK_TbStates_TbCountries_CountryId",
                table: "TbStates");

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

            migrationBuilder.DropTable(
                name: "TbNotificationPreferences");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "TbAuthorizedDistributors");

            migrationBuilder.DropTable(
                name: "TbBlockProducts");

            migrationBuilder.DropTable(
                name: "TbBrandDocuments");

            migrationBuilder.DropTable(
                name: "TbBuyBoxCalculations");

            migrationBuilder.DropTable(
                name: "TbBuyBoxHistories");

            migrationBuilder.DropTable(
                name: "TbCampaignProducts");

            migrationBuilder.DropTable(
                name: "TbCampaignVendors");

            migrationBuilder.DropTable(
                name: "TbCustomerSegmentPricings");

            migrationBuilder.DropTable(
                name: "TbDeliveryReviews");

            migrationBuilder.DropTable(
                name: "TbDisputeMessages");

            migrationBuilder.DropTable(
                name: "TbFBMInventories");

            migrationBuilder.DropTable(
                name: "TbFBMShipments");

            migrationBuilder.DropTable(
                name: "TbFlashSaleProducts");

            migrationBuilder.DropTable(
                name: "TbFulfillmentFees");

            migrationBuilder.DropTable(
                name: "TbLoyaltyPointsTransactions");

            migrationBuilder.DropTable(
                name: "TbMediaContents");

            migrationBuilder.DropTable(
                name: "TbMovitemsdetails");

            migrationBuilder.DropTable(
                name: "TbNotificationChannels");

            migrationBuilder.DropTable(
                name: "TbOfferCombinationPricings");

            migrationBuilder.DropTable(
                name: "TbPlatformTreasuries");

            migrationBuilder.DropTable(
                name: "TbPriceHistories");

            migrationBuilder.DropTable(
                name: "TbQuantityPricings");

            migrationBuilder.DropTable(
                name: "TbRequestComments");

            migrationBuilder.DropTable(
                name: "TbRequestDocuments");

            migrationBuilder.DropTable(
                name: "TbReviewVotes");

            migrationBuilder.DropTable(
                name: "TbSalesReviews");

            migrationBuilder.DropTable(
                name: "TbSellerPerformanceMetrics");

            migrationBuilder.DropTable(
                name: "TbSellerTierBenefits");

            migrationBuilder.DropTable(
                name: "TbShippingDetails");

            migrationBuilder.DropTable(
                name: "TbSupportTicketMessages");

            migrationBuilder.DropTable(
                name: "TbSuppressionReasons");

            migrationBuilder.DropTable(
                name: "TbUserOfferRatings");

            migrationBuilder.DropTable(
                name: "TbVendorTierHistories");

            migrationBuilder.DropTable(
                name: "TbVisibilityLogs");

            migrationBuilder.DropTable(
                name: "TbWalletTransactions");

            migrationBuilder.DropTable(
                name: "TbHomepageBlocks");

            migrationBuilder.DropTable(
                name: "TbBrandRegistrationRequests");

            migrationBuilder.DropTable(
                name: "TbCampaigns");

            migrationBuilder.DropTable(
                name: "TbDisputes");

            migrationBuilder.DropTable(
                name: "TbFlashSales");

            migrationBuilder.DropTable(
                name: "TbFulfillmentMethods");

            migrationBuilder.DropTable(
                name: "TbCustomerLoyalties");

            migrationBuilder.DropTable(
                name: "TbContentAreas");

            migrationBuilder.DropTable(
                name: "TbMoitems");

            migrationBuilder.DropTable(
                name: "TbMortems");

            migrationBuilder.DropTable(
                name: "TbWarehouses");

            migrationBuilder.DropTable(
                name: "TbSellerRequests");

            migrationBuilder.DropTable(
                name: "TbProductReviews");

            migrationBuilder.DropTable(
                name: "TbSupportTickets");

            migrationBuilder.DropTable(
                name: "TbProductVisibilityRules");

            migrationBuilder.DropTable(
                name: "TbOffers");

            migrationBuilder.DropTable(
                name: "TbSellerTiers");

            migrationBuilder.DropTable(
                name: "TbCustomerWallets");

            migrationBuilder.DropTable(
                name: "TbRefundRequests");

            migrationBuilder.DropTable(
                name: "TbVendorWallets");

            migrationBuilder.DropTable(
                name: "TbLoyaltyTiers");

            migrationBuilder.DropTable(
                name: "TbOfferConditions");

            migrationBuilder.DropTable(
                name: "TbWarranties");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TbOrders",
                table: "TbOrders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TbOrderDetails",
                table: "TbOrderDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TbItemCombinations",
                table: "TbItemCombinations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TbCouponCodes",
                table: "TbCouponCodes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TbCombinationAttributesValues",
                table: "TbCombinationAttributesValues");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TbCombinationAttributes",
                table: "TbCombinationAttributes");

            migrationBuilder.RenameTable(
                name: "TbOrders",
                newName: "TbOrder");

            migrationBuilder.RenameTable(
                name: "TbOrderDetails",
                newName: "TbOrderDetail");

            migrationBuilder.RenameTable(
                name: "TbItemCombinations",
                newName: "TbItemCombination");

            migrationBuilder.RenameTable(
                name: "TbCouponCodes",
                newName: "TbCoupons");

            migrationBuilder.RenameTable(
                name: "TbCombinationAttributesValues",
                newName: "TbCombinationAttributesValue");

            migrationBuilder.RenameTable(
                name: "TbCombinationAttributes",
                newName: "TbCombinationAttribute");

            migrationBuilder.RenameColumn(
                name: "VideoUrl",
                table: "TbItems",
                newName: "VideoLink");

            migrationBuilder.RenameIndex(
                name: "IX_TbOrders_UserId",
                table: "TbOrder",
                newName: "IX_TbOrder_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_TbOrders_ShippingCompanyId",
                table: "TbOrder",
                newName: "IX_TbOrder_ShippingCompanyId");

            migrationBuilder.RenameIndex(
                name: "IX_TbOrders_CurrentState",
                table: "TbOrder",
                newName: "IX_TbOrder_CurrentState");

            migrationBuilder.RenameIndex(
                name: "IX_TbOrders_CouponId",
                table: "TbOrder",
                newName: "IX_TbOrder_CouponId");

            migrationBuilder.RenameIndex(
                name: "IX_TbOrderDetails_OrderId",
                table: "TbOrderDetail",
                newName: "IX_TbOrderDetail_OrderId");

            migrationBuilder.RenameIndex(
                name: "IX_TbOrderDetails_ItemId",
                table: "TbOrderDetail",
                newName: "IX_TbOrderDetail_ItemId");

            migrationBuilder.RenameIndex(
                name: "IX_TbOrderDetails_CurrentState",
                table: "TbOrderDetail",
                newName: "IX_TbOrderDetail_CurrentState");

            migrationBuilder.RenameIndex(
                name: "IX_TbItemCombinations_ItemId",
                table: "TbItemCombination",
                newName: "IX_TbItemCombination_ItemId");

            migrationBuilder.RenameIndex(
                name: "IX_TbItemCombinations_CurrentState",
                table: "TbItemCombination",
                newName: "IX_TbItemCombination_CurrentState");

            migrationBuilder.RenameIndex(
                name: "IX_TbCouponCodes_CurrentState",
                table: "TbCoupons",
                newName: "IX_TbCoupons_CurrentState");

            migrationBuilder.RenameIndex(
                name: "IX_TbCombinationAttributesValues_CurrentState",
                table: "TbCombinationAttributesValue",
                newName: "IX_TbCombinationAttributesValue_CurrentState");

            migrationBuilder.RenameIndex(
                name: "IX_TbCombinationAttributesValues_CombinationAttributeId",
                table: "TbCombinationAttributesValue",
                newName: "IX_TbCombinationAttributesValue_CombinationAttributeId");

            migrationBuilder.RenameIndex(
                name: "IX_TbCombinationAttributesValues_AttributeId",
                table: "TbCombinationAttributesValue",
                newName: "IX_TbCombinationAttributesValue_AttributeId");

            migrationBuilder.RenameIndex(
                name: "IX_TbCombinationAttributes_ItemCombinationId",
                table: "TbCombinationAttribute",
                newName: "IX_TbCombinationAttribute_ItemCombinationId");

            migrationBuilder.RenameIndex(
                name: "IX_TbCombinationAttributes_CurrentState",
                table: "TbCombinationAttribute",
                newName: "IX_TbCombinationAttribute_CurrentState");

            migrationBuilder.AlterColumn<bool>(
                name: "IsRead",
                table: "TbUserNotifications",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

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
                name: "IsNewArrival",
                table: "TbItems",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

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

            migrationBuilder.AlterColumn<int>(
                name: "DisplayOrder",
                table: "TbBrands",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "UsageCount",
                table: "TbCoupons",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "TitleEN",
                table: "TbCoupons",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "TitleAR",
                table: "TbCoupons",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDateUTC",
                table: "TbCoupons",
                type: "datetime2(2)",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDateUTC",
                table: "TbCoupons",
                type: "datetime2(2)",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "TbCoupons",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TbOrder",
                table: "TbOrder",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TbOrderDetail",
                table: "TbOrderDetail",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TbItemCombination",
                table: "TbItemCombination",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TbCoupons",
                table: "TbCoupons",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TbCombinationAttributesValue",
                table: "TbCombinationAttributesValue",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TbCombinationAttribute",
                table: "TbCombinationAttribute",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "TbTestimonials",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CustomerImagePath = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CustomerName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CustomerTitle = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    TestimonialText = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbTestimonials", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TbUserNotifications_IsRead",
                table: "TbUserNotifications",
                column: "IsRead");

            migrationBuilder.CreateIndex(
                name: "IX_TbItems_IsNewArrival",
                table: "TbItems",
                column: "IsNewArrival");

            migrationBuilder.CreateIndex(
                name: "IX_TbItems_VideoProviderId",
                table: "TbItems",
                column: "VideoProviderId");

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
                name: "IX_TbBrands_DisplayOrder",
                table: "TbBrands",
                column: "DisplayOrder");

            migrationBuilder.CreateIndex(
                name: "IX_TbCoupons_Code",
                table: "TbCoupons",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TbTestimonials_CurrentState",
                table: "TbTestimonials",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbTestimonials_DisplayOrder",
                table: "TbTestimonials",
                column: "DisplayOrder");

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
                name: "FK_TbCombinationAttribute_TbItemCombination_ItemCombinationId",
                table: "TbCombinationAttribute",
                column: "ItemCombinationId",
                principalTable: "TbItemCombination",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbCombinationAttributesValue_TbAttributes_AttributeId",
                table: "TbCombinationAttributesValue",
                column: "AttributeId",
                principalTable: "TbAttributes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbCombinationAttributesValue_TbCombinationAttribute_CombinationAttributeId",
                table: "TbCombinationAttributesValue",
                column: "CombinationAttributeId",
                principalTable: "TbCombinationAttribute",
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
                name: "FK_TbItemCombination_TbItems_ItemId",
                table: "TbItemCombination",
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
                name: "FK_TbItems_TbVideoProviders_VideoProviderId",
                table: "TbItems",
                column: "VideoProviderId",
                principalTable: "TbVideoProviders",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TbOrder_AspNetUsers_UserId",
                table: "TbOrder",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbOrder_TbCoupons_CouponId",
                table: "TbOrder",
                column: "CouponId",
                principalTable: "TbCoupons",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TbOrder_TbShippingCompanies_ShippingCompanyId",
                table: "TbOrder",
                column: "ShippingCompanyId",
                principalTable: "TbShippingCompanies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TbOrderDetail_TbItems_ItemId",
                table: "TbOrderDetail",
                column: "ItemId",
                principalTable: "TbItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbOrderDetail_TbOrder_OrderId",
                table: "TbOrderDetail",
                column: "OrderId",
                principalTable: "TbOrder",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbStates_TbCountries_CountryId",
                table: "TbStates",
                column: "CountryId",
                principalTable: "TbCountries",
                principalColumn: "Id");

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
                name: "FK_TbUserNotifications_TbNotifications_NotificationId",
                table: "TbUserNotifications",
                column: "NotificationId",
                principalTable: "TbNotifications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
