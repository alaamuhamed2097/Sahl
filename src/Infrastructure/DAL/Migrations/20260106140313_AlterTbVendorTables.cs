using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class AlterTbVendorTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CommercialRegister",
                table: "TbVendors");

            migrationBuilder.DropColumn(
                name: "Discription",
                table: "TbVendors");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "TbVendors");

            migrationBuilder.DropColumn(
                name: "NameAr",
                table: "TbVendors");

            migrationBuilder.DropColumn(
                name: "NameEn",
                table: "TbVendors");

            migrationBuilder.DropColumn(
                name: "TaxNumber",
                table: "TbVendors");

            migrationBuilder.DropColumn(
                name: "VendorCode",
                table: "TbVendors");

            migrationBuilder.RenameColumn(
                name: "VATRegistered",
                table: "TbVendors",
                newName: "IsRealEstateRegistered");

            migrationBuilder.AlterColumn<string>(
                name: "PostalCode",
                table: "TbVendors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "TbVendors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "BirthDate",
                table: "TbVendors",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "CityId",
                table: "TbVendors",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("15D5A12D-A75D-4E41-96BD-19D5836EE201"));

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "TbVendors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "IdentificationImageBackPath",
                table: "TbVendors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "IdentificationImageFrontPath",
                table: "TbVendors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "IdentificationNumber",
                table: "TbVendors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "IdentificationType",
                table: "TbVendors",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "TbVendors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MiddleName",
                table: "TbVendors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PhoneCode",
                table: "TbVendors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "TbVendors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "TbVendors",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "StoreName",
                table: "TbVendors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "VendorType",
                table: "TbVendors",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TbVendors_CityId",
                table: "TbVendors",
                column: "CityId");

            migrationBuilder.AddForeignKey(
                name: "FK_TbVendors_TbCities_CityId",
                table: "TbVendors",
                column: "CityId",
                principalTable: "TbCities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TbVendors_TbCities_CityId",
                table: "TbVendors");

            migrationBuilder.DropIndex(
                name: "IX_TbVendors_CityId",
                table: "TbVendors");

            migrationBuilder.DropColumn(
                name: "BirthDate",
                table: "TbVendors");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "TbVendors");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "TbVendors");

            migrationBuilder.DropColumn(
                name: "IdentificationImageBackPath",
                table: "TbVendors");

            migrationBuilder.DropColumn(
                name: "IdentificationImageFrontPath",
                table: "TbVendors");

            migrationBuilder.DropColumn(
                name: "IdentificationNumber",
                table: "TbVendors");

            migrationBuilder.DropColumn(
                name: "IdentificationType",
                table: "TbVendors");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "TbVendors");

            migrationBuilder.DropColumn(
                name: "MiddleName",
                table: "TbVendors");

            migrationBuilder.DropColumn(
                name: "PhoneCode",
                table: "TbVendors");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "TbVendors");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "TbVendors");

            migrationBuilder.DropColumn(
                name: "StoreName",
                table: "TbVendors");

            migrationBuilder.DropColumn(
                name: "VendorType",
                table: "TbVendors");

            migrationBuilder.RenameColumn(
                name: "IsRealEstateRegistered",
                table: "TbVendors",
                newName: "VATRegistered");

            migrationBuilder.AlterColumn<string>(
                name: "PostalCode",
                table: "TbVendors",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "TbVendors",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "CommercialRegister",
                table: "TbVendors",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discription",
                table: "TbVendors",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "TbVendors",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "NameAr",
                table: "TbVendors",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameEn",
                table: "TbVendors",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TaxNumber",
                table: "TbVendors",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VendorCode",
                table: "TbVendors",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
