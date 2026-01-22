using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class removeCurrencyId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TbOrderPayments_TbCurrencies_CurrencyId",
                table: "TbOrderPayments");

            migrationBuilder.DropIndex(
                name: "IX_OrderPayments_CurrencyId",
                table: "TbOrderPayments");

            migrationBuilder.DropColumn(
                name: "CurrencyId",
                table: "TbOrderPayments");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CurrencyId",
                table: "TbOrderPayments",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_OrderPayments_CurrencyId",
                table: "TbOrderPayments",
                column: "CurrencyId");

            migrationBuilder.AddForeignKey(
                name: "FK_TbOrderPayments_TbCurrencies_CurrencyId",
                table: "TbOrderPayments",
                column: "CurrencyId",
                principalTable: "TbCurrencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
