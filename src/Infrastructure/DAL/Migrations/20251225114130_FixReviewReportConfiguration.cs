using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class FixReviewReportConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TbReviewReports_ReportId",
                table: "TbReviewReports");

            migrationBuilder.DropColumn(
                name: "ReportId",
                table: "TbReviewReports");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "TbReviewReports",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Details",
                table: "TbReviewReports",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TbCustomerId",
                table: "TbReviewReports",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TbReviewReports_CustomerId_ItemReviewId_Unique",
                table: "TbReviewReports",
                columns: new[] { "CustomerId", "ItemReviewId" },
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_TbReviewReports_Reason",
                table: "TbReviewReports",
                column: "Reason");

            migrationBuilder.CreateIndex(
                name: "IX_TbReviewReports_TbCustomerId",
                table: "TbReviewReports",
                column: "TbCustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_TbReviewReports_TbCustomer_TbCustomerId",
                table: "TbReviewReports",
                column: "TbCustomerId",
                principalTable: "TbCustomer",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TbReviewReports_TbCustomer_TbCustomerId",
                table: "TbReviewReports");

            migrationBuilder.DropIndex(
                name: "IX_TbReviewReports_CustomerId_ItemReviewId_Unique",
                table: "TbReviewReports");

            migrationBuilder.DropIndex(
                name: "IX_TbReviewReports_Reason",
                table: "TbReviewReports");

            migrationBuilder.DropIndex(
                name: "IX_TbReviewReports_TbCustomerId",
                table: "TbReviewReports");

            migrationBuilder.DropColumn(
                name: "TbCustomerId",
                table: "TbReviewReports");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "TbReviewReports",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Details",
                table: "TbReviewReports",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ReportId",
                table: "TbReviewReports",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_TbReviewReports_ReportId",
                table: "TbReviewReports",
                column: "ReportId",
                unique: true);
        }
    }
}
