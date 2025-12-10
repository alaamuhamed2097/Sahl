using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class OptimizeItemSearchPerformance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // ============================================================================
            // Phase 1: Create Indexes on TbItems (Base Product Information)
            // ============================================================================

            // Text search indexes
            migrationBuilder.CreateIndex(
                name: "IX_TbItems_TitleAr",
                table: "TbItems",
                column: "TitleAr");

            migrationBuilder.CreateIndex(
                name: "IX_TbItems_TitleEn",
                table: "TbItems",
                column: "TitleEn");

            // Basic filtering indexes
            migrationBuilder.CreateIndex(
                name: "IX_TbItems_CategoryId_BrandId",
                table: "TbItems",
                columns: new[] { "CategoryId", "BrandId" });

            migrationBuilder.CreateIndex(
                name: "IX_TbItems_IsActive_CreatedDate",
                table: "TbItems",
                columns: new[] { "IsActive", "CreatedDateUtc" });

            // ============================================================================
            // Phase 2: Create Indexes on TbOffers (Vendor Offers)
            // ============================================================================

            migrationBuilder.CreateIndex(
                name: "IX_TbOffers_ItemId_UserId",
                table: "TbOffers",
                columns: new[] { "ItemId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_TbOffers_VisibilityScope",
                table: "TbOffers",
                column: "VisibilityScope");

            migrationBuilder.CreateIndex(
                name: "IX_TbOffers_StorgeLocation",
                table: "TbOffers",
                column: "StorgeLocation");

            migrationBuilder.CreateIndex(
                name: "IX_TbOffers_UserId",
                table: "TbOffers",
                column: "UserId");

            // ============================================================================
            // Phase 3: Create Indexes on TbOfferCombinationPricing (Prices & Stock)
            // ============================================================================

            migrationBuilder.CreateIndex(
                name: "IX_TbOfferCombinationPricing_OfferId_SalesPrice",
                table: "TbOfferCombinationPricing",
                columns: new[] { "OfferId", "SalesPrice" });

            migrationBuilder.CreateIndex(
                name: "IX_TbOfferCombinationPricing_SalesPrice",
                table: "TbOfferCombinationPricing",
                column: "SalesPrice");

            migrationBuilder.CreateIndex(
                name: "IX_TbOfferCombinationPricing_StockStatus_AvailableQuantity",
                table: "TbOfferCombinationPricing",
                columns: new[] { "StockStatus", "AvailableQuantity" });

            migrationBuilder.CreateIndex(
                name: "IX_TbOfferCombinationPricing_IsDefault",
                table: "TbOfferCombinationPricing",
                column: "IsDefault")
                .Annotation("SqlServer:Include", new[] { "SalesPrice", "AvailableQuantity" });

            // ============================================================================
            // Phase 4: Create Stored Procedure for Advanced Multi-Vendor Search
            // ============================================================================

            migrationBuilder.Sql(@"
                CREATE PROCEDURE [dbo].[SpSearchItemsMultiVendor]
                    @SearchTerm NVARCHAR(255) = NULL,
                    @CategoryIds NVARCHAR(MAX) = NULL,
                    @BrandIds NVARCHAR(MAX) = NULL,
                    @MinPrice DECIMAL(18,2) = NULL,
                    @MaxPrice DECIMAL(18,2) = NULL,
                    @VendorIds NVARCHAR(MAX) = NULL,
                    @InStockOnly BIT = 0,
                    @FreeShippingOnly BIT = 0,
                    @OnSaleOnly BIT = 0,
                    @BuyBoxWinnersOnly BIT = 0,
                    @MaxDeliveryDays INT = NULL,
                    @SortBy NVARCHAR(50) = 'newest',
                    @PageNumber INT = 1,
                    @PageSize INT = 20
                AS
                BEGIN
                    SET NOCOUNT ON;
                    
                    DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;
                    
                    -- Temporary tables for comma-separated IDs
                    DECLARE @CategoryIdTable TABLE (Id UNIQUEIDENTIFIER);
                    DECLARE @BrandIdTable TABLE (Id UNIQUEIDENTIFIER);
                    DECLARE @VendorIdTable TABLE (Id NVARCHAR(450));
                    
                    -- Parse comma-separated category IDs
                    IF @CategoryIds IS NOT NULL AND @CategoryIds != ''
                    BEGIN
                        INSERT INTO @CategoryIdTable
                        SELECT CAST(value AS UNIQUEIDENTIFIER) 
                        FROM STRING_SPLIT(@CategoryIds, ',')
                        WHERE value != '';
                    END
                    
                    -- Parse comma-separated brand IDs
                    IF @BrandIds IS NOT NULL AND @BrandIds != ''
                    BEGIN
                        INSERT INTO @BrandIdTable
                        SELECT CAST(value AS UNIQUEIDENTIFIER) 
                        FROM STRING_SPLIT(@BrandIds, ',')
                        WHERE value != '';
                    END
                    
                    -- Parse comma-separated vendor IDs
                    IF @VendorIds IS NOT NULL AND @VendorIds != ''
                    BEGIN
                        INSERT INTO @VendorIdTable
                        SELECT value 
                        FROM STRING_SPLIT(@VendorIds, ',')
                        WHERE value != '';
                    END
                    
                    -- Main Query: Join Items with Offers and Pricing
                    ;WITH ItemOffers AS (
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
                            
                            -- Offer information
                            o.Id AS OfferId,
                            o.UserId AS VendorId,
                            o.HandlingTimeInDays,
                            
                            -- Price and quantity information
                            p.SalesPrice,
                            p.Price AS OriginalPrice,
                            p.AvailableQuantity,
                            p.StockStatus,
                            p.IsDefault AS IsBuyBoxWinner,
                            p.IsFreeShipping,
                            p.EstimatedDeliveryDays
                            
                        FROM TbItems i
                        INNER JOIN TbOffers o ON i.Id = o.ItemId
                        INNER JOIN TbOfferCombinationPricing p ON o.Id = p.OfferId
                        
                        WHERE 
                            -- Only active items
                            i.IsActive = 1
                            
                            -- Only public/visible offers (Active = 1)
                            AND o.VisibilityScope = 1
                            
                            -- Text search filter
                            AND (
                                @SearchTerm IS NULL 
                                OR i.TitleAr LIKE '%' + @SearchTerm + '%'
                                OR i.TitleEn LIKE '%' + @SearchTerm + '%'
                                OR i.ShortDescriptionAr LIKE '%' + @SearchTerm + '%'
                                OR i.ShortDescriptionEn LIKE '%' + @SearchTerm + '%'
                            )
                            
                            -- Category filter
                            AND (
                                NOT EXISTS (SELECT 1 FROM @CategoryIdTable)
                                OR i.CategoryId IN (SELECT Id FROM @CategoryIdTable)
                            )
                            
                            -- Brand filter
                            AND (
                                NOT EXISTS (SELECT 1 FROM @BrandIdTable)
                                OR i.BrandId IN (SELECT Id FROM @BrandIdTable)
                            )
                            
                            -- Vendor filter
                            AND (
                                NOT EXISTS (SELECT 1 FROM @VendorIdTable)
                                OR o.UserId IN (SELECT Id FROM @VendorIdTable)
                            )
                            
                            -- Price range filter
                            AND (@MinPrice IS NULL OR p.SalesPrice >= @MinPrice)
                            AND (@MaxPrice IS NULL OR p.SalesPrice <= @MaxPrice)
                            
                            -- Stock availability filter
                            AND (
                                @InStockOnly = 0 
                                OR (p.AvailableQuantity > 0 AND p.StockStatus = 1)
                            )
                            
                            -- Free shipping filter
                            AND (@FreeShippingOnly = 0 OR p.IsFreeShipping = 1)
                            
                            -- On sale filter (price < original price)
                            AND (@OnSaleOnly = 0 OR p.SalesPrice < p.Price)
                            
                            -- Buy box winners filter
                            AND (@BuyBoxWinnersOnly = 0 OR p.IsDefault = 1)
                            
                            -- Delivery days filter
                            AND (
                                @MaxDeliveryDays IS NULL 
                                OR p.EstimatedDeliveryDays <= @MaxDeliveryDays
                            )
                    ),
                    
                    -- Group results by item
                    GroupedItems AS (
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
                            
                            -- Calculate min and max prices
                            MIN(SalesPrice) AS MinPrice,
                            MAX(SalesPrice) AS MaxPrice,
                            
                            -- Count total offers for this item
                            COUNT(DISTINCT OfferId) AS OffersCount,
                            
                            -- Get fastest delivery time
                            MIN(EstimatedDeliveryDays) AS FastestDelivery,
                            
                            -- Get best offer (Buy Box winner)
                            (
                                SELECT TOP 1 
                                    CONCAT(
                                        OfferId, '|',
                                        VendorId, '|',
                                        SalesPrice, '|',
                                        OriginalPrice, '|',
                                        AvailableQuantity, '|',
                                        CAST(IsFreeShipping AS INT), '|',
                                        EstimatedDeliveryDays
                                    )
                                FROM ItemOffers io2
                                WHERE io2.ItemId = io.ItemId
                                ORDER BY 
                                    IsBuyBoxWinner DESC,
                                    SalesPrice ASC,
                                    EstimatedDeliveryDays ASC
                            ) AS BestOfferData
                            
                        FROM ItemOffers io
                        GROUP BY 
                            ItemId, TitleAr, TitleEn, ShortDescriptionAr, 
                            ShortDescriptionEn, CategoryId, BrandId, 
                            ThumbnailImage, CreatedDateUtc
                    ),
                    
                    -- Rank items by sort criteria
                    RankedItems AS (
                        SELECT 
                            *,
                            ROW_NUMBER() OVER (
                                ORDER BY 
                                    CASE 
                                        WHEN @SortBy = 'price_asc' THEN MinPrice
                                        WHEN @SortBy = 'price_desc' THEN MaxPrice
                                        WHEN @SortBy = 'fastest_delivery' THEN FastestDelivery
                                        ELSE CreatedDateUtc
                                    END,
                                    CASE 
                                        WHEN @SortBy = 'price_desc' THEN MaxPrice DESC
                                        WHEN @SortBy = 'newest' THEN CreatedDateUtc DESC
                                        WHEN @SortBy = 'fastest_delivery' THEN FastestDelivery ASC
                                        ELSE MinPrice ASC
                                    END
                            ) AS RowNum
                        FROM GroupedItems
                    )
                    
                    -- Final paginated results
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
                        MinPrice,
                        MaxPrice,
                        OffersCount,
                        FastestDelivery,
                        BestOfferData
                    FROM RankedItems
                    WHERE RowNum > @Offset AND RowNum <= (@Offset + @PageSize)
                    ORDER BY RowNum;
                    
                    -- Return total count
                    SELECT COUNT(DISTINCT ItemId) AS TotalRecords
                    FROM ItemOffers;
                END
            ");

            // ============================================================================
            // Create View for Quick Price Lookups (Denormalized for Performance)
            // ============================================================================

            migrationBuilder.Sql(@"
                CREATE VIEW [dbo].[VwItemBestPrices] AS
                SELECT 
                    i.Id AS ItemId,
                    MIN(p.SalesPrice) AS BestPrice,
                    MAX(p.AvailableQuantity) AS TotalStock,
                    COUNT(DISTINCT o.Id) AS TotalOffers,
                    MAX(CAST(p.IsFreeShipping AS INT)) AS HasFreeShipping,
                    MIN(p.EstimatedDeliveryDays) AS FastestDelivery
                FROM TbItems i
                INNER JOIN TbOffers o ON i.Id = o.ItemId
                INNER JOIN TbOfferCombinationPricing p ON o.Id = p.OfferId
                WHERE 
                    i.IsActive = 1 
                    AND o.VisibilityScope = 1
                    AND p.AvailableQuantity > 0
                GROUP BY i.Id
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop view
            migrationBuilder.Sql("DROP VIEW IF EXISTS [dbo].[VwItemBestPrices]");

            // Drop stored procedure
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[SpSearchItemsMultiVendor]");

            // Drop indexes on TbOfferCombinationPricing
            migrationBuilder.DropIndex(
                name: "IX_TbOfferCombinationPricing_OfferId_SalesPrice",
                table: "TbOfferCombinationPricing");

            migrationBuilder.DropIndex(
                name: "IX_TbOfferCombinationPricing_SalesPrice",
                table: "TbOfferCombinationPricing");

            migrationBuilder.DropIndex(
                name: "IX_TbOfferCombinationPricing_StockStatus_AvailableQuantity",
                table: "TbOfferCombinationPricing");

            migrationBuilder.DropIndex(
                name: "IX_TbOfferCombinationPricing_IsDefault",
                table: "TbOfferCombinationPricing");

            // Drop indexes on TbOffers
            migrationBuilder.DropIndex(
                name: "IX_TbOffers_ItemId_UserId",
                table: "TbOffers");

            migrationBuilder.DropIndex(
                name: "IX_TbOffers_VisibilityScope",
                table: "TbOffers");

            mi








                rationBuilder.DropIndex(
                name: "IX_TbOffers_StorgeLocation",
                table: "TbOffers");

            migrationBuilder.DropIndex(
                name: "IX_TbOffers_UserId",
                table: "TbOffers");

            // Drop indexes on TbItems
            migrationBuilder.DropIndex(
                name: "IX_TbItems_TitleAr",
                table: "TbItems");

            migrationBuilder.DropIndex(
                name: "IX_TbItems_TitleEn",
                table: "TbItems");

            migrationBuilder.DropIndex(
                name: "IX_TbItems_CategoryId_BrandId",
                table: "TbItems");

            migrationBuilder.DropIndex(
                name: "IX_TbItems_IsActive_CreatedDate",
                table: "TbItems");
        }
    }
}
