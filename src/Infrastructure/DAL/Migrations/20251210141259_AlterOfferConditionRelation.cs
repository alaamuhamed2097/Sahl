using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class AlterOfferConditionRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TbOffers_TbOfferConditions_OfferConditionId",
                table: "TbOffers");

            migrationBuilder.DropIndex(
                name: "IX_TbOffers_OfferConditionId",
                table: "TbOffers");

            migrationBuilder.DropColumn(
                name: "OfferConditionId",
                table: "TbOffers");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "TbVendors",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "OfferConditionId",
                table: "TbOfferCombinationPricings",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_TbVendors_UserId",
                table: "TbVendors",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TbOfferCombinationPricings_OfferConditionId",
                table: "TbOfferCombinationPricings",
                column: "OfferConditionId");

            migrationBuilder.Sql(@"delete from TbOfferCombinationPricings");

            migrationBuilder.AddForeignKey(
                name: "FK_TbOfferCombinationPricings_TbOfferConditions_OfferConditionId",
                table: "TbOfferCombinationPricings",
                column: "OfferConditionId",
                principalTable: "TbOfferConditions",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_TbVendors_AspNetUsers_UserId",
                table: "TbVendors",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TbOfferCombinationPricings_TbOfferConditions_OfferConditionId",
                table: "TbOfferCombinationPricings");

            migrationBuilder.DropForeignKey(
                name: "FK_TbVendors_AspNetUsers_UserId",
                table: "TbVendors");

            migrationBuilder.DropIndex(
                name: "IX_TbVendors_UserId",
                table: "TbVendors");

            migrationBuilder.DropIndex(
                name: "IX_TbOfferCombinationPricings_OfferConditionId",
                table: "TbOfferCombinationPricings");

            migrationBuilder.DropColumn(
                name: "OfferConditionId",
                table: "TbOfferCombinationPricings");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "TbVendors",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<Guid>(
                name: "OfferConditionId",
                table: "TbOffers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TbOffers_OfferConditionId",
                table: "TbOffers",
                column: "OfferConditionId");

            migrationBuilder.AddForeignKey(
                name: "FK_TbOffers_TbOfferConditions_OfferConditionId",
                table: "TbOffers",
                column: "OfferConditionId",
                principalTable: "TbOfferConditions",
                principalColumn: "Id");
        }
    }
}
