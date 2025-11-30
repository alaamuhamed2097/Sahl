using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class completePricingTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TbReviewVotes_VoteValue",
                table: "TbReviewVotes");

            migrationBuilder.DropIndex(
                name: "IX_TbProductVisibilityRules_IsVisible",
                table: "TbProductVisibilityRules");

            migrationBuilder.DropIndex(
                name: "IX_TbProductVisibilityRules_IsVisible_VisibilityStatus",
                table: "TbProductVisibilityRules");

            migrationBuilder.DropIndex(
                name: "IX_TbProductVisibilityRules_LastCheckedAt",
                table: "TbProductVisibilityRules");

            migrationBuilder.DropIndex(
                name: "IX_TbProductReviews_ReviewDate",
                table: "TbProductReviews");

            migrationBuilder.DropIndex(
                name: "IX_TbItems_IsNewArrival",
                table: "TbItems");

            migrationBuilder.DropColumn(
                name: "VoteValue",
                table: "TbReviewVotes");

            migrationBuilder.DropColumn(
                name: "WithType",
                table: "TbReviewVotes");

            migrationBuilder.DropColumn(
                name: "AllSellersActive",
                table: "TbProductVisibilityRules");

            migrationBuilder.DropColumn(
                name: "HasValidCategory",
                table: "TbProductVisibilityRules");

            migrationBuilder.DropColumn(
                name: "IsVisible",
                table: "TbProductVisibilityRules");

            migrationBuilder.DropColumn(
                name: "LastCheckedAt",
                table: "TbProductVisibilityRules");

            migrationBuilder.DropColumn(
                name: "ApprovedDate",
                table: "TbProductReviews");

            migrationBuilder.DropColumn(
                name: "Images",
                table: "TbProductReviews");

            migrationBuilder.DropColumn(
                name: "ModerationNotes",
                table: "TbProductReviews");

            migrationBuilder.DropColumn(
                name: "ReviewDate",
                table: "TbProductReviews");

            migrationBuilder.DropColumn(
                name: "VendorResponse",
                table: "TbProductReviews");

            migrationBuilder.DropColumn(
                name: "VendorResponseDate",
                table: "TbProductReviews");

            migrationBuilder.DropColumn(
                name: "Videos",
                table: "TbProductReviews");

            migrationBuilder.DropColumn(
                name: "IsNewArrival",
                table: "TbItems");

            migrationBuilder.DropColumn(
                name: "SubCategoryId",
                table: "TbItems");

            migrationBuilder.AlterColumn<int>(
                name: "VoteType",
                table: "TbReviewVotes",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "TbProductReviews",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<decimal>(
                name: "Rating",
                table: "TbProductReviews",
                type: "decimal(2,1)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(2,1)",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsEdited",
                table: "TbProductReviews",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "CostPrice",
                table: "TbOfferCombinationPricings",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "TbOfferCombinationPricings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastPriceUpdate",
                table: "TbOfferCombinationPricings",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastStockUpdate",
                table: "TbOfferCombinationPricings",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LowStockThreshold",
                table: "TbOfferCombinationPricings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MaxOrderQuantity",
                table: "TbOfferCombinationPricings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MinOrderQuantity",
                table: "TbOfferCombinationPricings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "SellerSKU",
                table: "TbOfferCombinationPricings",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "VideoUrl",
                table: "TbItems",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<Guid>(
                name: "UnitId",
                table: "TbItems",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "BrandId",
                table: "TbItems",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Barcode",
                table: "TbItemAttributeCombinationPricings",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "BasePrice",
                table: "TbItemAttributeCombinationPricings",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "CostPrice",
                table: "TbItemAttributeCombinationPricings",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "TbItemAttributeCombinationPricings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "ItemCombinationId",
                table: "TbItemAttributeCombinationPricings",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastPriceUpdate",
                table: "TbItemAttributeCombinationPricings",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SKU",
                table: "TbItemAttributeCombinationPricings",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "SuggestedPrice",
                table: "TbItemAttributeCombinationPricings",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "AffectsStock",
                table: "TbCategoryAttributes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVariant",
                table: "TbCategoryAttributes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "SortOrder",
                table: "TbCategoryAttributes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "ItemCombinationId",
                table: "TbBuyBoxCalculations",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<decimal>(
                name: "Score",
                table: "TbBuyBoxCalculations",
                type: "decimal(18,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "ScoreBreakdown",
                table: "TbBuyBoxCalculations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TbReviewReports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    ReportID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReviewID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Details = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbReviewReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbReviewReports_TbProductReviews_ReviewID",
                        column: x => x.ReviewID,
                        principalTable: "TbProductReviews",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TbItemAttributeCombinationPricings_ItemCombinationId",
                table: "TbItemAttributeCombinationPricings",
                column: "ItemCombinationId");

            migrationBuilder.CreateIndex(
                name: "IX_TbBuyBoxCalculations_ItemCombinationId",
                table: "TbBuyBoxCalculations",
                column: "ItemCombinationId");

            migrationBuilder.CreateIndex(
                name: "IX_TbReviewReports_CurrentState",
                table: "TbReviewReports",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbReviewReports_CustomerID",
                table: "TbReviewReports",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_TbReviewReports_ReportID",
                table: "TbReviewReports",
                column: "ReportID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TbReviewReports_ReviewID",
                table: "TbReviewReports",
                column: "ReviewID");

            migrationBuilder.CreateIndex(
                name: "IX_TbReviewReports_Status",
                table: "TbReviewReports",
                column: "Status");

            migrationBuilder.AddForeignKey(
                name: "FK_TbBuyBoxCalculations_TbItemCombinations_ItemCombinationId",
                table: "TbBuyBoxCalculations",
                column: "ItemCombinationId",
                principalTable: "TbItemCombinations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbItemAttributeCombinationPricings_TbItemCombinations_ItemCombinationId",
                table: "TbItemAttributeCombinationPricings",
                column: "ItemCombinationId",
                principalTable: "TbItemCombinations",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TbBuyBoxCalculations_TbItemCombinations_ItemCombinationId",
                table: "TbBuyBoxCalculations");

            migrationBuilder.DropForeignKey(
                name: "FK_TbItemAttributeCombinationPricings_TbItemCombinations_ItemCombinationId",
                table: "TbItemAttributeCombinationPricings");

            migrationBuilder.DropTable(
                name: "TbReviewReports");

            migrationBuilder.DropIndex(
                name: "IX_TbItemAttributeCombinationPricings_ItemCombinationId",
                table: "TbItemAttributeCombinationPricings");

            migrationBuilder.DropIndex(
                name: "IX_TbBuyBoxCalculations_ItemCombinationId",
                table: "TbBuyBoxCalculations");

            migrationBuilder.DropColumn(
                name: "IsEdited",
                table: "TbProductReviews");

            migrationBuilder.DropColumn(
                name: "CostPrice",
                table: "TbOfferCombinationPricings");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "TbOfferCombinationPricings");

            migrationBuilder.DropColumn(
                name: "LastPriceUpdate",
                table: "TbOfferCombinationPricings");

            migrationBuilder.DropColumn(
                name: "LastStockUpdate",
                table: "TbOfferCombinationPricings");

            migrationBuilder.DropColumn(
                name: "LowStockThreshold",
                table: "TbOfferCombinationPricings");

            migrationBuilder.DropColumn(
                name: "MaxOrderQuantity",
                table: "TbOfferCombinationPricings");

            migrationBuilder.DropColumn(
                name: "MinOrderQuantity",
                table: "TbOfferCombinationPricings");

            migrationBuilder.DropColumn(
                name: "SellerSKU",
                table: "TbOfferCombinationPricings");

            migrationBuilder.DropColumn(
                name: "Barcode",
                table: "TbItemAttributeCombinationPricings");

            migrationBuilder.DropColumn(
                name: "BasePrice",
                table: "TbItemAttributeCombinationPricings");

            migrationBuilder.DropColumn(
                name: "CostPrice",
                table: "TbItemAttributeCombinationPricings");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "TbItemAttributeCombinationPricings");

            migrationBuilder.DropColumn(
                name: "ItemCombinationId",
                table: "TbItemAttributeCombinationPricings");

            migrationBuilder.DropColumn(
                name: "LastPriceUpdate",
                table: "TbItemAttributeCombinationPricings");

            migrationBuilder.DropColumn(
                name: "SKU",
                table: "TbItemAttributeCombinationPricings");

            migrationBuilder.DropColumn(
                name: "SuggestedPrice",
                table: "TbItemAttributeCombinationPricings");

            migrationBuilder.DropColumn(
                name: "AffectsStock",
                table: "TbCategoryAttributes");

            migrationBuilder.DropColumn(
                name: "IsVariant",
                table: "TbCategoryAttributes");

            migrationBuilder.DropColumn(
                name: "SortOrder",
                table: "TbCategoryAttributes");

            migrationBuilder.DropColumn(
                name: "ItemCombinationId",
                table: "TbBuyBoxCalculations");

            migrationBuilder.DropColumn(
                name: "Score",
                table: "TbBuyBoxCalculations");

            migrationBuilder.DropColumn(
                name: "ScoreBreakdown",
                table: "TbBuyBoxCalculations");

            migrationBuilder.AlterColumn<string>(
                name: "VoteType",
                table: "TbReviewVotes",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "VoteValue",
                table: "TbReviewVotes",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "WithType",
                table: "TbReviewVotes",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "AllSellersActive",
                table: "TbProductVisibilityRules",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasValidCategory",
                table: "TbProductVisibilityRules",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVisible",
                table: "TbProductVisibilityRules",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastCheckedAt",
                table: "TbProductVisibilityRules",
                type: "datetime2(2)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "TbProductReviews",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "Rating",
                table: "TbProductReviews",
                type: "decimal(2,1)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(2,1)");

            migrationBuilder.AddColumn<DateTime>(
                name: "ApprovedDate",
                table: "TbProductReviews",
                type: "datetime2(2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Images",
                table: "TbProductReviews",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModerationNotes",
                table: "TbProductReviews",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReviewDate",
                table: "TbProductReviews",
                type: "datetime2(2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VendorResponse",
                table: "TbProductReviews",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "VendorResponseDate",
                table: "TbProductReviews",
                type: "datetime2(2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Videos",
                table: "TbProductReviews",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "VideoUrl",
                table: "TbItems",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "UnitId",
                table: "TbItems",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "BrandId",
                table: "TbItems",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<bool>(
                name: "IsNewArrival",
                table: "TbItems",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "SubCategoryId",
                table: "TbItems",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TbReviewVotes_VoteValue",
                table: "TbReviewVotes",
                column: "VoteValue");

            migrationBuilder.CreateIndex(
                name: "IX_TbProductVisibilityRules_IsVisible",
                table: "TbProductVisibilityRules",
                column: "IsVisible");

            migrationBuilder.CreateIndex(
                name: "IX_TbProductVisibilityRules_IsVisible_VisibilityStatus",
                table: "TbProductVisibilityRules",
                columns: new[] { "IsVisible", "VisibilityStatus" });

            migrationBuilder.CreateIndex(
                name: "IX_TbProductVisibilityRules_LastCheckedAt",
                table: "TbProductVisibilityRules",
                column: "LastCheckedAt");

            migrationBuilder.CreateIndex(
                name: "IX_TbProductReviews_ReviewDate",
                table: "TbProductReviews",
                column: "ReviewDate");

            migrationBuilder.CreateIndex(
                name: "IX_TbItems_IsNewArrival",
                table: "TbItems",
                column: "IsNewArrival");
        }
    }
}
