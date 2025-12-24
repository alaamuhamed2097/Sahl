using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class refactorHomePageBlockesAndCampains : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TbCampaignProducts_AspNetUsers_ApprovedByUserId",
                table: "TbCampaignProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_TbCampaignProducts_TbVendors_VendorId",
                table: "TbCampaignProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_TbCampaignVendors_AspNetUsers_ApprovedByUserId",
                table: "TbCampaignVendors");

            migrationBuilder.DropTable(
                name: "TbFlashSaleProducts");

            migrationBuilder.DropTable(
                name: "TbFlashSales");

            migrationBuilder.DropIndex(
                name: "IX_TbHomepageBlocks_ActiveFrom_ActiveTo",
                table: "TbHomepageBlocks");

            migrationBuilder.DropIndex(
                name: "IX_TbHomepageBlocks_BlockType",
                table: "TbHomepageBlocks");

            migrationBuilder.DropIndex(
                name: "IX_TbHomepageBlocks_IsActive",
                table: "TbHomepageBlocks");

            migrationBuilder.DropIndex(
                name: "IX_TbHomepageBlocks_IsActive_DisplayOrder",
                table: "TbHomepageBlocks");

            migrationBuilder.DropIndex(
                name: "IX_TbCampaignVendors_ApprovedByUserId",
                table: "TbCampaignVendors");

            migrationBuilder.DropIndex(
                name: "IX_TbCampaigns_CampaignType",
                table: "TbCampaigns");

            migrationBuilder.DropIndex(
                name: "IX_TbCampaigns_IsFeatured",
                table: "TbCampaigns");

            migrationBuilder.DropIndex(
                name: "IX_TbCampaigns_SlugEn",
                table: "TbCampaigns");

            migrationBuilder.DropIndex(
                name: "IX_TbCampaignProducts_ApprovedByUserId",
                table: "TbCampaignProducts");

            migrationBuilder.DropIndex(
                name: "IX_TbCampaignProducts_VendorId",
                table: "TbCampaignProducts");

            migrationBuilder.DropIndex(
                name: "IX_TbBlockProducts_IsActive",
                table: "TbBlockProducts");

            migrationBuilder.DropIndex(
                name: "IX_TbBlockProducts_IsFeatured",
                table: "TbBlockProducts");

            migrationBuilder.DropColumn(
                name: "ActiveFrom",
                table: "TbHomepageBlocks");

            migrationBuilder.DropColumn(
                name: "ActiveTo",
                table: "TbHomepageBlocks");

            migrationBuilder.DropColumn(
                name: "BackgroundColor",
                table: "TbHomepageBlocks");

            migrationBuilder.DropColumn(
                name: "BackgroundImagePath",
                table: "TbHomepageBlocks");

            migrationBuilder.DropColumn(
                name: "CssClass",
                table: "TbHomepageBlocks");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "TbHomepageBlocks");

            migrationBuilder.DropColumn(
                name: "TextColor",
                table: "TbHomepageBlocks");

            migrationBuilder.DropColumn(
                name: "AppliedAt",
                table: "TbCampaignVendors");

            migrationBuilder.DropColumn(
                name: "ApprovedByUserId",
                table: "TbCampaignVendors");

            migrationBuilder.DropColumn(
                name: "TotalCommissionPaid",
                table: "TbCampaignVendors");

            migrationBuilder.DropColumn(
                name: "TotalProductsApproved",
                table: "TbCampaignVendors");

            migrationBuilder.DropColumn(
                name: "TotalProductsSubmitted",
                table: "TbCampaignVendors");

            migrationBuilder.DropColumn(
                name: "TotalSales",
                table: "TbCampaignVendors");

            migrationBuilder.DropColumn(
                name: "BannerImagePath",
                table: "TbCampaigns");

            migrationBuilder.DropColumn(
                name: "BudgetLimit",
                table: "TbCampaigns");

            migrationBuilder.DropColumn(
                name: "CampaignType",
                table: "TbCampaigns");

            migrationBuilder.DropColumn(
                name: "DescriptionAr",
                table: "TbCampaigns");

            migrationBuilder.DropColumn(
                name: "DescriptionEn",
                table: "TbCampaigns");

            migrationBuilder.DropColumn(
                name: "DisplayOrder",
                table: "TbCampaigns");

            migrationBuilder.DropColumn(
                name: "FundingModel",
                table: "TbCampaigns");

            migrationBuilder.DropColumn(
                name: "IsFeatured",
                table: "TbCampaigns");

            migrationBuilder.DropColumn(
                name: "MaxParticipatingProducts",
                table: "TbCampaigns");

            migrationBuilder.DropColumn(
                name: "MinimumDiscountPercentage",
                table: "TbCampaigns");

            migrationBuilder.DropColumn(
                name: "MinimumOrderValue",
                table: "TbCampaigns");

            migrationBuilder.DropColumn(
                name: "PlatformFundingPercentage",
                table: "TbCampaigns");

            migrationBuilder.DropColumn(
                name: "SellerFundingPercentage",
                table: "TbCampaigns");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "TbCampaigns");

            migrationBuilder.DropColumn(
                name: "ThemeColor",
                table: "TbCampaigns");

            migrationBuilder.DropColumn(
                name: "ThumbnailImagePath",
                table: "TbCampaigns");

            migrationBuilder.DropColumn(
                name: "TotalSpent",
                table: "TbCampaigns");

            migrationBuilder.DropColumn(
                name: "ApprovedAt",
                table: "TbCampaignProducts");

            migrationBuilder.DropColumn(
                name: "ApprovedByUserId",
                table: "TbCampaignProducts");

            migrationBuilder.DropColumn(
                name: "DiscountPercentage",
                table: "TbCampaignProducts");

            migrationBuilder.DropColumn(
                name: "OriginalPrice",
                table: "TbCampaignProducts");

            migrationBuilder.DropColumn(
                name: "PlatformContribution",
                table: "TbCampaignProducts");

            migrationBuilder.DropColumn(
                name: "SoldQuantity",
                table: "TbCampaignProducts");

            migrationBuilder.DropColumn(
                name: "VendorContribution",
                table: "TbCampaignProducts");

            migrationBuilder.DropColumn(
                name: "VendorId",
                table: "TbCampaignProducts");

            migrationBuilder.DropColumn(
                name: "BadgeColor",
                table: "TbBlockProducts");

            migrationBuilder.DropColumn(
                name: "BadgeText",
                table: "TbBlockProducts");

            migrationBuilder.DropColumn(
                name: "FeaturedFrom",
                table: "TbBlockProducts");

            migrationBuilder.DropColumn(
                name: "FeaturedTo",
                table: "TbBlockProducts");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "TbBlockProducts");

            migrationBuilder.DropColumn(
                name: "IsFeatured",
                table: "TbBlockProducts");

            migrationBuilder.RenameColumn(
                name: "ViewAllLinkUrl",
                table: "TbHomepageBlocks",
                newName: "ViewAllLinkTitleEn");

            migrationBuilder.RenameColumn(
                name: "MaxItemsToDisplay",
                table: "TbHomepageBlocks",
                newName: "PersonalizationSource");

            migrationBuilder.RenameColumn(
                name: "DescriptionEn",
                table: "TbHomepageBlocks",
                newName: "SubtitleEn");

            migrationBuilder.RenameColumn(
                name: "DescriptionAr",
                table: "TbHomepageBlocks",
                newName: "SubtitleAr");

            migrationBuilder.RenameColumn(
                name: "BlockType",
                table: "TbHomepageBlocks",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "TitleEn",
                table: "TbCampaigns",
                newName: "NameEn");

            migrationBuilder.RenameColumn(
                name: "TitleAr",
                table: "TbCampaigns",
                newName: "NameAr");

            migrationBuilder.RenameColumn(
                name: "SlugEn",
                table: "TbCampaigns",
                newName: "BadgeTextEn");

            migrationBuilder.RenameColumn(
                name: "SlugAr",
                table: "TbCampaigns",
                newName: "BadgeTextAr");

            migrationBuilder.RenameColumn(
                name: "MaxProductsPerVendor",
                table: "TbCampaigns",
                newName: "MaxQuantityPerUser");

            migrationBuilder.RenameColumn(
                name: "StockQuantity",
                table: "TbCampaignProducts",
                newName: "StockLimit");

            migrationBuilder.AlterColumn<bool>(
                name: "ShowViewAllLink",
                table: "TbHomepageBlocks",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CampaignId",
                table: "TbHomepageBlocks",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DynamicSource",
                table: "TbHomepageBlocks",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Layout",
                table: "TbHomepageBlocks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ViewAllLinkTitleAr",
                table: "TbHomepageBlocks",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "VisibleFrom",
                table: "TbHomepageBlocks",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "VisibleTo",
                table: "TbHomepageBlocks",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PricingSystemType",
                table: "TbCategories",
                type: "int",
                nullable: false,
                defaultValue: 1,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "BadgeColor",
                table: "TbCampaigns",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FlashSaleEndTime",
                table: "TbCampaigns",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsFlashSale",
                table: "TbCampaigns",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "SoldCount",
                table: "TbCampaignProducts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "TbBlockCategories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    HomepageBlockId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbBlockCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbBlockCategories_TbCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "TbCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TbBlockCategories_TbHomepageBlocks_HomepageBlockId",
                        column: x => x.HomepageBlockId,
                        principalTable: "TbHomepageBlocks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TbUserItemViews",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ViewedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ViewDurationSeconds = table.Column<int>(type: "int", nullable: false),
                    SourceBlockType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ReferrerUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbUserItemViews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbUserItemViews_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TbUserItemViews_TbItems_ItemId",
                        column: x => x.ItemId,
                        principalTable: "TbItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "TbPricingSystemSettings",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "SystemType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "TbPricingSystemSettings",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "SystemType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "TbPricingSystemSettings",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "SystemType",
                value: 3);

            migrationBuilder.UpdateData(
                table: "TbPricingSystemSettings",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                column: "SystemType",
                value: 4);

            migrationBuilder.UpdateData(
                table: "TbPricingSystemSettings",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                column: "SystemType",
                value: 5);

            migrationBuilder.CreateIndex(
                name: "IX_TbHomepageBlocks_CampaignId",
                table: "TbHomepageBlocks",
                column: "CampaignId");

            migrationBuilder.CreateIndex(
                name: "IX_TbHomepageBlocks_IsVisible_DisplayOrder",
                table: "TbHomepageBlocks",
                columns: new[] { "IsVisible", "DisplayOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_TbBlockCategories_CategoryId",
                table: "TbBlockCategories",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_TbBlockCategories_HomepageBlockId",
                table: "TbBlockCategories",
                column: "HomepageBlockId");

            migrationBuilder.CreateIndex(
                name: "IX_TbBlockCategories_IsDeleted",
                table: "TbBlockCategories",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbUserItemViews_IsDeleted",
                table: "TbUserItemViews",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbUserItemViews_ItemId",
                table: "TbUserItemViews",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_TbUserItemViews_UserId",
                table: "TbUserItemViews",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TbHomepageBlocks_TbCampaigns_CampaignId",
                table: "TbHomepageBlocks",
                column: "CampaignId",
                principalTable: "TbCampaigns",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TbHomepageBlocks_TbCampaigns_CampaignId",
                table: "TbHomepageBlocks");

            migrationBuilder.DropTable(
                name: "TbBlockCategories");

            migrationBuilder.DropTable(
                name: "TbUserItemViews");

            migrationBuilder.DropIndex(
                name: "IX_TbHomepageBlocks_CampaignId",
                table: "TbHomepageBlocks");

            migrationBuilder.DropIndex(
                name: "IX_TbHomepageBlocks_IsVisible_DisplayOrder",
                table: "TbHomepageBlocks");

            migrationBuilder.DropColumn(
                name: "CampaignId",
                table: "TbHomepageBlocks");

            migrationBuilder.DropColumn(
                name: "DynamicSource",
                table: "TbHomepageBlocks");

            migrationBuilder.DropColumn(
                name: "Layout",
                table: "TbHomepageBlocks");

            migrationBuilder.DropColumn(
                name: "ViewAllLinkTitleAr",
                table: "TbHomepageBlocks");

            migrationBuilder.DropColumn(
                name: "VisibleFrom",
                table: "TbHomepageBlocks");

            migrationBuilder.DropColumn(
                name: "VisibleTo",
                table: "TbHomepageBlocks");

            migrationBuilder.DropColumn(
                name: "BadgeColor",
                table: "TbCampaigns");

            migrationBuilder.DropColumn(
                name: "FlashSaleEndTime",
                table: "TbCampaigns");

            migrationBuilder.DropColumn(
                name: "IsFlashSale",
                table: "TbCampaigns");

            migrationBuilder.DropColumn(
                name: "SoldCount",
                table: "TbCampaignProducts");

            migrationBuilder.RenameColumn(
                name: "ViewAllLinkTitleEn",
                table: "TbHomepageBlocks",
                newName: "ViewAllLinkUrl");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "TbHomepageBlocks",
                newName: "BlockType");

            migrationBuilder.RenameColumn(
                name: "SubtitleEn",
                table: "TbHomepageBlocks",
                newName: "DescriptionEn");

            migrationBuilder.RenameColumn(
                name: "SubtitleAr",
                table: "TbHomepageBlocks",
                newName: "DescriptionAr");

            migrationBuilder.RenameColumn(
                name: "PersonalizationSource",
                table: "TbHomepageBlocks",
                newName: "MaxItemsToDisplay");

            migrationBuilder.RenameColumn(
                name: "NameEn",
                table: "TbCampaigns",
                newName: "TitleEn");

            migrationBuilder.RenameColumn(
                name: "NameAr",
                table: "TbCampaigns",
                newName: "TitleAr");

            migrationBuilder.RenameColumn(
                name: "MaxQuantityPerUser",
                table: "TbCampaigns",
                newName: "MaxProductsPerVendor");

            migrationBuilder.RenameColumn(
                name: "BadgeTextEn",
                table: "TbCampaigns",
                newName: "SlugEn");

            migrationBuilder.RenameColumn(
                name: "BadgeTextAr",
                table: "TbCampaigns",
                newName: "SlugAr");

            migrationBuilder.RenameColumn(
                name: "StockLimit",
                table: "TbCampaignProducts",
                newName: "StockQuantity");

            migrationBuilder.AlterColumn<bool>(
                name: "ShowViewAllLink",
                table: "TbHomepageBlocks",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddColumn<DateTime>(
                name: "ActiveFrom",
                table: "TbHomepageBlocks",
                type: "datetime2(2)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ActiveTo",
                table: "TbHomepageBlocks",
                type: "datetime2(2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BackgroundColor",
                table: "TbHomepageBlocks",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BackgroundImagePath",
                table: "TbHomepageBlocks",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CssClass",
                table: "TbHomepageBlocks",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "TbHomepageBlocks",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<string>(
                name: "TextColor",
                table: "TbHomepageBlocks",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PricingSystemType",
                table: "TbCategories",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 1);

            migrationBuilder.AddColumn<DateTime>(
                name: "AppliedAt",
                table: "TbCampaignVendors",
                type: "datetime2(2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApprovedByUserId",
                table: "TbCampaignVendors",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalCommissionPaid",
                table: "TbCampaignVendors",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "TotalProductsApproved",
                table: "TbCampaignVendors",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalProductsSubmitted",
                table: "TbCampaignVendors",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalSales",
                table: "TbCampaignVendors",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "BannerImagePath",
                table: "TbCampaigns",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "BudgetLimit",
                table: "TbCampaigns",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CampaignType",
                table: "TbCampaigns",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionAr",
                table: "TbCampaigns",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionEn",
                table: "TbCampaigns",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DisplayOrder",
                table: "TbCampaigns",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FundingModel",
                table: "TbCampaigns",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsFeatured",
                table: "TbCampaigns",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "MaxParticipatingProducts",
                table: "TbCampaigns",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "MinimumDiscountPercentage",
                table: "TbCampaigns",
                type: "decimal(5,2)",
                nullable: false,
                defaultValue: 20m);

            migrationBuilder.AddColumn<decimal>(
                name: "MinimumOrderValue",
                table: "TbCampaigns",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PlatformFundingPercentage",
                table: "TbCampaigns",
                type: "decimal(5,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "SellerFundingPercentage",
                table: "TbCampaigns",
                type: "decimal(5,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "TbCampaigns",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ThemeColor",
                table: "TbCampaigns",
                type: "nvarchar(7)",
                maxLength: 7,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ThumbnailImagePath",
                table: "TbCampaigns",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalSpent",
                table: "TbCampaigns",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "ApprovedAt",
                table: "TbCampaignProducts",
                type: "datetime2(2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApprovedByUserId",
                table: "TbCampaignProducts",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "DiscountPercentage",
                table: "TbCampaignProducts",
                type: "decimal(5,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "OriginalPrice",
                table: "TbCampaignProducts",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PlatformContribution",
                table: "TbCampaignProducts",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SoldQuantity",
                table: "TbCampaignProducts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "VendorContribution",
                table: "TbCampaignProducts",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "VendorId",
                table: "TbCampaignProducts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "BadgeColor",
                table: "TbBlockProducts",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BadgeText",
                table: "TbBlockProducts",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FeaturedFrom",
                table: "TbBlockProducts",
                type: "datetime2(2)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FeaturedTo",
                table: "TbBlockProducts",
                type: "datetime2(2)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "TbBlockProducts",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsFeatured",
                table: "TbBlockProducts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "TbFlashSales",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    BannerImagePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    DescriptionAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    DurationInHours = table.Column<int>(type: "int", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2(2)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    MaxProducts = table.Column<int>(type: "int", nullable: true),
                    MinimumDiscountPercentage = table.Column<decimal>(type: "decimal(5,2)", nullable: false, defaultValue: 20m),
                    MinimumSellerRating = table.Column<decimal>(type: "decimal(3,2)", nullable: false, defaultValue: 4.0m),
                    ShowCountdownTimer = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    StartDate = table.Column<DateTime>(type: "datetime2(2)", nullable: false),
                    ThemeColor = table.Column<string>(type: "nvarchar(7)", maxLength: 7, nullable: true),
                    TitleAr = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    TitleEn = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    TotalSales = table.Column<int>(type: "int", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbFlashSales", x => x.Id);
                    table.CheckConstraint("CK_FlashSale_DurationInHours", "[DurationInHours] >= 6 AND [DurationInHours] <= 48");
                });

            migrationBuilder.CreateTable(
                name: "TbFlashSaleProducts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    FlashSaleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VendorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AddToCartCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    AvailableQuantity = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    DiscountPercentage = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    FlashSalePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    OriginalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SoldQuantity = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true),
                    ViewCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbFlashSaleProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbFlashSaleProducts_TbFlashSales_FlashSaleId",
                        column: x => x.FlashSaleId,
                        principalTable: "TbFlashSales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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

            migrationBuilder.UpdateData(
                table: "TbPricingSystemSettings",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "SystemType",
                value: 0);

            migrationBuilder.UpdateData(
                table: "TbPricingSystemSettings",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "SystemType",
                value: 1);

            migrationBuilder.UpdateData(
                table: "TbPricingSystemSettings",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "SystemType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "TbPricingSystemSettings",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                column: "SystemType",
                value: 3);

            migrationBuilder.UpdateData(
                table: "TbPricingSystemSettings",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                column: "SystemType",
                value: 4);

            migrationBuilder.CreateIndex(
                name: "IX_TbHomepageBlocks_ActiveFrom_ActiveTo",
                table: "TbHomepageBlocks",
                columns: new[] { "ActiveFrom", "ActiveTo" });

            migrationBuilder.CreateIndex(
                name: "IX_TbHomepageBlocks_BlockType",
                table: "TbHomepageBlocks",
                column: "BlockType");

            migrationBuilder.CreateIndex(
                name: "IX_TbHomepageBlocks_IsActive",
                table: "TbHomepageBlocks",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_TbHomepageBlocks_IsActive_DisplayOrder",
                table: "TbHomepageBlocks",
                columns: new[] { "IsActive", "DisplayOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_TbCampaignVendors_ApprovedByUserId",
                table: "TbCampaignVendors",
                column: "ApprovedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TbCampaigns_CampaignType",
                table: "TbCampaigns",
                column: "CampaignType");

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
                name: "IX_TbCampaignProducts_ApprovedByUserId",
                table: "TbCampaignProducts",
                column: "ApprovedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TbCampaignProducts_VendorId",
                table: "TbCampaignProducts",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_TbBlockProducts_IsActive",
                table: "TbBlockProducts",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_TbBlockProducts_IsFeatured",
                table: "TbBlockProducts",
                column: "IsFeatured");

            migrationBuilder.CreateIndex(
                name: "IX_TbFlashSaleProducts_DisplayOrder",
                table: "TbFlashSaleProducts",
                column: "DisplayOrder");

            migrationBuilder.CreateIndex(
                name: "IX_TbFlashSaleProducts_FlashSaleId",
                table: "TbFlashSaleProducts",
                column: "FlashSaleId");

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
                name: "IX_TbFlashSaleProducts_IsDeleted",
                table: "TbFlashSaleProducts",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbFlashSaleProducts_ItemId",
                table: "TbFlashSaleProducts",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_TbFlashSaleProducts_VendorId",
                table: "TbFlashSaleProducts",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_TbFlashSales_EndDate",
                table: "TbFlashSales",
                column: "EndDate");

            migrationBuilder.CreateIndex(
                name: "IX_TbFlashSales_IsActive",
                table: "TbFlashSales",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_TbFlashSales_IsDeleted",
                table: "TbFlashSales",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbFlashSales_StartDate",
                table: "TbFlashSales",
                column: "StartDate");

            migrationBuilder.CreateIndex(
                name: "IX_TbFlashSales_StartDate_EndDate_IsActive",
                table: "TbFlashSales",
                columns: new[] { "StartDate", "EndDate", "IsActive" });

            migrationBuilder.AddForeignKey(
                name: "FK_TbCampaignProducts_AspNetUsers_ApprovedByUserId",
                table: "TbCampaignProducts",
                column: "ApprovedByUserId",
                principalTable: "AspNetUsers",
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
        }
    }
}
