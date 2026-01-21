using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class refactorOrderAndPaymentTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TbOrders_TbShippingCompanies_TbShippingCompanyId",
                table: "TbOrders");

            migrationBuilder.DropIndex(
                name: "IX_TbOrders_TbShippingCompanyId",
                table: "TbOrders");

            migrationBuilder.DropColumn(
                name: "InvoiceId",
                table: "TbOrders");

            migrationBuilder.DropColumn(
                name: "TbShippingCompanyId",
                table: "TbOrders");

            migrationBuilder.AddColumn<string>(
                name: "ApiKey",
                table: "TbShippingCompanies",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AverageDeliveryDays",
                table: "TbShippingCompanies",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "TbShippingCompanies",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "TbShippingCompanies",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "TrackingApiEndpoint",
                table: "TbShippingCompanies",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Website",
                table: "TbShippingCompanies",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "DiscountAmount",
                table: "TbOrderShipments",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TaxAmount",
                table: "TbOrderShipments",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TaxPercentage",
                table: "TbOrderShipments",
                type: "decimal(5,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalWeight",
                table: "TbOrderShipments",
                type: "decimal(10,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "CardPaidAmount",
                table: "TbOrders",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "CashPaidAmount",
                table: "TbOrders",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalPaidAmount",
                table: "TbOrders",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "WalletPaidAmount",
                table: "TbOrders",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<string>(
                name: "PaymentGatewayResponse",
                table: "TbOrderPayments",
                type: "nvarchar(4000)",
                maxLength: 4000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IpAddress",
                table: "TbOrderPayments",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastRetryAt",
                table: "TbOrderPayments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RefundTransactionId",
                table: "TbOrderPayments",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RetryAttempts",
                table: "TbOrderPayments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "WalletTransactionId",
                table: "TbOrderPayments",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TbShipmentPayments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    ShipmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PaymentStatus = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TransactionId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PaidAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    TbOrderShipmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbShipmentPayments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbShipmentPayments_TbOrderShipments_ShipmentId",
                        column: x => x.ShipmentId,
                        principalTable: "TbOrderShipments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TbShipmentPayments_TbOrderShipments_TbOrderShipmentId",
                        column: x => x.TbOrderShipmentId,
                        principalTable: "TbOrderShipments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TbShipmentPayments_TbOrders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "TbOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TbOrderPayments_WalletTransactionId",
                table: "TbOrderPayments",
                column: "WalletTransactionId",
                unique: true,
                filter: "[WalletTransactionId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ShipmentPayments_IsDeleted",
                table: "TbShipmentPayments",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_ShipmentPayments_OrderId",
                table: "TbShipmentPayments",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_ShipmentPayments_ShipmentId",
                table: "TbShipmentPayments",
                column: "ShipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_ShipmentPayments_Status",
                table: "TbShipmentPayments",
                column: "PaymentStatus");

            migrationBuilder.CreateIndex(
                name: "IX_TbShipmentPayments_TbOrderShipmentId",
                table: "TbShipmentPayments",
                column: "TbOrderShipmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_TbOrderPayments_TbWalletTransactions_WalletTransactionId",
                table: "TbOrderPayments",
                column: "WalletTransactionId",
                principalTable: "TbWalletTransactions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TbOrderPayments_TbWalletTransactions_WalletTransactionId",
                table: "TbOrderPayments");

            migrationBuilder.DropTable(
                name: "TbShipmentPayments");

            migrationBuilder.DropIndex(
                name: "IX_TbOrderPayments_WalletTransactionId",
                table: "TbOrderPayments");

            migrationBuilder.DropColumn(
                name: "ApiKey",
                table: "TbShippingCompanies");

            migrationBuilder.DropColumn(
                name: "AverageDeliveryDays",
                table: "TbShippingCompanies");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "TbShippingCompanies");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "TbShippingCompanies");

            migrationBuilder.DropColumn(
                name: "TrackingApiEndpoint",
                table: "TbShippingCompanies");

            migrationBuilder.DropColumn(
                name: "Website",
                table: "TbShippingCompanies");

            migrationBuilder.DropColumn(
                name: "DiscountAmount",
                table: "TbOrderShipments");

            migrationBuilder.DropColumn(
                name: "TaxAmount",
                table: "TbOrderShipments");

            migrationBuilder.DropColumn(
                name: "TaxPercentage",
                table: "TbOrderShipments");

            migrationBuilder.DropColumn(
                name: "TotalWeight",
                table: "TbOrderShipments");

            migrationBuilder.DropColumn(
                name: "CardPaidAmount",
                table: "TbOrders");

            migrationBuilder.DropColumn(
                name: "CashPaidAmount",
                table: "TbOrders");

            migrationBuilder.DropColumn(
                name: "TotalPaidAmount",
                table: "TbOrders");

            migrationBuilder.DropColumn(
                name: "WalletPaidAmount",
                table: "TbOrders");

            migrationBuilder.DropColumn(
                name: "IpAddress",
                table: "TbOrderPayments");

            migrationBuilder.DropColumn(
                name: "LastRetryAt",
                table: "TbOrderPayments");

            migrationBuilder.DropColumn(
                name: "RefundTransactionId",
                table: "TbOrderPayments");

            migrationBuilder.DropColumn(
                name: "RetryAttempts",
                table: "TbOrderPayments");

            migrationBuilder.DropColumn(
                name: "WalletTransactionId",
                table: "TbOrderPayments");

            migrationBuilder.AddColumn<string>(
                name: "InvoiceId",
                table: "TbOrders",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TbShippingCompanyId",
                table: "TbOrders",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PaymentGatewayResponse",
                table: "TbOrderPayments",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(4000)",
                oldMaxLength: 4000,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TbOrders_TbShippingCompanyId",
                table: "TbOrders",
                column: "TbShippingCompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_TbOrders_TbShippingCompanies_TbShippingCompanyId",
                table: "TbOrders",
                column: "TbShippingCompanyId",
                principalTable: "TbShippingCompanies",
                principalColumn: "Id");
        }
    }
}
