using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class addDevelopmentSettingsSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "TbDevelopmentSettings",
                columns: new[] { "Id", "CreatedBy", "IsMultiVendorSystem", "UpdatedBy", "UpdatedDateUtc" },
                values: new object[] { new Guid("11111111-1111-1111-1111-111111111111"), new Guid("00000000-0000-0000-0000-000000000000"), true, null, null });

            migrationBuilder.CreateIndex(
                name: "IX_TbDevelopmentSettings_IsMultiVendorSystem",
                table: "TbDevelopmentSettings",
                column: "IsMultiVendorSystem");

            migrationBuilder.AddCheckConstraint(
                name: "CK_TbDevelopmentSettings_SingleRow",
                table: "TbDevelopmentSettings",
                sql: "Id = '11111111-1111-1111-1111-111111111111'");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TbDevelopmentSettings_IsMultiVendorSystem",
                table: "TbDevelopmentSettings");

            migrationBuilder.DropCheckConstraint(
                name: "CK_TbDevelopmentSettings_SingleRow",
                table: "TbDevelopmentSettings");

            migrationBuilder.DeleteData(
                table: "TbDevelopmentSettings",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"));
        }
    }
}
