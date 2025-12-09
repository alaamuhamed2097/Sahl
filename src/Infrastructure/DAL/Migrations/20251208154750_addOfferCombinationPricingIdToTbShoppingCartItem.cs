using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class addOfferCombinationPricingIdToTbShoppingCartItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM [TbShoppingCartItems];");

            migrationBuilder.DropForeignKey(
                name: "FK_TbShoppingCartItems_TbItems_ItemId",
                table: "TbShoppingCartItems");

            migrationBuilder.DropForeignKey(
                name: "FK_TbShoppingCartItems_TbOffers_OfferId",
                table: "TbShoppingCartItems");

            migrationBuilder.RenameColumn(
                name: "OfferId",
                table: "TbShoppingCartItems",
                newName: "OfferCombinationPricingId");

            migrationBuilder.RenameIndex(
                name: "IX_TbShoppingCartItems_OfferId",
                table: "TbShoppingCartItems",
                newName: "IX_TbShoppingCartItems_OfferCombinationPricingId");

            migrationBuilder.CreateIndex(
                name: "IX_TbShoppingCartItems_ShoppingCartId_ItemId_OfferCombinationPricingId",
                table: "TbShoppingCartItems",
                columns: new[] { "ShoppingCartId", "ItemId", "OfferCombinationPricingId" });

            migrationBuilder.AddForeignKey(
                name: "FK_TbShoppingCartItems_TbItems_ItemId",
                table: "TbShoppingCartItems",
                column: "ItemId",
                principalTable: "TbItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbShoppingCartItems_TbOfferCombinationPricings_OfferCombinationPricingId",
                table: "TbShoppingCartItems",
                column: "OfferCombinationPricingId",
                principalTable: "TbOfferCombinationPricings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TbShoppingCartItems_TbItems_ItemId",
                table: "TbShoppingCartItems");

            migrationBuilder.DropForeignKey(
                name: "FK_TbShoppingCartItems_TbOfferCombinationPricings_OfferCombinationPricingId",
                table: "TbShoppingCartItems");

            migrationBuilder.DropIndex(
                name: "IX_TbShoppingCartItems_ShoppingCartId_ItemId_OfferCombinationPricingId",
                table: "TbShoppingCartItems");

            migrationBuilder.RenameColumn(
                name: "OfferCombinationPricingId",
                table: "TbShoppingCartItems",
                newName: "OfferId");

            migrationBuilder.RenameIndex(
                name: "IX_TbShoppingCartItems_OfferCombinationPricingId",
                table: "TbShoppingCartItems",
                newName: "IX_TbShoppingCartItems_OfferId");

            migrationBuilder.AddForeignKey(
                name: "FK_TbShoppingCartItems_TbItems_ItemId",
                table: "TbShoppingCartItems",
                column: "ItemId",
                principalTable: "TbItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbShoppingCartItems_TbOffers_OfferId",
                table: "TbShoppingCartItems",
                column: "OfferId",
                principalTable: "TbOffers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
