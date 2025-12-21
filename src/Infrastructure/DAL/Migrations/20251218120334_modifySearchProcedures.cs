using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class modifySearchProcedures : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
ALTER PROCEDURE [dbo].[SpSearchItemsMultiVendor]
    -- Search Context Parameters
    @SearchTerm NVARCHAR(255) = NULL,      -- من البحث في الصفحة الرئيسية
    @CategoryId UNIQUEIDENTIFIER = NULL,   -- لما يفتح كاتيجوري معين
    @VendorId UNIQUEIDENTIFIER = NULL,     -- لما يفتح منتجات بائع معين
    @BrandId UNIQUEIDENTIFIER = NULL,      -- لما يفتح براند معين
    
    -- Filter Parameters
    @MinPrice DECIMAL(18,2) = NULL,
    @MaxPrice DECIMAL(18,2) = NULL,
    @MinItemRating DECIMAL(3,2) = NULL,
    @InStockOnly BIT = 0,
    @FreeShippingOnly BIT = 0,
    @ConditionId UNIQUEIDENTIFIER = NULL,
    @WithWarrantyOnly BIT = 0,
    @AttributeIds NVARCHAR(MAX) = NULL,
    @AttributeValues NVARCHAR(MAX) = NULL,
    
    -- Sorting & Pagination
    @SortBy NVARCHAR(50) = 'relevance',
    @PageNumber INT = 1,
    @PageSize INT = 20
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;
    
    -- Temporary table for attribute filters
    DECLARE @AttributeIdTable TABLE (Id UNIQUEIDENTIFIER, ValueIds NVARCHAR(MAX));

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
            b.NameAr AS BrandNameAr,
            b.NameEn AS BrandNameEn,
            i.ThumbnailImage,
            i.CreatedDateUtc,
            i.AverageRating,
            
            -- Offer data
            o.Id AS OfferId,
            o.IsBuyBoxWinner,
            o.IsFreeShipping,
            o.HandlingTimeInDays,
            
            -- Pricing data
            p.SalesPrice,
            p.Price,
            p.AvailableQuantity,
            p.StockStatus,
            p.ItemCombinationId
            
        FROM TbItems i
        INNER JOIN TbOffers o ON i.Id = o.ItemId
        INNER JOIN TbOfferCombinationPricings p ON o.Id = p.OfferId
        LEFT JOIN TbBrands b ON i.BrandId = b.Id AND b.IsDeleted = 0
        WHERE
            -- Active items only
            i.IsActive = 1 AND i.IsDeleted = 0
            
            -- Active offers only (VisibilityScope: 1 = Active/Public)
            AND o.VisibilityScope = 1 AND o.IsDeleted = 0
            
            -- Active pricing only
            AND p.IsDeleted = 0
            
            -- Stock Status Filter (InStock=1, OutOfStock=2, LimitedStock=3, ComingSoon=4)
            AND p.StockStatus IN (1, 2, 3, 4)
            
            -- Text search filter (Case-Insensitive for English, Accent-Insensitive for Arabic)
            AND (
                @SearchTerm IS NULL 
                OR i.TitleAr COLLATE Arabic_CI_AI LIKE '%' + @SearchTerm COLLATE Arabic_CI_AI + '%'
                OR i.TitleEn COLLATE Latin1_General_CI_AI LIKE '%' + @SearchTerm COLLATE Latin1_General_CI_AI + '%'
                OR i.ShortDescriptionAr COLLATE Arabic_CI_AI LIKE '%' + @SearchTerm COLLATE Arabic_CI_AI + '%'
                OR i.ShortDescriptionEn COLLATE Latin1_General_CI_AI LIKE '%' + @SearchTerm COLLATE Latin1_General_CI_AI + '%'
            )
            
            -- Context filters
            AND (@CategoryId IS NULL OR i.CategoryId = @CategoryId)
            AND (@VendorId IS NULL OR o.VendorId = @VendorId)
            AND (@BrandId IS NULL OR i.BrandId = @BrandId)
            
            -- Additional filters
            AND (@ConditionId IS NULL OR p.OfferConditionId = @ConditionId)
            AND (@WithWarrantyOnly = 0 OR o.WarrantyId IS NOT NULL)
            AND (@MinPrice IS NULL OR p.SalesPrice >= @MinPrice)
            AND (@MaxPrice IS NULL OR p.SalesPrice <= @MaxPrice)
            AND (@MinItemRating IS NULL OR i.AverageRating >= @MinItemRating)
            AND (@InStockOnly = 0 OR (p.AvailableQuantity > 0 AND p.StockStatus = 1))
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
                    FROM TbCombinationAttributesValues cav
                    WHERE cav.ItemCombinationId = ib.ItemCombinationId
                        AND cav.AttributeId = attr.Id
                        AND cav.Id IN (
                            SELECT CAST(value AS UNIQUEIDENTIFIER)
                            FROM STRING_SPLIT(attr.ValueIds, ',')
                        )
                )
            )
    ),
    
    -- Select BuyBox Winner or Best Offer per Item
    BestOffers AS (
        SELECT
            ItemId,
            TitleAr,
            TitleEn,
            ShortDescriptionAr,
            ShortDescriptionEn,
            CategoryId,
            BrandId,
            BrandNameAr,
            BrandNameEn,
            ThumbnailImage,
            CreatedDateUtc,
            AverageRating,
            SalesPrice,
            Price,
            AvailableQuantity,
            StockStatus,
            IsBuyBoxWinner,
            IsFreeShipping,
            HandlingTimeInDays,
            -- Prioritize BuyBox winner, then lowest price, then fastest shipping
            ROW_NUMBER() OVER (
                PARTITION BY ItemId 
                ORDER BY 
                    IsBuyBoxWinner DESC,
                    SalesPrice ASC,
                    HandlingTimeInDays ASC
            ) AS OfferRank
        FROM AttributeFiltered
    ),
    
    -- Get only the best offer per item
    SelectedOffers AS (
        SELECT
            ItemId,
            TitleAr,
            TitleEn,
            ShortDescriptionAr,
            ShortDescriptionEn,
            CategoryId,
            BrandId,
            BrandNameAr,
            BrandNameEn,
            ThumbnailImage,
            CreatedDateUtc,
            AverageRating,
            Price,
            SalesPrice,
            AvailableQuantity,
            CASE StockStatus
                WHEN 1 THEN 'InStock'
                WHEN 2 THEN 'OutOfStock'
                WHEN 3 THEN 'LimitedStock'
                WHEN 4 THEN 'ComingSoon'
                ELSE 'Unknown'
            END AS StockStatus,
            IsBuyBoxWinner,
            IsFreeShipping
        FROM BestOffers
        WHERE OfferRank = 1
    ),
    
    -- Apply sorting and ranking
    RankedItems AS (
        SELECT
            *,
            ROW_NUMBER() OVER (
                ORDER BY
                    CASE WHEN @SortBy = 'price_asc' THEN SalesPrice ELSE 99999999 END ASC,
                    CASE WHEN @SortBy = 'price_desc' THEN SalesPrice ELSE 0 END DESC,
                    CASE WHEN @SortBy = 'newest arrival' THEN DATEDIFF(SECOND, '2000-01-01', CreatedDateUtc) ELSE 0 END DESC,
                    CASE WHEN @SortBy = 'best seller' THEN CAST(IsBuyBoxWinner AS INT) ELSE 0 END DESC,
                    CASE WHEN @SortBy = 'customer review' THEN ISNULL(AverageRating, 0) ELSE 0 END DESC,
                    CASE WHEN @SortBy IN ('relevance', 'Featured') THEN CAST(IsBuyBoxWinner AS INT) ELSE 0 END DESC,
                    ItemId
            ) AS RowNum
        FROM SelectedOffers
    )
    
    -- Return paginated results with total count
    SELECT 
        ItemId,
        TitleAr,
        TitleEn,
        ShortDescriptionAr,
        ShortDescriptionEn,
        CategoryId,
        BrandId,
        BrandNameAr,
        BrandNameEn,
        ThumbnailImage,
        CreatedDateUtc,
        AverageRating,
        Price,
        SalesPrice,
        AvailableQuantity,
        StockStatus,
        IsFreeShipping,
        (SELECT COUNT(*) FROM SelectedOffers) as TotalRecords
    FROM RankedItems
    WHERE RowNum > @Offset AND RowNum <= (@Offset + @PageSize)
    ORDER BY RowNum;

END
");

            migrationBuilder.Sql(@"
ALTER   PROCEDURE [dbo].[SpGetAvailableSearchFilters]
    @SearchTerm NVARCHAR(255) = NULL,      -- من البحث في الصفحة الرئيسية
    @CategoryId UNIQUEIDENTIFIER = NULL,   -- لما يفتح كاتيجوري معين
    @VendorId UNIQUEIDENTIFIER = NULL      -- لما يفتح منتجات بائع معين
AS
BEGIN
    SET NOCOUNT ON;
    
    -- =============================
    -- Base Items Query
    -- =============================
    ;WITH BaseItems AS (
        SELECT DISTINCT
            i.Id AS ItemId,
            i.CategoryId,
            i.BrandId,
            o.VendorId,
            p.OfferConditionId,
            o.IsFreeShipping,
            o.WarrantyId,
            p.SalesPrice,
            p.AvailableQuantity,
            p.StockStatus,
            ic.Id AS ItemCombinationId
        FROM TbItems i
        INNER JOIN TbOffers o ON i.Id = o.ItemId
        INNER JOIN TbOfferCombinationPricings p ON o.Id = p.OfferId
        LEFT JOIN TbItemCombinations ic ON p.ItemCombinationId = ic.Id
        WHERE
            i.IsActive = 1 AND i.IsDeleted = 0
            AND o.VisibilityScope = 1 AND o.IsDeleted = 0
            AND p.IsDeleted = 0
            AND (
                @SearchTerm IS NULL 
                OR i.TitleAr COLLATE ARABIC_CI_AI LIKE '%' + @SearchTerm + '%' COLLATE ARABIC_CI_AI
                OR i.TitleEn LIKE '%' + @SearchTerm + '%'
                OR i.ShortDescriptionAr COLLATE ARABIC_CI_AI LIKE '%' + @SearchTerm + '%' COLLATE ARABIC_CI_AI
                OR i.ShortDescriptionEn LIKE '%' + @SearchTerm + '%'
            )
            AND (@CategoryId IS NULL OR i.CategoryId = @CategoryId)
            AND (@VendorId IS NULL OR o.VendorId = @VendorId)
    )
    
    SELECT 
        -- Categories JSON
        (
            SELECT 
                c.Id,
                c.TitleAr,
                c.TitleEn,
                c.Icon,
                COUNT(DISTINCT b.ItemId) AS ItemCount
            FROM BaseItems b
            INNER JOIN TbCategories c ON b.CategoryId = c.Id
            WHERE c.IsDeleted = 0
            GROUP BY c.Id, c.TitleAr, c.TitleEn, c.Icon
            HAVING COUNT(DISTINCT b.ItemId) > 0
            ORDER BY ItemCount DESC
            FOR JSON PATH
        ) AS CategoriesJson,
        
        -- Brands JSON
        (
            SELECT 
                br.Id,
                br.NameAr,
                br.NameEn,
                br.LogoPath AS LogoUrl,
                COUNT(DISTINCT b.ItemId) AS ItemCount
            FROM BaseItems b
            INNER JOIN TbBrands br ON b.BrandId = br.Id
            WHERE br.IsDeleted = 0
            GROUP BY br.Id, br.NameAr, br.NameEn, br.LogoPath
            HAVING COUNT(DISTINCT b.ItemId) > 0
            ORDER BY ItemCount DESC
            FOR JSON PATH
        ) AS BrandsJson,
        
        -- Vendors JSON (بس لو مش داخل من صفحة بائع)
        (
            SELECT 
                v.Id,
                ISNULL(v.CompanyName, '') AS StoreName,
                ISNULL(v.CompanyName, '') AS StoreNameAr,
                '' AS LogoUrl,
                COUNT(DISTINCT b.ItemId) AS ItemCount,
                ISNULL(v.Rating, 0) AS AvgRating
            FROM BaseItems b
            INNER JOIN TbVendors v ON b.VendorId = v.Id
            WHERE v.IsActive = 1 AND v.IsDeleted = 0
                AND @VendorId IS NULL  -- بس لو مش داخل من صفحة بائع
            GROUP BY v.Id, v.CompanyName, v.Rating
            HAVING COUNT(DISTINCT b.ItemId) > 0
            ORDER BY ItemCount DESC
            FOR JSON PATH
        ) AS VendorsJson,
        
        -- Price Range JSON
        (
            SELECT 
                MIN(SalesPrice) AS MinPrice,
                MAX(SalesPrice) AS MaxPrice,
                CAST(AVG(SalesPrice) AS DECIMAL(18,2)) AS AvgPrice
            FROM BaseItems
            FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
        ) AS PriceRangeJson,
        
        -- Attributes JSON
        (
            SELECT 
                attr.Id AS AttributeId,
                attr.TitleAr AS NameAr,
                attr.TitleEn AS NameEn,
                0 AS DisplayOrder,
                JSON_QUERY((
                    SELECT 
                        av2.Id AS ValueId,
                        av2.Value AS ValueAr,
                        av2.Value AS ValueEn,
                        COUNT(DISTINCT b2.ItemId) AS ItemCount
                    FROM BaseItems b2
                    INNER JOIN TbCombinationAttributesValues av2 ON b2.ItemCombinationId = av2.ItemCombinationId
                    WHERE av2.AttributeId = attr.Id
                    GROUP BY av2.Id, av2.Value
                    HAVING COUNT(DISTINCT b2.ItemId) > 0
                    ORDER BY ItemCount DESC
                    FOR JSON PATH
                )) AS [Values]
            FROM (
                SELECT DISTINCT a.Id, a.TitleAr, a.TitleEn
                FROM BaseItems b
                INNER JOIN TbCombinationAttributesValues ca ON b.ItemCombinationId = ca.ItemCombinationId
                INNER JOIN TbAttributes a ON ca.AttributeId = a.Id
                WHERE a.IsDeleted = 0
            ) attr
            ORDER BY attr.TitleEn
            FOR JSON PATH
        ) AS AttributesJson,
        
        -- Conditions JSON
        (
            SELECT 
                oc.Id,
                oc.NameAr,
                oc.NameEn,
                '' AS Description,
                COUNT(DISTINCT b.ItemId) AS ItemCount
            FROM BaseItems b
            INNER JOIN TbOfferConditions oc ON b.OfferConditionId = oc.Id
            WHERE oc.IsDeleted = 0
            GROUP BY oc.Id, oc.NameAr, oc.NameEn
            HAVING COUNT(DISTINCT b.ItemId) > 0
            ORDER BY ItemCount DESC
            FOR JSON PATH
        ) AS ConditionsJson,
        
        -- Feature Flags JSON
        (
            SELECT 
                SUM(CASE WHEN IsFreeShipping = 1 THEN 1 ELSE 0 END) AS FreeShippingCount,
                CASE WHEN SUM(CASE WHEN IsFreeShipping = 1 THEN 1 ELSE 0 END) > 0 THEN 1 ELSE 0 END AS HasFreeShipping,
                SUM(CASE WHEN WarrantyId IS NOT NULL THEN 1 ELSE 0 END) AS WithWarrantyCount,
                CASE WHEN SUM(CASE WHEN WarrantyId IS NOT NULL THEN 1 ELSE 0 END) > 0 THEN 1 ELSE 0 END AS HasWarranty,
                SUM(CASE WHEN AvailableQuantity > 0 AND StockStatus = 1 THEN 1 ELSE 0 END) AS InStockCount,
                CASE WHEN SUM(CASE WHEN AvailableQuantity > 0 AND StockStatus = 1 THEN 1 ELSE 0 END) > 0 THEN 1 ELSE 0 END AS HasInStock,
                COUNT(DISTINCT ItemId) AS TotalItems
            FROM BaseItems
            FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
        ) AS FeaturesJson;
END
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
