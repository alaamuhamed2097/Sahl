using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateItemCombinationPricing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TbItemAttributeCombinationPricings_FinalPrice",
                table: "TbItemAttributeCombinationPricings");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "TbItemAttributeCombinationPricings");

            migrationBuilder.RenameColumn(
                name: "FinalPrice",
                table: "TbItemAttributeCombinationPricings",
                newName: "SalesPrice");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "TbItemAttributeCombinationPricings",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_TbItemAttributeCombinationPricings_Price",
                table: "TbItemAttributeCombinationPricings",
                column: "Price");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TbItemAttributeCombinationPricings_Price",
                table: "TbItemAttributeCombinationPricings");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "TbItemAttributeCombinationPricings");

            migrationBuilder.RenameColumn(
                name: "SalesPrice",
                table: "TbItemAttributeCombinationPricings",
                newName: "FinalPrice");

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "TbItemAttributeCombinationPricings",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TbItemAttributeCombinationPricings_FinalPrice",
                table: "TbItemAttributeCombinationPricings",
                column: "FinalPrice");
        }
    }
}
