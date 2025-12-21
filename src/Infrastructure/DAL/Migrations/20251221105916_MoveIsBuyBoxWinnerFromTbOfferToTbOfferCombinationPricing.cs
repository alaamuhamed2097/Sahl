using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class MoveIsBuyBoxWinnerFromTbOfferToTbOfferCombinationPricing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("delete from TbBuyBoxCalculations");
            migrationBuilder.DropForeignKey(
                name: "FK_TbBuyBoxCalculations_TbOffers_TbOfferId",
                table: "TbBuyBoxCalculations");

            migrationBuilder.DropForeignKey(
                name: "FK_TbBuyBoxCalculations_TbOffers_WinningOfferId",
                table: "TbBuyBoxCalculations");

            migrationBuilder.DropIndex(
                name: "IX_TbOffers_BuyBoxWinner_Filtered_NC",
                table: "TbOffers");

            migrationBuilder.DropIndex(
                name: "IX_TbOffers_ItemId_Visibility_Deleted_Covering_NC",
                table: "TbOffers");

            migrationBuilder.DropIndex(
                name: "IX_TbOfferPricing_OfferId_Deleted_Covering_NC",
                table: "TbOfferCombinationPricings");

            migrationBuilder.DropIndex(
                name: "IX_TbBuyBoxCalculations_TbOfferId",
                table: "TbBuyBoxCalculations");

            migrationBuilder.DropColumn(
                name: "IsBuyBoxWinner",
                table: "TbOffers");

            migrationBuilder.DropColumn(
                name: "TbOfferId",
                table: "TbBuyBoxCalculations");

            migrationBuilder.RenameColumn(
                name: "WinningOfferId",
                table: "TbBuyBoxCalculations",
                newName: "WinningOfferCombinationId");

            migrationBuilder.RenameIndex(
                name: "IX_TbBuyBoxCalculations_WinningOfferId",
                table: "TbBuyBoxCalculations",
                newName: "IX_TbBuyBoxCalculations_WinningOfferCombinationId");

            migrationBuilder.AddColumn<bool>(
                name: "IsBuyBoxWinner",
                table: "TbOfferCombinationPricings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_TbOffers_ItemId_Visibility_Deleted_Covering_NC",
                table: "TbOffers",
                columns: new[] { "ItemId", "VisibilityScope", "IsDeleted" })
                .Annotation("SqlServer:Include", new[] { "VendorId", "HandlingTimeInDays", "FulfillmentType" });

            migrationBuilder.CreateIndex(
                name: "IX_TbOfferPricing_BuyBoxWinner_Filtered_NC",
                table: "TbOfferCombinationPricings",
                column: "IsBuyBoxWinner",
                filter: "[IsBuyBoxWinner] = 1");

            migrationBuilder.CreateIndex(
                name: "IX_TbOfferPricing_OfferId_Deleted_Covering_NC",
                table: "TbOfferCombinationPricings",
                columns: new[] { "OfferId", "IsDeleted" })
                .Annotation("SqlServer:Include", new[] { "SalesPrice", "IsBuyBoxWinner", "Price", "AvailableQuantity", "StockStatus" });

            migrationBuilder.AddForeignKey(
                name: "FK_TbBuyBoxCalculations_TbOfferCombinationPricings_WinningOfferCombinationId",
                table: "TbBuyBoxCalculations",
                column: "WinningOfferCombinationId",
                principalTable: "TbOfferCombinationPricings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TbBuyBoxCalculations_TbOfferCombinationPricings_WinningOfferCombinationId",
                table: "TbBuyBoxCalculations");

            migrationBuilder.DropIndex(
                name: "IX_TbOffers_ItemId_Visibility_Deleted_Covering_NC",
                table: "TbOffers");

            migrationBuilder.DropIndex(
                name: "IX_TbOfferPricing_BuyBoxWinner_Filtered_NC",
                table: "TbOfferCombinationPricings");

            migrationBuilder.DropIndex(
                name: "IX_TbOfferPricing_OfferId_Deleted_Covering_NC",
                table: "TbOfferCombinationPricings");

            migrationBuilder.DropColumn(
                name: "IsBuyBoxWinner",
                table: "TbOfferCombinationPricings");

            migrationBuilder.RenameColumn(
                name: "WinningOfferCombinationId",
                table: "TbBuyBoxCalculations",
                newName: "WinningOfferId");

            migrationBuilder.RenameIndex(
                name: "IX_TbBuyBoxCalculations_WinningOfferCombinationId",
                table: "TbBuyBoxCalculations",
                newName: "IX_TbBuyBoxCalculations_WinningOfferId");

            migrationBuilder.AddColumn<bool>(
                name: "IsBuyBoxWinner",
                table: "TbOffers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "TbOfferId",
                table: "TbBuyBoxCalculations",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TbOffers_BuyBoxWinner_Filtered_NC",
                table: "TbOffers",
                column: "IsBuyBoxWinner",
                filter: "[IsBuyBoxWinner] = 1");

            migrationBuilder.CreateIndex(
                name: "IX_TbOffers_ItemId_Visibility_Deleted_Covering_NC",
                table: "TbOffers",
                columns: new[] { "ItemId", "VisibilityScope", "IsDeleted" })
                .Annotation("SqlServer:Include", new[] { "VendorId", "IsBuyBoxWinner", "HandlingTimeInDays", "FulfillmentType" });

            migrationBuilder.CreateIndex(
                name: "IX_TbOfferPricing_OfferId_Deleted_Covering_NC",
                table: "TbOfferCombinationPricings",
                columns: new[] { "OfferId", "IsDeleted" })
                .Annotation("SqlServer:Include", new[] { "SalesPrice", "Price", "AvailableQuantity", "StockStatus" });

            migrationBuilder.CreateIndex(
                name: "IX_TbBuyBoxCalculations_TbOfferId",
                table: "TbBuyBoxCalculations",
                column: "TbOfferId");

            migrationBuilder.AddForeignKey(
                name: "FK_TbBuyBoxCalculations_TbOffers_TbOfferId",
                table: "TbBuyBoxCalculations",
                column: "TbOfferId",
                principalTable: "TbOffers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TbBuyBoxCalculations_TbOffers_WinningOfferId",
                table: "TbBuyBoxCalculations",
                column: "WinningOfferId",
                principalTable: "TbOffers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
