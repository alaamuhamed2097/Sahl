using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class updateTbPricingSystemSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DescriptionAr",
                table: "TbPricingSystemSettings");

            migrationBuilder.DropColumn(
                name: "DescriptionEn",
                table: "TbPricingSystemSettings");

            migrationBuilder.RenameColumn(
                name: "SystemName",
                table: "TbPricingSystemSettings",
                newName: "SystemNameEn");

            migrationBuilder.AddColumn<string>(
                name: "SystemNameAr",
                table: "TbPricingSystemSettings",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "TbPricingSystemSettings",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "SystemNameAr",
                value: "التسعير القياسي");

            migrationBuilder.UpdateData(
                table: "TbPricingSystemSettings",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "SystemNameAr",
                value: "تسعير بالتركيبات");

            migrationBuilder.UpdateData(
                table: "TbPricingSystemSettings",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "SystemNameAr",
                value: "تسعير حسب الكمية");

            migrationBuilder.UpdateData(
                table: "TbPricingSystemSettings",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                column: "SystemNameAr",
                value: "التركيبات مع مستويات الكمية");

            migrationBuilder.UpdateData(
                table: "TbPricingSystemSettings",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                column: "SystemNameAr",
                value: "تسعير حسب شريحة العميل");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SystemNameAr",
                table: "TbPricingSystemSettings");

            migrationBuilder.RenameColumn(
                name: "SystemNameEn",
                table: "TbPricingSystemSettings",
                newName: "SystemName");

            migrationBuilder.AddColumn<string>(
                name: "DescriptionAr",
                table: "TbPricingSystemSettings",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionEn",
                table: "TbPricingSystemSettings",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "TbPricingSystemSettings",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                columns: new[] { "DescriptionAr", "DescriptionEn" },
                values: new object[] { "????? ???? - ??? ?????", "Standard pricing - Price and quantity" });

            migrationBuilder.UpdateData(
                table: "TbPricingSystemSettings",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                columns: new[] { "DescriptionAr", "DescriptionEn" },
                values: new object[] { "????? ??????????", "Combination-based pricing" });

            migrationBuilder.UpdateData(
                table: "TbPricingSystemSettings",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                columns: new[] { "DescriptionAr", "DescriptionEn" },
                values: new object[] { "????? ????????", "Quantity tier pricing" });

            migrationBuilder.UpdateData(
                table: "TbPricingSystemSettings",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                columns: new[] { "DescriptionAr", "DescriptionEn" },
                values: new object[] { "??????? ?? ?????", "Combinations with quantity tiers" });

            migrationBuilder.UpdateData(
                table: "TbPricingSystemSettings",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                columns: new[] { "DescriptionAr", "DescriptionEn" },
                values: new object[] { "????? ??? ????? ???????", "Customer segment pricing" });
        }
    }
}
