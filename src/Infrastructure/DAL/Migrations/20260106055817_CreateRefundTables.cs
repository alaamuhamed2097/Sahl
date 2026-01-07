using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class CreateRefundTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_TbWalletTransactions_TbRefundRequests_RefundId",
            //    table: "TbWalletTransactions");

            migrationBuilder.DropTable(
                name: "TbRefundRequests");

            migrationBuilder.AddColumn<decimal>(
                name: "TaxPrecentage",
                table: "TbOrders",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<string>(
                name: "ReviewNumber",
                table: "TbItemReviews",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "TbRefunds",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    Number = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    OrderDetailId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeliveryAddressId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VendorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RefundReason = table.Column<int>(type: "int", nullable: false),
                    RefundReasonDetails = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    RejectionReason = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    RefundStatus = table.Column<int>(type: "int", nullable: false),
                    ReturnShippingCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    RefundAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    RequestedItemsCount = table.Column<int>(type: "int", nullable: false),
                    ApprovedItemsCount = table.Column<int>(type: "int", nullable: false),
                    RefundTransactionId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReturnTrackingNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequestDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ApprovedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReturnedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RefundedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AdminNotes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    AdminUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbRefunds", x => x.Id);
                    table.CheckConstraint("CK_Refund_Amounts", "[RefundAmount] >= 0 AND [ReturnShippingCost] >= 0");
                    table.CheckConstraint("CK_Refund_ItemsCount", "[ApprovedItemsCount] <= [RequestedItemsCount]");
                    table.ForeignKey(
                        name: "FK_TbRefunds_AspNetUsers_AdminUserId",
                        column: x => x.AdminUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_TbRefunds_TbCustomerAddresses_DeliveryAddressId",
                        column: x => x.DeliveryAddressId,
                        principalTable: "TbCustomerAddresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TbRefunds_TbCustomer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "TbCustomer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TbRefunds_TbOrderDetails_OrderDetailId",
                        column: x => x.OrderDetailId,
                        principalTable: "TbOrderDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TbRefunds_TbVendors_VendorId",
                        column: x => x.VendorId,
                        principalTable: "TbVendors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TbRefundItemVideos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    RefundId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VideoUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbRefundItemVideos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbRefundItemVideos_TbRefunds_RefundId",
                        column: x => x.RefundId,
                        principalTable: "TbRefunds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TbRefundStatusHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    RefundId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OldStatus = table.Column<int>(type: "int", nullable: false),
                    NewStatus = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbRefundStatusHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbRefundStatusHistories_TbRefunds_RefundId",
                        column: x => x.RefundId,
                        principalTable: "TbRefunds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TbRefundItemVideos_IsDeleted",
                table: "TbRefundItemVideos",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbRefundItemVideos_RefundId",
                table: "TbRefundItemVideos",
                column: "RefundId");

            migrationBuilder.CreateIndex(
                name: "IX_TbRefunds_AdminUserId",
                table: "TbRefunds",
                column: "AdminUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TbRefunds_CustomerId",
                table: "TbRefunds",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_TbRefunds_CustomerId_RefundStatus",
                table: "TbRefunds",
                columns: new[] { "CustomerId", "RefundStatus" });

            migrationBuilder.CreateIndex(
                name: "IX_TbRefunds_DeliveryAddressId",
                table: "TbRefunds",
                column: "DeliveryAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_TbRefunds_IsDeleted",
                table: "TbRefunds",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbRefunds_Number",
                table: "TbRefunds",
                column: "Number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TbRefunds_OrderDetailId",
                table: "TbRefunds",
                column: "OrderDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_TbRefunds_RequestDate",
                table: "TbRefunds",
                column: "RequestDate");

            migrationBuilder.CreateIndex(
                name: "IX_TbRefunds_VendorId",
                table: "TbRefunds",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_TbRefunds_VendorId_RefundStatus",
                table: "TbRefunds",
                columns: new[] { "VendorId", "RefundStatus" });

            migrationBuilder.CreateIndex(
                name: "IX_TbRefundStatusHistories_IsDeleted",
                table: "TbRefundStatusHistories",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbRefundStatusHistories_RefundId",
                table: "TbRefundStatusHistories",
                column: "RefundId");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_TbWalletTransactions_TbRefunds_RefundId",
            //    table: "TbWalletTransactions",
            //    column: "RefundId",
            //    principalTable: "TbRefunds",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TbOrders_TbCustomerAddresses_TbCustomerAddressId",
                table: "TbOrders");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_TbWalletTransactions_TbRefunds_RefundId",
            //    table: "TbWalletTransactions");

            migrationBuilder.DropTable(
                name: "TbRefundItemVideos");

            migrationBuilder.DropTable(
                name: "TbRefundStatusHistories");

            migrationBuilder.DropTable(
                name: "TbRefunds");

            migrationBuilder.DropIndex(
                name: "IX_TbOrders_TbCustomerAddressId",
                table: "TbOrders");

            migrationBuilder.DropColumn(
                name: "TaxPrecentage",
                table: "TbOrders");

            migrationBuilder.DropColumn(
                name: "TbCustomerAddressId",
                table: "TbOrders");

            migrationBuilder.AlterColumn<int>(
                name: "ReviewNumber",
                table: "TbItemReviews",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateTable(
                name: "TbRefundRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AdminNotes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    AdminUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CustomerNotes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ProcessedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RefundAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RefundCompletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RefundFailureReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    RefundReason = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    RefundReasonDetails = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    RefundStatus = table.Column<int>(type: "int", nullable: false),
                    RefundTransactionId = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    RequestedAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbRefundRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbRefundRequests_TbOrders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "TbOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TbRefundRequests_IsDeleted",
                table: "TbRefundRequests",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbRefundRequests_OrderId",
                table: "TbRefundRequests",
                column: "OrderId");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_TbWalletTransactions_TbRefundRequests_RefundId",
            //    table: "TbWalletTransactions",
            //    column: "RefundId",
            //    principalTable: "TbRefundRequests",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Restrict);
        }
    }
}
