using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class addUniqueIndexToItemCombination : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_HomePageSlider_DateRange",
                table: "TbHomePageSlider");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "TbHomePageSlider");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "TbHomePageSlider");

            migrationBuilder.Sql(@"
        UPDATE TbItemCombinations 
        SET IsDefault = 0 
        WHERE IsDeleted = 0;
    ");

            migrationBuilder.Sql(@"
        WITH RankedCombinations AS (
            SELECT Id,
                   ROW_NUMBER() OVER (PARTITION BY ItemId ORDER BY Id ASC) AS RowNum
            FROM TbItemCombinations
            WHERE IsDeleted = 0
        )
        UPDATE TbItemCombinations
        SET IsDefault = 1
        FROM TbItemCombinations ic
        INNER JOIN RankedCombinations rc ON ic.Id = rc.Id
        WHERE rc.RowNum = 1;
    ");

            migrationBuilder.CreateIndex(
                name: "IX_TbItemCombinations_ItemId_IsDefault",
                table: "TbItemCombinations",
                columns: new[] { "ItemId", "IsDefault" },
                unique: true,
                filter: "[IsDeleted] = 0 AND [IsDefault] = 1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TbItemCombinations_ItemId_IsDefault",
                table: "TbItemCombinations");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "TbHomePageSlider",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "TbHomePageSlider",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_HomePageSlider_DateRange",
                table: "TbHomePageSlider",
                columns: new[] { "StartDate", "EndDate" });
        }
    }
}
