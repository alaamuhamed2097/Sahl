using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class CreateTbItemCombinationImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TbItemCombinationImages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    Path = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    ItemCombinationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbItemCombinationImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbItemCombinationImages_TbItemCombinations_ItemCombinationId",
                        column: x => x.ItemCombinationId,
                        principalTable: "TbItemCombinations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TbItemCombinationImages_IsDeleted",
                table: "TbItemCombinationImages",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbItemCombinationImages_ItemCombinationId",
                table: "TbItemCombinationImages",
                column: "ItemCombinationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TbItemCombinationImages");
        }
    }
}
