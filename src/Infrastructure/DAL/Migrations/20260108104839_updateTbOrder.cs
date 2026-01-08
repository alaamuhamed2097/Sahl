using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class updateTbOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_OrderShipments_Number",
                table: "TbOrderShipments");

            migrationBuilder.DropColumn(
                name: "ShipmentNumber",
                table: "TbOrderShipments");

            migrationBuilder.RenameColumn(
                name: "TaxPrecentage",
                table: "TbOrders",
                newName: "SubTotal");

            migrationBuilder.RenameColumn(
                name: "PaymentDate",
                table: "TbOrders",
                newName: "PaidAt");

            migrationBuilder.AlterColumn<string>(
                name: "TrackingNumber",
                table: "TbOrderShipments",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "TbOrderShipments",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Number",
                table: "TbOrderShipments",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Number",
                table: "TbOrders",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<decimal>(
                name: "DiscountAmount",
                table: "TbOrders",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "TbOrders",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TaxPercentage",
                table: "TbOrders",
                type: "decimal(5,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<string>(
                name: "TransactionId",
                table: "TbOrderPayments",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "TbOrderPayments",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FailureReason",
                table: "TbOrderPayments",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GatewayTransactionId",
                table: "TbOrderPayments",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PaymentMethodType",
                table: "TbOrderPayments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "TbShipmentStatusHistory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    ShipmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    StatusDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbShipmentStatusHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbShipmentStatusHistory_TbOrderShipments_ShipmentId",
                        column: x => x.ShipmentId,
                        principalTable: "TbOrderShipments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderShipments_Number",
                table: "TbOrderShipments",
                column: "Number");

            migrationBuilder.CreateIndex(
                name: "IX_TbShipmentStatusHistory_IsDeleted",
                table: "TbShipmentStatusHistory",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbShipmentStatusHistory_ShipmentId",
                table: "TbShipmentStatusHistory",
                column: "ShipmentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TbShipmentStatusHistory");

            migrationBuilder.DropIndex(
                name: "IX_OrderShipments_Number",
                table: "TbOrderShipments");

            migrationBuilder.DropColumn(
                name: "Number",
                table: "TbOrderShipments");

            migrationBuilder.DropColumn(
                name: "DiscountAmount",
                table: "TbOrders");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "TbOrders");

            migrationBuilder.DropColumn(
                name: "TaxPercentage",
                table: "TbOrders");

            migrationBuilder.DropColumn(
                name: "FailureReason",
                table: "TbOrderPayments");

            migrationBuilder.DropColumn(
                name: "GatewayTransactionId",
                table: "TbOrderPayments");

            migrationBuilder.DropColumn(
                name: "PaymentMethodType",
                table: "TbOrderPayments");

            migrationBuilder.RenameColumn(
                name: "SubTotal",
                table: "TbOrders",
                newName: "TaxPrecentage");

            migrationBuilder.RenameColumn(
                name: "PaidAt",
                table: "TbOrders",
                newName: "PaymentDate");

            migrationBuilder.AlterColumn<string>(
                name: "TrackingNumber",
                table: "TbOrderShipments",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "TbOrderShipments",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShipmentNumber",
                table: "TbOrderShipments",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Number",
                table: "TbOrders",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "TransactionId",
                table: "TbOrderPayments",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "TbOrderPayments",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderShipments_Number",
                table: "TbOrderShipments",
                column: "ShipmentNumber");
        }
    }
}
