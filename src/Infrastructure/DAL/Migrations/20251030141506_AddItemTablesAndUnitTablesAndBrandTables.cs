using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddItemTablesAndUnitTablesAndBrandTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TbBrands",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    NameEn = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TitleEn = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TitleAr = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DescriptionEn = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    DescriptionAr = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    LogoPath = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    WebsiteUrl = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IsFavorite = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbBrands", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TbUnits",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    TitleAr = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TitleEn = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbUnits", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TbVideoProviders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    TitleAr = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TitleEn = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbVideoProviders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TbUnitConversions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    FromUnitId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ToUnitId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ConversionFactor = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbUnitConversions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbUnitConversions_TbUnits_FromUnitId",
                        column: x => x.FromUnitId,
                        principalTable: "TbUnits",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TbUnitConversions_TbUnits_ToUnitId",
                        column: x => x.ToUnitId,
                        principalTable: "TbUnits",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TbItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    TitleAr = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TitleEn = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ShortDescriptionAr = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ShortDescriptionEn = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DescriptionAr = table.Column<string>(type: "nvarchar(max)", maxLength: 50000, nullable: false),
                    DescriptionEn = table.Column<string>(type: "nvarchar(max)", maxLength: 50000, nullable: false),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UnitId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BrandId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VideoProviderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    VideoLink = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ThumbnailImage = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    StockStatus = table.Column<bool>(type: "bit", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsNewArrival = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsBestSeller = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsRecommended = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true),
                    SEOTitle = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SEODescription = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    SEOMetaTags = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbItems_TbBrands_BrandId",
                        column: x => x.BrandId,
                        principalTable: "TbBrands",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TbItems_TbCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "TbCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TbItems_TbUnits_UnitId",
                        column: x => x.UnitId,
                        principalTable: "TbUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TbItems_TbVideoProviders_VideoProviderId",
                        column: x => x.VideoProviderId,
                        principalTable: "TbVideoProviders",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TbItemAttribute",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    ItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.PrimaryKey("PK_TbItemAttribute", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbItemAttributes_TbAttributes_AttributeId",
                        column: x => x.AttributeId,
                        principalTable: "TbAttributes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TbItemAttributes_TbItems_ItemId",
                        column: x => x.ItemId,
                        principalTable: "TbItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TbItemAttributeCombinationPricings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    ItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AttributeIds = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    FinalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbItemAttributeCombinationPricings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbItemAttributeCombinationPricings_TbItems_ItemId",
                        column: x => x.ItemId,
                        principalTable: "TbItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TbItemImages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    Path = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbItemImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbItemImages_TbItems_ItemId",
                        column: x => x.ItemId,
                        principalTable: "TbItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TbBrands_CurrentState",
                table: "TbBrands",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbBrands_DisplayOrder",
                table: "TbBrands",
                column: "DisplayOrder");

            migrationBuilder.CreateIndex(
                name: "IX_TbBrands_IsFavorite",
                table: "TbBrands",
                column: "IsFavorite");

            migrationBuilder.CreateIndex(
                name: "IX_TbItemAttribute_AttributeId",
                table: "TbItemAttribute",
                column: "AttributeId");

            migrationBuilder.CreateIndex(
                name: "IX_TbItemAttribute_CurrentState",
                table: "TbItemAttribute",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbItemAttribute_ItemId",
                table: "TbItemAttribute",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_TbItemAttributeCombinationPricings_AttributeIds",
                table: "TbItemAttributeCombinationPricings",
                column: "AttributeIds");

            migrationBuilder.CreateIndex(
                name: "IX_TbItemAttributeCombinationPricings_CurrentState",
                table: "TbItemAttributeCombinationPricings",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbItemAttributeCombinationPricings_FinalPrice",
                table: "TbItemAttributeCombinationPricings",
                column: "FinalPrice");

            migrationBuilder.CreateIndex(
                name: "IX_TbItemAttributeCombinationPricings_ItemId",
                table: "TbItemAttributeCombinationPricings",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_TbItemImages_CurrentState",
                table: "TbItemImages",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbItemImages_ItemId",
                table: "TbItemImages",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_TbItemImages_Order",
                table: "TbItemImages",
                column: "Order");

            migrationBuilder.CreateIndex(
                name: "IX_TbItems_BrandId",
                table: "TbItems",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_TbItems_CategoryId",
                table: "TbItems",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_TbItems_CurrentState",
                table: "TbItems",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbItems_IsBestSeller",
                table: "TbItems",
                column: "IsBestSeller");

            migrationBuilder.CreateIndex(
                name: "IX_TbItems_IsNewArrival",
                table: "TbItems",
                column: "IsNewArrival");

            migrationBuilder.CreateIndex(
                name: "IX_TbItems_IsRecommended",
                table: "TbItems",
                column: "IsRecommended");

            migrationBuilder.CreateIndex(
                name: "IX_TbItems_Price",
                table: "TbItems",
                column: "Price");

            migrationBuilder.CreateIndex(
                name: "IX_TbItems_UnitId",
                table: "TbItems",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_TbItems_VideoProviderId",
                table: "TbItems",
                column: "VideoProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_TbUnitConversions_CurrentState",
                table: "TbUnitConversions",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbUnitConversions_FromUnitId",
                table: "TbUnitConversions",
                column: "FromUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_TbUnitConversions_ToUnitId",
                table: "TbUnitConversions",
                column: "ToUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_TbUnits_CurrentState",
                table: "TbUnits",
                column: "CurrentState");

            migrationBuilder.CreateIndex(
                name: "IX_TbVideoProviders_CurrentState",
                table: "TbVideoProviders",
                column: "CurrentState");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TbItemAttribute");

            migrationBuilder.DropTable(
                name: "TbItemAttributeCombinationPricings");

            migrationBuilder.DropTable(
                name: "TbItemImages");

            migrationBuilder.DropTable(
                name: "TbUnitConversions");

            migrationBuilder.DropTable(
                name: "TbItems");

            migrationBuilder.DropTable(
                name: "TbBrands");

            migrationBuilder.DropTable(
                name: "TbUnits");

            migrationBuilder.DropTable(
                name: "TbVideoProviders");
        }
    }
}
