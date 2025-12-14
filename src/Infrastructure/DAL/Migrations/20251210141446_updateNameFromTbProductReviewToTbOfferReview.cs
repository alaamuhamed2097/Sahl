using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class updateNameFromTbProductReviewToTbOfferReview : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProductID",
                table: "TbProductReviews",
                newName: "OfferID");

            migrationBuilder.RenameIndex(
                name: "IX_TbProductReviews_ProductID_CustomerID",
                table: "TbProductReviews",
                newName: "IX_TbProductReviews_OfferID_CustomerID");

            migrationBuilder.RenameIndex(
                name: "IX_TbProductReviews_ProductID",
                table: "TbProductReviews",
                newName: "IX_TbProductReviews_OfferID");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "TbVisibilityLogs",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "TbPriceHistories",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "TbCampaignVendors",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OfferID",
                table: "TbProductReviews",
                newName: "ProductID");

            migrationBuilder.RenameIndex(
                name: "IX_TbProductReviews_OfferID_CustomerID",
                table: "TbProductReviews",
                newName: "IX_TbProductReviews_ProductID_CustomerID");

            migrationBuilder.RenameIndex(
                name: "IX_TbProductReviews_OfferID",
                table: "TbProductReviews",
                newName: "IX_TbProductReviews_ProductID");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "TbVisibilityLogs",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "TbPriceHistories",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "TbCampaignVendors",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);
        }
    }
}
