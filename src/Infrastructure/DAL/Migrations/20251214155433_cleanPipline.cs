using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class cleanPipline : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_TbCombinationAttributesValues_TbCombinationAttributes_CombinationAttributeId",
            //    table: "TbCombinationAttributesValues");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_TbLoyaltyPointsTransactions_TbProductReviews_ReviewId",
            //    table: "TbLoyaltyPointsTransactions");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_TbReviewReports_TbProductReviews_ReviewID",
            //    table: "TbReviewReports");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_TbReviewVotes_TbProductReviews_ReviewID",
            //    table: "TbReviewVotes");

            //migrationBuilder.DropTable(
            //    name: "TbAttributeValuePriceModifiers");

            //migrationBuilder.DropTable(
            //    name: "TbCombinationAttributes");

            //migrationBuilder.DropTable(
            //    name: "TbProductReviews");

            //migrationBuilder.DropColumn(
            //    name: "LastPriceUpdate",
            //    table: "TbOfferCombinationPricings");

            //migrationBuilder.DropColumn(
            //    name: "BasePrice",
            //    table: "TbItemCombinations");

            //migrationBuilder.RenameColumn(
            //    name: "CombinationAttributeId",
            //    table: "TbCombinationAttributesValues",
            //    newName: "ItemCombinationId");

            //migrationBuilder.RenameIndex(
            //    name: "IX_TbCombinationAttributesValues_CombinationAttributeId",
            //    table: "TbCombinationAttributesValues",
            //    newName: "IX_TbCombinationAttributesValues_ItemCombinationId");

            //migrationBuilder.AlterColumn<string>(
            //    name: "UserId",
            //    table: "TbVendors",
            //    type: "nvarchar(450)",
            //    nullable: false,
            //    defaultValue: "",
            //    oldClrType: typeof(string),
            //    oldType: "nvarchar(max)",
            //    oldNullable: true);

            //migrationBuilder.AlterColumn<int>(
            //    name: "Reason",
            //    table: "TbReviewReports",
            //    type: "int",
            //    maxLength: 100,
            //    nullable: false,
            //    oldClrType: typeof(string),
            //    oldType: "nvarchar(100)",
            //    oldMaxLength: 100);

            //migrationBuilder.AddColumn<Guid>(
            //    name: "OfferConditionId",
            //    table: "TbOfferCombinationPricings",
            //    type: "uniqueidentifier",
            //    nullable: false,
            //    defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            //migrationBuilder.CreateTable(
            //    name: "TbCustomer",
            //    columns: table => new
            //    {
            //        Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
            //        UserId = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
            //        IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
            //        CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
            //        UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
            //        UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_TbCustomer", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "TbItemCombinationImages",
            //    columns: table => new
            //    {
            //        Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
            //        Path = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
            //        Order = table.Column<int>(type: "int", nullable: false),
            //        ItemCombinationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
            //        CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
            //        UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
            //        UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_TbItemCombinationImages", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_TbItemCombinationImages_TbItemCombinations_ItemCombinationId",
            //            column: x => x.ItemCombinationId,
            //            principalTable: "TbItemCombinations",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "TbOfferReviews",
            //    columns: table => new
            //    {
            //        Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
            //        ReviewNumber = table.Column<int>(type: "int", nullable: false),
            //        OfferID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        CustomerID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        OrderItemID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
            //        Rating = table.Column<decimal>(type: "decimal(2,1)", nullable: false),
            //        ReviewTitle = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
            //        ReviewText = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        IsVerifiedPurchase = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
            //        HelpfulCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
            //        NotHelpfulCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
            //        Status = table.Column<int>(type: "int", nullable: false),
            //        IsEdited = table.Column<bool>(type: "bit", nullable: false),
            //        IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
            //        CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
            //        UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
            //        UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_TbOfferReviews", x => x.Id);
            //    });

            //migrationBuilder.CreateIndex(
            //    name: "IX_TbVendors_UserId",
            //    table: "TbVendors",
            //    column: "UserId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_TbOfferCombinationPricings_OfferConditionId",
            //    table: "TbOfferCombinationPricings",
            //    column: "OfferConditionId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_TbCustomer_IsDeleted",
            //    table: "TbCustomer",
            //    column: "IsDeleted");

            //migrationBuilder.CreateIndex(
            //    name: "IX_TbItemCombinationImages_IsDeleted",
            //    table: "TbItemCombinationImages",
            //    column: "IsDeleted");

            //migrationBuilder.CreateIndex(
            //    name: "IX_TbItemCombinationImages_ItemCombinationId",
            //    table: "TbItemCombinationImages",
            //    column: "ItemCombinationId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_TbOfferReviews_CustomerID",
            //    table: "TbOfferReviews",
            //    column: "CustomerID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_TbOfferReviews_IsDeleted",
            //    table: "TbOfferReviews",
            //    column: "IsDeleted");

            //migrationBuilder.CreateIndex(
            //    name: "IX_TbOfferReviews_IsVerifiedPurchase",
            //    table: "TbOfferReviews",
            //    column: "IsVerifiedPurchase");

            //migrationBuilder.CreateIndex(
            //    name: "IX_TbOfferReviews_OfferID",
            //    table: "TbOfferReviews",
            //    column: "OfferID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_TbOfferReviews_OfferID_CustomerID",
            //    table: "TbOfferReviews",
            //    columns: new[] { "OfferID", "CustomerID" });

            //migrationBuilder.CreateIndex(
            //    name: "IX_TbOfferReviews_OrderItemID",
            //    table: "TbOfferReviews",
            //    column: "OrderItemID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_TbOfferReviews_Rating",
            //    table: "TbOfferReviews",
            //    column: "Rating");

            //migrationBuilder.CreateIndex(
            //    name: "IX_TbOfferReviews_ReviewNumber",
            //    table: "TbOfferReviews",
            //    column: "ReviewNumber",
            //    unique: true);

            //migrationBuilder.CreateIndex(
            //    name: "IX_TbOfferReviews_Status",
            //    table: "TbOfferReviews",
            //    column: "Status");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_TbCombinationAttributesValues_TbItemCombinations_ItemCombinationId",
            //    table: "TbCombinationAttributesValues",
            //    column: "ItemCombinationId",
            //    principalTable: "TbItemCombinations",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_TbLoyaltyPointsTransactions_TbOfferReviews_ReviewId",
            //    table: "TbLoyaltyPointsTransactions",
            //    column: "ReviewId",
            //    principalTable: "TbOfferReviews",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Restrict);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_TbOfferCombinationPricings_TbOfferConditions_OfferConditionId",
            //    table: "TbOfferCombinationPricings",
            //    column: "OfferConditionId",
            //    principalTable: "TbOfferConditions",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_TbReviewReports_TbOfferReviews_ReviewID",
            //    table: "TbReviewReports",
            //    column: "ReviewID",
            //    principalTable: "TbOfferReviews",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_TbReviewVotes_TbOfferReviews_ReviewID",
            //    table: "TbReviewVotes",
            //    column: "ReviewID",
            //    principalTable: "TbOfferReviews",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_TbVendors_AspNetUsers_UserId",
            //    table: "TbVendors",
            //    column: "UserId",
            //    principalTable: "AspNetUsers",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TbCombinationAttributesValues_TbItemCombinations_ItemCombinationId",
                table: "TbCombinationAttributesValues");

            migrationBuilder.DropForeignKey(
                name: "FK_TbLoyaltyPointsTransactions_TbOfferReviews_ReviewId",
                table: "TbLoyaltyPointsTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_TbOfferCombinationPricings_TbOfferConditions_OfferConditionId",
                table: "TbOfferCombinationPricings");

            migrationBuilder.DropForeignKey(
                name: "FK_TbReviewReports_TbOfferReviews_ReviewID",
                table: "TbReviewReports");

            migrationBuilder.DropForeignKey(
                name: "FK_TbReviewVotes_TbOfferReviews_ReviewID",
                table: "TbReviewVotes");

            migrationBuilder.DropForeignKey(
                name: "FK_TbVendors_AspNetUsers_UserId",
                table: "TbVendors");

            migrationBuilder.DropTable(
                name: "TbCustomer");

            migrationBuilder.DropTable(
                name: "TbItemCombinationImages");

            migrationBuilder.DropTable(
                name: "TbOfferReviews");

            migrationBuilder.DropIndex(
                name: "IX_TbVendors_UserId",
                table: "TbVendors");

            migrationBuilder.DropIndex(
                name: "IX_TbOfferCombinationPricings_OfferConditionId",
                table: "TbOfferCombinationPricings");

            migrationBuilder.DropColumn(
                name: "OfferConditionId",
                table: "TbOfferCombinationPricings");

            migrationBuilder.RenameColumn(
                name: "ItemCombinationId",
                table: "TbCombinationAttributesValues",
                newName: "CombinationAttributeId");

            migrationBuilder.RenameIndex(
                name: "IX_TbCombinationAttributesValues_ItemCombinationId",
                table: "TbCombinationAttributesValues",
                newName: "IX_TbCombinationAttributesValues_CombinationAttributeId");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "TbVendors",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Reason",
                table: "TbReviewReports",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastPriceUpdate",
                table: "TbOfferCombinationPricings",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "BasePrice",
                table: "TbItemCombinations",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TbAttributeValuePriceModifiers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    AttributeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CombinationAttributeValueId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ModifierType = table.Column<int>(type: "int", nullable: false),
                    ModifierValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PriceModifierCategory = table.Column<int>(type: "int", nullable: false),
                    TbCombinationAttributesValueId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbAttributeValuePriceModifiers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbAttributeValuePriceModifiers_TbAttributes_AttributeId",
                        column: x => x.AttributeId,
                        principalTable: "TbAttributes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TbAttributeValuePriceModifiers_TbCombinationAttributesValues_CombinationAttributeValueId",
                        column: x => x.CombinationAttributeValueId,
                        principalTable: "TbCombinationAttributesValues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TbAttributeValuePriceModifiers_TbCombinationAttributesValues_TbCombinationAttributesValueId",
                        column: x => x.TbCombinationAttributesValueId,
                        principalTable: "TbCombinationAttributesValues",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TbCombinationAttributes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    ItemCombinationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbCombinationAttributes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbCombinationAttributes_TbItemCombinations_ItemCombinationId",
                        column: x => x.ItemCombinationId,
                        principalTable: "TbItemCombinations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TbProductReviews",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CustomerID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HelpfulCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsEdited = table.Column<bool>(type: "bit", nullable: false),
                    IsVerifiedPurchase = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    NotHelpfulCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    OrderItemID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ProductID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Rating = table.Column<decimal>(type: "decimal(2,1)", nullable: false),
                    ReviewNumber = table.Column<int>(type: "int", nullable: false),
                    ReviewText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReviewTitle = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbProductReviews", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TbAttributeValuePriceModifiers_AttributeId",
                table: "TbAttributeValuePriceModifiers",
                column: "AttributeId");

            migrationBuilder.CreateIndex(
                name: "IX_TbAttributeValuePriceModifiers_CombinationAttributeValueId",
                table: "TbAttributeValuePriceModifiers",
                column: "CombinationAttributeValueId");

            migrationBuilder.CreateIndex(
                name: "IX_TbAttributeValuePriceModifiers_IsDeleted",
                table: "TbAttributeValuePriceModifiers",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbAttributeValuePriceModifiers_TbCombinationAttributesValueId",
                table: "TbAttributeValuePriceModifiers",
                column: "TbCombinationAttributesValueId");

            migrationBuilder.CreateIndex(
                name: "IX_TbCombinationAttributes_IsDeleted",
                table: "TbCombinationAttributes",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbCombinationAttributes_ItemCombinationId",
                table: "TbCombinationAttributes",
                column: "ItemCombinationId");

            migrationBuilder.CreateIndex(
                name: "IX_TbProductReviews_CustomerID",
                table: "TbProductReviews",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_TbProductReviews_IsDeleted",
                table: "TbProductReviews",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TbProductReviews_IsVerifiedPurchase",
                table: "TbProductReviews",
                column: "IsVerifiedPurchase");

            migrationBuilder.CreateIndex(
                name: "IX_TbProductReviews_OrderItemID",
                table: "TbProductReviews",
                column: "OrderItemID");

            migrationBuilder.CreateIndex(
                name: "IX_TbProductReviews_ProductID",
                table: "TbProductReviews",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_TbProductReviews_ProductID_CustomerID",
                table: "TbProductReviews",
                columns: new[] { "ProductID", "CustomerID" });

            migrationBuilder.CreateIndex(
                name: "IX_TbProductReviews_Rating",
                table: "TbProductReviews",
                column: "Rating");

            migrationBuilder.CreateIndex(
                name: "IX_TbProductReviews_ReviewNumber",
                table: "TbProductReviews",
                column: "ReviewNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TbProductReviews_Status",
                table: "TbProductReviews",
                column: "Status");

            migrationBuilder.AddForeignKey(
                name: "FK_TbCombinationAttributesValues_TbCombinationAttributes_CombinationAttributeId",
                table: "TbCombinationAttributesValues",
                column: "CombinationAttributeId",
                principalTable: "TbCombinationAttributes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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
