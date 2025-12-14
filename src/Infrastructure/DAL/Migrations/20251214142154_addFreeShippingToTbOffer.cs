using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class addFreeShippingToTbOffer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TbOffers_ItemId_Visibility_Deleted_Covering_NC",
                table: "TbOffers");

            migrationBuilder.DropColumn(
                name: "StorgeLocation",
                table: "TbOffers");

            migrationBuilder.AddColumn<bool>(
                name: "IsFreeShipping",
                table: "TbOffers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_TbOffers_ItemId_Visibility_Deleted_Covering_NC",
                table: "TbOffers",
                columns: new[] { "ItemId", "VisibilityScope", "IsDeleted" })
                .Annotation("SqlServer:Include", new[] { "VendorId", "IsBuyBoxWinner", "HandlingTimeInDays", "FulfillmentType" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TbOffers_ItemId_Visibility_Deleted_Covering_NC",
                table: "TbOffers");

            migrationBuilder.DropColumn(
                name: "IsFreeShipping",
                table: "TbOffers");

            migrationBuilder.AddColumn<int>(
                name: "StorgeLocation",
                table: "TbOffers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TbOffers_ItemId_Visibility_Deleted_Covering_NC",
                table: "TbOffers",
                columns: new[] { "ItemId", "VisibilityScope", "IsDeleted" })
                .Annotation("SqlServer:Include", new[] { "VendorId", "IsBuyBoxWinner", "HandlingTimeInDays", "FulfillmentType", "StorgeLocation" });
        }
    }
}
