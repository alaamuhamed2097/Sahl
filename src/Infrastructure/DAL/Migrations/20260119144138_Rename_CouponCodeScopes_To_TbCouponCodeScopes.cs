using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class Rename_CouponCodeScopes_To_TbCouponCodeScopes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CouponCodeScopes_TbCouponCodes_CouponCodeId",
                table: "CouponCodeScopes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CouponCodeScopes",
                table: "CouponCodeScopes");

            migrationBuilder.RenameTable(
                name: "CouponCodeScopes",
                newName: "TbCouponCodeScopes");

            // 🔥 Rename ALL indexes
            migrationBuilder.RenameIndex(
                name: "IX_CouponCodeScopes_IsDeleted",
                table: "TbCouponCodeScopes",
                newName: "IX_TbCouponCodeScopes_IsDeleted");

            migrationBuilder.RenameIndex(
                name: "IX_CouponCodeScope_CouponCodeId",
                table: "TbCouponCodeScopes",
                newName: "IX_TbCouponCodeScopes_CouponCodeId");

            migrationBuilder.RenameIndex(
                name: "IX_CouponCodeScope_CouponId_Type_ScopeId",
                table: "TbCouponCodeScopes",
                newName: "IX_TbCouponCodeScopes_CouponId_Type_ScopeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TbCouponCodeScopes",
                table: "TbCouponCodeScopes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TbCouponCodeScopes_TbCouponCodes_CouponCodeId",
                table: "TbCouponCodeScopes",
                column: "CouponCodeId",
                principalTable: "TbCouponCodes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TbCouponCodeScopes_TbCouponCodes_CouponCodeId",
                table: "TbCouponCodeScopes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TbCouponCodeScopes",
                table: "TbCouponCodeScopes");

            migrationBuilder.RenameTable(
                name: "TbCouponCodeScopes",
                newName: "CouponCodeScopes");

            migrationBuilder.RenameIndex(
                name: "IX_TbCouponCodeScopes_IsDeleted",
                table: "CouponCodeScopes",
                newName: "IX_CouponCodeScopes_IsDeleted");

            migrationBuilder.RenameIndex(
                name: "IX_TbCouponCodeScopes_CouponCodeId",
                table: "CouponCodeScopes",
                newName: "IX_CouponCodeScope_CouponCodeId");

            migrationBuilder.RenameIndex(
                name: "IX_TbCouponCodeScopes_CouponId_Type_ScopeId",
                table: "CouponCodeScopes",
                newName: "IX_CouponCodeScope_CouponId_Type_ScopeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CouponCodeScopes",
                table: "CouponCodeScopes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CouponCodeScopes_TbCouponCodes_CouponCodeId",
                table: "CouponCodeScopes",
                column: "CouponCodeId",
                principalTable: "TbCouponCodes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
