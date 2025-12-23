using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class CreateTbCombinationAttributes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TbCombinationAttributesValues_TbItemCombinations_ItemCombinationId",
                table: "TbCombinationAttributesValues");

            migrationBuilder.DropIndex(
                name: "IX_TbCombinationAttributesValues_ItemCombinationId",
                table: "TbCombinationAttributesValues");

            migrationBuilder.DropColumn(
                name: "ItemCombinationId",
                table: "TbCombinationAttributesValues");

            migrationBuilder.CreateTable(
                name: "TbCombinationAttributes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    ItemCombinationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AttributeValueId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbCombinationAttributes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbCombinationAttributesValue_TbCombinationAttributes_AttributeValueId",
                        column: x => x.AttributeValueId,
                        principalTable: "TbCombinationAttributesValues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TbItemCombination_TbCombinationAttributes_ItemCombinationId",
                        column: x => x.ItemCombinationId,
                        principalTable: "TbItemCombinations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TbCombinationAttributes_AttributeValueId",
                table: "TbCombinationAttributes",
                column: "AttributeValueId");

            migrationBuilder.CreateIndex(
                name: "IX_TbCombinationAttributes_IsDeleted",
                table: "TbCombinationAttributes",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbCombinationAttributes_ItemCombinationId",
                table: "TbCombinationAttributes",
                column: "ItemCombinationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TbCombinationAttributes");

            migrationBuilder.AddColumn<Guid>(
                name: "ItemCombinationId",
                table: "TbCombinationAttributesValues",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_TbCombinationAttributesValues_ItemCombinationId",
                table: "TbCombinationAttributesValues",
                column: "ItemCombinationId");

            migrationBuilder.AddForeignKey(
                name: "FK_TbCombinationAttributesValues_TbItemCombinations_ItemCombinationId",
                table: "TbCombinationAttributesValues",
                column: "ItemCombinationId",
                principalTable: "TbItemCombinations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
