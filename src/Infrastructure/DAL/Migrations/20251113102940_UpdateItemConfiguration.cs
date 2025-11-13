using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateItemConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TbItems_Price",
                table: "TbItems");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "TbItems");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "TbItems");

            migrationBuilder.AddColumn<bool>(
                name: "IsDefault",
                table: "TbItemAttributeCombinationPricings",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDefault",
                table: "TbItemAttributeCombinationPricings");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "TbItems",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "TbItems",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TbItems_Price",
                table: "TbItems",
                column: "Price");
        }
    }
}
