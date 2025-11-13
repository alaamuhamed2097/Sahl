using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class RemovePriceQuantityAddIsDefaultToCombination : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add IsDefault column to TbItemAttributeCombinationPricings
            migrationBuilder.AddColumn<bool>(
                name: "IsDefault",
                table: "TbItemAttributeCombinationPricings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            // Set first combination for each item as default (where AttributeIds is empty or first record)
            migrationBuilder.Sql(@"
                WITH RankedCombinations AS (
                    SELECT 
                        Id,
                        ItemId,
                        AttributeIds,
                        ROW_NUMBER() OVER (
                            PARTITION BY ItemId 
                            ORDER BY 
                                CASE WHEN AttributeIds = '' THEN 0 ELSE 1 END,
                                CreatedDateUtc
                        ) AS RowNum
                    FROM TbItemAttributeCombinationPricings
                    WHERE CurrentState = 1
                )
                UPDATE TbItemAttributeCombinationPricings
                SET IsDefault = 1
                FROM TbItemAttributeCombinationPricings cp
                INNER JOIN RankedCombinations rc ON cp.Id = rc.Id
                WHERE rc.RowNum = 1;
            ");

            // Add index on IsDefault for better query performance
            migrationBuilder.CreateIndex(
                name: "IX_TbItemAttributeCombinationPricings_IsDefault",
                table: "TbItemAttributeCombinationPricings",
                column: "IsDefault");

            // Add unique constraint: only one default per item
            migrationBuilder.CreateIndex(
                name: "IX_TbItemAttributeCombinationPricings_ItemId_IsDefault",
                table: "TbItemAttributeCombinationPricings",
                columns: new[] { "ItemId", "IsDefault" },
                unique: true,
                filter: "[IsDefault] = 1 AND [CurrentState] = 1");

            // Drop Price index from TbItems (if exists)
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_TbItems_Price' AND object_id = OBJECT_ID('TbItems'))
                BEGIN
                    DROP INDEX IX_TbItems_Price ON TbItems;
                END
            ");

            // Remove Price and Quantity columns from TbItems
            migrationBuilder.DropColumn(
                name: "Price",
                table: "TbItems");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "TbItems");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Re-add Price and Quantity columns to TbItems
            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "TbItems",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "TbItems",
                type: "int",
                nullable: true);

            // Restore Price index
            migrationBuilder.CreateIndex(
                name: "IX_TbItems_Price",
                table: "TbItems",
                column: "Price");

            // Drop unique constraint
            migrationBuilder.DropIndex(
                name: "IX_TbItemAttributeCombinationPricings_ItemId_IsDefault",
                table: "TbItemAttributeCombinationPricings");

            // Drop IsDefault index
            migrationBuilder.DropIndex(
                name: "IX_TbItemAttributeCombinationPricings_IsDefault",
                table: "TbItemAttributeCombinationPricings");

            // Remove IsDefault column
            migrationBuilder.DropColumn(
                name: "IsDefault",
                table: "TbItemAttributeCombinationPricings");
        }
    }
}
