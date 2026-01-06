using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class AlterTbRefunds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TbOrders_TbCouponCodes_TbCouponCodeId",
                table: "TbOrders");

            migrationBuilder.DropIndex(
                name: "IX_TbOrders_TbCouponCodeId",
                table: "TbOrders");

            migrationBuilder.DropColumn(
                name: "TbCouponCodeId",
                table: "TbOrders");

            migrationBuilder.RenameColumn(
                name: "ReturnedDate",
                table: "TbRefunds",
                newName: "ReturnedDateUTC");

            migrationBuilder.RenameColumn(
                name: "RequestDate",
                table: "TbRefunds",
                newName: "RequestDateUTC");

            migrationBuilder.RenameColumn(
                name: "RefundedDate",
                table: "TbRefunds",
                newName: "RefundedDateUTC");

            migrationBuilder.RenameColumn(
                name: "ApprovedDate",
                table: "TbRefunds",
                newName: "ApprovedDateUTC");

            migrationBuilder.RenameIndex(
                name: "IX_TbRefunds_RequestDate",
                table: "TbRefunds",
                newName: "IX_TbRefunds_RequestDateUTC");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ReturnedDateUTC",
                table: "TbRefunds",
                newName: "ReturnedDate");

            migrationBuilder.RenameColumn(
                name: "RequestDateUTC",
                table: "TbRefunds",
                newName: "RequestDate");

            migrationBuilder.RenameColumn(
                name: "RefundedDateUTC",
                table: "TbRefunds",
                newName: "RefundedDate");

            migrationBuilder.RenameColumn(
                name: "ApprovedDateUTC",
                table: "TbRefunds",
                newName: "ApprovedDate");

            migrationBuilder.RenameIndex(
                name: "IX_TbRefunds_RequestDateUTC",
                table: "TbRefunds",
                newName: "IX_TbRefunds_RequestDate");

            migrationBuilder.AddColumn<Guid>(
                name: "TbCouponCodeId",
                table: "TbOrders",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TbCustomerAddressId",
                table: "TbOrders",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TbOrders_TbCouponCodeId",
                table: "TbOrders",
                column: "TbCouponCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_TbOrders_TbCustomerAddressId",
                table: "TbOrders",
                column: "TbCustomerAddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_TbOrders_TbCouponCodes_TbCouponCodeId",
                table: "TbOrders",
                column: "TbCouponCodeId",
                principalTable: "TbCouponCodes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TbOrders_TbCustomerAddresses_TbCustomerAddressId",
                table: "TbOrders",
                column: "TbCustomerAddressId",
                principalTable: "TbCustomerAddresses",
                principalColumn: "Id");
        }
    }
}
