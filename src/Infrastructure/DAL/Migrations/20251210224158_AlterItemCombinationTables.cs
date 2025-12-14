using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class AlterItemCombinationTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TbCombinationAttributesValues_TbCombinationAttributes_CombinationAttributeId",
                table: "TbCombinationAttributesValues");

            migrationBuilder.DropTable(
                name: "TbAttributeValuePriceModifiers");

            migrationBuilder.DropTable(
                name: "TbCombinationAttributes");

            migrationBuilder.DropColumn(
                name: "LastPriceUpdate",
                table: "TbOfferCombinationPricings");

            migrationBuilder.DropColumn(
                name: "BasePrice",
                table: "TbItemCombinations");

            migrationBuilder.RenameColumn(
                name: "CombinationAttributeId",
                table: "TbCombinationAttributesValues",
                newName: "ItemCombinationId");

            migrationBuilder.RenameIndex(
                name: "IX_TbCombinationAttributesValues_CombinationAttributeId",
                table: "TbCombinationAttributesValues",
                newName: "IX_TbCombinationAttributesValues_ItemCombinationId");

            migrationBuilder.AddForeignKey(
                name: "FK_TbCombinationAttributesValues_TbItemCombinations_ItemCombinationId",
                table: "TbCombinationAttributesValues",
                column: "ItemCombinationId",
                principalTable: "TbItemCombinations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TbCombinationAttributesValues_TbItemCombinations_ItemCombinationId",
                table: "TbCombinationAttributesValues");

            migrationBuilder.RenameColumn(
                name: "ItemCombinationId",
                table: "TbCombinationAttributesValues",
                newName: "CombinationAttributeId");

            migrationBuilder.RenameIndex(
                name: "IX_TbCombinationAttributesValues_ItemCombinationId",
                table: "TbCombinationAttributesValues",
                newName: "IX_TbCombinationAttributesValues_CombinationAttributeId");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastPriceUpdate",
                table: "TbOfferCombinationPricings",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "BasePrice",
                table: "TbItemCombinations",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TbAttributeValuePriceModifiers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    AttributeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CombinationAttributeValueId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ModifierType = table.Column<int>(type: "int", nullable: false),
                    ModifierValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PriceModifierCategory = table.Column<int>(type: "int", nullable: false),
                    TbCombinationAttributesValueId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
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
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TbAttributeValuePriceModifiers_TbCombinationAttributesValues_CombinationAttributeValueId",
                        column: x => x.CombinationAttributeValueId,
                        principalTable: "TbCombinationAttributesValues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TbAttributeValuePriceModifiers_TbCombinationAttributesValues_TbCombinationAttributesValueId",
                        column: x => x.TbCombinationAttributesValueId,
                        principalTable: "TbCombinationAttributesValues",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TbCombinationAttributes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    ItemCombinationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbCombinationAttributes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbCombinationAttributes_TbItemCombinations_ItemCombinationId",
                        column: x => x.ItemCombinationId,
                        principalTable: "TbItemCombinations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TbAttributeValuePriceModifiers_AttributeId",
                table: "TbAttributeValuePriceModifiers",
                column: "AttributeId");

            migrationBuilder.CreateIndex(
                name: "IX_TbAttributeValuePriceModifiers_CombinationAttributeValueId",
                table: "TbAttributeValuePriceModifiers",
                column: "CombinationAttributeValueId");

            migrationBuilder.CreateIndex(
                name: "IX_TbAttributeValuePriceModifiers_IsDeleted",
                table: "TbAttributeValuePriceModifiers",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbAttributeValuePriceModifiers_TbCombinationAttributesValueId",
                table: "TbAttributeValuePriceModifiers",
                column: "TbCombinationAttributesValueId");

            migrationBuilder.CreateIndex(
                name: "IX_TbCombinationAttributes_IsDeleted",
                table: "TbCombinationAttributes",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbCombinationAttributes_ItemCombinationId",
                table: "TbCombinationAttributes",
                column: "ItemCombinationId");

            migrationBuilder.AddForeignKey(
                name: "FK_TbCombinationAttributesValues_TbCombinationAttributes_CombinationAttributeId",
                table: "TbCombinationAttributesValues",
                column: "CombinationAttributeId",
                principalTable: "TbCombinationAttributes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
