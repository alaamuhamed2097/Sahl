using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class AlterTbItemCombinationAndTbOfferCombinationPricing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TbItemCombinations_Barcode",
                table: "TbItemCombinations");

            migrationBuilder.DropIndex(
                name: "IX_TbItemCombinations_SKU",
                table: "TbItemCombinations");

            migrationBuilder.DropColumn(
                name: "Barcode",
                table: "TbItemCombinations");

            migrationBuilder.DropColumn(
                name: "SKU",
                table: "TbItemCombinations");

            migrationBuilder.AddColumn<string>(
                name: "Barcode",
                table: "TbOfferCombinationPricings",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SKU",
                table: "TbOfferCombinationPricings",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");
            // ✅ Populate unique values BEFORE creating indexes
            migrationBuilder.Sql(@"
                UPDATE TbOfferCombinationPricings
                SET 
                    SKU = CONCAT('SKU-', REPLACE(CONVERT(varchar(36), Id), '-', '')),
                    Barcode = CONCAT('BAR-', REPLACE(CONVERT(varchar(36), Id), '-', ''))
                WHERE 
                    SKU = '' OR Barcode = '';
            ");

            migrationBuilder.CreateIndex(
                name: "IX_TbOfferCombinationPricings_Barcode",
                table: "TbOfferCombinationPricings",
                column: "Barcode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TbOfferCombinationPricings_SKU",
                table: "TbOfferCombinationPricings",
                column: "SKU",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TbOfferCombinationPricings_Barcode",
                table: "TbOfferCombinationPricings");

            migrationBuilder.DropIndex(
                name: "IX_TbOfferCombinationPricings_SKU",
                table: "TbOfferCombinationPricings");

            migrationBuilder.DropColumn(
                name: "Barcode",
                table: "TbOfferCombinationPricings");

            migrationBuilder.DropColumn(
                name: "SKU",
                table: "TbOfferCombinationPricings");

            migrationBuilder.AddColumn<string>(
                name: "Barcode",
                table: "TbItemCombinations",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SKU",
                table: "TbItemCombinations",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

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
        }
    }
}
