using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class addSystemSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TbSettings");

            migrationBuilder.AddColumn<Guid>(
                name: "WarehouseId",
                table: "TbOffers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<string>(
                name: "ReviewNumber",
                table: "TbItemReviews",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "TbGeneralSettings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    FacebookUrl = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    InstagramUrl = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    TwitterUrl = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    LinkedInUrl = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    WhatsAppNumber = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true),
                    SEOTitle = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    SEODescription = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    SEOMetaTags = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbGeneralSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TbSystemSettings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    SettingKey = table.Column<int>(type: "int", nullable: false),
                    SettingValue = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    DataType = table.Column<int>(type: "int", nullable: false),
                    Category = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbSystemSettings", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TbGeneralSettings_Email",
                table: "TbGeneralSettings",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TbGeneralSettings_IsDeleted",
                table: "TbGeneralSettings",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbSystemSettings_Category_SettingKey",
                table: "TbSystemSettings",
                columns: new[] { "Category", "SettingKey" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TbSystemSettings_IsDeleted",
                table: "TbSystemSettings",
                column: "IsDeleted");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TbGeneralSettings");

            migrationBuilder.DropTable(
                name: "TbSystemSettings");

            migrationBuilder.DropColumn(
                name: "WarehouseId",
                table: "TbOffers");

            migrationBuilder.AlterColumn<int>(
                name: "ReviewNumber",
                table: "TbItemReviews",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateTable(
                name: "TbSettings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FacebookUrl = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    InstagramUrl = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    LinkedInUrl = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    MainBannerPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrderExtraCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OrderTaxPercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PhoneCode = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    SEODescription = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    SEOMetaTags = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    SEOTitle = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    ShippingAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TwitterUrl = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true),
                    WhatsAppCode = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    WhatsAppNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbSettings", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TbSettings_IsDeleted",
                table: "TbSettings",
                column: "IsDeleted");
        }
    }
}
