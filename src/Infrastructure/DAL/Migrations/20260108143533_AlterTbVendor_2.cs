using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class AlterTbVendor_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "TbVendors");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "TbVendors");

            migrationBuilder.DropColumn(
                name: "MiddleName",
                table: "TbVendors");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "TbVendors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "TbVendors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MiddleName",
                table: "TbVendors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
