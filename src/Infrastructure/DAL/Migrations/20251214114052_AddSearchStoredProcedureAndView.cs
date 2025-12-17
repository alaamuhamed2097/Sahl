using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddSearchStoredProcedureAndView : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TbItems_TbBrands",
                table: "TbItems");

            migrationBuilder.DropForeignKey(
                name: "FK_TbItems_TbCategories",
                table: "TbItems");

            migrationBuilder.DropForeignKey(
                name: "FK_TbItems_TbUnits",
                table: "TbItems");

            migrationBuilder.DropForeignKey(
                name: "FK_TbOfferCombinationPricing_TbItemCombination",
                table: "TbOfferCombinationPricing");

            migrationBuilder.DropForeignKey(
                name: "FK_TbOfferCombinationPricing_TbItemCombinations_TbItemCombinationId",
                table: "TbOfferCombinationPricing");

            migrationBuilder.DropForeignKey(
                name: "FK_TbOfferCombinationPricing_TbOffer",
                table: "TbOfferCombinationPricing");

            migrationBuilder.DropForeignKey(
                name: "FK_TbOfferPriceHistories_TbOfferCombinationPricing_OfferCombinationPricingId",
                table: "TbOfferPriceHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_TbOfferPriceHistories_TbOfferCombinationPricing_TbOfferCombinationPricingId",
                table: "TbOfferPriceHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_TbOffers_TbItems",
                table: "TbOffers");

            migrationBuilder.DropForeignKey(
                name: "FK_TbOffers_TbOfferConditions",
                table: "TbOffers");

            migrationBuilder.DropForeignKey(
                name: "FK_TbOffers_TbOfferConditions_TbOfferConditionId",
                table: "TbOffers");

            migrationBuilder.DropForeignKey(
                name: "FK_TbOffers_TbVendors",
                table: "TbOffers");

            migrationBuilder.DropForeignKey(
                name: "FK_TbOffers_TbWarranties",
                table: "TbOffers");

            migrationBuilder.DropForeignKey(
                name: "FK_TbOffers_TbWarranties_TbWarrantyId",
                table: "TbOffers");

            migrationBuilder.DropForeignKey(
                name: "FK_TbOrderDetails_TbOfferCombinationPricing_OfferCombinationPricingId",
                table: "TbOrderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_TbShoppingCartItems_TbOfferCombinationPricing_OfferCombinationPricingId",
                table: "TbShoppingCartItems");

            migrationBuilder.DropIndex(
                name: "IX_TbOffers_ItemId",
                table: "TbOffers");

            migrationBuilder.DropIndex(
                name: "IX_TbOffers_TbOfferConditionId",
                table: "TbOffers");

            migrationBuilder.DropIndex(
                name: "IX_TbOffers_TbWarrantyId",
                table: "TbOffers");

            migrationBuilder.DropIndex(
                name: "IX_TbItems_IsActive",
                table: "TbItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TbOfferCombinationPricing",
                table: "TbOfferCombinationPricing");

            migrationBuilder.DropColumn(
                name: "TbOfferConditionId",
                table: "TbOffers");

            migrationBuilder.DropColumn(
                name: "TbWarrantyId",
                table: "TbOffers");

            migrationBuilder.RenameTable(
                name: "TbOfferCombinationPricing",
                newName: "TbOfferCombinationPricings");

            migrationBuilder.RenameIndex(
                name: "IX_TbOffers_VendorId",
                table: "TbOffers",
                newName: "IX_TbOffers_VendorId_NC");

            migrationBuilder.RenameIndex(
                name: "IX_TbItems_CategoryId",
                table: "TbItems",
                newName: "IX_TbItems_CategoryId_NC");

            migrationBuilder.RenameIndex(
                name: "IX_TbItems_BrandId",
                table: "TbItems",
                newName: "IX_TbItems_BrandId_NC");

            migrationBuilder.RenameIndex(
                name: "IX_TbOfferCombinationPricing_TbItemCombinationId",
                table: "TbOfferCombinationPricings",
                newName: "IX_TbOfferCombinationPricings_TbItemCombinationId");

            migrationBuilder.RenameIndex(
                name: "IX_TbOfferCombinationPricing_OfferId",
                table: "TbOfferCombinationPricings",
                newName: "IX_TbOfferCombinationPricings_OfferId");

            migrationBuilder.RenameIndex(
                name: "IX_TbOfferCombinationPricing_ItemCombinationId",
                table: "TbOfferCombinationPricings",
                newName: "IX_TbOfferCombinationPricings_ItemCombinationId");

            migrationBuilder.RenameIndex(
                name: "IX_TbOfferCombinationPricing_IsDeleted",
                table: "TbOfferCombinationPricings",
                newName: "IX_TbOfferCombinationPricings_IsDeleted");

            migrationBuilder.AlterColumn<bool>(
                name: "IsBuyBoxWinner",
                table: "TbOffers",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<int>(
                name: "HandlingTimeInDays",
                table: "TbOffers",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "FulfillmentType",
                table: "TbOffers",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 2);

            migrationBuilder.AlterColumn<int>(
                name: "VisibilityScope",
                table: "TbItems",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 1);

            migrationBuilder.AlterColumn<int>(
                name: "ReturnedQuantity",
                table: "TbOfferCombinationPricings",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "ReservedQuantity",
                table: "TbOfferCombinationPricings",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "RefundedQuantity",
                table: "TbOfferCombinationPricings",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "MinOrderQuantity",
                table: "TbOfferCombinationPricings",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 1);

            migrationBuilder.AlterColumn<int>(
                name: "MaxOrderQuantity",
                table: "TbOfferCombinationPricings",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 999);

            migrationBuilder.AlterColumn<int>(
                name: "LowStockThreshold",
                table: "TbOfferCombinationPricings",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 5);

            migrationBuilder.AlterColumn<int>(
                name: "LockedQuantity",
                table: "TbOfferCombinationPricings",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastStockUpdate",
                table: "TbOfferCombinationPricings",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2(2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastPriceUpdate",
                table: "TbOfferCombinationPricings",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2(2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "InTransitQuantity",
                table: "TbOfferCombinationPricings",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "DamagedQuantity",
                table: "TbOfferCombinationPricings",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TbOfferCombinationPricings",
                table: "TbOfferCombinationPricings",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_TbOffers_BuyBoxWinner_Filtered_NC",
                table: "TbOffers",
                column: "IsBuyBoxWinner",
                filter: "[IsBuyBoxWinner] = 1");

            migrationBuilder.CreateIndex(
                name: "IX_TbOffers_ItemId_Visibility_Deleted_Covering_NC",
                table: "TbOffers",
                columns: new[] { "ItemId", "VisibilityScope", "IsDeleted" })
                .Annotation("SqlServer:Include", new[] { "VendorId", "IsBuyBoxWinner", "HandlingTimeInDays", "FulfillmentType", "StorgeLocation" });

            migrationBuilder.CreateIndex(
                name: "IX_TbOffers_Public_Filtered_NC",
                table: "TbOffers",
                columns: new[] { "VisibilityScope", "IsDeleted" },
                filter: "[VisibilityScope] = 1 AND [IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_TbOffers_VendorId_ItemId_Unique",
                table: "TbOffers",
                columns: new[] { "VendorId", "ItemId" },
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_TbItems_Active_Filtered_NC",
                table: "TbItems",
                columns: new[] { "IsActive", "IsDeleted" },
                filter: "[IsActive] = 1 AND [IsDeleted] = 0")
                .Annotation("SqlServer:Include", new[] { "TitleAr", "TitleEn", "CategoryId", "BrandId", "ThumbnailImage", "CreatedDateUtc" });

            migrationBuilder.CreateIndex(
                name: "IX_TbItems_CategoryId_BrandId_NC",
                table: "TbItems",
                columns: new[] { "CategoryId", "BrandId" });

            migrationBuilder.CreateIndex(
                name: "IX_TbItems_CreatedDateUtc_NC",
                table: "TbItems",
                column: "CreatedDateUtc",
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "IX_TbItems_TitleAr_NC",
                table: "TbItems",
                column: "TitleAr");

            migrationBuilder.CreateIndex(
                name: "IX_TbItems_TitleEn_NC",
                table: "TbItems",
                column: "TitleEn");

            migrationBuilder.CreateIndex(
                name: "IX_TbOfferPricing_InStock_Filtered_NC",
                table: "TbOfferCombinationPricings",
                columns: new[] { "StockStatus", "AvailableQuantity" },
                filter: "[AvailableQuantity] > 0 AND [StockStatus] = 1 AND [IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_TbOfferPricing_OfferId_Deleted_Covering_NC",
                table: "TbOfferCombinationPricings",
                columns: new[] { "OfferId", "IsDeleted" })
                .Annotation("SqlServer:Include", new[] { "SalesPrice", "Price", "AvailableQuantity", "StockStatus" });

            migrationBuilder.CreateIndex(
                name: "IX_TbOfferPricing_SalesPrice_Deleted_NC",
                table: "TbOfferCombinationPricings",
                columns: new[] { "SalesPrice", "IsDeleted" },
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_TbOfferPricing_SalesPrice_NC",
                table: "TbOfferCombinationPricings",
                column: "SalesPrice");

            migrationBuilder.AddForeignKey(
                name: "FK_TbItems_TbBrands_BrandId",
                table: "TbItems",
                column: "BrandId",
                principalTable: "TbBrands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbItems_TbCategories_CategoryId",
                table: "TbItems",
                column: "CategoryId",
                principalTable: "TbCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbItems_TbUnits_UnitId",
                table: "TbItems",
                column: "UnitId",
                principalTable: "TbUnits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbOfferCombinationPricings_TbItemCombinations_ItemCombinationId",
                table: "TbOfferCombinationPricings",
                column: "ItemCombinationId",
                principalTable: "TbItemCombinations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbOfferCombinationPricings_TbItemCombinations_TbItemCombinationId",
                table: "TbOfferCombinationPricings",
                column: "TbItemCombinationId",
                principalTable: "TbItemCombinations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TbOfferCombinationPricings_TbOffers_OfferId",
                table: "TbOfferCombinationPricings",
                column: "OfferId",
                principalTable: "TbOffers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbOfferPriceHistories_TbOfferCombinationPricings_OfferCombinationPricingId",
                table: "TbOfferPriceHistories",
                column: "OfferCombinationPricingId",
                principalTable: "TbOfferCombinationPricings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbOfferPriceHistories_TbOfferCombinationPricings_TbOfferCombinationPricingId",
                table: "TbOfferPriceHistories",
                column: "TbOfferCombinationPricingId",
                principalTable: "TbOfferCombinationPricings",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TbOffers_TbItems_ItemId",
                table: "TbOffers",
                column: "ItemId",
                principalTable: "TbItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbOffers_TbOfferConditions_OfferConditionId",
                table: "TbOffers",
                column: "OfferConditionId",
                principalTable: "TbOfferConditions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TbOffers_TbVendors_VendorId",
                table: "TbOffers",
                column: "VendorId",
                principalTable: "TbVendors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbOffers_TbWarranties_WarrantyId",
                table: "TbOffers",
                column: "WarrantyId",
                principalTable: "TbWarranties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbOrderDetails_TbOfferCombinationPricings_OfferCombinationPricingId",
                table: "TbOrderDetails",
                column: "OfferCombinationPricingId",
                principalTable: "TbOfferCombinationPricings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbShoppingCartItems_TbOfferCombinationPricings_OfferCombinationPricingId",
                table: "TbShoppingCartItems",
                column: "OfferCombinationPricingId",
                principalTable: "TbOfferCombinationPricings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            // Create Advanced Search Stored Procedure with Scoring
            migrationBuilder.Sql(@"
                CREATE OR ALTER PROCEDURE [dbo].[SpSearchItemsMultiVendor]
                    @SearchTerm NVARCHAR(255) = NULL,
                    @CategoryIds NVARCHAR(MAX) = NULL,
                    @BrandIds NVARCHAR(MAX) = NULL,
                    @VendorIds NVARCHAR(MAX) = NULL,
                    @MinPrice DECIMAL(18,2) = NULL,
                    @MaxPrice DECIMAL(18,2) = NULL,
                    @InStockOnly BIT = 0,
                    @SortBy NVARCHAR(50) = 'relevance',
                    @PageNumber INT = 1,
                    @PageSize INT = 20
                AS
                BEGIN
                    SET NOCOUNT ON;

                    DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;
                    DECLARE @CategoryIdTable TABLE (Id UNIQUEIDENTIFIER);
                    DECLARE @BrandIdTable TABLE (Id UNIQUEIDENTIFIER);
                    DECLARE @VendorIdTable TABLE (Id UNIQUEIDENTIFIER);

                    IF @CategoryIds IS NOT NULL AND @CategoryIds != ''
                    BEGIN
                        INSERT INTO @CategoryIdTable
                        SELECT CAST(value AS UNIQUEIDENTIFIER) 
                        FROM STRING_SPLIT(@CategoryIds, ',')
                        WHERE value != '';
                    END


                    IF @BrandIds IS NOT NULL AND @BrandIds != ''
                    BEGIN
                        INSERT INTO @BrandIdTable
                        SELECT CAST(value AS UNIQUEIDENTIFIER) 
                        FROM STRING_SPLIT(@BrandIds, ',')
                        WHERE value != '';
                    END

                    IF @VendorIds IS NOT NULL AND @VendorIds != ''
                    BEGIN
                        INSERT INTO @VendorIdTable
                        SELECT CAST(value AS UNIQUEIDENTIFIER)
                        FROM STRING_SPLIT(@VendorIds, ',')
                        WHERE value != '';
                    END

                    ;WITH ItemBase AS (
                        SELECT
                            i.Id AS ItemId,
                            i.TitleAr,
                            i.TitleEn,
                            i.ShortDescriptionAr,
                            i.ShortDescriptionEn,
                            i.CategoryId,
                            i.BrandId,
                            i.ThumbnailImage,
                            i.CreatedDateUtc,
                            o.Id AS OfferId,
                            o.VendorId,
                            o.IsBuyBoxWinner,
                            p.SalesPrice,
                            p.Price AS OriginalPrice,
                            p.AvailableQuantity,
                            p.StockStatus
                        FROM TbItems i WITH (INDEX(IX_TbItems_Active_Filtered_NC))
                        INNER JOIN TbOffers o WITH (INDEX(IX_TbOffers_ItemId_Visibility_Deleted_Covering_NC)) 
                            ON i.Id = o.ItemId
                        INNER JOIN TbOfferCombinationPricings p WITH (INDEX(IX_TbOfferPricing_OfferId_Deleted_Covering_NC)) 
                            ON o.Id = p.OfferId
                        WHERE
                            i.IsActive = 1 AND i.IsDeleted = 0
                            AND o.VisibilityScope = 1 AND o.IsDeleted = 0
                            AND p.IsDeleted = 0
                            AND (
                                @SearchTerm IS NULL 
                                OR i.TitleAr LIKE '%' + @SearchTerm + '%'
                                OR i.TitleEn LIKE '%' + @SearchTerm + '%'
                                OR i.ShortDescriptionAr LIKE '%' + @SearchTerm + '%'
                                OR i.ShortDescriptionEn LIKE '%' + @SearchTerm + '%'
                            )
                            AND (
                                NOT EXISTS (SELECT 1 FROM @CategoryIdTable)
                                OR i.CategoryId IN (SELECT Id FROM @CategoryIdTable)
                            )
                            AND (
                                NOT EXISTS (SELECT 1 FROM @BrandIdTable)
                                OR i.BrandId IN (SELECT Id FROM @BrandIdTable)
                            )
                            AND (
                                NOT EXISTS (SELECT 1 FROM @VendorIdTable)
                                OR o.VendorId IN (SELECT Id FROM @VendorIdTable)
                            )
                            AND (@MinPrice IS NULL OR p.SalesPrice >= @MinPrice)
                            AND (@MaxPrice IS NULL OR p.SalesPrice <= @MaxPrice)
                            AND (@InStockOnly = 0 OR (p.AvailableQuantity > 0 AND p.StockStatus = 1))
                    ),
                    ScoredItems AS (
                        SELECT
                            *,
                            CASE
                                WHEN @SearchTerm IS NULL THEN 0
                                WHEN TitleAr LIKE @SearchTerm + '%' OR TitleEn LIKE @SearchTerm + '%' THEN 1.0
                                WHEN TitleAr LIKE '%' + @SearchTerm + '%' OR TitleEn LIKE '%' + @SearchTerm + '%' THEN 0.8
                                WHEN ShortDescriptionAr LIKE '%' + @SearchTerm + '%' OR ShortDescriptionEn LIKE '%' + @SearchTerm + '%' THEN 0.5
                                ELSE 0.3
                            END AS TextScore,
                            CASE WHEN IsBuyBoxWinner = 1 THEN 1.0 ELSE 0 END AS BuyBoxScore,
                            CASE
                                WHEN AvailableQuantity >= 50 THEN 1.0
                                WHEN AvailableQuantity >= 10 THEN 0.7
                                WHEN AvailableQuantity > 0 THEN 0.4
                                ELSE 0
                            END AS StockScore,
                            CASE
                                WHEN SalesPrice < OriginalPrice THEN 1.0
                                ELSE 0.5
                            END AS PriceScore
                        FROM ItemBase
                    ),
                    Aggregated AS (
                        SELECT
                            ItemId,
                            TitleAr,
                            TitleEn,
                            ShortDescriptionAr,
                            ShortDescriptionEn,
                            CategoryId,
                            BrandId,
                            ThumbnailImage,
                            CreatedDateUtc,
                            MIN(SalesPrice) AS MinPrice,
                            MAX(SalesPrice) AS MaxPrice,
                            COUNT(DISTINCT OfferId) AS OffersCount,
                            MAX(TextScore) AS TextScore,
                            MAX(BuyBoxScore) AS BuyBoxScore,
                            AVG(StockScore) AS StockScore,
                            AVG(PriceScore) AS PriceScore,
                            (
                                SELECT TOP 1 
                                    CONCAT(
                                        OfferId, '|',
                                        VendorId, '|',
                                        SalesPrice, '|',
                                        OriginalPrice, '|',
                                        AvailableQuantity, '|',
                                        CAST(IsBuyBoxWinner AS INT)
                                    )
                                FROM ScoredItems si2
                                WHERE si2.ItemId = si.ItemId
                                ORDER BY IsBuyBoxWinner DESC, SalesPrice ASC
                            ) AS BestOfferData
                        FROM ScoredItems si
                        GROUP BY 
                            ItemId, TitleAr, TitleEn, ShortDescriptionAr, 
                            ShortDescriptionEn, CategoryId, BrandId, 
                            ThumbnailImage, CreatedDateUtc
                    ),
                    RankedItems AS (
                        SELECT
                            *,
                            (
                                (TextScore * 0.35) +
                                (BuyBoxScore * 0.20) +
                                (StockScore * 0.10) +
                                (PriceScore * 0.15) +
                                (CAST(OffersCount AS FLOAT) / 10.0 * 0.10)
                            ) AS FinalScore,
                            ROW_NUMBER() OVER (
                                ORDER BY
                                    CASE WHEN @SortBy = 'price_asc' THEN MinPrice ELSE 99999999 END ASC,
                                    CASE WHEN @SortBy = 'price_desc' THEN MaxPrice ELSE 0 END DESC,
                                    CASE WHEN @SortBy = 'newest' THEN DATEDIFF(SECOND, '2000-01-01', CreatedDateUtc) ELSE 0 END DESC,
                                    CASE WHEN @SortBy = 'relevance' THEN
                                        (
                                            (TextScore * 0.35) +
                                            (BuyBoxScore * 0.20) +
                                            (StockScore * 0.10) +
                                            (PriceScore * 0.15) +
                                            (CAST(OffersCount AS FLOAT) / 10.0 * 0.10)
                                        )
                                    ELSE 0 END DESC,
                                    ItemId
                            ) AS RowNum
                        FROM Aggregated
                    )
                    SELECT 
                        ItemId, TitleAr, TitleEn, ShortDescriptionAr, ShortDescriptionEn,
                        CategoryId, BrandId, ThumbnailImage, CreatedDateUtc,
                        MinPrice, MaxPrice, OffersCount, BestOfferData, FinalScore
                    FROM RankedItems
                    WHERE RowNum > @Offset AND RowNum <= (@Offset + @PageSize)
                    ORDER BY RowNum;

                    SELECT COUNT(*) AS TotalRecords FROM Aggregated;
                END
            ");

            // Create Materialized View for Best Prices
            migrationBuilder.Sql(@"
                CREATE OR ALTER VIEW [dbo].[VwItemBestPrices] AS
                SELECT 
                    i.Id AS ItemId,
                    MIN(p.SalesPrice) AS BestPrice,
                    SUM(p.AvailableQuantity) AS TotalStock,
                    COUNT(DISTINCT o.Id) AS TotalOffers,
                    AVG(CAST(o.IsBuyBoxWinner AS FLOAT)) AS BuyBoxRatio
                FROM TbItems i WITH (INDEX(IX_TbItems_Active_Filtered_NC))
                INNER JOIN TbOffers o WITH (INDEX(IX_TbOffers_Public_Filtered_NC)) 
                    ON i.Id = o.ItemId
                INNER JOIN TbOfferCombinationPricings p WITH (INDEX(IX_TbOfferPricing_InStock_Filtered_NC)) 
                    ON o.Id = p.OfferId
                WHERE 
                    i.IsActive = 1 AND i.IsDeleted = 0
                    AND o.VisibilityScope = 1 AND o.IsDeleted = 0
                    AND p.IsDeleted = 0
                    AND p.AvailableQuantity > 0
                GROUP BY i.Id
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
                name: "FK_TbOfferCombinationPricings_TbItemCombinations_ItemCombinationId",
                table: "TbOfferCombinationPricings");

            migrationBuilder.DropForeignKey(
                name: "FK_TbOfferCombinationPricings_TbItemCombinations_TbItemCombinationId",
                table: "TbOfferCombinationPricings");

            migrationBuilder.DropForeignKey(
                name: "FK_TbOfferCombinationPricings_TbOffers_OfferId",
                table: "TbOfferCombinationPricings");

            migrationBuilder.DropForeignKey(
                name: "FK_TbOfferPriceHistories_TbOfferCombinationPricings_OfferCombinationPricingId",
                table: "TbOfferPriceHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_TbOfferPriceHistories_TbOfferCombinationPricings_TbOfferCombinationPricingId",
                table: "TbOfferPriceHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_TbOffers_TbItems_ItemId",
                table: "TbOffers");

            migrationBuilder.DropForeignKey(
                name: "FK_TbOffers_TbOfferConditions_OfferConditionId",
                table: "TbOffers");

            migrationBuilder.DropForeignKey(
                name: "FK_TbOffers_TbVendors_VendorId",
                table: "TbOffers");

            migrationBuilder.DropForeignKey(
                name: "FK_TbOffers_TbWarranties_WarrantyId",
                table: "TbOffers");

            migrationBuilder.DropForeignKey(
                name: "FK_TbOrderDetails_TbOfferCombinationPricings_OfferCombinationPricingId",
                table: "TbOrderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_TbShoppingCartItems_TbOfferCombinationPricings_OfferCombinationPricingId",
                table: "TbShoppingCartItems");

            migrationBuilder.DropIndex(
                name: "IX_TbOffers_BuyBoxWinner_Filtered_NC",
                table: "TbOffers");

            migrationBuilder.DropIndex(
                name: "IX_TbOffers_ItemId_Visibility_Deleted_Covering_NC",
                table: "TbOffers");

            migrationBuilder.DropIndex(
                name: "IX_TbOffers_Public_Filtered_NC",
                table: "TbOffers");

            migrationBuilder.DropIndex(
                name: "IX_TbOffers_VendorId_ItemId_Unique",
                table: "TbOffers");

            migrationBuilder.DropIndex(
                name: "IX_TbItems_Active_Filtered_NC",
                table: "TbItems");

            migrationBuilder.DropIndex(
                name: "IX_TbItems_CategoryId_BrandId_NC",
                table: "TbItems");

            migrationBuilder.DropIndex(
                name: "IX_TbItems_CreatedDateUtc_NC",
                table: "TbItems");

            migrationBuilder.DropIndex(
                name: "IX_TbItems_TitleAr_NC",
                table: "TbItems");

            migrationBuilder.DropIndex(
                name: "IX_TbItems_TitleEn_NC",
                table: "TbItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TbOfferCombinationPricings",
                table: "TbOfferCombinationPricings");

            migrationBuilder.DropIndex(
                name: "IX_TbOfferPricing_InStock_Filtered_NC",
                table: "TbOfferCombinationPricings");

            migrationBuilder.DropIndex(
                name: "IX_TbOfferPricing_OfferId_Deleted_Covering_NC",
                table: "TbOfferCombinationPricings");

            migrationBuilder.DropIndex(
                name: "IX_TbOfferPricing_SalesPrice_Deleted_NC",
                table: "TbOfferCombinationPricings");

            migrationBuilder.DropIndex(
                name: "IX_TbOfferPricing_SalesPrice_NC",
                table: "TbOfferCombinationPricings");

            migrationBuilder.RenameTable(
                name: "TbOfferCombinationPricings",
                newName: "TbOfferCombinationPricing");

            migrationBuilder.RenameIndex(
                name: "IX_TbOffers_VendorId_NC",
                table: "TbOffers",
                newName: "IX_TbOffers_VendorId");

            migrationBuilder.RenameIndex(
                name: "IX_TbItems_CategoryId_NC",
                table: "TbItems",
                newName: "IX_TbItems_CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_TbItems_BrandId_NC",
                table: "TbItems",
                newName: "IX_TbItems_BrandId");

            migrationBuilder.RenameIndex(
                name: "IX_TbOfferCombinationPricings_TbItemCombinationId",
                table: "TbOfferCombinationPricing",
                newName: "IX_TbOfferCombinationPricing_TbItemCombinationId");

            migrationBuilder.RenameIndex(
                name: "IX_TbOfferCombinationPricings_OfferId",
                table: "TbOfferCombinationPricing",
                newName: "IX_TbOfferCombinationPricing_OfferId");

            migrationBuilder.RenameIndex(
                name: "IX_TbOfferCombinationPricings_ItemCombinationId",
                table: "TbOfferCombinationPricing",
                newName: "IX_TbOfferCombinationPricing_ItemCombinationId");

            migrationBuilder.RenameIndex(
                name: "IX_TbOfferCombinationPricings_IsDeleted",
                table: "TbOfferCombinationPricing",
                newName: "IX_TbOfferCombinationPricing_IsDeleted");

            migrationBuilder.AlterColumn<bool>(
                name: "IsBuyBoxWinner",
                table: "TbOffers",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<int>(
                name: "HandlingTimeInDays",
                table: "TbOffers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "FulfillmentType",
                table: "TbOffers",
                type: "int",
                nullable: false,
                defaultValue: 2,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<Guid>(
                name: "TbOfferConditionId",
                table: "TbOffers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TbWarrantyId",
                table: "TbOffers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "VisibilityScope",
                table: "TbItems",
                type: "int",
                nullable: false,
                defaultValue: 1,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ReturnedQuantity",
                table: "TbOfferCombinationPricing",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ReservedQuantity",
                table: "TbOfferCombinationPricing",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "RefundedQuantity",
                table: "TbOfferCombinationPricing",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "MinOrderQuantity",
                table: "TbOfferCombinationPricing",
                type: "int",
                nullable: false,
                defaultValue: 1,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "MaxOrderQuantity",
                table: "TbOfferCombinationPricing",
                type: "int",
                nullable: false,
                defaultValue: 999,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "LowStockThreshold",
                table: "TbOfferCombinationPricing",
                type: "int",
                nullable: false,
                defaultValue: 5,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "LockedQuantity",
                table: "TbOfferCombinationPricing",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastStockUpdate",
                table: "TbOfferCombinationPricing",
                type: "datetime2(2)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastPriceUpdate",
                table: "TbOfferCombinationPricing",
                type: "datetime2(2)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "InTransitQuantity",
                table: "TbOfferCombinationPricing",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "DamagedQuantity",
                table: "TbOfferCombinationPricing",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TbOfferCombinationPricing",
                table: "TbOfferCombinationPricing",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_TbOffers_ItemId",
                table: "TbOffers",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_TbOffers_TbOfferConditionId",
                table: "TbOffers",
                column: "TbOfferConditionId");

            migrationBuilder.CreateIndex(
                name: "IX_TbOffers_TbWarrantyId",
                table: "TbOffers",
                column: "TbWarrantyId");

            migrationBuilder.CreateIndex(
                name: "IX_TbItems_IsActive",
                table: "TbItems",
                column: "IsActive");

            migrationBuilder.AddForeignKey(
                name: "FK_TbItems_TbBrands",
                table: "TbItems",
                column: "BrandId",
                principalTable: "TbBrands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbItems_TbCategories",
                table: "TbItems",
                column: "CategoryId",
                principalTable: "TbCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbItems_TbUnits",
                table: "TbItems",
                column: "UnitId",
                principalTable: "TbUnits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbOfferCombinationPricing_TbItemCombination",
                table: "TbOfferCombinationPricing",
                column: "ItemCombinationId",
                principalTable: "TbItemCombinations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbOfferCombinationPricing_TbItemCombinations_TbItemCombinationId",
                table: "TbOfferCombinationPricing",
                column: "TbItemCombinationId",
                principalTable: "TbItemCombinations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TbOfferCombinationPricing_TbOffer",
                table: "TbOfferCombinationPricing",
                column: "OfferId",
                principalTable: "TbOffers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbOfferPriceHistories_TbOfferCombinationPricing_OfferCombinationPricingId",
                table: "TbOfferPriceHistories",
                column: "OfferCombinationPricingId",
                principalTable: "TbOfferCombinationPricing",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbOfferPriceHistories_TbOfferCombinationPricing_TbOfferCombinationPricingId",
                table: "TbOfferPriceHistories",
                column: "TbOfferCombinationPricingId",
                principalTable: "TbOfferCombinationPricing",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TbOffers_TbItems",
                table: "TbOffers",
                column: "ItemId",
                principalTable: "TbItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbOffers_TbOfferConditions",
                table: "TbOffers",
                column: "OfferConditionId",
                principalTable: "TbOfferConditions",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_TbOffers_TbOfferConditions_TbOfferConditionId",
                table: "TbOffers",
                column: "TbOfferConditionId",
                principalTable: "TbOfferConditions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TbOffers_TbVendors",
                table: "TbOffers",
                column: "VendorId",
                principalTable: "TbVendors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbOffers_TbWarranties",
                table: "TbOffers",
                column: "WarrantyId",
                principalTable: "TbWarranties",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_TbOffers_TbWarranties_TbWarrantyId",
                table: "TbOffers",
                column: "TbWarrantyId",
                principalTable: "TbWarranties",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TbOrderDetails_TbOfferCombinationPricing_OfferCombinationPricingId",
                table: "TbOrderDetails",
                column: "OfferCombinationPricingId",
                principalTable: "TbOfferCombinationPricing",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbShoppingCartItems_TbOfferCombinationPricing_OfferCombinationPricingId",
                table: "TbShoppingCartItems",
                column: "OfferCombinationPricingId",
                principalTable: "TbOfferCombinationPricing",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.Sql("DROP VIEW IF EXISTS [dbo].[VwItemBestPrices]");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[SpSearchItemsMultiVendor]");
        }
    }
}
