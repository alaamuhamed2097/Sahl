using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class AlterItemRelatedTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TbItemAttribute_TbAttributes_AttributeId",
                table: "TbItemAttribute");

            migrationBuilder.DropForeignKey(
                name: "FK_TbItemAttribute_TbItems_ItemId",
                table: "TbItemAttribute");

            migrationBuilder.DropForeignKey(
                name: "FK_TbMovitemsdetails_TbItemAttributeCombinationPricings_ItemAttributeCombinationPricingId",
                table: "TbMovitemsdetails");

            migrationBuilder.DropForeignKey(
                name: "FK_TbOffers_TbOfferConditions_TbOfferConditionId",
                table: "TbOffers");

            migrationBuilder.DropTable(
                name: "TbItemAttributeCombinationPricings");

            migrationBuilder.DropIndex(
                name: "IX_TbOffers_IsActive",
                table: "TbOffers");

            migrationBuilder.DropIndex(
                name: "IX_TbOffers_Item_Active_Price",
                table: "TbOffers");

            migrationBuilder.DropIndex(
                name: "IX_TbOffers_Price",
                table: "TbOffers");

            migrationBuilder.DropIndex(
                name: "IX_TbOffers_TbOfferConditionId",
                table: "TbOffers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TbItemAttribute",
                table: "TbItemAttribute");

            migrationBuilder.DropColumn(
                name: "Condition",
                table: "TbOffers");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "TbOffers");

            migrationBuilder.DropColumn(
                name: "OriginalPrice",
                table: "TbOffers");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "TbOffers");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "TbOffers");

            migrationBuilder.DropColumn(
                name: "SellerRating",
                table: "TbOffers");

            migrationBuilder.DropColumn(
                name: "ShippingCost",
                table: "TbOffers");

            migrationBuilder.DropColumn(
                name: "TbOfferConditionId",
                table: "TbOffers");

            migrationBuilder.DropColumn(
                name: "SellerSKU",
                table: "TbOfferCombinationPricings");

            migrationBuilder.DropColumn(
                name: "Barcode",
                table: "TbItems");

            migrationBuilder.DropColumn(
                name: "SKU",
                table: "TbItems");

            migrationBuilder.RenameTable(
                name: "TbItemAttribute",
                newName: "TbItemAttributes");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "TbOfferCombinationPricings",
                newName: "IsDefault");

            migrationBuilder.RenameColumn(
                name: "ItemAttributeCombinationPricingId",
                table: "TbMovitemsdetails",
                newName: "ItemCombinationId");

            migrationBuilder.RenameIndex(
                name: "IX_TbMovitemsdetails_ItemAttributeCombinationPricingId",
                table: "TbMovitemsdetails",
                newName: "IX_TbMovitemsdetails_ItemCombinationId");

            migrationBuilder.RenameIndex(
                name: "IX_TbItemAttribute_ItemId",
                table: "TbItemAttributes",
                newName: "IX_TbItemAttributes_ItemId");

            migrationBuilder.RenameIndex(
                name: "IX_TbItemAttribute_CurrentState",
                table: "TbItemAttributes",
                newName: "IX_TbItemAttributes_CurrentState");

            migrationBuilder.RenameIndex(
                name: "IX_TbItemAttribute_AttributeId",
                table: "TbItemAttributes",
                newName: "IX_TbItemAttributes_AttributeId");

            migrationBuilder.AddColumn<int>(
                name: "HandlingTimeInDays",
                table: "TbOffers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StorgeLocation",
                table: "TbOffers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "VisibilityScope",
                table: "TbOffers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "DescriptionEn",
                table: "TbItems",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "DescriptionAr",
                table: "TbItems",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<decimal>(
                name: "BasePrice",
                table: "TbItems",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Barcode",
                table: "TbItemCombinations",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "BasePrice",
                table: "TbItemCombinations",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDefault",
                table: "TbItemCombinations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "SKU",
                table: "TbItemCombinations",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "DisplayOrder",
                table: "TbItemAttributes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TbItemAttributes",
                table: "TbItemAttributes",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "TbAttributeValuePriceModifiers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    CombinationAttributeValueId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AttributeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifierType = table.Column<int>(type: "int", nullable: false),
                    ModifierValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbAttributeValuePriceModifiers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbAttributeValuePriceModifiers_TbAttributes_AttributeId",
                        column: x => x.AttributeId,
                        principalTable: "TbAttributes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TbAttributeValuePriceModifiers_TbCombinationAttributesValues_CombinationAttributeValueId",
                        column: x => x.CombinationAttributeValueId,
                        principalTable: "TbCombinationAttributesValues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TbOfferPriceHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    ItemCombinationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OfferCombinationPricingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OldPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NewPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ChangeNote = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbOfferPriceHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbOfferPriceHistories_TbItemCombinations_ItemCombinationId",
                        column: x => x.ItemCombinationId,
                        principalTable: "TbItemCombinations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TbOfferPriceHistories_TbOfferCombinationPricings_OfferCombinationPricingId",
                        column: x => x.OfferCombinationPricingId,
                        principalTable: "TbOfferCombinationPricings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TbOfferStatusHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    OfferId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OldStatus = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NewStatus = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ChangeNote = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbOfferStatusHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbOfferStatusHistories_TbOffers_OfferId",
                        column: x => x.OfferId,
                        principalTable: "TbOffers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TbOffers_OfferConditionId",
                table: "TbOffers",
                column: "OfferConditionId");

            migrationBuilder.CreateIndex(
                name: "IX_TbAttributeValuePriceModifiers_AttributeId",
                table: "TbAttributeValuePriceModifiers",
                column: "AttributeId");

            migrationBuilder.CreateIndex(
                name: "IX_TbAttributeValuePriceModifiers_CombinationAttributeValueId",
                table: "TbAttributeValuePriceModifiers",
                column: "CombinationAttributeValueId");

            migrationBuilder.CreateIndex(
                name: "IX_TbAttributeValuePriceModifiers_CurrentState",
                table: "TbAttributeValuePriceModifiers",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbOfferPriceHistories_CurrentState",
                table: "TbOfferPriceHistories",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbOfferPriceHistories_ItemCombinationId",
                table: "TbOfferPriceHistories",
                column: "ItemCombinationId");

            migrationBuilder.CreateIndex(
                name: "IX_TbOfferPriceHistories_OfferCombinationPricingId",
                table: "TbOfferPriceHistories",
                column: "OfferCombinationPricingId");

            migrationBuilder.CreateIndex(
                name: "IX_TbOfferStatusHistories_CurrentState",
                table: "TbOfferStatusHistories",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbOfferStatusHistories_OfferId",
                table: "TbOfferStatusHistories",
                column: "OfferId");

            migrationBuilder.AddForeignKey(
                name: "FK_TbItemAttributes_TbAttributes_AttributeId",
                table: "TbItemAttributes",
                column: "AttributeId",
                principalTable: "TbAttributes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbItemAttributes_TbItems_ItemId",
                table: "TbItemAttributes",
                column: "ItemId",
                principalTable: "TbItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbMovitemsdetails_TbItemCombinations_ItemCombinationId",
                table: "TbMovitemsdetails",
                column: "ItemCombinationId",
                principalTable: "TbItemCombinations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TbOffers_TbOfferConditions_OfferConditionId",
                table: "TbOffers",
                column: "OfferConditionId",
                principalTable: "TbOfferConditions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TbItemAttributes_TbAttributes_AttributeId",
                table: "TbItemAttributes");

            migrationBuilder.DropForeignKey(
                name: "FK_TbItemAttributes_TbItems_ItemId",
                table: "TbItemAttributes");

            migrationBuilder.DropForeignKey(
                name: "FK_TbMovitemsdetails_TbItemCombinations_ItemCombinationId",
                table: "TbMovitemsdetails");

            migrationBuilder.DropForeignKey(
                name: "FK_TbOffers_TbOfferConditions_OfferConditionId",
                table: "TbOffers");

            migrationBuilder.DropTable(
                name: "TbAttributeValuePriceModifiers");

            migrationBuilder.DropTable(
                name: "TbOfferPriceHistories");

            migrationBuilder.DropTable(
                name: "TbOfferStatusHistories");

            migrationBuilder.DropIndex(
                name: "IX_TbOffers_OfferConditionId",
                table: "TbOffers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TbItemAttributes",
                table: "TbItemAttributes");

            migrationBuilder.DropColumn(
                name: "HandlingTimeInDays",
                table: "TbOffers");

            migrationBuilder.DropColumn(
                name: "StorgeLocation",
                table: "TbOffers");

            migrationBuilder.DropColumn(
                name: "VisibilityScope",
                table: "TbOffers");

            migrationBuilder.DropColumn(
                name: "BasePrice",
                table: "TbItems");

            migrationBuilder.DropColumn(
                name: "Barcode",
                table: "TbItemCombinations");

            migrationBuilder.DropColumn(
                name: "BasePrice",
                table: "TbItemCombinations");

            migrationBuilder.DropColumn(
                name: "IsDefault",
                table: "TbItemCombinations");

            migrationBuilder.DropColumn(
                name: "SKU",
                table: "TbItemCombinations");

            migrationBuilder.DropColumn(
                name: "DisplayOrder",
                table: "TbItemAttributes");

            migrationBuilder.RenameTable(
                name: "TbItemAttributes",
                newName: "TbItemAttribute");

            migrationBuilder.RenameColumn(
                name: "IsDefault",
                table: "TbOfferCombinationPricings",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "ItemCombinationId",
                table: "TbMovitemsdetails",
                newName: "ItemAttributeCombinationPricingId");

            migrationBuilder.RenameIndex(
                name: "IX_TbMovitemsdetails_ItemCombinationId",
                table: "TbMovitemsdetails",
                newName: "IX_TbMovitemsdetails_ItemAttributeCombinationPricingId");

            migrationBuilder.RenameIndex(
                name: "IX_TbItemAttributes_ItemId",
                table: "TbItemAttribute",
                newName: "IX_TbItemAttribute_ItemId");

            migrationBuilder.RenameIndex(
                name: "IX_TbItemAttributes_CurrentState",
                table: "TbItemAttribute",
                newName: "IX_TbItemAttribute_CurrentState");

            migrationBuilder.RenameIndex(
                name: "IX_TbItemAttributes_AttributeId",
                table: "TbItemAttribute",
                newName: "IX_TbItemAttribute_AttributeId");

            migrationBuilder.AddColumn<string>(
                name: "Condition",
                table: "TbOffers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "TbOffers",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<decimal>(
                name: "OriginalPrice",
                table: "TbOffers",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "TbOffers",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "TbOffers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "SellerRating",
                table: "TbOffers",
                type: "decimal(3,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ShippingCost",
                table: "TbOffers",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<Guid>(
                name: "TbOfferConditionId",
                table: "TbOffers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SellerSKU",
                table: "TbOfferCombinationPricings",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DescriptionEn",
                table: "TbItems",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "DescriptionAr",
                table: "TbItems",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AddColumn<string>(
                name: "Barcode",
                table: "TbItems",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SKU",
                table: "TbItems",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TbItemAttribute",
                table: "TbItemAttribute",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "TbItemAttributeCombinationPricings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    ItemCombinationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AttributeIds = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Barcode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    BasePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CostPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    LastPriceUpdate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    SKU = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SalesPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SuggestedPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbItemAttributeCombinationPricings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbItemAttributeCombinationPricings_TbItemCombinations_ItemCombinationId",
                        column: x => x.ItemCombinationId,
                        principalTable: "TbItemCombinations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TbItemAttributeCombinationPricings_TbItems_ItemId",
                        column: x => x.ItemId,
                        principalTable: "TbItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TbOffers_IsActive",
                table: "TbOffers",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_TbOffers_Item_Active_Price",
                table: "TbOffers",
                columns: new[] { "ItemId", "IsActive", "Price" });

            migrationBuilder.CreateIndex(
                name: "IX_TbOffers_Price",
                table: "TbOffers",
                column: "Price");

            migrationBuilder.CreateIndex(
                name: "IX_TbOffers_TbOfferConditionId",
                table: "TbOffers",
                column: "TbOfferConditionId");

            migrationBuilder.CreateIndex(
                name: "IX_TbItemAttributeCombinationPricings_AttributeIds",
                table: "TbItemAttributeCombinationPricings",
                column: "AttributeIds");

            migrationBuilder.CreateIndex(
                name: "IX_TbItemAttributeCombinationPricings_CurrentState",
                table: "TbItemAttributeCombinationPricings",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbItemAttributeCombinationPricings_ItemCombinationId",
                table: "TbItemAttributeCombinationPricings",
                column: "ItemCombinationId");

            migrationBuilder.CreateIndex(
                name: "IX_TbItemAttributeCombinationPricings_ItemId",
                table: "TbItemAttributeCombinationPricings",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_TbItemAttributeCombinationPricings_Price",
                table: "TbItemAttributeCombinationPricings",
                column: "Price");

            migrationBuilder.AddForeignKey(
                name: "FK_TbItemAttribute_TbAttributes_AttributeId",
                table: "TbItemAttribute",
                column: "AttributeId",
                principalTable: "TbAttributes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbItemAttribute_TbItems_ItemId",
                table: "TbItemAttribute",
                column: "ItemId",
                principalTable: "TbItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbMovitemsdetails_TbItemAttributeCombinationPricings_ItemAttributeCombinationPricingId",
                table: "TbMovitemsdetails",
                column: "ItemAttributeCombinationPricingId",
                principalTable: "TbItemAttributeCombinationPricings",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TbOffers_TbOfferConditions_TbOfferConditionId",
                table: "TbOffers",
                column: "TbOfferConditionId",
                principalTable: "TbOfferConditions",
                principalColumn: "Id");
        }
    }
}
