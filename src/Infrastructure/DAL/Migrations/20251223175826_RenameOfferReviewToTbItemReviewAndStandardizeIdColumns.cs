using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class RenameOfferReviewToTbItemReviewAndStandardizeIdColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TbLoyaltyPointsTransactions_TbOfferReviews_ReviewId",
                table: "TbLoyaltyPointsTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_TbReviewReports_TbOfferReviews_ReviewID",
                table: "TbReviewReports");

            migrationBuilder.DropForeignKey(
                name: "FK_TbReviewVotes_TbOfferReviews_ReviewID",
                table: "TbReviewVotes");

            migrationBuilder.DropTable(
                name: "TbOfferReviews");

            migrationBuilder.RenameColumn(
                name: "VendorID",
                table: "TbSalesReviews",
                newName: "VendorId");

            migrationBuilder.RenameColumn(
                name: "OrderItemID",
                table: "TbSalesReviews",
                newName: "OrderItemId");

            migrationBuilder.RenameColumn(
                name: "CustomerID",
                table: "TbSalesReviews",
                newName: "CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_TbSalesReviews_VendorID_CustomerID",
                table: "TbSalesReviews",
                newName: "IX_TbSalesReviews_VendorId_CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_TbSalesReviews_VendorID",
                table: "TbSalesReviews",
                newName: "IX_TbSalesReviews_VendorId");

            migrationBuilder.RenameIndex(
                name: "IX_TbSalesReviews_OrderItemID",
                table: "TbSalesReviews",
                newName: "IX_TbSalesReviews_OrderItemId");

            migrationBuilder.RenameIndex(
                name: "IX_TbSalesReviews_CustomerID",
                table: "TbSalesReviews",
                newName: "IX_TbSalesReviews_CustomerId");

            migrationBuilder.RenameColumn(
                name: "CustomerID",
                table: "TbReviewVotes",
                newName: "CustomerId");

            migrationBuilder.RenameColumn(
                name: "ReviewID",
                table: "TbReviewVotes",
                newName: "ItemReviewId");

            migrationBuilder.RenameIndex(
                name: "IX_TbReviewVotes_CustomerID",
                table: "TbReviewVotes",
                newName: "IX_TbReviewVotes_CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_TbReviewVotes_ReviewID_CustomerID_VoteType",
                table: "TbReviewVotes",
                newName: "IX_TbReviewVotes_ItemReviewId_CustomerId_VoteType");

            migrationBuilder.RenameIndex(
                name: "IX_TbReviewVotes_ReviewID",
                table: "TbReviewVotes",
                newName: "IX_TbReviewVotes_ItemReviewId");

            migrationBuilder.RenameColumn(
                name: "ReportID",
                table: "TbReviewReports",
                newName: "ReportId");

            migrationBuilder.RenameColumn(
                name: "CustomerID",
                table: "TbReviewReports",
                newName: "CustomerId");

            migrationBuilder.RenameColumn(
                name: "ReviewID",
                table: "TbReviewReports",
                newName: "ItemReviewId");

            migrationBuilder.RenameIndex(
                name: "IX_TbReviewReports_ReportID",
                table: "TbReviewReports",
                newName: "IX_TbReviewReports_ReportId");

            migrationBuilder.RenameIndex(
                name: "IX_TbReviewReports_CustomerID",
                table: "TbReviewReports",
                newName: "IX_TbReviewReports_CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_TbReviewReports_ReviewID",
                table: "TbReviewReports",
                newName: "IX_TbReviewReports_ItemReviewId");

            migrationBuilder.RenameColumn(
                name: "OrderID",
                table: "TbDeliveryReviews",
                newName: "OrderId");

            migrationBuilder.RenameColumn(
                name: "CustomerID",
                table: "TbDeliveryReviews",
                newName: "CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_TbDeliveryReviews_OrderID_CustomerID",
                table: "TbDeliveryReviews",
                newName: "IX_TbDeliveryReviews_OrderId_CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_TbDeliveryReviews_OrderID",
                table: "TbDeliveryReviews",
                newName: "IX_TbDeliveryReviews_OrderId");

            migrationBuilder.RenameIndex(
                name: "IX_TbDeliveryReviews_CustomerID",
                table: "TbDeliveryReviews",
                newName: "IX_TbDeliveryReviews_CustomerId");

            migrationBuilder.CreateTable(
                name: "TbItemReviews",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    ReviewNumber = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Rating = table.Column<decimal>(type: "decimal(2,1)", nullable: false),
                    ReviewTitle = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ReviewText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsVerifiedPurchase = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    HelpfulCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    NotHelpfulCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Status = table.Column<int>(type: "int", nullable: false),
                    IsEdited = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbItemReviews", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TbItemReviews_CustomerId",
                table: "TbItemReviews",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_TbItemReviews_IsDeleted",
                table: "TbItemReviews",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbItemReviews_IsVerifiedPurchase",
                table: "TbItemReviews",
                column: "IsVerifiedPurchase");

            migrationBuilder.CreateIndex(
                name: "IX_TbItemReviews_ItemId",
                table: "TbItemReviews",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_TbItemReviews_ItemId_CustomerId",
                table: "TbItemReviews",
                columns: new[] { "ItemId", "CustomerId" });

            migrationBuilder.CreateIndex(
                name: "IX_TbItemReviews_OrderItemId",
                table: "TbItemReviews",
                column: "OrderItemId");

            migrationBuilder.CreateIndex(
                name: "IX_TbItemReviews_Rating",
                table: "TbItemReviews",
                column: "Rating");

            migrationBuilder.CreateIndex(
                name: "IX_TbItemReviews_ReviewNumber",
                table: "TbItemReviews",
                column: "ReviewNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TbItemReviews_Status",
                table: "TbItemReviews",
                column: "Status");

            migrationBuilder.AddForeignKey(
                name: "FK_TbLoyaltyPointsTransactions_TbItemReviews_ReviewId",
                table: "TbLoyaltyPointsTransactions",
                column: "ReviewId",
                principalTable: "TbItemReviews",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbReviewReports_TbItemReviews_ItemReviewId",
                table: "TbReviewReports",
                column: "ItemReviewId",
                principalTable: "TbItemReviews",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbReviewVotes_TbItemReviews_ItemReviewId",
                table: "TbReviewVotes",
                column: "ItemReviewId",
                principalTable: "TbItemReviews",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TbLoyaltyPointsTransactions_TbItemReviews_ReviewId",
                table: "TbLoyaltyPointsTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_TbReviewReports_TbItemReviews_ItemReviewId",
                table: "TbReviewReports");

            migrationBuilder.DropForeignKey(
                name: "FK_TbReviewVotes_TbItemReviews_ItemReviewId",
                table: "TbReviewVotes");

            migrationBuilder.DropTable(
                name: "TbItemReviews");

            migrationBuilder.RenameColumn(
                name: "VendorId",
                table: "TbSalesReviews",
                newName: "VendorID");

            migrationBuilder.RenameColumn(
                name: "OrderItemId",
                table: "TbSalesReviews",
                newName: "OrderItemID");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "TbSalesReviews",
                newName: "CustomerID");

            migrationBuilder.RenameIndex(
                name: "IX_TbSalesReviews_VendorId_CustomerId",
                table: "TbSalesReviews",
                newName: "IX_TbSalesReviews_VendorID_CustomerID");

            migrationBuilder.RenameIndex(
                name: "IX_TbSalesReviews_VendorId",
                table: "TbSalesReviews",
                newName: "IX_TbSalesReviews_VendorID");

            migrationBuilder.RenameIndex(
                name: "IX_TbSalesReviews_OrderItemId",
                table: "TbSalesReviews",
                newName: "IX_TbSalesReviews_OrderItemID");

            migrationBuilder.RenameIndex(
                name: "IX_TbSalesReviews_CustomerId",
                table: "TbSalesReviews",
                newName: "IX_TbSalesReviews_CustomerID");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "TbReviewVotes",
                newName: "CustomerID");

            migrationBuilder.RenameColumn(
                name: "ItemReviewId",
                table: "TbReviewVotes",
                newName: "ReviewID");

            migrationBuilder.RenameIndex(
                name: "IX_TbReviewVotes_CustomerId",
                table: "TbReviewVotes",
                newName: "IX_TbReviewVotes_CustomerID");

            migrationBuilder.RenameIndex(
                name: "IX_TbReviewVotes_ItemReviewId_CustomerId_VoteType",
                table: "TbReviewVotes",
                newName: "IX_TbReviewVotes_ReviewID_CustomerID_VoteType");

            migrationBuilder.RenameIndex(
                name: "IX_TbReviewVotes_ItemReviewId",
                table: "TbReviewVotes",
                newName: "IX_TbReviewVotes_ReviewID");

            migrationBuilder.RenameColumn(
                name: "ReportId",
                table: "TbReviewReports",
                newName: "ReportID");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "TbReviewReports",
                newName: "CustomerID");

            migrationBuilder.RenameColumn(
                name: "ItemReviewId",
                table: "TbReviewReports",
                newName: "ReviewID");

            migrationBuilder.RenameIndex(
                name: "IX_TbReviewReports_ReportId",
                table: "TbReviewReports",
                newName: "IX_TbReviewReports_ReportID");

            migrationBuilder.RenameIndex(
                name: "IX_TbReviewReports_CustomerId",
                table: "TbReviewReports",
                newName: "IX_TbReviewReports_CustomerID");

            migrationBuilder.RenameIndex(
                name: "IX_TbReviewReports_ItemReviewId",
                table: "TbReviewReports",
                newName: "IX_TbReviewReports_ReviewID");

            migrationBuilder.RenameColumn(
                name: "OrderId",
                table: "TbDeliveryReviews",
                newName: "OrderID");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "TbDeliveryReviews",
                newName: "CustomerID");

            migrationBuilder.RenameIndex(
                name: "IX_TbDeliveryReviews_OrderId_CustomerId",
                table: "TbDeliveryReviews",
                newName: "IX_TbDeliveryReviews_OrderID_CustomerID");

            migrationBuilder.RenameIndex(
                name: "IX_TbDeliveryReviews_OrderId",
                table: "TbDeliveryReviews",
                newName: "IX_TbDeliveryReviews_OrderID");

            migrationBuilder.RenameIndex(
                name: "IX_TbDeliveryReviews_CustomerId",
                table: "TbDeliveryReviews",
                newName: "IX_TbDeliveryReviews_CustomerID");

            migrationBuilder.CreateTable(
                name: "TbOfferReviews",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CustomerID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HelpfulCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsEdited = table.Column<bool>(type: "bit", nullable: false),
                    IsVerifiedPurchase = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    NotHelpfulCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    OfferID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderItemID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Rating = table.Column<decimal>(type: "decimal(2,1)", nullable: false),
                    ReviewNumber = table.Column<int>(type: "int", nullable: false),
                    ReviewText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReviewTitle = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbOfferReviews", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TbOfferReviews_CustomerID",
                table: "TbOfferReviews",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_TbOfferReviews_IsDeleted",
                table: "TbOfferReviews",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbOfferReviews_IsVerifiedPurchase",
                table: "TbOfferReviews",
                column: "IsVerifiedPurchase");

            migrationBuilder.CreateIndex(
                name: "IX_TbOfferReviews_OfferID",
                table: "TbOfferReviews",
                column: "OfferID");

            migrationBuilder.CreateIndex(
                name: "IX_TbOfferReviews_OfferID_CustomerID",
                table: "TbOfferReviews",
                columns: new[] { "OfferID", "CustomerID" });

            migrationBuilder.CreateIndex(
                name: "IX_TbOfferReviews_OrderItemID",
                table: "TbOfferReviews",
                column: "OrderItemID");

            migrationBuilder.CreateIndex(
                name: "IX_TbOfferReviews_Rating",
                table: "TbOfferReviews",
                column: "Rating");

            migrationBuilder.CreateIndex(
                name: "IX_TbOfferReviews_ReviewNumber",
                table: "TbOfferReviews",
                column: "ReviewNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TbOfferReviews_Status",
                table: "TbOfferReviews",
                column: "Status");

            migrationBuilder.AddForeignKey(
                name: "FK_TbLoyaltyPointsTransactions_TbOfferReviews_ReviewId",
                table: "TbLoyaltyPointsTransactions",
                column: "ReviewId",
                principalTable: "TbOfferReviews",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbReviewReports_TbOfferReviews_ReviewID",
                table: "TbReviewReports",
                column: "ReviewID",
                principalTable: "TbOfferReviews",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbReviewVotes_TbOfferReviews_ReviewID",
                table: "TbReviewVotes",
                column: "ReviewID",
                principalTable: "TbOfferReviews",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
