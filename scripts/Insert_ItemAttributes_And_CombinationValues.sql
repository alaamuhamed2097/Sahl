USE [Basit]
GO

-- Variable to hold the dynamic SQL
DECLARE @SQL NVARCHAR(MAX) = '';

-- 1. Generate Inserts for TbItemAttributes (AffectsPricing = 0)
SELECT @SQL = @SQL + 
'INSERT INTO [dbo].[TbItemAttributes] ([Id], [ItemId], [AttributeId], [Value], [DisplayOrder], [TitleAr], [TitleEn], [IsRangeFieldType], [FieldType], [MaxLength], [CreatedBy], [CreatedDateUtc], [UpdatedBy], [UpdatedDateUtc], [IsDeleted]) VALUES (' +
'NEWID(), ' + 
'''' + CAST(i.Id AS NVARCHAR(100)) + ''', ' +
'''' + CAST(ca.AttributeId AS NVARCHAR(100)) + ''', ' +
-- Logic for Value: FieldType 7 (MultiSelectList) -> List of IDs, FieldType 6 (List) -> Single ID, else Text
CASE WHEN a.FieldType = 7 THEN 
    -- Concatenate the first 2 available option IDs for the MultiSelect dummy value
    '''''' + STRING_AGG(CAST(o.Id AS NVARCHAR(100)), ',') WITHIN GROUP (ORDER BY o.DisplayOrder) + ''''
WHEN a.FieldType = 6 THEN 
    -- Get the first available option ID
    '''' + (SELECT TOP 1 CAST(Id AS NVARCHAR(100)) FROM dbo.TbAttributeOptions WHERE AttributeId = ca.AttributeId ORDER BY DisplayOrder) + ''''
ELSE 
    -- Text/Integer dummy value based on Attribute Title
    CASE 
        WHEN a.TitleEn LIKE '%Color%' THEN '''Black'''
        WHEN a.TitleEn LIKE '%Size%' THEN '''L'''
        WHEN a.TitleEn LIKE '%Material%' THEN '''Cotton'''
        WHEN a.TitleEn LIKE '%Warranty%' THEN '''2 Years'''
        WHEN a.TitleEn LIKE '%Dimensions%' THEN '''10x20x30 cm'''
        WHEN a.TitleEn LIKE '%Special%' THEN '''Waterproof, Fast Charging'''
        WHEN a.TitleEn LIKE '%OS%' THEN '''Android 13'''
        ELSE '''Standard'''
    END
END + ', ' + 
CAST(ca.DisplayOrder AS NVARCHAR(10)) + ', ' + 
'''' + REPLACE(a.TitleAr, '''', '''''') + ''', ' +
'''' + REPLACE(a.TitleEn, '''', '''''') + ''', ' +
CAST(a.IsRangeFieldType AS NVARCHAR(10)) + ', ' + 
CAST(a.FieldType AS NVARCHAR(10)) + ', ' + 
CAST(ISNULL(a.MaxLength, 0) AS NVARCHAR(10)) + ', ' +  -- Changed to use 0 instead of NULL
'''' + CAST(i.CreatedBy AS NVARCHAR(100)) + ''', ' +
'''' + CONVERT(NVARCHAR(30), i.CreatedDateUtc, 121) + ''', ' +
'NULL, NULL, 0);' + CHAR(13) + CHAR(10)
FROM dbo.TbItems i
INNER JOIN dbo.TbCategoryAttributes ca ON i.CategoryId = ca.CategoryId
INNER JOIN dbo.TbAttributes a ON ca.AttributeId = a.Id
-- Join for MultiSelect options (FieldType 7)
LEFT JOIN dbo.TbAttributeOptions o ON a.Id = o.AttributeId AND a.FieldType = 7
-- Filter for AffectsPricing = 0
WHERE ca.AffectsPricing = 0 
GROUP BY i.Id, ca.AttributeId, a.FieldType, a.IsRangeFieldType, a.MaxLength, a.TitleAr, a.TitleEn, ca.DisplayOrder, i.CreatedBy, i.CreatedDateUtc
ORDER BY i.Id, ca.DisplayOrder;

-- 2. Generate Inserts for TbCombinationAttributesValues (AffectsPricing = 1)
SELECT @SQL = @SQL + 
'INSERT INTO [dbo].[TbCombinationAttributesValues] ([Id], [AttributeId], [Value], [CreatedBy], [CreatedDateUtc], [UpdatedBy], [UpdatedDateUtc], [IsDeleted]) VALUES (' +
'NEWID(), ' + 
'''' + CAST(ca.AttributeId AS NVARCHAR(100)) + ''', ' +
-- Logic for Value: Always an AttributeOption ID (Taking the first available for dummy data)
'''' + (SELECT TOP 1 CAST(o.Id AS NVARCHAR(100)) FROM dbo.TbAttributeOptions o WHERE o.AttributeId = ca.AttributeId ORDER BY o.DisplayOrder) + ''', ' +
'''' + CAST(i.CreatedBy AS NVARCHAR(100)) + ''', ' +
'''' + CONVERT(NVARCHAR(30), i.CreatedDateUtc, 121) + ''', ' +
'NULL, NULL, 0);' + CHAR(13) + CHAR(10)
FROM dbo.TbItems i
INNER JOIN dbo.TbCategoryAttributes ca ON i.CategoryId = ca.CategoryId
-- Filter for AffectsPricing = 1
WHERE ca.AffectsPricing = 1 
ORDER BY i.Id, ca.DisplayOrder;

-- Optional: Print to review before executing
-- PRINT @SQL;

-- Execute the generated batch
EXEC sp_executesql @SQL;
GO