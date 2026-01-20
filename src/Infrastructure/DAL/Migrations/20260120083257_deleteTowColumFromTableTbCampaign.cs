using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class deleteTowColumFromTableTbCampaign : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SoldCount",
                table: "TbCampaignProducts");

            migrationBuilder.DropColumn(
                name: "StockLimit",
                table: "TbCampaignProducts");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SoldCount",
                table: "TbCampaignProducts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StockLimit",
                table: "TbCampaignProducts",
                type: "int",
                nullable: true);
        }
    }
}
