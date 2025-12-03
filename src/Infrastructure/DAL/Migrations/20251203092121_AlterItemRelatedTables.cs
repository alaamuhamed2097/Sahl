using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class AlterItemRelatedTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TbOrderPayments_TbOrders_OrderId",
                table: "TbOrderPayments");

            migrationBuilder.AddColumn<Guid>(
                name: "PricingSystemId",
                table: "TbCategories",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("11111111-1111-1111-1111-111111111111"));

            migrationBuilder.AddColumn<int>(
                name: "PriceModifierCategory",
                table: "TbAttributeValuePriceModifiers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "TbCombinationAttributesValueId",
                table: "TbAttributeValuePriceModifiers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TbItemCombinations_Barcode",
                table: "TbItemCombinations",
                column: "Barcode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TbItemCombinations_SKU",
                table: "TbItemCombinations",
                column: "SKU",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TbCategories_PricingSystemId",
                table: "TbCategories",
                column: "PricingSystemId");

            migrationBuilder.CreateIndex(
                name: "IX_TbAttributeValuePriceModifiers_TbCombinationAttributesValueId",
                table: "TbAttributeValuePriceModifiers",
                column: "TbCombinationAttributesValueId");

            migrationBuilder.AddForeignKey(
                name: "FK_TbAttributeValuePriceModifiers_TbCombinationAttributesValues_TbCombinationAttributesValueId",
                table: "TbAttributeValuePriceModifiers",
                column: "TbCombinationAttributesValueId",
                principalTable: "TbCombinationAttributesValues",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TbCategories_TbPricingSystemSettings_PricingSystemId",
                table: "TbCategories",
                column: "PricingSystemId",
                principalTable: "TbPricingSystemSettings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbOrderPayments_TbOrders_OrderId",
                table: "TbOrderPayments",
                column: "OrderId",
                principalTable: "TbOrders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TbAttributeValuePriceModifiers_TbCombinationAttributesValues_TbCombinationAttributesValueId",
                table: "TbAttributeValuePriceModifiers");

            migrationBuilder.DropForeignKey(
                name: "FK_TbCategories_TbPricingSystemSettings_PricingSystemId",
                table: "TbCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_TbOrderPayments_TbOrders_OrderId",
                table: "TbOrderPayments");

            migrationBuilder.DropIndex(
                name: "IX_TbItemCombinations_Barcode",
                table: "TbItemCombinations");

            migrationBuilder.DropIndex(
                name: "IX_TbItemCombinations_SKU",
                table: "TbItemCombinations");

            migrationBuilder.DropIndex(
                name: "IX_TbCategories_PricingSystemId",
                table: "TbCategories");

            migrationBuilder.DropIndex(
                name: "IX_TbAttributeValuePriceModifiers_TbCombinationAttributesValueId",
                table: "TbAttributeValuePriceModifiers");

            migrationBuilder.DropColumn(
                name: "PricingSystemId",
                table: "TbCategories");

            migrationBuilder.DropColumn(
                name: "PriceModifierCategory",
                table: "TbAttributeValuePriceModifiers");

            migrationBuilder.DropColumn(
                name: "TbCombinationAttributesValueId",
                table: "TbAttributeValuePriceModifiers");

            migrationBuilder.AddForeignKey(
                name: "FK_TbOrderPayments_TbOrders_OrderId",
                table: "TbOrderPayments",
                column: "OrderId",
                principalTable: "TbOrders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
