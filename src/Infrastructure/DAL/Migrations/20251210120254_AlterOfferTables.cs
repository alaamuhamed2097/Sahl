using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class AlterOfferTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TbOfferPriceHistories_TbItemCombinations_ItemCombinationId",
                table: "TbOfferPriceHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_TbOfferPriceHistories_TbItemCombinations_TbItemCombinationId",
                table: "TbOfferPriceHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_TbWarranties_TbCities_CityId",
                table: "TbWarranties");

            migrationBuilder.DropIndex(
                name: "IX_TbWarranties_CityId",
                table: "TbWarranties");

            migrationBuilder.DropIndex(
                name: "IX_TbOfferPriceHistories_ItemCombinationId",
                table: "TbOfferPriceHistories");

            migrationBuilder.DropIndex(
                name: "IX_TbOfferPriceHistories_TbItemCombinationId",
                table: "TbOfferPriceHistories");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "TbWarranties");

            migrationBuilder.DropColumn(
                name: "ItemCombinationId",
                table: "TbOfferPriceHistories");

            migrationBuilder.DropColumn(
                name: "TbItemCombinationId",
                table: "TbOfferPriceHistories");

            migrationBuilder.DropColumn(
                name: "IsDefault",
                table: "TbOfferCombinationPricings");

            migrationBuilder.AddColumn<bool>(
                name: "IsBuyBoxWinner",
                table: "TbOffers",
                type: "bit",
                nullable: false,
                defaultValue: false);

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsBuyBoxWinner",
                table: "TbOffers");

            migrationBuilder.DropColumn(
                name: "VendorRatingForThisItem",
                table: "TbOffers");

            migrationBuilder.DropColumn(
                name: "VendorSalesCountForThisItem",
                table: "TbOffers");

            migrationBuilder.AddColumn<Guid>(
                name: "CityId",
                table: "TbWarranties",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ItemCombinationId",
                table: "TbOfferPriceHistories",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "TbItemCombinationId",
                table: "TbOfferPriceHistories",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDefault",
                table: "TbOfferCombinationPricings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_TbWarranties_CityId",
                table: "TbWarranties",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_TbOfferPriceHistories_ItemCombinationId",
                table: "TbOfferPriceHistories",
                column: "ItemCombinationId");

            migrationBuilder.CreateIndex(
                name: "IX_TbOfferPriceHistories_TbItemCombinationId",
                table: "TbOfferPriceHistories",
                column: "TbItemCombinationId");

            migrationBuilder.AddForeignKey(
                name: "FK_TbOfferPriceHistories_TbItemCombinations_ItemCombinationId",
                table: "TbOfferPriceHistories",
                column: "ItemCombinationId",
                principalTable: "TbItemCombinations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbOfferPriceHistories_TbItemCombinations_TbItemCombinationId",
                table: "TbOfferPriceHistories",
                column: "TbItemCombinationId",
                principalTable: "TbItemCombinations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TbWarranties_TbCities_CityId",
                table: "TbWarranties",
                column: "CityId",
                principalTable: "TbCities",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
