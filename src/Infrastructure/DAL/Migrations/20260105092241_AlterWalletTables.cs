using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class AlterWalletTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TbCustomerWallets_TbCurrencies_CurrencyId",
                table: "TbCustomerWallets");

            migrationBuilder.DropForeignKey(
                name: "FK_TbWalletTransactions_AspNetUsers_ProcessedByUserId",
                table: "TbWalletTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_TbWalletTransactions_TbCustomerWallets_CustomerWalletId",
                table: "TbWalletTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_TbWalletTransactions_TbOrders_OrderId",
                table: "TbWalletTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_TbWalletTransactions_TbRefundRequests_RefundId",
                table: "TbWalletTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_TbWalletTransactions_TbVendorWallets_VendorWalletId",
                table: "TbWalletTransactions");

            migrationBuilder.DropTable(
                name: "TbPlatformTreasuries");

            migrationBuilder.DropTable(
                name: "TbVendorWallets");

            migrationBuilder.DropIndex(
                name: "IX_TbWalletTransactions_CreatedDateUtc",
                table: "TbWalletTransactions");

            migrationBuilder.DropIndex(
                name: "IX_TbWalletTransactions_CustomerWalletId",
                table: "TbWalletTransactions");

            migrationBuilder.DropIndex(
                name: "IX_TbWalletTransactions_CustomerWalletId_Status",
                table: "TbWalletTransactions");

            migrationBuilder.DropIndex(
                name: "IX_TbWalletTransactions_OrderId",
                table: "TbWalletTransactions");

            migrationBuilder.DropIndex(
                name: "IX_TbWalletTransactions_ProcessedByUserId",
                table: "TbWalletTransactions");

            migrationBuilder.DropIndex(
                name: "IX_TbWalletTransactions_ReferenceNumber",
                table: "TbWalletTransactions");

            migrationBuilder.DropIndex(
                name: "IX_TbWalletTransactions_RefundId",
                table: "TbWalletTransactions");

            migrationBuilder.DropIndex(
                name: "IX_TbWalletTransactions_VendorWalletId",
                table: "TbWalletTransactions");

            migrationBuilder.DropIndex(
                name: "IX_TbWalletTransactions_VendorWalletId_Status",
                table: "TbWalletTransactions");

            migrationBuilder.DropIndex(
                name: "IX_TbCustomerWallets_AvailableBalance",
                table: "TbCustomerWallets");

            migrationBuilder.DropIndex(
                name: "IX_TbCustomerWallets_CurrencyId",
                table: "TbCustomerWallets");

            migrationBuilder.DropColumn(
                name: "BalanceAfter",
                table: "TbWalletTransactions");

            migrationBuilder.DropColumn(
                name: "BalanceBefore",
                table: "TbWalletTransactions");

            migrationBuilder.DropColumn(
                name: "CustomerWalletId",
                table: "TbWalletTransactions");

            migrationBuilder.DropColumn(
                name: "DescriptionAr",
                table: "TbWalletTransactions");

            migrationBuilder.DropColumn(
                name: "DescriptionEn",
                table: "TbWalletTransactions");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "TbWalletTransactions");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "TbWalletTransactions");

            migrationBuilder.DropColumn(
                name: "ProcessedByUserId",
                table: "TbWalletTransactions");

            migrationBuilder.DropColumn(
                name: "ProcessedDate",
                table: "TbWalletTransactions");

            migrationBuilder.DropColumn(
                name: "ReferenceNumber",
                table: "TbWalletTransactions");

            migrationBuilder.DropColumn(
                name: "RefundId",
                table: "TbWalletTransactions");

            migrationBuilder.DropColumn(
                name: "VendorWalletId",
                table: "TbWalletTransactions");

            migrationBuilder.DropColumn(
                name: "AvailableBalance",
                table: "TbCustomerWallets");

            migrationBuilder.DropColumn(
                name: "CurrencyId",
                table: "TbCustomerWallets");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "TbWalletTransactions",
                newName: "TransactionStatus");

            migrationBuilder.RenameIndex(
                name: "IX_TbWalletTransactions_Status",
                table: "TbWalletTransactions",
                newName: "IX_TbWalletTransactions_TransactionStatus");

            migrationBuilder.RenameColumn(
                name: "TotalSpent",
                table: "TbCustomerWallets",
                newName: "LockedBalance");

            migrationBuilder.RenameColumn(
                name: "TotalEarned",
                table: "TbCustomerWallets",
                newName: "Balance");

            migrationBuilder.AddColumn<int>(
                name: "Direction",
                table: "TbWalletTransactions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "FeeAmount",
                table: "TbWalletTransactions",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<Guid>(
                name: "ReferenceId",
                table: "TbWalletTransactions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "ReferenceType",
                table: "TbWalletTransactions",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "WalletId",
                table: "TbWalletTransactions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "TbWalletChargingRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaymentMethodId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    GatewayTransactionId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FailureReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbWalletChargingRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbWalletChargingRequests_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TbWalletSettings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    MinChargingAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 10m),
                    MaxChargingAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 10000m),
                    MaxDailyChargingLimit = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 50000m),
                    ChargingFeePercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    ChargingFeeFixed = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    IsChargingEnabled = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsPaymentEnabled = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsTransferEnabled = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbWalletSettings", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TbWalletTransactions_ReferenceId",
                table: "TbWalletTransactions",
                column: "ReferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_TbWalletTransactions_WalletId",
                table: "TbWalletTransactions",
                column: "WalletId");

            migrationBuilder.CreateIndex(
                name: "IX_TbCustomerWallets_Balance",
                table: "TbCustomerWallets",
                column: "Balance");

            migrationBuilder.CreateIndex(
                name: "IX_TbCustomerWallets_LockedBalance",
                table: "TbCustomerWallets",
                column: "LockedBalance");

            migrationBuilder.CreateIndex(
                name: "IX_TbWalletChargingRequests_IsDeleted",
                table: "TbWalletChargingRequests",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbWalletChargingRequests_PaymentMethodId",
                table: "TbWalletChargingRequests",
                column: "PaymentMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_TbWalletChargingRequests_Status",
                table: "TbWalletChargingRequests",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_TbWalletChargingRequests_UserId",
                table: "TbWalletChargingRequests",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TbWalletSettings_IsDeleted",
                table: "TbWalletSettings",
                column: "IsDeleted");

            migrationBuilder.AddForeignKey(
                name: "FK_TbWalletTransactions_TbCustomerWallets_WalletId",
                table: "TbWalletTransactions",
                column: "WalletId",
                principalTable: "TbCustomerWallets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TbWalletTransactions_TbCustomerWallets_WalletId",
                table: "TbWalletTransactions");

            migrationBuilder.DropTable(
                name: "TbWalletChargingRequests");

            migrationBuilder.DropTable(
                name: "TbWalletSettings");

            migrationBuilder.DropIndex(
                name: "IX_TbWalletTransactions_ReferenceId",
                table: "TbWalletTransactions");

            migrationBuilder.DropIndex(
                name: "IX_TbWalletTransactions_WalletId",
                table: "TbWalletTransactions");

            migrationBuilder.DropIndex(
                name: "IX_TbCustomerWallets_Balance",
                table: "TbCustomerWallets");

            migrationBuilder.DropIndex(
                name: "IX_TbCustomerWallets_LockedBalance",
                table: "TbCustomerWallets");

            migrationBuilder.DropColumn(
                name: "Direction",
                table: "TbWalletTransactions");

            migrationBuilder.DropColumn(
                name: "FeeAmount",
                table: "TbWalletTransactions");

            migrationBuilder.DropColumn(
                name: "ReferenceId",
                table: "TbWalletTransactions");

            migrationBuilder.DropColumn(
                name: "ReferenceType",
                table: "TbWalletTransactions");

            migrationBuilder.DropColumn(
                name: "WalletId",
                table: "TbWalletTransactions");

            migrationBuilder.RenameColumn(
                name: "TransactionStatus",
                table: "TbWalletTransactions",
                newName: "Status");

            migrationBuilder.RenameIndex(
                name: "IX_TbWalletTransactions_TransactionStatus",
                table: "TbWalletTransactions",
                newName: "IX_TbWalletTransactions_Status");

            migrationBuilder.RenameColumn(
                name: "LockedBalance",
                table: "TbCustomerWallets",
                newName: "TotalSpent");

            migrationBuilder.RenameColumn(
                name: "Balance",
                table: "TbCustomerWallets",
                newName: "TotalEarned");

            migrationBuilder.AddColumn<decimal>(
                name: "BalanceAfter",
                table: "TbWalletTransactions",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "BalanceBefore",
                table: "TbWalletTransactions",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CustomerWalletId",
                table: "TbWalletTransactions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionAr",
                table: "TbWalletTransactions",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DescriptionEn",
                table: "TbWalletTransactions",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "TbWalletTransactions",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "OrderId",
                table: "TbWalletTransactions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProcessedByUserId",
                table: "TbWalletTransactions",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ProcessedDate",
                table: "TbWalletTransactions",
                type: "datetime2(2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReferenceNumber",
                table: "TbWalletTransactions",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "RefundId",
                table: "TbWalletTransactions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "VendorWalletId",
                table: "TbWalletTransactions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "AvailableBalance",
                table: "TbCustomerWallets",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<Guid>(
                name: "CurrencyId",
                table: "TbCustomerWallets",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "TbPlatformTreasuries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    CurrencyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CollectedCommissions = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CustomerWalletsTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    LastReconciliationDate = table.Column<DateTime>(type: "datetime2(2)", nullable: true),
                    LastUpdatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PendingCommissions = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    PendingPayouts = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    ProcessedPayouts = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    TotalBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    TotalCommissions = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalPayouts = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalRefunds = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalRevenue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true),
                    VendorWalletsTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbPlatformTreasuries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbPlatformTreasuries_TbCurrencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "TbCurrencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TbVendorWallets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    CurrencyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VendorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AvailableBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    LastTransactionDate = table.Column<DateTime>(type: "datetime2(2)", nullable: true),
                    LastWithdrawalDate = table.Column<DateTime>(type: "datetime2(2)", nullable: true),
                    PendingBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    TotalCommissionPaid = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    TotalEarned = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    TotalWithdrawn = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true),
                    WithdrawalFeePercentage = table.Column<decimal>(type: "decimal(5,2)", nullable: false, defaultValue: 2.5m)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbVendorWallets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbVendorWallets_TbCurrencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "TbCurrencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TbVendorWallets_TbVendors_VendorId",
                        column: x => x.VendorId,
                        principalTable: "TbVendors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TbWalletTransactions_CreatedDateUtc",
                table: "TbWalletTransactions",
                column: "CreatedDateUtc");

            migrationBuilder.CreateIndex(
                name: "IX_TbWalletTransactions_CustomerWalletId",
                table: "TbWalletTransactions",
                column: "CustomerWalletId");

            migrationBuilder.CreateIndex(
                name: "IX_TbWalletTransactions_CustomerWalletId_Status",
                table: "TbWalletTransactions",
                columns: new[] { "CustomerWalletId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_TbWalletTransactions_OrderId",
                table: "TbWalletTransactions",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_TbWalletTransactions_ProcessedByUserId",
                table: "TbWalletTransactions",
                column: "ProcessedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TbWalletTransactions_ReferenceNumber",
                table: "TbWalletTransactions",
                column: "ReferenceNumber",
                unique: true,
                filter: "[ReferenceNumber] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_TbWalletTransactions_RefundId",
                table: "TbWalletTransactions",
                column: "RefundId");

            migrationBuilder.CreateIndex(
                name: "IX_TbWalletTransactions_VendorWalletId",
                table: "TbWalletTransactions",
                column: "VendorWalletId");

            migrationBuilder.CreateIndex(
                name: "IX_TbWalletTransactions_VendorWalletId_Status",
                table: "TbWalletTransactions",
                columns: new[] { "VendorWalletId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_TbCustomerWallets_AvailableBalance",
                table: "TbCustomerWallets",
                column: "AvailableBalance");

            migrationBuilder.CreateIndex(
                name: "IX_TbCustomerWallets_CurrencyId",
                table: "TbCustomerWallets",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_TbPlatformTreasuries_CurrencyId",
                table: "TbPlatformTreasuries",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_TbPlatformTreasuries_IsDeleted",
                table: "TbPlatformTreasuries",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbPlatformTreasuries_LastReconciliationDate",
                table: "TbPlatformTreasuries",
                column: "LastReconciliationDate");

            migrationBuilder.CreateIndex(
                name: "IX_TbVendorWallets_AvailableBalance",
                table: "TbVendorWallets",
                column: "AvailableBalance");

            migrationBuilder.CreateIndex(
                name: "IX_TbVendorWallets_CurrencyId",
                table: "TbVendorWallets",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_TbVendorWallets_IsDeleted",
                table: "TbVendorWallets",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbVendorWallets_VendorId",
                table: "TbVendorWallets",
                column: "VendorId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TbCustomerWallets_TbCurrencies_CurrencyId",
                table: "TbCustomerWallets",
                column: "CurrencyId",
                principalTable: "TbCurrencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbWalletTransactions_AspNetUsers_ProcessedByUserId",
                table: "TbWalletTransactions",
                column: "ProcessedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbWalletTransactions_TbCustomerWallets_CustomerWalletId",
                table: "TbWalletTransactions",
                column: "CustomerWalletId",
                principalTable: "TbCustomerWallets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbWalletTransactions_TbOrders_OrderId",
                table: "TbWalletTransactions",
                column: "OrderId",
                principalTable: "TbOrders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbWalletTransactions_TbRefundRequests_RefundId",
                table: "TbWalletTransactions",
                column: "RefundId",
                principalTable: "TbRefundRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbWalletTransactions_TbVendorWallets_VendorWalletId",
                table: "TbWalletTransactions",
                column: "VendorWalletId",
                principalTable: "TbVendorWallets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
