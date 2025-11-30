using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class CreateNewCatalogTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TbItemAttributes_TbAttributes_AttributeId",
                table: "TbItemAttribute");

            migrationBuilder.DropForeignKey(
                name: "FK_TbItemAttributes_TbItems_ItemId",
                table: "TbItemAttribute");

            migrationBuilder.DropForeignKey(
                name: "FK_TbOrder_TbPromoCodes_PromoCodeId",
                table: "TbOrder");

            migrationBuilder.DropTable(
                name: "TbPromoCodes");

            migrationBuilder.DropIndex(
                name: "IX_TbItems_IsBestSeller",
                table: "TbItems");

            migrationBuilder.DropIndex(
                name: "IX_TbItems_IsRecommended",
                table: "TbItems");

            migrationBuilder.DropIndex(
                name: "IX_TbBrands_IsFavorite",
                table: "TbBrands");

            migrationBuilder.DropColumn(
                name: "IsBestSeller",
                table: "TbItems");

            migrationBuilder.DropColumn(
                name: "IsRecommended",
                table: "TbItems");

            migrationBuilder.DropColumn(
                name: "StockStatus",
                table: "TbItems");

            migrationBuilder.DropColumn(
                name: "IsFavorite",
                table: "TbBrands");

            migrationBuilder.RenameColumn(
                name: "PromoCodeId",
                table: "TbOrder",
                newName: "CouponId");

            migrationBuilder.RenameIndex(
                name: "IX_TbOrder_PromoCodeId",
                table: "TbOrder",
                newName: "IX_TbOrder_CouponId");

            migrationBuilder.AlterColumn<string>(
                name: "VideoLink",
                table: "TbItems",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "UnitId",
                table: "TbItems",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "BrandId",
                table: "TbItems",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<string>(
                name: "Barcode",
                table: "TbItems",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "MaximumPrice",
                table: "TbItems",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "MinimumPrice",
                table: "TbItems",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SKU",
                table: "TbItems",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "SubCategoryId",
                table: "TbItems",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "VisibilityScope",
                table: "TbItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "TbItemAttribute",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AddColumn<int>(
                name: "FieldType",
                table: "TbItemAttribute",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsRangeFieldType",
                table: "TbItemAttribute",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "MaxLength",
                table: "TbItemAttribute",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "TitleAr",
                table: "TbItemAttribute",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TitleEn",
                table: "TbItemAttribute",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsRoot",
                table: "TbCategories",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "WebsiteUrl",
                table: "TbBrands",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TitleEn",
                table: "TbBrands",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TitleAr",
                table: "TbBrands",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DescriptionEn",
                table: "TbBrands",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DescriptionAr",
                table: "TbBrands",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsPopular",
                table: "TbBrands",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<int>(
                name: "MaxLength",
                table: "TbAttributes",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "TbCoupons",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    TitleAR = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TitleEN = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Value = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StartDateUTC = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    EndDateUTC = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UsageLimit = table.Column<int>(type: "int", nullable: true),
                    UsageCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    CouponCodeType = table.Column<int>(type: "int", nullable: false),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbCoupons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TbItemCombination",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    ItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbItemCombination", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbItemCombination_TbItems_ItemId",
                        column: x => x.ItemId,
                        principalTable: "TbItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TbCombinationAttribute",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    ItemCombinationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbCombinationAttribute", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbCombinationAttribute_TbItemCombination_ItemCombinationId",
                        column: x => x.ItemCombinationId,
                        principalTable: "TbItemCombination",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TbCombinationAttributesValue",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    CombinationAttributeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AttributeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbCombinationAttributesValue", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbCombinationAttributesValue_TbAttributes_AttributeId",
                        column: x => x.AttributeId,
                        principalTable: "TbAttributes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TbCombinationAttributesValue_TbCombinationAttribute_CombinationAttributeId",
                        column: x => x.CombinationAttributeId,
                        principalTable: "TbCombinationAttribute",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TbCombinationAttribute_CurrentState",
                table: "TbCombinationAttribute",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbCombinationAttribute_ItemCombinationId",
                table: "TbCombinationAttribute",
                column: "ItemCombinationId");

            migrationBuilder.CreateIndex(
                name: "IX_TbCombinationAttributesValue_AttributeId",
                table: "TbCombinationAttributesValue",
                column: "AttributeId");

            migrationBuilder.CreateIndex(
                name: "IX_TbCombinationAttributesValue_CombinationAttributeId",
                table: "TbCombinationAttributesValue",
                column: "CombinationAttributeId");

            migrationBuilder.CreateIndex(
                name: "IX_TbCombinationAttributesValue_CurrentState",
                table: "TbCombinationAttributesValue",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbCoupons_Code",
                table: "TbCoupons",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TbCoupons_CurrentState",
                table: "TbCoupons",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbItemCombination_CurrentState",
                table: "TbItemCombination",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbItemCombination_ItemId",
                table: "TbItemCombination",
                column: "ItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_TbItemAttribute_TbAttributes_AttributeId",
                table: "TbItemAttribute",
                column: "AttributeId",
                principalTable: "TbAttributes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbItemAttribute_TbItems_ItemId",
                table: "TbItemAttribute",
                column: "ItemId",
                principalTable: "TbItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbOrder_TbCoupons_CouponId",
                table: "TbOrder",
                column: "CouponId",
                principalTable: "TbCoupons",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TbItemAttribute_TbAttributes_AttributeId",
                table: "TbItemAttribute");

            migrationBuilder.DropForeignKey(
                name: "FK_TbItemAttribute_TbItems_ItemId",
                table: "TbItemAttribute");

            migrationBuilder.DropForeignKey(
                name: "FK_TbOrder_TbCoupons_CouponId",
                table: "TbOrder");

            migrationBuilder.DropTable(
                name: "TbCombinationAttributesValue");

            migrationBuilder.DropTable(
                name: "TbCoupons");

            migrationBuilder.DropTable(
                name: "TbCombinationAttribute");

            migrationBuilder.DropTable(
                name: "TbItemCombination");

            migrationBuilder.DropColumn(
                name: "Barcode",
                table: "TbItems");

            migrationBuilder.DropColumn(
                name: "MaximumPrice",
                table: "TbItems");

            migrationBuilder.DropColumn(
                name: "MinimumPrice",
                table: "TbItems");

            migrationBuilder.DropColumn(
                name: "SKU",
                table: "TbItems");

            migrationBuilder.DropColumn(
                name: "SubCategoryId",
                table: "TbItems");

            migrationBuilder.DropColumn(
                name: "VisibilityScope",
                table: "TbItems");

            migrationBuilder.DropColumn(
                name: "FieldType",
                table: "TbItemAttribute");

            migrationBuilder.DropColumn(
                name: "IsRangeFieldType",
                table: "TbItemAttribute");

            migrationBuilder.DropColumn(
                name: "MaxLength",
                table: "TbItemAttribute");

            migrationBuilder.DropColumn(
                name: "TitleAr",
                table: "TbItemAttribute");

            migrationBuilder.DropColumn(
                name: "TitleEn",
                table: "TbItemAttribute");

            migrationBuilder.DropColumn(
                name: "IsRoot",
                table: "TbCategories");

            migrationBuilder.DropColumn(
                name: "IsPopular",
                table: "TbBrands");

            migrationBuilder.RenameColumn(
                name: "CouponId",
                table: "TbOrder",
                newName: "PromoCodeId");

            migrationBuilder.RenameIndex(
                name: "IX_TbOrder_CouponId",
                table: "TbOrder",
                newName: "IX_TbOrder_PromoCodeId");

            migrationBuilder.AlterColumn<string>(
                name: "VideoLink",
                table: "TbItems",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<Guid>(
                name: "UnitId",
                table: "TbItems",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "BrandId",
                table: "TbItems",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsBestSeller",
                table: "TbItems",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsRecommended",
                table: "TbItems",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "StockStatus",
                table: "TbItems",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "TbItemAttribute",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "WebsiteUrl",
                table: "TbBrands",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "TitleEn",
                table: "TbBrands",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "TitleAr",
                table: "TbBrands",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "DescriptionEn",
                table: "TbBrands",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "DescriptionAr",
                table: "TbBrands",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<bool>(
                name: "IsFavorite",
                table: "TbBrands",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<int>(
                name: "MaxLength",
                table: "TbAttributes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "TbPromoCodes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    Code = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    EndDateUTC = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    PromoCodeType = table.Column<int>(type: "int", nullable: false),
                    StartDateUTC = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    TitleAR = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TitleEN = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true),
                    UsageCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    UsageLimit = table.Column<int>(type: "int", nullable: true),
                    Value = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbPromoCodes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TbItems_IsBestSeller",
                table: "TbItems",
                column: "IsBestSeller");

            migrationBuilder.CreateIndex(
                name: "IX_TbItems_IsRecommended",
                table: "TbItems",
                column: "IsRecommended");

            migrationBuilder.CreateIndex(
                name: "IX_TbBrands_IsFavorite",
                table: "TbBrands",
                column: "IsFavorite");

            migrationBuilder.CreateIndex(
                name: "IX_TbPromoCodes_Code",
                table: "TbPromoCodes",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TbPromoCodes_CurrentState",
                table: "TbPromoCodes",
                column: "CurrentState");

            migrationBuilder.AddForeignKey(
                name: "FK_TbItemAttributes_TbAttributes_AttributeId",
                table: "TbItemAttribute",
                column: "AttributeId",
                principalTable: "TbAttributes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbItemAttributes_TbItems_ItemId",
                table: "TbItemAttribute",
                column: "ItemId",
                principalTable: "TbItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbOrder_TbPromoCodes_PromoCodeId",
                table: "TbOrder",
                column: "PromoCodeId",
                principalTable: "TbPromoCodes",
                principalColumn: "Id");
        }
    }
}
