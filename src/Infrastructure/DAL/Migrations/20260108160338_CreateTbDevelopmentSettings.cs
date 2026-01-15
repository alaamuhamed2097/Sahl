using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class CreateTbDevelopmentSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"UPDATE TbOffers
SET WarehouseId = '7D96227E-F5CA-4B23-A0F5-751241FA0852',FulfillmentType = 1");
            migrationBuilder.AlterColumn<int>(
                name: "EstimatedDeliveryDays",
                table: "TbOffers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "TbDevelopmentSettings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    IsMultiVendorSystem = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbDevelopmentSettings", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TbOffers_WarehouseId",
                table: "TbOffers",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_TbDevelopmentSettings_IsDeleted",
                table: "TbDevelopmentSettings",
                column: "IsDeleted");

            migrationBuilder.AddForeignKey(
                name: "FK_TbOffers_TbWarehouses_WarehouseId",
                table: "TbOffers",
                column: "WarehouseId",
                principalTable: "TbWarehouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TbOffers_TbWarehouses_WarehouseId",
                table: "TbOffers");

            migrationBuilder.DropTable(
                name: "TbDevelopmentSettings");

            migrationBuilder.DropIndex(
                name: "IX_TbOffers_WarehouseId",
                table: "TbOffers");

            migrationBuilder.AlterColumn<int>(
                name: "EstimatedDeliveryDays",
                table: "TbOffers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
