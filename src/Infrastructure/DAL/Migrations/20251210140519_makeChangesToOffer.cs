using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class makeChangesToOffer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TbItems_TbBrands_BrandId",
                table: "TbItems");

            migrationBuilder.DropForeignKey(
                name: "FK_TbItems_TbCategories_CategoryId",
                table: "TbItems");

            migrationBuilder.DropForeignKey(
                name: "FK_TbItems_TbUnits_UnitId",
                table: "TbItems");

            migrationBuilder.DropForeignKey(
                name: "FK_TbOfferCombinationPricings_TbItemCombinations_ItemCombinationId",
                table: "TbOfferCombinationPricings");

            migrationBuilder.DropForeignKey(
                name: "FK_TbOfferCombinationPricings_TbItemCombinations_TbItemCombinationId",
                table: "TbOfferCombinationPricings");

            migrationBuilder.DropForeignKey(
                name: "FK_TbOfferCombinationPricings_TbOffers_OfferId",
                table: "TbOfferCombinationPricings");

            migrationBuilder.DropForeignKey(
                name: "FK_TbOfferPriceHistories_TbOfferCombinationPricings_OfferCombinationPricingId",
                table: "TbOfferPriceHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_TbOfferPriceHistories_TbOfferCombinationPricings_TbOfferCombinationPricingId",
                table: "TbOfferPriceHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_TbOffers_TbItems_ItemId",
                table: "TbOffers");

            migrationBuilder.DropForeignKey(
                name: "FK_TbOffers_TbOfferConditions_OfferConditionId",
                table: "TbOffers");

            migrationBuilder.DropForeignKey(
                name: "FK_TbOffers_TbVendors_VendorId",
                table: "TbOffers");

            migrationBuilder.DropForeignKey(
                name: "FK_TbOffers_TbWarranties_WarrantyId",
                table: "TbOffers");

            migrationBuilder.DropForeignKey(
                name: "FK_TbOrderDetails_TbOfferCombinationPricings_OfferCombinationPricingId",
                table: "TbOrderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_TbShoppingCartItems_TbOfferCombinationPricings_OfferCombinationPricingId",
                table: "TbShoppingCartItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TbOfferCombinationPricings",
                table: "TbOfferCombinationPricings");

            migrationBuilder.RenameTable(
                name: "TbOfferCombinationPricings",
                newName: "TbOfferCombinationPricing");

            migrationBuilder.RenameIndex(
                name: "IX_TbOfferCombinationPricings_TbItemCombinationId",
                table: "TbOfferCombinationPricing",
                newName: "IX_TbOfferCombinationPricing_TbItemCombinationId");

            migrationBuilder.RenameIndex(
                name: "IX_TbOfferCombinationPricings_OfferId",
                table: "TbOfferCombinationPricing",
                newName: "IX_TbOfferCombinationPricing_OfferId");

            migrationBuilder.RenameIndex(
                name: "IX_TbOfferCombinationPricings_ItemCombinationId",
                table: "TbOfferCombinationPricing",
                newName: "IX_TbOfferCombinationPricing_ItemCombinationId");

            migrationBuilder.RenameIndex(
                name: "IX_TbOfferCombinationPricings_IsDeleted",
                table: "TbOfferCombinationPricing",
                newName: "IX_TbOfferCombinationPricing_IsDeleted");

            migrationBuilder.AlterColumn<int>(
                name: "HandlingTimeInDays",
                table: "TbOffers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "FulfillmentType",
                table: "TbOffers",
                type: "int",
                nullable: false,
                defaultValue: 2,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<Guid>(
                name: "TbOfferConditionId",
                table: "TbOffers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TbWarrantyId",
                table: "TbOffers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "VisibilityScope",
                table: "TbItems",
                type: "int",
                nullable: false,
                defaultValue: 1,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<Guid>(
                name: "TbBrandId",
                table: "TbItems",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TbCategoryId",
                table: "TbItems",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ReturnedQuantity",
                table: "TbOfferCombinationPricing",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ReservedQuantity",
                table: "TbOfferCombinationPricing",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "RefundedQuantity",
                table: "TbOfferCombinationPricing",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "MinOrderQuantity",
                table: "TbOfferCombinationPricing",
                type: "int",
                nullable: false,
                defaultValue: 1,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "MaxOrderQuantity",
                table: "TbOfferCombinationPricing",
                type: "int",
                nullable: false,
                defaultValue: 999,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "LowStockThreshold",
                table: "TbOfferCombinationPricing",
                type: "int",
                nullable: false,
                defaultValue: 5,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "LockedQuantity",
                table: "TbOfferCombinationPricing",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastStockUpdate",
                table: "TbOfferCombinationPricing",
                type: "datetime2(2)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastPriceUpdate",
                table: "TbOfferCombinationPricing",
                type: "datetime2(2)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "InTransitQuantity",
                table: "TbOfferCombinationPricing",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "DamagedQuantity",
                table: "TbOfferCombinationPricing",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "AvailableQuantity",
                table: "TbOfferCombinationPricing",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TbOfferCombinationPricing",
                table: "TbOfferCombinationPricing",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_TbOffers_TbOfferConditionId",
                table: "TbOffers",
                column: "TbOfferConditionId");

            migrationBuilder.CreateIndex(
                name: "IX_TbOffers_TbWarrantyId",
                table: "TbOffers",
                column: "TbWarrantyId");

            migrationBuilder.CreateIndex(
                name: "IX_TbItems_TbBrandId",
                table: "TbItems",
                column: "TbBrandId");

            migrationBuilder.CreateIndex(
                name: "IX_TbItems_TbCategoryId",
                table: "TbItems",
                column: "TbCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_TbItems_TbBrands",
                table: "TbItems",
                column: "BrandId",
                principalTable: "TbBrands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbItems_TbBrands_TbBrandId",
                table: "TbItems",
                column: "TbBrandId",
                principalTable: "TbBrands",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TbItems_TbCategories",
                table: "TbItems",
                column: "CategoryId",
                principalTable: "TbCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbItems_TbCategories_TbCategoryId",
                table: "TbItems",
                column: "TbCategoryId",
                principalTable: "TbCategories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TbItems_TbUnits",
                table: "TbItems",
                column: "UnitId",
                principalTable: "TbUnits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbOfferCombinationPricing_TbItemCombination",
                table: "TbOfferCombinationPricing",
                column: "ItemCombinationId",
                principalTable: "TbItemCombinations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbOfferCombinationPricing_TbItemCombinations_TbItemCombinationId",
                table: "TbOfferCombinationPricing",
                column: "TbItemCombinationId",
                principalTable: "TbItemCombinations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TbOfferCombinationPricing_TbOffer",
                table: "TbOfferCombinationPricing",
                column: "OfferId",
                principalTable: "TbOffers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbOfferPriceHistories_TbOfferCombinationPricing_OfferCombinationPricingId",
                table: "TbOfferPriceHistories",
                column: "OfferCombinationPricingId",
                principalTable: "TbOfferCombinationPricing",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbOfferPriceHistories_TbOfferCombinationPricing_TbOfferCombinationPricingId",
                table: "TbOfferPriceHistories",
                column: "TbOfferCombinationPricingId",
                principalTable: "TbOfferCombinationPricing",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TbOffers_TbItems",
                table: "TbOffers",
                column: "ItemId",
                principalTable: "TbItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbOffers_TbOfferConditions",
                table: "TbOffers",
                column: "OfferConditionId",
                principalTable: "TbOfferConditions",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_TbOffers_TbOfferConditions_TbOfferConditionId",
                table: "TbOffers",
                column: "TbOfferConditionId",
                principalTable: "TbOfferConditions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TbOffers_TbVendors",
                table: "TbOffers",
                column: "VendorId",
                principalTable: "TbVendors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbOffers_TbWarranties",
                table: "TbOffers",
                column: "WarrantyId",
                principalTable: "TbWarranties",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_TbOffers_TbWarranties_TbWarrantyId",
                table: "TbOffers",
                column: "TbWarrantyId",
                principalTable: "TbWarranties",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TbOrderDetails_TbOfferCombinationPricing_OfferCombinationPricingId",
                table: "TbOrderDetails",
                column: "OfferCombinationPricingId",
                principalTable: "TbOfferCombinationPricing",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbShoppingCartItems_TbOfferCombinationPricing_OfferCombinationPricingId",
                table: "TbShoppingCartItems",
                column: "OfferCombinationPricingId",
                principalTable: "TbOfferCombinationPricing",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TbItems_TbBrands",
                table: "TbItems");

            migrationBuilder.DropForeignKey(
                name: "FK_TbItems_TbBrands_TbBrandId",
                table: "TbItems");

            migrationBuilder.DropForeignKey(
                name: "FK_TbItems_TbCategories",
                table: "TbItems");

            migrationBuilder.DropForeignKey(
                name: "FK_TbItems_TbCategories_TbCategoryId",
                table: "TbItems");

            migrationBuilder.DropForeignKey(
                name: "FK_TbItems_TbUnits",
                table: "TbItems");

            migrationBuilder.DropForeignKey(
                name: "FK_TbOfferCombinationPricing_TbItemCombination",
                table: "TbOfferCombinationPricing");

            migrationBuilder.DropForeignKey(
                name: "FK_TbOfferCombinationPricing_TbItemCombinations_TbItemCombinationId",
                table: "TbOfferCombinationPricing");

            migrationBuilder.DropForeignKey(
                name: "FK_TbOfferCombinationPricing_TbOffer",
                table: "TbOfferCombinationPricing");

            migrationBuilder.DropForeignKey(
                name: "FK_TbOfferPriceHistories_TbOfferCombinationPricing_OfferCombinationPricingId",
                table: "TbOfferPriceHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_TbOfferPriceHistories_TbOfferCombinationPricing_TbOfferCombinationPricingId",
                table: "TbOfferPriceHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_TbOffers_TbItems",
                table: "TbOffers");

            migrationBuilder.DropForeignKey(
                name: "FK_TbOffers_TbOfferConditions",
                table: "TbOffers");

            migrationBuilder.DropForeignKey(
                name: "FK_TbOffers_TbOfferConditions_TbOfferConditionId",
                table: "TbOffers");

            migrationBuilder.DropForeignKey(
                name: "FK_TbOffers_TbVendors",
                table: "TbOffers");

            migrationBuilder.DropForeignKey(
                name: "FK_TbOffers_TbWarranties",
                table: "TbOffers");

            migrationBuilder.DropForeignKey(
                name: "FK_TbOffers_TbWarranties_TbWarrantyId",
                table: "TbOffers");

            migrationBuilder.DropForeignKey(
                name: "FK_TbOrderDetails_TbOfferCombinationPricing_OfferCombinationPricingId",
                table: "TbOrderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_TbShoppingCartItems_TbOfferCombinationPricing_OfferCombinationPricingId",
                table: "TbShoppingCartItems");

            migrationBuilder.DropIndex(
                name: "IX_TbOffers_TbOfferConditionId",
                table: "TbOffers");

            migrationBuilder.DropIndex(
                name: "IX_TbOffers_TbWarrantyId",
                table: "TbOffers");

            migrationBuilder.DropIndex(
                name: "IX_TbItems_TbBrandId",
                table: "TbItems");

            migrationBuilder.DropIndex(
                name: "IX_TbItems_TbCategoryId",
                table: "TbItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TbOfferCombinationPricing",
                table: "TbOfferCombinationPricing");

            migrationBuilder.DropColumn(
                name: "TbOfferConditionId",
                table: "TbOffers");

            migrationBuilder.DropColumn(
                name: "TbWarrantyId",
                table: "TbOffers");

            migrationBuilder.DropColumn(
                name: "TbBrandId",
                table: "TbItems");

            migrationBuilder.DropColumn(
                name: "TbCategoryId",
                table: "TbItems");

            migrationBuilder.RenameTable(
                name: "TbOfferCombinationPricing",
                newName: "TbOfferCombinationPricings");

            migrationBuilder.RenameIndex(
                name: "IX_TbOfferCombinationPricing_TbItemCombinationId",
                table: "TbOfferCombinationPricings",
                newName: "IX_TbOfferCombinationPricings_TbItemCombinationId");

            migrationBuilder.RenameIndex(
                name: "IX_TbOfferCombinationPricing_OfferId",
                table: "TbOfferCombinationPricings",
                newName: "IX_TbOfferCombinationPricings_OfferId");

            migrationBuilder.RenameIndex(
                name: "IX_TbOfferCombinationPricing_ItemCombinationId",
                table: "TbOfferCombinationPricings",
                newName: "IX_TbOfferCombinationPricings_ItemCombinationId");

            migrationBuilder.RenameIndex(
                name: "IX_TbOfferCombinationPricing_IsDeleted",
                table: "TbOfferCombinationPricings",
                newName: "IX_TbOfferCombinationPricings_IsDeleted");

            migrationBuilder.AlterColumn<int>(
                name: "HandlingTimeInDays",
                table: "TbOffers",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "FulfillmentType",
                table: "TbOffers",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 2);

            migrationBuilder.AlterColumn<int>(
                name: "VisibilityScope",
                table: "TbItems",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 1);

            migrationBuilder.AlterColumn<int>(
                name: "ReturnedQuantity",
                table: "TbOfferCombinationPricings",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "ReservedQuantity",
                table: "TbOfferCombinationPricings",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "RefundedQuantity",
                table: "TbOfferCombinationPricings",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "MinOrderQuantity",
                table: "TbOfferCombinationPricings",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 1);

            migrationBuilder.AlterColumn<int>(
                name: "MaxOrderQuantity",
                table: "TbOfferCombinationPricings",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 999);

            migrationBuilder.AlterColumn<int>(
                name: "LowStockThreshold",
                table: "TbOfferCombinationPricings",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 5);

            migrationBuilder.AlterColumn<int>(
                name: "LockedQuantity",
                table: "TbOfferCombinationPricings",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastStockUpdate",
                table: "TbOfferCombinationPricings",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2(2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastPriceUpdate",
                table: "TbOfferCombinationPricings",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2(2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "InTransitQuantity",
                table: "TbOfferCombinationPricings",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "DamagedQuantity",
                table: "TbOfferCombinationPricings",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "AvailableQuantity",
                table: "TbOfferCombinationPricings",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TbOfferCombinationPricings",
                table: "TbOfferCombinationPricings",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TbItems_TbBrands_BrandId",
                table: "TbItems",
                column: "BrandId",
                principalTable: "TbBrands",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TbItems_TbCategories_CategoryId",
                table: "TbItems",
                column: "CategoryId",
                principalTable: "TbCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbItems_TbUnits_UnitId",
                table: "TbItems",
                column: "UnitId",
                principalTable: "TbUnits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbOfferCombinationPricings_TbItemCombinations_ItemCombinationId",
                table: "TbOfferCombinationPricings",
                column: "ItemCombinationId",
                principalTable: "TbItemCombinations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbOfferCombinationPricings_TbItemCombinations_TbItemCombinationId",
                table: "TbOfferCombinationPricings",
                column: "TbItemCombinationId",
                principalTable: "TbItemCombinations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TbOfferCombinationPricings_TbOffers_OfferId",
                table: "TbOfferCombinationPricings",
                column: "OfferId",
                principalTable: "TbOffers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbOfferPriceHistories_TbOfferCombinationPricings_OfferCombinationPricingId",
                table: "TbOfferPriceHistories",
                column: "OfferCombinationPricingId",
                principalTable: "TbOfferCombinationPricings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbOfferPriceHistories_TbOfferCombinationPricings_TbOfferCombinationPricingId",
                table: "TbOfferPriceHistories",
                column: "TbOfferCombinationPricingId",
                principalTable: "TbOfferCombinationPricings",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TbOffers_TbItems_ItemId",
                table: "TbOffers",
                column: "ItemId",
                principalTable: "TbItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbOffers_TbOfferConditions_OfferConditionId",
                table: "TbOffers",
                column: "OfferConditionId",
                principalTable: "TbOfferConditions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TbOffers_TbVendors_VendorId",
                table: "TbOffers",
                column: "VendorId",
                principalTable: "TbVendors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbOffers_TbWarranties_WarrantyId",
                table: "TbOffers",
                column: "WarrantyId",
                principalTable: "TbWarranties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbOrderDetails_TbOfferCombinationPricings_OfferCombinationPricingId",
                table: "TbOrderDetails",
                column: "OfferCombinationPricingId",
                principalTable: "TbOfferCombinationPricings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbShoppingCartItems_TbOfferCombinationPricings_OfferCombinationPricingId",
                table: "TbShoppingCartItems",
                column: "OfferCombinationPricingId",
                principalTable: "TbOfferCombinationPricings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
