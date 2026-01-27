using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class renameTbBlockProductsToTbBlockItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TbBlockProducts_TbHomepageBlocks_HomepageBlockId",
                table: "TbBlockProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_TbBlockProducts_TbItems_ItemId",
                table: "TbBlockProducts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TbBlockProducts",
                table: "TbBlockProducts");

            migrationBuilder.RenameTable(
                name: "TbBlockProducts",
                newName: "TbBlockItems");

            migrationBuilder.RenameIndex(
                name: "IX_TbBlockProducts_ItemId",
                table: "TbBlockItems",
                newName: "IX_TbBlockItems_ItemId");

            migrationBuilder.RenameIndex(
                name: "IX_TbBlockProducts_IsDeleted",
                table: "TbBlockItems",
                newName: "IX_TbBlockItems_IsDeleted");

            migrationBuilder.RenameIndex(
                name: "IX_TbBlockProducts_HomepageBlockId_ItemId",
                table: "TbBlockItems",
                newName: "IX_TbBlockItems_HomepageBlockId_ItemId");

            migrationBuilder.RenameIndex(
                name: "IX_TbBlockProducts_HomepageBlockId_DisplayOrder",
                table: "TbBlockItems",
                newName: "IX_TbBlockItems_HomepageBlockId_DisplayOrder");

            migrationBuilder.RenameIndex(
                name: "IX_TbBlockProducts_HomepageBlockId",
                table: "TbBlockItems",
                newName: "IX_TbBlockItems_HomepageBlockId");

            migrationBuilder.RenameIndex(
                name: "IX_TbBlockProducts_DisplayOrder",
                table: "TbBlockItems",
                newName: "IX_TbBlockItems_DisplayOrder");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TbBlockItems",
                table: "TbBlockItems",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TbBlockItems_TbHomepageBlocks_HomepageBlockId",
                table: "TbBlockItems",
                column: "HomepageBlockId",
                principalTable: "TbHomepageBlocks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbBlockItems_TbItems_ItemId",
                table: "TbBlockItems",
                column: "ItemId",
                principalTable: "TbItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TbBlockItems_TbHomepageBlocks_HomepageBlockId",
                table: "TbBlockItems");

            migrationBuilder.DropForeignKey(
                name: "FK_TbBlockItems_TbItems_ItemId",
                table: "TbBlockItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TbBlockItems",
                table: "TbBlockItems");

            migrationBuilder.RenameTable(
                name: "TbBlockItems",
                newName: "TbBlockProducts");

            migrationBuilder.RenameIndex(
                name: "IX_TbBlockItems_ItemId",
                table: "TbBlockProducts",
                newName: "IX_TbBlockProducts_ItemId");

            migrationBuilder.RenameIndex(
                name: "IX_TbBlockItems_IsDeleted",
                table: "TbBlockProducts",
                newName: "IX_TbBlockProducts_IsDeleted");

            migrationBuilder.RenameIndex(
                name: "IX_TbBlockItems_HomepageBlockId_ItemId",
                table: "TbBlockProducts",
                newName: "IX_TbBlockProducts_HomepageBlockId_ItemId");

            migrationBuilder.RenameIndex(
                name: "IX_TbBlockItems_HomepageBlockId_DisplayOrder",
                table: "TbBlockProducts",
                newName: "IX_TbBlockProducts_HomepageBlockId_DisplayOrder");

            migrationBuilder.RenameIndex(
                name: "IX_TbBlockItems_HomepageBlockId",
                table: "TbBlockProducts",
                newName: "IX_TbBlockProducts_HomepageBlockId");

            migrationBuilder.RenameIndex(
                name: "IX_TbBlockItems_DisplayOrder",
                table: "TbBlockProducts",
                newName: "IX_TbBlockProducts_DisplayOrder");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TbBlockProducts",
                table: "TbBlockProducts",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TbBlockProducts_TbHomepageBlocks_HomepageBlockId",
                table: "TbBlockProducts",
                column: "HomepageBlockId",
                principalTable: "TbHomepageBlocks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbBlockProducts_TbItems_ItemId",
                table: "TbBlockProducts",
                column: "ItemId",
                principalTable: "TbItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
