using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class AlterQuantityTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TbQuantityPricings");

            migrationBuilder.CreateTable(
                name: "TbQuantityTierPricings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    OfferCombinationPricingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MinQuantity = table.Column<int>(type: "int", nullable: false),
                    MaxQuantity = table.Column<int>(type: "int", nullable: true),
                    PricePerUnit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SalesPricePerUnit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DiscountPercentage = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbQuantityTierPricings", x => x.Id);
                    table.CheckConstraint("CK_QuantityPricing_Quantities", "[MaxQuantity] IS NULL OR [MaxQuantity] > [MinQuantity]");
                    table.ForeignKey(
                        name: "FK_TbQuantityTierPricings_TbOfferCombinationPricings_OfferCombinationPricingId",
                        column: x => x.OfferCombinationPricingId,
                        principalTable: "TbOfferCombinationPricings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TbQuantityTierPricings_IsDeleted",
                table: "TbQuantityTierPricings",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbQuantityTierPricings_MaxQuantity",
                table: "TbQuantityTierPricings",
                column: "MaxQuantity");

            migrationBuilder.CreateIndex(
                name: "IX_TbQuantityTierPricings_MinQuantity",
                table: "TbQuantityTierPricings",
                column: "MinQuantity");

            migrationBuilder.CreateIndex(
                name: "IX_TbQuantityTierPricings_OfferCombinationPricingId",
                table: "TbQuantityTierPricings",
                column: "OfferCombinationPricingId");

            migrationBuilder.CreateIndex(
                name: "IX_TbQuantityTierPricings_OfferCombinationPricingId_MinQuantity_MaxQuantity",
                table: "TbQuantityTierPricings",
                columns: new[] { "OfferCombinationPricingId", "MinQuantity", "MaxQuantity" },
                unique: true,
                filter: "[MaxQuantity] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TbQuantityTierPricings");

            migrationBuilder.CreateTable(
                name: "TbQuantityPricings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    OfferId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    DiscountPercentage = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    MaximumQuantity = table.Column<int>(type: "int", nullable: true),
                    MinimumQuantity = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbQuantityPricings", x => x.Id);
                    table.CheckConstraint("CK_QuantityPricing_Quantities", "[MaximumQuantity] IS NULL OR [MaximumQuantity] > [MinimumQuantity]");
                    table.ForeignKey(
                        name: "FK_TbQuantityPricings_TbOffers_OfferId",
                        column: x => x.OfferId,
                        principalTable: "TbOffers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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
