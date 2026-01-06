using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class changeHandlingTimeInDaysToEstimatedDeliveryDays : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TbOffers_ItemId_Visibility_Deleted_Covering_NC",
                table: "TbOffers");

            migrationBuilder.RenameColumn(
                name: "HandlingTimeInDays",
                table: "TbOffers",
                newName: "EstimatedDeliveryDays");

            migrationBuilder.CreateIndex(
                name: "IX_TbOffers_ItemId_Visibility_Deleted_Covering_NC",
                table: "TbOffers",
                columns: new[] { "ItemId", "VisibilityScope", "IsDeleted" })
                .Annotation("SqlServer:Include", new[] { "VendorId", "EstimatedDeliveryDays", "FulfillmentType" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TbOffers_ItemId_Visibility_Deleted_Covering_NC",
                table: "TbOffers");

            migrationBuilder.RenameColumn(
                name: "EstimatedDeliveryDays",
                table: "TbOffers",
                newName: "HandlingTimeInDays");

            migrationBuilder.CreateIndex(
                name: "IX_TbOffers_ItemId_Visibility_Deleted_Covering_NC",
                table: "TbOffers",
                columns: new[] { "ItemId", "VisibilityScope", "IsDeleted" })
                .Annotation("SqlServer:Include", new[] { "VendorId", "HandlingTimeInDays", "FulfillmentType" });
        }
    }
}
