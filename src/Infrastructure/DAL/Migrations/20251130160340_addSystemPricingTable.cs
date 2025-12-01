using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class addSystemPricingTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "DisplayOrder",
                table: "TbCategories",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "PricingSystemType",
                table: "TbCategories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "TbPricingSystemSettings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    SystemName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SystemType = table.Column<int>(type: "int", nullable: false),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    DescriptionAr = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DescriptionEn = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbPricingSystemSettings", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "TbPricingSystemSettings",
                columns: new[] { "Id", "CreatedBy", "CreatedDateUtc", "CurrentState", "DescriptionAr", "DescriptionEn", "DisplayOrder", "IsEnabled", "SystemName", "SystemType", "UpdatedBy", "UpdatedDateUtc" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(2025, 11, 30, 0, 0, 0, 0, DateTimeKind.Utc), 1, "????? ???? - ??? ?????", "Standard pricing - Price and quantity", 1, true, "Standard Pricing", 0, null, null },
                    { new Guid("22222222-2222-2222-2222-222222222222"), new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(2025, 11, 30, 0, 0, 0, 0, DateTimeKind.Utc), 1, "????? ??????????", "Combination-based pricing", 2, true, "Combination Pricing", 1, null, null },
                    { new Guid("33333333-3333-3333-3333-333333333333"), new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(2025, 11, 30, 0, 0, 0, 0, DateTimeKind.Utc), 1, "????? ????????", "Quantity tier pricing", 3, true, "Quantity Pricing", 2, null, null },
                    { new Guid("44444444-4444-4444-4444-444444444444"), new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(2025, 11, 30, 0, 0, 0, 0, DateTimeKind.Utc), 1, "??????? ?? ?????", "Combinations with quantity tiers", 4, true, "Combination + Quantity", 3, null, null },
                    { new Guid("55555555-5555-5555-5555-555555555555"), new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(2025, 11, 30, 0, 0, 0, 0, DateTimeKind.Utc), 1, "????? ??? ????? ???????", "Customer segment pricing", 5, true, "Customer Segment Pricing", 4, null, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_TbPricingSystemSettings_CurrentState",
                table: "TbPricingSystemSettings",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbPricingSystemSettings_SystemType",
                table: "TbPricingSystemSettings",
                column: "SystemType",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TbPricingSystemSettings");

            migrationBuilder.DropColumn(
                name: "PricingSystemType",
                table: "TbCategories");

            migrationBuilder.AlterColumn<int>(
                name: "DisplayOrder",
                table: "TbCategories",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);
        }
    }
}
