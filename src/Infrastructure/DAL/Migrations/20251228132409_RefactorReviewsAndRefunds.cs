using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class RefactorReviewsAndRefunds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop foreign keys first
            migrationBuilder.DropForeignKey(name: "FK_TbRefundRequests_AspNetUsers_AdminUserId", table: "TbRefundRequests");

            // Drop indexes
            migrationBuilder.DropIndex(name: "IX_TbRefundRequests_AdminUserId", table: "TbRefundRequests");
            migrationBuilder.DropIndex(name: "IX_TbSalesReviews_VendorID_CustomerID", table: "TbSalesReviews");
            migrationBuilder.DropIndex(name: "IX_TbSalesReviews_VendorID", table: "TbSalesReviews");
            migrationBuilder.DropIndex(name: "IX_TbSalesReviews_OrderItemID", table: "TbSalesReviews");
            migrationBuilder.DropIndex(name: "IX_TbSalesReviews_CustomerID", table: "TbSalesReviews");
            migrationBuilder.DropIndex(name: "IX_TbReviewVotes_CustomerID", table: "TbReviewVotes");
            migrationBuilder.DropIndex(name: "IX_TbReviewReports_CustomerID", table: "TbReviewReports");

            // Drop columns
            migrationBuilder.DropColumn(name: "VendorType", table: "TbVendors");
            migrationBuilder.DropColumn(name: "RefundMethod", table: "TbRefundRequests");
            migrationBuilder.DropColumn(name: "UserName", table: "TbRefundRequests");
            migrationBuilder.DropColumn(name: "EndDateUTC", table: "TbCouponCodes");
            migrationBuilder.DropColumn(name: "StartDateUTC", table: "TbCouponCodes");

            // Rename columns
            migrationBuilder.RenameColumn(name: "Rating", table: "TbVendors", newName: "AverageRating");
            migrationBuilder.RenameColumn(name: "ContactName", table: "TbVendors", newName: "NameEn");
            migrationBuilder.RenameColumn(name: "CompanyName", table: "TbVendors", newName: "NameAr");
            migrationBuilder.RenameColumn(name: "VendorID", table: "TbSalesReviews", newName: "VendorId");
            migrationBuilder.RenameColumn(name: "OrderItemID", table: "TbSalesReviews", newName: "OrderItemId");
            migrationBuilder.RenameColumn(name: "CustomerID", table: "TbSalesReviews", newName: "CustomerId");
            migrationBuilder.RenameColumn(name: "CustomerID", table: "TbReviewVotes", newName: "CustomerId");
            migrationBuilder.RenameColumn(name: "AdminComments", table: "TbRefundRequests", newName: "RefundReasonDetails");
            migrationBuilder.RenameColumn(name: "TitleEN", table: "TbCouponCodes", newName: "TitleEn");
            migrationBuilder.RenameColumn(name: "TitleAR", table: "TbCouponCodes", newName: "TitleAr");
            migrationBuilder.RenameColumn(name: "Value", table: "TbCouponCodes", newName: "DiscountValue");
            migrationBuilder.RenameColumn(name: "CouponCodeType", table: "TbCouponCodes", newName: "DiscountType");

            // Add and alter columns
            migrationBuilder.AddColumn<string>(name: "Discription", table: "TbVendors", type: "nvarchar(max)", nullable: true);
            migrationBuilder.AlterColumn<int>(name: "Status", table: "TbReviewReports", type: "int", nullable: false, defaultValue: 0, oldClrType: typeof(int), oldType: "int");
            migrationBuilder.AlterColumn<string>(name: "Details", table: "TbReviewReports", type: "nvarchar(1000)", maxLength: 1000, nullable: true, oldClrType: typeof(string), oldType: "nvarchar(max)", oldNullable: true);
            migrationBuilder.AlterColumn<string>(name: "RefundReason", table: "TbRefundRequests", type: "nvarchar(200)", maxLength: 200, nullable: false, oldClrType: typeof(string), oldType: "nvarchar(500)", oldMaxLength: 500);
            migrationBuilder.AlterColumn<decimal>(name: "RefundAmount", table: "TbRefundRequests", type: "decimal(18,2)", nullable: false, defaultValue: 0m, oldClrType: typeof(decimal), oldType: "decimal(10,2)", oldNullable: true);
            migrationBuilder.AddColumn<string>(name: "AdminNotes", table: "TbRefundRequests", type: "nvarchar(1000)", maxLength: 1000, nullable: true);
            migrationBuilder.AddColumn<string>(name: "CustomerNotes", table: "TbRefundRequests", type: "nvarchar(1000)", maxLength: 1000, nullable: true);
            migrationBuilder.AddColumn<DateTime>(name: "RefundCompletedDate", table: "TbRefundRequests", type: "datetime2", nullable: true);
            migrationBuilder.AddColumn<string>(name: "RefundFailureReason", table: "TbRefundRequests", type: "nvarchar(500)", maxLength: 500, nullable: true);
            migrationBuilder.AddColumn<int>(name: "RefundStatus", table: "TbRefundRequests", type: "int", nullable: false, defaultValue: 0);
            migrationBuilder.AddColumn<string>(name: "RefundTransactionId", table: "TbRefundRequests", type: "nvarchar(200)", maxLength: 200, nullable: true);
            migrationBuilder.AddColumn<decimal>(name: "RequestedAmount", table: "TbRefundRequests", type: "decimal(18,2)", nullable: false, defaultValue: 0m);
            migrationBuilder.AddColumn<string>(name: "UserId", table: "TbRefundRequests", type: "nvarchar(450)", maxLength: 450, nullable: false, defaultValue: "");
            migrationBuilder.AddColumn<Guid>(name: "TbOrderId", table: "TbOrderPayments", type: "uniqueidentifier", nullable: true);
            migrationBuilder.AddColumn<string>(name: "Address", table: "TbCustomerAddresses", type: "nvarchar(200)", maxLength: 200, nullable: false, defaultValue: "");
            migrationBuilder.AlterColumn<string>(name: "UserId", table: "TbCustomer", type: "nvarchar(450)", maxLength: 450, nullable: false, defaultValue: "", oldClrType: typeof(string), oldType: "nvarchar(250)", oldMaxLength: 250, oldNullable: true);
            migrationBuilder.AddColumn<string>(name: "DescriptionAr", table: "TbCouponCodes", type: "nvarchar(500)", maxLength: 500, nullable: true);
            migrationBuilder.AddColumn<string>(name: "DescriptionEn", table: "TbCouponCodes", type: "nvarchar(500)", maxLength: 500, nullable: true);
            migrationBuilder.AddColumn<DateTime>(name: "ExpiryDate", table: "TbCouponCodes", type: "datetime2(2)", nullable: true, defaultValueSql: "GETUTCDATE()");
            migrationBuilder.AddColumn<bool>(name: "IsActive", table: "TbCouponCodes", type: "bit", nullable: false, defaultValue: false);
            migrationBuilder.AddColumn<bool>(name: "IsFirstOrderOnly", table: "TbCouponCodes", type: "bit", nullable: false, defaultValue: false);
            migrationBuilder.AddColumn<decimal>(name: "MaxDiscountAmount", table: "TbCouponCodes", type: "decimal(18,2)", nullable: true);
            migrationBuilder.AddColumn<decimal>(name: "MinimumOrderAmount", table: "TbCouponCodes", type: "decimal(18,2)", nullable: true);
            migrationBuilder.AddColumn<DateTime>(name: "StartDate", table: "TbCouponCodes", type: "datetime2(2)", nullable: true, defaultValueSql: "GETUTCDATE()");
            migrationBuilder.AddColumn<int>(name: "UsageLimitPerUser", table: "TbCouponCodes", type: "int", nullable: true);

            // Create indexes
            migrationBuilder.CreateIndex(name: "IX_TbSalesReviews_VendorId_CustomerId", table: "TbSalesReviews", columns: new[] { "VendorId", "CustomerId" });
            migrationBuilder.CreateIndex(name: "IX_TbSalesReviews_VendorId", table: "TbSalesReviews", column: "VendorId");
            migrationBuilder.CreateIndex(name: "IX_TbSalesReviews_OrderItemId", table: "TbSalesReviews", column: "OrderItemId");
            migrationBuilder.CreateIndex(name: "IX_TbSalesReviews_CustomerId", table: "TbSalesReviews", column: "CustomerId");
            migrationBuilder.CreateIndex(name: "IX_TbReviewVotes_CustomerId", table: "TbReviewVotes", column: "CustomerId");
            migrationBuilder.CreateIndex(name: "IX_TbReviewReports_CustomerId", table: "TbReviewReports", column: "CustomerId");
            migrationBuilder.CreateIndex(name: "IX_TbOrderPayments_TbOrderId", table: "TbOrderPayments", column: "TbOrderId");

            // Add foreign keys
            migrationBuilder.AddForeignKey(name: "FK_TbOrderPayments_TbOrders_TbOrderId", table: "TbOrderPayments", column: "TbOrderId", principalTable: "TbOrders", principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop foreign keys
            migrationBuilder.DropForeignKey(name: "FK_TbOrderPayments_TbOrders_TbOrderId", table: "TbOrderPayments");

            // Drop indexes
            migrationBuilder.DropIndex(name: "IX_TbSalesReviews_VendorId_CustomerId", table: "TbSalesReviews");
            migrationBuilder.DropIndex(name: "IX_TbSalesReviews_VendorId", table: "TbSalesReviews");
            migrationBuilder.DropIndex(name: "IX_TbSalesReviews_OrderItemId", table: "TbSalesReviews");
            migrationBuilder.DropIndex(name: "IX_TbSalesReviews_CustomerId", table: "TbSalesReviews");
            migrationBuilder.DropIndex(name: "IX_TbReviewVotes_CustomerId", table: "TbReviewVotes");
            migrationBuilder.DropIndex(name: "IX_TbReviewReports_CustomerId", table: "TbReviewReports");
            migrationBuilder.DropIndex(name: "IX_TbOrderPayments_TbOrderId", table: "TbOrderPayments");

            // Rename columns back
            migrationBuilder.RenameColumn(name: "AverageRating", table: "TbVendors", newName: "Rating");
            migrationBuilder.RenameColumn(name: "NameEn", table: "TbVendors", newName: "ContactName");
            migrationBuilder.RenameColumn(name: "NameAr", table: "TbVendors", newName: "CompanyName");
            migrationBuilder.RenameColumn(name: "VendorId", table: "TbSalesReviews", newName: "VendorID");
            migrationBuilder.RenameColumn(name: "OrderItemId", table: "TbSalesReviews", newName: "OrderItemID");
            migrationBuilder.RenameColumn(name: "CustomerId", table: "TbSalesReviews", newName: "CustomerID");
            migrationBuilder.RenameColumn(name: "CustomerId", table: "TbReviewVotes", newName: "CustomerID");
            migrationBuilder.RenameColumn(name: "RefundReasonDetails", table: "TbRefundRequests", newName: "AdminComments");
            migrationBuilder.RenameColumn(name: "TitleEn", table: "TbCouponCodes", newName: "TitleEN");
            migrationBuilder.RenameColumn(name: "TitleAr", table: "TbCouponCodes", newName: "TitleAR");
            migrationBuilder.RenameColumn(name: "DiscountValue", table: "TbCouponCodes", newName: "Value");
            migrationBuilder.RenameColumn(name: "DiscountType", table: "TbCouponCodes", newName: "CouponCodeType");

            // Drop added columns
            migrationBuilder.DropColumn(name: "Discription", table: "TbVendors");
            migrationBuilder.DropColumn(name: "AdminNotes", table: "TbRefundRequests");
            migrationBuilder.DropColumn(name: "CustomerNotes", table: "TbRefundRequests");
            migrationBuilder.DropColumn(name: "RefundCompletedDate", table: "TbRefundRequests");
            migrationBuilder.DropColumn(name: "RefundFailureReason", table: "TbRefundRequests");
            migrationBuilder.DropColumn(name: "RefundStatus", table: "TbRefundRequests");
            migrationBuilder.DropColumn(name: "RefundTransactionId", table: "TbRefundRequests");
            migrationBuilder.DropColumn(name: "RequestedAmount", table: "TbRefundRequests");
            migrationBuilder.DropColumn(name: "UserId", table: "TbRefundRequests");
            migrationBuilder.DropColumn(name: "TbOrderId", table: "TbOrderPayments");
            migrationBuilder.DropColumn(name: "Address", table: "TbCustomerAddresses");
            migrationBuilder.DropColumn(name: "DescriptionAr", table: "TbCouponCodes");
            migrationBuilder.DropColumn(name: "DescriptionEn", table: "TbCouponCodes");
            migrationBuilder.DropColumn(name: "ExpiryDate", table: "TbCouponCodes");
            migrationBuilder.DropColumn(name: "IsActive", table: "TbCouponCodes");
            migrationBuilder.DropColumn(name: "IsFirstOrderOnly", table: "TbCouponCodes");
            migrationBuilder.DropColumn(name: "MaxDiscountAmount", table: "TbCouponCodes");
            migrationBuilder.DropColumn(name: "MinimumOrderAmount", table: "TbCouponCodes");
            migrationBuilder.DropColumn(name: "StartDate", table: "TbCouponCodes");
            migrationBuilder.DropColumn(name: "UsageLimitPerUser", table: "TbCouponCodes");

            // Alter columns back to original state
            migrationBuilder.AlterColumn<int>(name: "Status", table: "TbReviewReports", type: "int", nullable: false, oldClrType: typeof(int), oldType: "int", oldDefaultValue: 0);
            migrationBuilder.AlterColumn<string>(name: "Details", table: "TbReviewReports", type: "nvarchar(max)", nullable: true, oldClrType: typeof(string), oldType: "nvarchar(1000)", oldMaxLength: 1000, oldNullable: true);
            migrationBuilder.AlterColumn<string>(name: "RefundReason", table: "TbRefundRequests", type: "nvarchar(500)", maxLength: 500, nullable: false, oldClrType: typeof(string), oldType: "nvarchar(200)", oldMaxLength: 200);
            migrationBuilder.AlterColumn<decimal>(name: "RefundAmount", table: "TbRefundRequests", type: "decimal(10,2)", nullable: true, oldClrType: typeof(decimal), oldType: "decimal(18,2)");
            migrationBuilder.AlterColumn<string>(name: "UserId", table: "TbCustomer", type: "nvarchar(250)", maxLength: 250, nullable: true, oldClrType: typeof(string), oldType: "nvarchar(450)", oldMaxLength: 450);

            // Add back dropped columns
            migrationBuilder.AddColumn<int>(name: "VendorType", table: "TbVendors", type: "int", nullable: false, defaultValue: 0);
            migrationBuilder.AddColumn<int>(name: "RefundMethod", table: "TbRefundRequests", type: "int", nullable: true);
            migrationBuilder.AddColumn<string>(name: "UserName", table: "TbRefundRequests", type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "");
            migrationBuilder.AddColumn<DateTime>(name: "EndDateUTC", table: "TbCouponCodes", type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()");
            migrationBuilder.AddColumn<DateTime>(name: "StartDateUTC", table: "TbCouponCodes", type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()");

            // Create indexes back
            migrationBuilder.CreateIndex(name: "IX_TbRefundRequests_AdminUserId", table: "TbRefundRequests", column: "AdminUserId");
            migrationBuilder.CreateIndex(name: "IX_TbSalesReviews_VendorID_CustomerID", table: "TbSalesReviews", columns: new[] { "VendorID", "CustomerID" });
            migrationBuilder.CreateIndex(name: "IX_TbSalesReviews_VendorID", table: "TbSalesReviews", column: "VendorID");
            migrationBuilder.CreateIndex(name: "IX_TbSalesReviews_OrderItemID", table: "TbSalesReviews", column: "OrderItemID");
            migrationBuilder.CreateIndex(name: "IX_TbSalesReviews_CustomerID", table: "TbSalesReviews", column: "CustomerID");
            migrationBuilder.CreateIndex(name: "IX_TbReviewVotes_CustomerID", table: "TbReviewVotes", column: "CustomerID");
            migrationBuilder.CreateIndex(name: "IX_TbReviewReports_CustomerID", table: "TbReviewReports", column: "CustomerID");

            // Add foreign keys back
            migrationBuilder.AddForeignKey(name: "FK_TbRefundRequests_AspNetUsers_AdminUserId", table: "TbRefundRequests", column: "AdminUserId", principalTable: "AspNetUsers", principalColumn: "Id");
        }
    }
}
