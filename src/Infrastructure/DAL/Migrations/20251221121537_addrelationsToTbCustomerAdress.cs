using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class addrelationsToTbCustomerAdress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM TbOrderDetails");
            migrationBuilder.Sql("DELETE FROM TbOrders WHERE DeliveryAddressId IS NOT NULL");
            migrationBuilder.Sql("DELETE FROM TbCustomerAddresses");


            migrationBuilder.DropForeignKey(
                name: "FK_TbCustomerAddresses_AspNetUsers_UserId",
                table: "TbCustomerAddresses");

            migrationBuilder.DropIndex(
                name: "IX_TbCustomerAddresses_UserId",
                table: "TbCustomerAddresses");

            migrationBuilder.RenameIndex(
                name: "IX_TbCustomerAddresses_IsDeleted",
                table: "TbCustomerAddresses",
                newName: "IX_CustomerAddress_IsDeleted");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "TbCustomerAddresses",
                type: "varchar(15)",
                unicode: false,
                maxLength: 15,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "PhoneCode",
                table: "TbCustomerAddresses",
                type: "varchar(4)",
                unicode: false,
                maxLength: 4,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(4)",
                oldMaxLength: 4);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDefault",
                table: "TbCustomerAddresses",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "TbCustomerAddresses",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TbCityId",
                table: "TbCustomerAddresses",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomerAddress_CityId",
                table: "TbCustomerAddresses",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerAddress_UserId_IsDefault_IsDeleted",
                table: "TbCustomerAddresses",
                columns: new[] { "UserId", "IsDefault", "IsDeleted" },
                filter: "[IsDefault] = 1 AND [IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerAddress_UserId_IsDeleted",
                table: "TbCustomerAddresses",
                columns: new[] { "UserId", "IsDeleted" },
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_TbCustomerAddresses_ApplicationUserId",
                table: "TbCustomerAddresses",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TbCustomerAddresses_TbCityId",
                table: "TbCustomerAddresses",
                column: "TbCityId");

            migrationBuilder.AddCheckConstraint(
                name: "CK_CustomerAddress_PhoneCode_Format",
                table: "TbCustomerAddresses",
                sql: "[PhoneCode] LIKE '+[0-9]%' OR [PhoneCode] NOT LIKE '%[^0-9]%'");

            migrationBuilder.AddCheckConstraint(
                name: "CK_CustomerAddress_PhoneNumber_Format",
                table: "TbCustomerAddresses",
                sql: "[PhoneNumber] NOT LIKE '%[^0-9]%'");

            migrationBuilder.AddForeignKey(
                name: "FK_TbCustomerAddresses_AspNetUsers_ApplicationUserId",
                table: "TbCustomerAddresses",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TbCustomerAddresses_AspNetUsers_UserId",
                table: "TbCustomerAddresses",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbCustomerAddresses_TbCities_CityId",
                table: "TbCustomerAddresses",
                column: "CityId",
                principalTable: "TbCities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbCustomerAddresses_TbCities_TbCityId",
                table: "TbCustomerAddresses",
                column: "TbCityId",
                principalTable: "TbCities",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TbCustomerAddresses_AspNetUsers_ApplicationUserId",
                table: "TbCustomerAddresses");

            migrationBuilder.DropForeignKey(
                name: "FK_TbCustomerAddresses_AspNetUsers_UserId",
                table: "TbCustomerAddresses");

            migrationBuilder.DropForeignKey(
                name: "FK_TbCustomerAddresses_TbCities_CityId",
                table: "TbCustomerAddresses");

            migrationBuilder.DropForeignKey(
                name: "FK_TbCustomerAddresses_TbCities_TbCityId",
                table: "TbCustomerAddresses");

            migrationBuilder.DropIndex(
                name: "IX_CustomerAddress_CityId",
                table: "TbCustomerAddresses");

            migrationBuilder.DropIndex(
                name: "IX_CustomerAddress_UserId_IsDefault_IsDeleted",
                table: "TbCustomerAddresses");

            migrationBuilder.DropIndex(
                name: "IX_CustomerAddress_UserId_IsDeleted",
                table: "TbCustomerAddresses");

            migrationBuilder.DropIndex(
                name: "IX_TbCustomerAddresses_ApplicationUserId",
                table: "TbCustomerAddresses");

            migrationBuilder.DropIndex(
                name: "IX_TbCustomerAddresses_TbCityId",
                table: "TbCustomerAddresses");

            migrationBuilder.DropCheckConstraint(
                name: "CK_CustomerAddress_PhoneCode_Format",
                table: "TbCustomerAddresses");

            migrationBuilder.DropCheckConstraint(
                name: "CK_CustomerAddress_PhoneNumber_Format",
                table: "TbCustomerAddresses");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "TbCustomerAddresses");

            migrationBuilder.DropColumn(
                name: "TbCityId",
                table: "TbCustomerAddresses");

            migrationBuilder.RenameIndex(
                name: "IX_CustomerAddress_IsDeleted",
                table: "TbCustomerAddresses",
                newName: "IX_TbCustomerAddresses_IsDeleted");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "TbCustomerAddresses",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(15)",
                oldUnicode: false,
                oldMaxLength: 15);

            migrationBuilder.AlterColumn<string>(
                name: "PhoneCode",
                table: "TbCustomerAddresses",
                type: "nvarchar(4)",
                maxLength: 4,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(4)",
                oldUnicode: false,
                oldMaxLength: 4);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDefault",
                table: "TbCustomerAddresses",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_TbCustomerAddresses_UserId",
                table: "TbCustomerAddresses",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TbCustomerAddresses_AspNetUsers_UserId",
                table: "TbCustomerAddresses",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
