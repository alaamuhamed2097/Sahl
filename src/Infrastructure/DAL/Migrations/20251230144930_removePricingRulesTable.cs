using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class removePricingRulesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TbMediaContents");

            migrationBuilder.DropTable(
                name: "TbContentAreas");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TbContentAreas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    AreaCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    DescriptionAr = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DescriptionEn = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    TitleAr = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TitleEn = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbContentAreas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TbMediaContents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    ContentAreaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    DescriptionAr = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DescriptionEn = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    LinkUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    MediaPath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    MediaType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Image"),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TitleAr = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TitleEn = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbMediaContents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbMediaContents_TbContentAreas_ContentAreaId",
                        column: x => x.ContentAreaId,
                        principalTable: "TbContentAreas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TbContentAreas_AreaCode",
                table: "TbContentAreas",
                column: "AreaCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TbContentAreas_DisplayOrder",
                table: "TbContentAreas",
                column: "DisplayOrder");

            migrationBuilder.CreateIndex(
                name: "IX_TbContentAreas_IsActive",
                table: "TbContentAreas",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_TbContentAreas_IsDeleted",
                table: "TbContentAreas",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbMediaContents_ContentAreaId",
                table: "TbMediaContents",
                column: "ContentAreaId");

            migrationBuilder.CreateIndex(
                name: "IX_TbMediaContents_DisplayOrder",
                table: "TbMediaContents",
                column: "DisplayOrder");

            migrationBuilder.CreateIndex(
                name: "IX_TbMediaContents_IsActive",
                table: "TbMediaContents",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_TbMediaContents_IsDeleted",
                table: "TbMediaContents",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbMediaContents_MediaType",
                table: "TbMediaContents",
                column: "MediaType");

            migrationBuilder.CreateIndex(
                name: "IX_TbQuantityPricings_IsActive",
                table: "TbQuantityPricings",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_TbQuantityPricings_IsDeleted",
                table: "TbQuantityPricings",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbQuantityPricings_MinimumQuantity",
                table: "TbQuantityPricings",
                column: "MinimumQuantity");

            migrationBuilder.CreateIndex(
                name: "IX_TbQuantityPricings_OfferId",
                table: "TbQuantityPricings",
                column: "OfferId");

            migrationBuilder.CreateIndex(
                name: "IX_TbQuantityPricings_OfferId_MinimumQuantity",
                table: "TbQuantityPricings",
                columns: new[] { "OfferId", "MinimumQuantity" },
                unique: true);
        }
    }
}
