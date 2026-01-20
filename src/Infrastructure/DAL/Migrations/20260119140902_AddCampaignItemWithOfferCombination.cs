using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddCampaignItemWithOfferCombination : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.Sql(@"DELETE FROM [dbo].[TbCampaignProducts]");

			migrationBuilder.DropForeignKey(
                name: "FK_TbCampaignProducts_TbItems_ItemId",
                table: "TbCampaignProducts");

            migrationBuilder.DropIndex(
                name: "IX_TbCampaignProducts_DisplayOrder",
                table: "TbCampaignProducts");

            migrationBuilder.DropColumn(
                name: "CampaignPrice",
                table: "TbCampaignProducts");

            migrationBuilder.DropColumn(
                name: "DisplayOrder",
                table: "TbCampaignProducts");

            migrationBuilder.RenameColumn(
                name: "ItemId",
                table: "TbCampaignProducts",
                newName: "OfferCombinationPricingId");

            migrationBuilder.RenameIndex(
                name: "IX_TbCampaignProducts_ItemId",
                table: "TbCampaignProducts",
                newName: "IX_TbCampaignProducts_OfferCombinationPricingId");

            migrationBuilder.RenameIndex(
                name: "IX_TbCampaignProducts_CampaignId_ItemId",
                table: "TbCampaignProducts",
                newName: "IX_TbCampaignProducts_CampaignId_OfferCombinationPricingId");

            migrationBuilder.AddColumn<Guid>(
                name: "VendorId",
                table: "TbCampaignProducts",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TbCampaignProducts_VendorId",
                table: "TbCampaignProducts",
                column: "VendorId");

            migrationBuilder.AddForeignKey(
                name: "FK_TbCampaignProducts_TbOfferCombinationPricings_OfferCombinationPricingId",
                table: "TbCampaignProducts",
                column: "OfferCombinationPricingId",
                principalTable: "TbOfferCombinationPricings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TbCampaignProducts_TbOfferCombinationPricings_OfferCombinationPricingId",
                table: "TbCampaignProducts");

            migrationBuilder.DropIndex(
                name: "IX_TbCampaignProducts_VendorId",
                table: "TbCampaignProducts");

            migrationBuilder.DropColumn(
                name: "VendorId",
                table: "TbCampaignProducts");

            migrationBuilder.RenameColumn(
                name: "OfferCombinationPricingId",
                table: "TbCampaignProducts",
                newName: "ItemId");

            migrationBuilder.RenameIndex(
                name: "IX_TbCampaignProducts_OfferCombinationPricingId",
                table: "TbCampaignProducts",
                newName: "IX_TbCampaignProducts_ItemId");

            migrationBuilder.RenameIndex(
                name: "IX_TbCampaignProducts_CampaignId_OfferCombinationPricingId",
                table: "TbCampaignProducts",
                newName: "IX_TbCampaignProducts_CampaignId_ItemId");

            migrationBuilder.AddColumn<decimal>(
                name: "CampaignPrice",
                table: "TbCampaignProducts",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "DisplayOrder",
                table: "TbCampaignProducts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TbCampaignProducts_DisplayOrder",
                table: "TbCampaignProducts",
                column: "DisplayOrder");

            migrationBuilder.AddForeignKey(
                name: "FK_TbCampaignProducts_TbItems_ItemId",
                table: "TbCampaignProducts",
                column: "ItemId",
                principalTable: "TbItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
