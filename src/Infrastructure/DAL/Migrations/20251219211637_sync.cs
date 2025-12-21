using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class sync : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TbOfferCombinationPricings_TbItemCombinations_TbItemCombinationId",
                table: "TbOfferCombinationPricings");

            migrationBuilder.DropForeignKey(
                name: "FK_TbOfferPriceHistories_TbOfferCombinationPricings_TbOfferCombinationPricingId",
                table: "TbOfferPriceHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_TbOfferStatusHistories_TbOffers_TbOfferId",
                table: "TbOfferStatusHistories");

            migrationBuilder.DropIndex(
                name: "IX_TbOfferStatusHistories_TbOfferId",
                table: "TbOfferStatusHistories");

            migrationBuilder.DropIndex(
                name: "IX_TbOfferPriceHistories_TbOfferCombinationPricingId",
                table: "TbOfferPriceHistories");

            migrationBuilder.DropIndex(
                name: "IX_TbOfferCombinationPricings_TbItemCombinationId",
                table: "TbOfferCombinationPricings");

            migrationBuilder.DropColumn(
                name: "TbOfferId",
                table: "TbOfferStatusHistories");

            migrationBuilder.DropColumn(
                name: "VendorRatingForThisItem",
                table: "TbOffers");

            migrationBuilder.DropColumn(
                name: "VendorSalesCountForThisItem",
                table: "TbOffers");

            migrationBuilder.DropColumn(
                name: "TbOfferCombinationPricingId",
                table: "TbOfferPriceHistories");

            migrationBuilder.DropColumn(
                name: "TbItemCombinationId",
                table: "TbOfferCombinationPricings");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TbOfferId",
                table: "TbOfferStatusHistories",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "OfferConditionId",
                table: "TbOffers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "VendorRatingForThisItem",
                table: "TbOffers",
                type: "decimal(3,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "VendorSalesCountForThisItem",
                table: "TbOffers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "TbOfferCombinationPricingId",
                table: "TbOfferPriceHistories",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TbItemCombinationId",
                table: "TbOfferCombinationPricings",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TbOfferStatusHistories_TbOfferId",
                table: "TbOfferStatusHistories",
                column: "TbOfferId");

            migrationBuilder.CreateIndex(
                name: "IX_TbOffers_OfferConditionId",
                table: "TbOffers",
                column: "OfferConditionId");

            migrationBuilder.CreateIndex(
                name: "IX_TbOfferPriceHistories_TbOfferCombinationPricingId",
                table: "TbOfferPriceHistories",
                column: "TbOfferCombinationPricingId");

            migrationBuilder.CreateIndex(
                name: "IX_TbOfferCombinationPricings_TbItemCombinationId",
                table: "TbOfferCombinationPricings",
                column: "TbItemCombinationId");

            migrationBuilder.AddForeignKey(
                name: "FK_TbOfferCombinationPricings_TbItemCombinations_TbItemCombinationId",
                table: "TbOfferCombinationPricings",
                column: "TbItemCombinationId",
                principalTable: "TbItemCombinations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TbOfferPriceHistories_TbOfferCombinationPricings_TbOfferCombinationPricingId",
                table: "TbOfferPriceHistories",
                column: "TbOfferCombinationPricingId",
                principalTable: "TbOfferCombinationPricings",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TbOffers_TbOfferConditions_OfferConditionId",
                table: "TbOffers",
                column: "OfferConditionId",
                principalTable: "TbOfferConditions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TbOfferStatusHistories_TbOffers_TbOfferId",
                table: "TbOfferStatusHistories",
                column: "TbOfferId",
                principalTable: "TbOffers",
                principalColumn: "Id");
        }
    }
}
