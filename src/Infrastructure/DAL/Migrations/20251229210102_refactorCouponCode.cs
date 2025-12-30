using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class refactorCouponCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TbOrderShipments_TbWarehouses_WarehouseId",
                table: "TbOrderShipments");

            migrationBuilder.DropIndex(
                name: "IX_TbOrderShipmentItems_OrderDetailId",
                table: "TbOrderShipmentItems");

            migrationBuilder.DropColumn(
                name: "DescriptionAr",
                table: "TbCouponCodes");

            migrationBuilder.DropColumn(
                name: "DescriptionEn",
                table: "TbCouponCodes");

            migrationBuilder.AlterColumn<Guid>(
                name: "WarehouseId",
                table: "TbOrderShipments",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TitleEn",
                table: "TbCouponCodes",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "TitleAr",
                table: "TbCouponCodes",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                table: "TbCouponCodes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2(2)",
                oldNullable: true,
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.AlterColumn<bool>(
                name: "IsFirstOrderOnly",
                table: "TbCouponCodes",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "TbCouponCodes",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ExpiryDate",
                table: "TbCouponCodes",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2(2)",
                oldNullable: true,
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "TbCouponCodes",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<decimal>(
                name: "MinimumProductPrice",
                table: "TbCouponCodes",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PlatformSharePercentage",
                table: "TbCouponCodes",
                type: "decimal(5,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PromoType",
                table: "TbCouponCodes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "VendorId",
                table: "TbCouponCodes",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CouponCodeScopes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    CouponCodeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ScopeType = table.Column<int>(type: "int", nullable: false),
                    ScopeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CouponCodeScopes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CouponCodeScopes_TbCouponCodes_CouponCodeId",
                        column: x => x.CouponCodeId,
                        principalTable: "TbCouponCodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TbOrderShipmentItems_OrderDetailId",
                table: "TbOrderShipmentItems",
                column: "OrderDetailId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TbCouponCodes_Active_Dates",
                table: "TbCouponCodes",
                columns: new[] { "IsActive", "StartDate", "ExpiryDate" });

            migrationBuilder.CreateIndex(
                name: "IX_TbCouponCodes_VendorId",
                table: "TbCouponCodes",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_CouponCodeScope_CouponCodeId",
                table: "CouponCodeScopes",
                column: "CouponCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_CouponCodeScope_CouponId_Type_ScopeId",
                table: "CouponCodeScopes",
                columns: new[] { "CouponCodeId", "ScopeType", "ScopeId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CouponCodeScopes_IsDeleted",
                table: "CouponCodeScopes",
                column: "IsDeleted");

            migrationBuilder.AddForeignKey(
                name: "FK_TbCouponCodes_TbVendors_VendorId",
                table: "TbCouponCodes",
                column: "VendorId",
                principalTable: "TbVendors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbOrderShipments_TbWarehouses_WarehouseId",
                table: "TbOrderShipments",
                column: "WarehouseId",
                principalTable: "TbWarehouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TbCouponCodes_TbVendors_VendorId",
                table: "TbCouponCodes");

            migrationBuilder.DropForeignKey(
                name: "FK_TbOrderShipments_TbWarehouses_WarehouseId",
                table: "TbOrderShipments");

            migrationBuilder.DropTable(
                name: "CouponCodeScopes");

            migrationBuilder.DropIndex(
                name: "IX_TbOrderShipmentItems_OrderDetailId",
                table: "TbOrderShipmentItems");

            migrationBuilder.DropIndex(
                name: "IX_TbCouponCodes_Active_Dates",
                table: "TbCouponCodes");

            migrationBuilder.DropIndex(
                name: "IX_TbCouponCodes_VendorId",
                table: "TbCouponCodes");

            migrationBuilder.DropColumn(
                name: "MinimumProductPrice",
                table: "TbCouponCodes");

            migrationBuilder.DropColumn(
                name: "PlatformSharePercentage",
                table: "TbCouponCodes");

            migrationBuilder.DropColumn(
                name: "PromoType",
                table: "TbCouponCodes");

            migrationBuilder.DropColumn(
                name: "VendorId",
                table: "TbCouponCodes");

            migrationBuilder.AlterColumn<Guid>(
                name: "WarehouseId",
                table: "TbOrderShipments",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<string>(
                name: "TitleEn",
                table: "TbCouponCodes",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "TitleAr",
                table: "TbCouponCodes",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                table: "TbCouponCodes",
                type: "datetime2(2)",
                nullable: true,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<bool>(
                name: "IsFirstOrderOnly",
                table: "TbCouponCodes",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "TbCouponCodes",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ExpiryDate",
                table: "TbCouponCodes",
                type: "datetime2(2)",
                nullable: true,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "TbCouponCodes",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionAr",
                table: "TbCouponCodes",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionEn",
                table: "TbCouponCodes",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TbOrderShipmentItems_OrderDetailId",
                table: "TbOrderShipmentItems",
                column: "OrderDetailId");

            migrationBuilder.AddForeignKey(
                name: "FK_TbOrderShipments_TbWarehouses_WarehouseId",
                table: "TbOrderShipments",
                column: "WarehouseId",
                principalTable: "TbWarehouses",
                principalColumn: "Id");
        }
    }
}
