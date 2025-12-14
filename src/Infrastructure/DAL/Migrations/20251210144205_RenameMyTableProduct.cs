using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class RenameMyTableProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TbLoyaltyPointsTransactions_TbProductReviews_ReviewId",
                table: "TbLoyaltyPointsTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_TbReviewReports_TbProductReviews_ReviewID",
                table: "TbReviewReports");

            migrationBuilder.DropForeignKey(
                name: "FK_TbReviewVotes_TbProductReviews_ReviewID",
                table: "TbReviewVotes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TbProductReviews",
                table: "TbProductReviews");

            migrationBuilder.RenameTable(
                name: "TbProductReviews",
                newName: "TbOfferReviews");

            migrationBuilder.RenameIndex(
                name: "IX_TbProductReviews_Status",
                table: "TbOfferReviews",
                newName: "IX_TbOfferReviews_Status");

            migrationBuilder.RenameIndex(
                name: "IX_TbProductReviews_ReviewNumber",
                table: "TbOfferReviews",
                newName: "IX_TbOfferReviews_ReviewNumber");

            migrationBuilder.RenameIndex(
                name: "IX_TbProductReviews_Rating",
                table: "TbOfferReviews",
                newName: "IX_TbOfferReviews_Rating");

            migrationBuilder.RenameIndex(
                name: "IX_TbProductReviews_OrderItemID",
                table: "TbOfferReviews",
                newName: "IX_TbOfferReviews_OrderItemID");

            migrationBuilder.RenameIndex(
                name: "IX_TbProductReviews_OfferID_CustomerID",
                table: "TbOfferReviews",
                newName: "IX_TbOfferReviews_OfferID_CustomerID");

            migrationBuilder.RenameIndex(
                name: "IX_TbProductReviews_OfferID",
                table: "TbOfferReviews",
                newName: "IX_TbOfferReviews_OfferID");

            migrationBuilder.RenameIndex(
                name: "IX_TbProductReviews_IsVerifiedPurchase",
                table: "TbOfferReviews",
                newName: "IX_TbOfferReviews_IsVerifiedPurchase");

            migrationBuilder.RenameIndex(
                name: "IX_TbProductReviews_IsDeleted",
                table: "TbOfferReviews",
                newName: "IX_TbOfferReviews_IsDeleted");

            migrationBuilder.RenameIndex(
                name: "IX_TbProductReviews_CustomerID",
                table: "TbOfferReviews",
                newName: "IX_TbOfferReviews_CustomerID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TbOfferReviews",
                table: "TbOfferReviews",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TbLoyaltyPointsTransactions_TbOfferReviews_ReviewId",
                table: "TbLoyaltyPointsTransactions",
                column: "ReviewId",
                principalTable: "TbOfferReviews",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbReviewReports_TbOfferReviews_ReviewID",
                table: "TbReviewReports",
                column: "ReviewID",
                principalTable: "TbOfferReviews",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbReviewVotes_TbOfferReviews_ReviewID",
                table: "TbReviewVotes",
                column: "ReviewID",
                principalTable: "TbOfferReviews",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TbLoyaltyPointsTransactions_TbOfferReviews_ReviewId",
                table: "TbLoyaltyPointsTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_TbReviewReports_TbOfferReviews_ReviewID",
                table: "TbReviewReports");

            migrationBuilder.DropForeignKey(
                name: "FK_TbReviewVotes_TbOfferReviews_ReviewID",
                table: "TbReviewVotes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TbOfferReviews",
                table: "TbOfferReviews");

            migrationBuilder.RenameTable(
                name: "TbOfferReviews",
                newName: "TbProductReviews");

            migrationBuilder.RenameIndex(
                name: "IX_TbOfferReviews_Status",
                table: "TbProductReviews",
                newName: "IX_TbProductReviews_Status");

            migrationBuilder.RenameIndex(
                name: "IX_TbOfferReviews_ReviewNumber",
                table: "TbProductReviews",
                newName: "IX_TbProductReviews_ReviewNumber");

            migrationBuilder.RenameIndex(
                name: "IX_TbOfferReviews_Rating",
                table: "TbProductReviews",
                newName: "IX_TbProductReviews_Rating");

            migrationBuilder.RenameIndex(
                name: "IX_TbOfferReviews_OrderItemID",
                table: "TbProductReviews",
                newName: "IX_TbProductReviews_OrderItemID");

            migrationBuilder.RenameIndex(
                name: "IX_TbOfferReviews_OfferID_CustomerID",
                table: "TbProductReviews",
                newName: "IX_TbProductReviews_OfferID_CustomerID");

            migrationBuilder.RenameIndex(
                name: "IX_TbOfferReviews_OfferID",
                table: "TbProductReviews",
                newName: "IX_TbProductReviews_OfferID");

            migrationBuilder.RenameIndex(
                name: "IX_TbOfferReviews_IsVerifiedPurchase",
                table: "TbProductReviews",
                newName: "IX_TbProductReviews_IsVerifiedPurchase");

            migrationBuilder.RenameIndex(
                name: "IX_TbOfferReviews_IsDeleted",
                table: "TbProductReviews",
                newName: "IX_TbProductReviews_IsDeleted");

            migrationBuilder.RenameIndex(
                name: "IX_TbOfferReviews_CustomerID",
                table: "TbProductReviews",
                newName: "IX_TbProductReviews_CustomerID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TbProductReviews",
                table: "TbProductReviews",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TbLoyaltyPointsTransactions_TbProductReviews_ReviewId",
                table: "TbLoyaltyPointsTransactions",
                column: "ReviewId",
                principalTable: "TbProductReviews",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbReviewReports_TbProductReviews_ReviewID",
                table: "TbReviewReports",
                column: "ReviewID",
                principalTable: "TbProductReviews",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TbReviewVotes_TbProductReviews_ReviewID",
                table: "TbReviewVotes",
                column: "ReviewID",
                principalTable: "TbProductReviews",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
