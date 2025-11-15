using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class readAddEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TbItemAttributes_TbItems_ItemId",
                table: "TbItemAttribute");

            migrationBuilder.DropForeignKey(
                name: "FK_TbItemAttributeCombinationPricings_TbItems_ItemId",
                table: "TbItemAttributeCombinationPricings");

            migrationBuilder.DropForeignKey(
                name: "FK_TbItemImages_TbItems_ItemId",
                table: "TbItemImages");

            migrationBuilder.DropForeignKey(
                name: "FK_TbOrderDetail_TbItems_ItemId",
                table: "TbOrderDetail");

            //migrationBuilder.DropTable(
            //    name: "TbItem");

            //migrationBuilder.CreateIndex(
            //    name: "IX_TbItemAttributeCombinationPricings_Price",
            //    table: "TbItemAttributeCombinationPricings",
            //    column: "Price");

            migrationBuilder.AddForeignKey(
                name: "FK_TbItemAttributes_TbItems_ItemId",
                table: "TbItemAttribute",
                column: "ItemId",
                principalTable: "TbItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbItemAttributeCombinationPricings_TbItems_ItemId",
                table: "TbItemAttributeCombinationPricings",
                column: "ItemId",
                principalTable: "TbItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbItemImages_TbItems_ItemId",
                table: "TbItemImages",
                column: "ItemId",
                principalTable: "TbItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbOrderDetail_TbItems_ItemId",
                table: "TbOrderDetail",
                column: "ItemId",
                principalTable: "TbItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TbItemAttributes_TbItems_ItemId",
                table: "TbItemAttribute");

            migrationBuilder.DropForeignKey(
                name: "FK_TbItemAttributeCombinationPricings_TbItems_ItemId",
                table: "TbItemAttributeCombinationPricings");

            migrationBuilder.DropForeignKey(
                name: "FK_TbItemImages_TbItems_ItemId",
                table: "TbItemImages");

            migrationBuilder.DropForeignKey(
                name: "FK_TbItems_TbBrands_BrandId",
                table: "TbItems");

            migrationBuilder.DropForeignKey(
                name: "FK_TbItems_TbCategories_CategoryId",
                table: "TbItems");

            migrationBuilder.DropForeignKey(
                name: "FK_TbItems_TbUnits_UnitId",
                table: "TbItems");

            migrationBuilder.DropForeignKey(
                name: "FK_TbItems_TbVideoProviders_VideoProviderId",
                table: "TbItems");

            migrationBuilder.DropForeignKey(
                name: "FK_TbOrderDetail_TbItems_ItemId",
                table: "TbOrderDetail");

            migrationBuilder.DropIndex(
                name: "IX_TbItemAttributeCombinationPricings_Price",
                table: "TbItemAttributeCombinationPricings");

            migrationBuilder.CreateTable(
                name: "TbItem",
                columns: table => new
                {
                    BrandId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UnitId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    VideoProviderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TempId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TempId2 = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TempId3 = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TempId4 = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.UniqueConstraint("AK_TbItem_TempId1", x => x.TempId1);
                    table.UniqueConstraint("AK_TbItem_TempId2", x => x.TempId2);
                    table.UniqueConstraint("AK_TbItem_TempId3", x => x.TempId3);
                    table.UniqueConstraint("AK_TbItem_TempId4", x => x.TempId4);
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

            migrationBuilder.AddForeignKey(
                name: "FK_TbItemAttributes_TbItems_ItemId",
                table: "TbItemAttribute",
                column: "ItemId",
                principalTable: "TbItem",
                principalColumn: "TempId2",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbItemAttributeCombinationPricings_TbItems_ItemId",
                table: "TbItemAttributeCombinationPricings",
                column: "ItemId",
                principalTable: "TbItem",
                principalColumn: "TempId3",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbItemImages_TbItems_ItemId",
                table: "TbItemImages",
                column: "ItemId",
                principalTable: "TbItem",
                principalColumn: "TempId4",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbOrderDetail_TbItems_ItemId",
                table: "TbOrderDetail",
                column: "ItemId",
                principalTable: "TbItem",
                principalColumn: "TempId1",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
