using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class replaceOfferUserIdWithVendorId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TbOffers_AspNetUsers_UserId",
                table: "TbOffers");

            migrationBuilder.DropIndex(
                name: "IX_TbOffers_UserId",
                table: "TbOffers");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "TbOffers");

            migrationBuilder.AddColumn<Guid>(
                name: "VendorId",
                table: "TbOffers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("A95C3AC2-CD41-4832-A951-51443F5FD18B"));

            migrationBuilder.CreateIndex(
                name: "IX_TbOffers_VendorId",
                table: "TbOffers",
                column: "VendorId");

            migrationBuilder.AddForeignKey(
                name: "FK_TbOffers_TbVendors_VendorId",
                table: "TbOffers",
                column: "VendorId",
                principalTable: "TbVendors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TbOffers_TbVendors_VendorId",
                table: "TbOffers");

            migrationBuilder.DropIndex(
                name: "IX_TbOffers_VendorId",
                table: "TbOffers");

            migrationBuilder.DropColumn(
                name: "VendorId",
                table: "TbOffers");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "TbOffers",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_TbOffers_UserId",
                table: "TbOffers",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TbOffers_AspNetUsers_UserId",
                table: "TbOffers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
