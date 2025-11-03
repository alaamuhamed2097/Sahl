using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddCategoryTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TbAttributes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    TitleAr = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TitleEn = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsRangeFieldType = table.Column<bool>(type: "bit", nullable: false),
                    FieldType = table.Column<int>(type: "int", nullable: false),
                    MaxLength = table.Column<int>(type: "int", nullable: false),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbAttributes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TbCategories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    TitleAr = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TitleEn = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsFinal = table.Column<bool>(type: "bit", nullable: false),
                    IsHomeCategory = table.Column<bool>(type: "bit", nullable: false),
                    IsFeaturedCategory = table.Column<bool>(type: "bit", nullable: false),
                    IsMainCategory = table.Column<bool>(type: "bit", nullable: false),
                    PriceRequired = table.Column<bool>(type: "bit", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    TreeViewSerial = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Icon = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TbAttributeOptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    TitleAr = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TitleEn = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    AttributeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbAttributeOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbAttributeOptions_TbAttributes_AttributeId",
                        column: x => x.AttributeId,
                        principalTable: "TbAttributes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TbCategoryAttributes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AttributeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AffectsPricing = table.Column<bool>(type: "bit", nullable: false),
                    IsRequired = table.Column<bool>(type: "bit", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbCategoryAttributes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbCategoryAttributes_TbAttributes_AttributeId",
                        column: x => x.AttributeId,
                        principalTable: "TbAttributes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TbCategoryAttributes_TbCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "TbCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TbAttributeOptions_AttributeId",
                table: "TbAttributeOptions",
                column: "AttributeId");

            migrationBuilder.CreateIndex(
                name: "IX_TbAttributeOptions_CurrentState",
                table: "TbAttributeOptions",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbAttributes_CurrentState",
                table: "TbAttributes",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbCategories_CurrentState",
                table: "TbCategories",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbCategoryAttributes_AttributeId",
                table: "TbCategoryAttributes",
                column: "AttributeId");

            migrationBuilder.CreateIndex(
                name: "IX_TbCategoryAttributes_CategoryId",
                table: "TbCategoryAttributes",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_TbCategoryAttributes_CurrentState",
                table: "TbCategoryAttributes",
                column: "CurrentState");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TbAttributeOptions");

            migrationBuilder.DropTable(
                name: "TbCategoryAttributes");

            migrationBuilder.DropTable(
                name: "TbAttributes");

            migrationBuilder.DropTable(
                name: "TbCategories");
        }
    }
}
