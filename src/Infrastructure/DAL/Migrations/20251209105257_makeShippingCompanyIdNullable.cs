using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class makeShippingCompanyIdNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TbOrderShipments_TbShippingCompanies_ShippingCompanyId",
                table: "TbOrderShipments");

            migrationBuilder.AlterColumn<Guid>(
                name: "ShippingCompanyId",
                table: "TbOrderShipments",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_TbOrderShipments_TbShippingCompanies_ShippingCompanyId",
                table: "TbOrderShipments",
                column: "ShippingCompanyId",
                principalTable: "TbShippingCompanies",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TbOrderShipments_TbShippingCompanies_ShippingCompanyId",
                table: "TbOrderShipments");

            migrationBuilder.AlterColumn<Guid>(
                name: "ShippingCompanyId",
                table: "TbOrderShipments",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TbOrderShipments_TbShippingCompanies_ShippingCompanyId",
                table: "TbOrderShipments",
                column: "ShippingCompanyId",
                principalTable: "TbShippingCompanies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
