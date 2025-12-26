using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class PreviewMyChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TbReviewReports_TbItemReviews_ItemReviewId",
                table: "TbReviewReports");

            migrationBuilder.DropTable(
                name: "TbDeliveryReviews");

            migrationBuilder.DropIndex(
                name: "IX_TbItemReviews_IsVerifiedPurchase",
                table: "TbItemReviews");

            migrationBuilder.DropColumn(
                name: "IsVerifiedPurchase",
                table: "TbItemReviews");

            migrationBuilder.RenameColumn(
                name: "OrderItemId",
                table: "TbItemReviews",
                newName: "TbVendorId");

            migrationBuilder.RenameIndex(
                name: "IX_TbItemReviews_OrderItemId",
                table: "TbItemReviews",
                newName: "IX_TbItemReviews_TbVendorId");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "TbCustomer",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250,
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "TbShippingCompanyReview",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderDetailId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ShippingCompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Rating = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ReviewText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsEdited = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbShippingCompanyReview", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbShippingCompanyReview_TbCustomer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "TbCustomer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TbShippingCompanyReview_TbOrderDetails_OrderDetailId",
                        column: x => x.OrderDetailId,
                        principalTable: "TbOrderDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TbShippingCompanyReview_TbShippingCompanies_ShippingCompanyId",
                        column: x => x.ShippingCompanyId,
                        principalTable: "TbShippingCompanies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TbVendorReviews",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VendorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderDetailId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Rating = table.Column<decimal>(type: "decimal(2,1)", nullable: false),
                    ReviewText = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IsEdited = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    IsVerifiedPurchase = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    TbOrderDetailId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbVendorReviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbVendorReviews_TbCustomer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "TbCustomer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TbVendorReviews_TbOrderDetails_OrderDetailId",
                        column: x => x.OrderDetailId,
                        principalTable: "TbOrderDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TbVendorReviews_TbOrderDetails_TbOrderDetailId",
                        column: x => x.TbOrderDetailId,
                        principalTable: "TbOrderDetails",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TbVendorReviews_TbVendors_VendorId",
                        column: x => x.VendorId,
                        principalTable: "TbVendors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TbCustomer_UserId",
                table: "TbCustomer",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TbShippingCompanyReview_CustomerId",
                table: "TbShippingCompanyReview",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_TbShippingCompanyReview_IsDeleted",
                table: "TbShippingCompanyReview",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbShippingCompanyReview_OrderDetailId",
                table: "TbShippingCompanyReview",
                column: "OrderDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_TbShippingCompanyReview_ShippingCompanyId",
                table: "TbShippingCompanyReview",
                column: "ShippingCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_TbVendorReviews_CustomerId",
                table: "TbVendorReviews",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_TbVendorReviews_CustomerId_VendorId",
                table: "TbVendorReviews",
                columns: new[] { "CustomerId", "VendorId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TbVendorReviews_IsDeleted",
                table: "TbVendorReviews",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbVendorReviews_OrderDetailId",
                table: "TbVendorReviews",
                column: "OrderDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_TbVendorReviews_Rating",
                table: "TbVendorReviews",
                column: "Rating");

            migrationBuilder.CreateIndex(
                name: "IX_TbVendorReviews_Status",
                table: "TbVendorReviews",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_TbVendorReviews_TbOrderDetailId",
                table: "TbVendorReviews",
                column: "TbOrderDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_TbVendorReviews_VendorId",
                table: "TbVendorReviews",
                column: "VendorId");

            migrationBuilder.AddForeignKey(
                name: "FK_TbCustomer_AspNetUsers_UserId",
                table: "TbCustomer",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbItemReviews_TbCustomer_CustomerId",
                table: "TbItemReviews",
                column: "CustomerId",
                principalTable: "TbCustomer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbItemReviews_TbItems_ItemId",
                table: "TbItemReviews",
                column: "ItemId",
                principalTable: "TbItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbItemReviews_TbVendors_TbVendorId",
                table: "TbItemReviews",
                column: "TbVendorId",
                principalTable: "TbVendors",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TbReviewReports_TbCustomer_CustomerId",
                table: "TbReviewReports",
                column: "CustomerId",
                principalTable: "TbCustomer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbReviewReports_TbItemReviews_ItemReviewId",
                table: "TbReviewReports",
                column: "ItemReviewId",
                principalTable: "TbItemReviews",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TbCustomer_AspNetUsers_UserId",
                table: "TbCustomer");

            migrationBuilder.DropForeignKey(
                name: "FK_TbItemReviews_TbCustomer_CustomerId",
                table: "TbItemReviews");

            migrationBuilder.DropForeignKey(
                name: "FK_TbItemReviews_TbItems_ItemId",
                table: "TbItemReviews");

            migrationBuilder.DropForeignKey(
                name: "FK_TbItemReviews_TbVendors_TbVendorId",
                table: "TbItemReviews");

            migrationBuilder.DropForeignKey(
                name: "FK_TbReviewReports_TbCustomer_CustomerId",
                table: "TbReviewReports");

            migrationBuilder.DropForeignKey(
                name: "FK_TbReviewReports_TbItemReviews_ItemReviewId",
                table: "TbReviewReports");

            migrationBuilder.DropTable(
                name: "TbShippingCompanyReview");

            migrationBuilder.DropTable(
                name: "TbVendorReviews");

            migrationBuilder.DropIndex(
                name: "IX_TbCustomer_UserId",
                table: "TbCustomer");

            migrationBuilder.RenameColumn(
                name: "TbVendorId",
                table: "TbItemReviews",
                newName: "OrderItemId");

            migrationBuilder.RenameIndex(
                name: "IX_TbItemReviews_TbVendorId",
                table: "TbItemReviews",
                newName: "IX_TbItemReviews_OrderItemId");

            migrationBuilder.AddColumn<bool>(
                name: "IsVerifiedPurchase",
                table: "TbItemReviews",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "TbCustomer",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldMaxLength: 450);

            migrationBuilder.CreateTable(
                name: "TbDeliveryReviews",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    CourierDeliveryRating = table.Column<decimal>(type: "decimal(2,1)", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OverallRating = table.Column<decimal>(type: "decimal(2,1)", nullable: true),
                    PackagingRating = table.Column<decimal>(type: "decimal(2,1)", nullable: true),
                    ReviewDate = table.Column<DateTime>(type: "datetime2(2)", nullable: true),
                    ReviewNumber = table.Column<int>(type: "int", nullable: false),
                    ShippingAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbDeliveryReviews", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TbItemReviews_IsVerifiedPurchase",
                table: "TbItemReviews",
                column: "IsVerifiedPurchase");

            migrationBuilder.CreateIndex(
                name: "IX_TbDeliveryReviews_CustomerId",
                table: "TbDeliveryReviews",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_TbDeliveryReviews_IsDeleted",
                table: "TbDeliveryReviews",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbDeliveryReviews_OrderId",
                table: "TbDeliveryReviews",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_TbDeliveryReviews_OrderId_CustomerId",
                table: "TbDeliveryReviews",
                columns: new[] { "OrderId", "CustomerId" });

            migrationBuilder.CreateIndex(
                name: "IX_TbDeliveryReviews_OverallRating",
                table: "TbDeliveryReviews",
                column: "OverallRating");

            migrationBuilder.CreateIndex(
                name: "IX_TbDeliveryReviews_ReviewDate",
                table: "TbDeliveryReviews",
                column: "ReviewDate");

            migrationBuilder.CreateIndex(
                name: "IX_TbDeliveryReviews_ReviewNumber",
                table: "TbDeliveryReviews",
                column: "ReviewNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TbDeliveryReviews_Status",
                table: "TbDeliveryReviews",
                column: "Status");

            migrationBuilder.AddForeignKey(
                name: "FK_TbReviewReports_TbItemReviews_ItemReviewId",
                table: "TbReviewReports",
                column: "ItemReviewId",
                principalTable: "TbItemReviews",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
