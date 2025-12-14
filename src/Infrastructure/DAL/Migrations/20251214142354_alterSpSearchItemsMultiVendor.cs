using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class alterSpSearchItemsMultiVendor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"ALTER PROCEDURE [dbo].[SpSearchItemsMultiVendor]
    -- Parameters (same as before)
    @SearchTerm NVARCHAR(255) = NULL,
    @CategoryIds NVARCHAR(MAX) = NULL,
    @BrandIds NVARCHAR(MAX) = NULL,
    @MinPrice DECIMAL(18,2) = NULL,
    @MaxPrice DECIMAL(18,2) = NULL,
    @MinItemRating DECIMAL(3,2) = NULL,
    @InStockOnly BIT = 0,
    @FreeShippingOnly BIT = 0,
    @VendorIds NVARCHAR(MAX) = NULL,
    @ConditionIds NVARCHAR(MAX) = NULL,
    @WithWarrantyOnly BIT = 0,
    @AttributeIds NVARCHAR(MAX) = NULL,
    @AttributeValues NVARCHAR(MAX) = NULL,
    @SortBy NVARCHAR(50) = 'relevance',
    @PageNumber INT = 1,
    @PageSize INT = 20
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;
    
    -- Temporary tables
    DECLARE @CategoryIdTable TABLE (Id UNIQUEIDENTIFIER);
    DECLARE @BrandIdTable TABLE (Id UNIQUEIDENTIFIER);
    DECLARE @VendorIdTable TABLE (Id UNIQUEIDENTIFIER);
    DECLARE @ConditionIdTable TABLE (Id UNIQUEIDENTIFIER);
    DECLARE @AttributeIdTable TABLE (Id UNIQUEIDENTIFIER, ValueIds NVARCHAR(MAX));

    -- Parse category IDs
    IF @CategoryIds IS NOT NULL AND @CategoryIds != ''
    BEGIN
        INSERT INTO @CategoryIdTable
        SELECT CAST(value AS UNIQUEIDENTIFIER) 
        FROM STRING_SPLIT(@CategoryIds, ',')
        WHERE value != '';
    END

    -- Parse brand IDs
    IF @BrandIds IS NOT NULL AND @BrandIds != ''
    BEGIN
        INSERT INTO @BrandIdTable
        SELECT CAST(value AS UNIQUEIDENTIFIER) 
        FROM STRING_SPLIT(@BrandIds, ',')
        WHERE value != '';
    END

    -- Parse vendor IDs
    IF @VendorIds IS NOT NULL AND @VendorIds != ''
    BEGIN
        INSERT INTO @VendorIdTable
        SELECT CAST(value AS UNIQUEIDENTIFIER)
        FROM STRING_SPLIT(@VendorIds, ',')
        WHERE value != '';
    END

    -- Parse condition IDs
    IF @ConditionIds IS NOT NULL AND @ConditionIds != ''
    BEGIN
        INSERT INTO @ConditionIdTable
        SELECT CAST(value AS UNIQUEIDENTIFIER)
        FROM STRING_SPLIT(@ConditionIds, ',')
        WHERE value != '';
    END

    -- Parse attribute filters
    IF @AttributeIds IS NOT NULL AND @AttributeIds != ''
    BEGIN
        DECLARE @AttrIdList TABLE (RowNum INT, AttributeId UNIQUEIDENTIFIER);
        DECLARE @AttrValueList TABLE (RowNum INT, ValueIds NVARCHAR(MAX));
        
        INSERT INTO @AttrIdList
        SELECT 
            ROW_NUMBER() OVER (ORDER BY (SELECT NULL)),
            CAST(value AS UNIQUEIDENTIFIER)
        FROM STRING_SPLIT(@AttributeIds, ',')
        WHERE value != '';
        
        INSERT INTO @AttrValueList
        SELECT 
            ROW_NUMBER() OVER (ORDER BY (SELECT NULL)),
            value
        FROM STRING_SPLIT(@AttributeValues, '|')
        WHERE value != '';
        
        INSERT INTO @AttributeIdTable
        SELECT a.AttributeId, v.ValueIds
        FROM @AttrIdList a
        INNER JOIN @AttrValueList v ON a.RowNum = v.RowNum;
    END

    -- =============================
    -- Main Search Query
    -- =============================
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
            
            -- Offer data
            o.Id AS OfferId,
            o.VendorId,
            o.IsBuyBoxWinner,
            o.OfferConditionId,
            o.WarrantyId,
            o.IsFreeShipping,                    -- ✅ من TbOffer
            o.HandlingTimeInDays,                -- ✅ من TbOffer
            o.VendorRatingForThisItem,           -- ✅ من TbOffer
            o.FulfillmentType,                   -- ✅ من TbOffer
            
            -- Pricing data
            p.SalesPrice,
            p.Price AS OriginalPrice,
            p.AvailableQuantity,
            p.StockStatus,
            p.ItemCombinationId
            
        FROM TbItems i
        INNER JOIN TbOffers o ON i.Id = o.ItemId
        INNER JOIN TbOfferCombinationPricings p ON o.Id = p.OfferId
        WHERE
            -- Active items only
            i.IsActive = 1 AND i.IsDeleted = 0
            
            -- Active offers only (VisibilityScope: 1 = Active/Public)
            AND o.VisibilityScope = 1 AND o.IsDeleted = 0
            
            -- Active pricing only
            AND p.IsDeleted = 0
            
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
                OR o.VendorId IN (SELECT Id FROM @VendorIdTable)
            )
            
            -- Condition filter
            AND (
                NOT EXISTS (SELECT 1 FROM @ConditionIdTable)
                OR o.OfferConditionId IN (SELECT Id FROM @ConditionIdTable)
            )
            
            -- Warranty filter
            AND (@WithWarrantyOnly = 0 OR o.WarrantyId IS NOT NULL)
            
            -- Price range filter
            AND (@MinPrice IS NULL OR p.SalesPrice >= @MinPrice)
            AND (@MaxPrice IS NULL OR p.SalesPrice <= @MaxPrice)
            
            -- Stock availability filter
            AND (@InStockOnly = 0 OR (p.AvailableQuantity > 0 AND p.StockStatus = 1))
            
            -- Free shipping filter (✅ من TbOffer)
            AND (@FreeShippingOnly = 0 OR o.IsFreeShipping = 1)
    ),
    
    -- Filter by attributes (if specified)
    AttributeFiltered AS (
        SELECT DISTINCT ib.*
        FROM ItemBase ib
        WHERE 
            NOT EXISTS (SELECT 1 FROM @AttributeIdTable)
            OR
            NOT EXISTS (
                SELECT 1 
                FROM @AttributeIdTable attr
                WHERE NOT EXISTS (
                    SELECT 1
                    FROM TbItemCombinations ic
                    INNER JOIN TbCombinationAttributes ca ON ic.Id = ca.ItemCombinationId
                    INNER JOIN TbItemAttributeValues iav ON ca.AttributeValueId = iav.Id
                    WHERE ic.ItemId = ib.ItemId
                        AND iav.AttributeId = attr.Id
                        AND iav.Id IN (
                            SELECT CAST(value AS UNIQUEIDENTIFIER)
                            FROM STRING_SPLIT(attr.ValueIds, ',')
                        )
                )
            )
    ),
    
    -- Apply scoring
    ScoredItems AS (
        SELECT
            *,
            -- Text relevance score
            CASE
                WHEN @SearchTerm IS NULL THEN 0
                WHEN TitleAr LIKE @SearchTerm + '%' OR TitleEn LIKE @SearchTerm + '%' THEN 1.0
                WHEN TitleAr LIKE '%' + @SearchTerm + '%' OR TitleEn LIKE '%' + @SearchTerm + '%' THEN 0.8
                WHEN ShortDescriptionAr LIKE '%' + @SearchTerm + '%' OR ShortDescriptionEn LIKE '%' + @SearchTerm + '%' THEN 0.5
                ELSE 0.3
            END AS TextScore,
            
            -- Buy box score
            CASE WHEN IsBuyBoxWinner = 1 THEN 1.0 ELSE 0 END AS BuyBoxScore,
            
            -- Stock score
            CASE
                WHEN AvailableQuantity >= 50 THEN 1.0
                WHEN AvailableQuantity >= 10 THEN 0.7
                WHEN AvailableQuantity > 0 THEN 0.4
                ELSE 0
            END AS StockScore,
            
            -- Price score (discount score)
            CASE
                WHEN SalesPrice < OriginalPrice THEN 1.0
                ELSE 0.5
            END AS PriceScore,
            
            -- Shipping score (✅ using HandlingTimeInDays from TbOffer)
            CASE
                WHEN IsFreeShipping = 1 THEN 1.0
                WHEN HandlingTimeInDays <= 2 THEN 0.8
                WHEN HandlingTimeInDays <= 5 THEN 0.5
                ELSE 0.3
            END AS ShippingScore,
            
            -- Vendor rating score
            CASE
                WHEN VendorRatingForThisItem >= 4.5 THEN 1.0
                WHEN VendorRatingForThisItem >= 4.0 THEN 0.8
                WHEN VendorRatingForThisItem >= 3.5 THEN 0.6
                WHEN VendorRatingForThisItem >= 3.0 THEN 0.4
                ELSE 0.2
            END AS VendorScore
        FROM AttributeFiltered
    ),
    
    -- Aggregate by item
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
            MIN(HandlingTimeInDays) AS FastestDelivery,  -- ✅ Using HandlingTimeInDays
            MAX(TextScore) AS TextScore,
            MAX(BuyBoxScore) AS BuyBoxScore,
            AVG(StockScore) AS StockScore,
            AVG(PriceScore) AS PriceScore,
            AVG(ShippingScore) AS ShippingScore,
            AVG(VendorScore) AS VendorScore,
            (
                SELECT TOP 1 
                    CONCAT(
                        OfferId, '|',
                        VendorId, '|',
                        SalesPrice, '|',
                        OriginalPrice, '|',
                        AvailableQuantity, '|',
                        CAST(IsFreeShipping AS INT), '|',
                        HandlingTimeInDays, '|',                    -- ✅ من TbOffer
                        CAST(IsBuyBoxWinner AS INT), '|',
                        FulfillmentType                              -- ✅ من TbOffer
                    )
                FROM ScoredItems si2
                WHERE si2.ItemId = si.ItemId
                ORDER BY IsBuyBoxWinner DESC, SalesPrice ASC, HandlingTimeInDays ASC
            ) AS BestOfferData
        FROM ScoredItems si
        GROUP BY 
            ItemId, TitleAr, TitleEn, ShortDescriptionAr, 
            ShortDescriptionEn, CategoryId, BrandId, 
            ThumbnailImage, CreatedDateUtc
    ),
    
    -- Calculate final score and rank
    RankedItems AS (
        SELECT
            *,
            -- Calculate final relevance score
            (
                (TextScore * 0.25) +
                (BuyBoxScore * 0.20) +
                (VendorScore * 0.15) +
                (StockScore * 0.10) +
                (PriceScore * 0.10) +
                (ShippingScore * 0.10) +
                (CAST(OffersCount AS FLOAT) / 10.0 * 0.10)
            ) AS FinalScore,
            ROW_NUMBER() OVER (
                ORDER BY
                    CASE WHEN @SortBy = 'price_asc' THEN MinPrice ELSE 99999999 END ASC,
                    CASE WHEN @SortBy = 'price_desc' THEN MaxPrice ELSE 0 END DESC,
                    CASE WHEN @SortBy = 'newest arrival' THEN DATEDIFF(SECOND, '2000-01-01', CreatedDateUtc) ELSE 0 END DESC,
                    CASE WHEN @SortBy = 'best seller' THEN BuyBoxScore ELSE 0 END DESC,
                    CASE WHEN @SortBy = 'customer review' THEN VendorScore ELSE 0 END DESC,
                    CASE WHEN @SortBy IN ('relevance', 'Featured') THEN
                        (
                            (TextScore * 0.25) +
                            (BuyBoxScore * 0.20) +
                            (VendorScore * 0.15) +
                            (StockScore * 0.10) +
                            (PriceScore * 0.10) +
                            (ShippingScore * 0.10) +
                            (CAST(OffersCount AS FLOAT) / 10.0 * 0.10)
                        )
                    ELSE 0 END DESC,
                    ItemId
            ) AS RowNum
        FROM Aggregated
    )
    
    -- Return paginated results
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
        BestOfferData,
        FinalScore
    FROM RankedItems
    WHERE RowNum > @Offset AND RowNum <= (@Offset + @PageSize)
    ORDER BY RowNum;

    -- Return total count
    SELECT COUNT(*) AS TotalRecords FROM Aggregated;
END");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                ALTER PROCEDURE [dbo].[SpSearchItemsMultiVendor]
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
        }
    }
}
