using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class addWishlist : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TbWishlist",
                columns: table => new
                {
                    WishlistId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    CustomerId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbWishlist", x => x.WishlistId);
                    table.ForeignKey(
                        name: "FK_TbWishlist_AspNetUsers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TbWishlistItem",
                columns: table => new
                {
                    WishlistItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    WishlistId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ItemCombinationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbWishlistItem", x => x.WishlistItemId);
                    table.ForeignKey(
                        name: "FK_TbWishlistItem_TbItemCombinations_ItemCombinationId",
                        column: x => x.ItemCombinationId,
                        principalTable: "TbItemCombinations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TbWishlistItem_TbWishlist_WishlistId",
                        column: x => x.WishlistId,
                        principalTable: "TbWishlist",
                        principalColumn: "WishlistId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TbWishlist_IsDeleted",
                table: "TbWishlist",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Wishlist_CustomerId_IsDeleted",
                table: "TbWishlist",
                columns: new[] { "CustomerId", "IsDeleted" },
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_TbWishlistItem_IsDeleted",
                table: "TbWishlistItem",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_WishlistItem_DateAdded",
                table: "TbWishlistItem",
                column: "DateAdded");

            migrationBuilder.CreateIndex(
                name: "IX_WishlistItem_ItemCombinationId_IsDeleted",
                table: "TbWishlistItem",
                columns: new[] { "ItemCombinationId", "IsDeleted" },
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_WishlistItem_WishlistId_IsDeleted",
                table: "TbWishlistItem",
                columns: new[] { "WishlistId", "IsDeleted" },
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "UQ_WishlistItem_WishlistId_ItemCombinationId",
                table: "TbWishlistItem",
                columns: new[] { "WishlistId", "ItemCombinationId" },
                unique: true,
                filter: "[IsDeleted] = 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TbWishlistItem");

            migrationBuilder.DropTable(
                name: "TbWishlist");
        }
    }
}
