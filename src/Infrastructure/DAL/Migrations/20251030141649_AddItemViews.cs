using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddItemViews : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add VwItems
            migrationBuilder.Sql(@"                
CREATE OR ALTER VIEW [dbo].[VwItems]
                AS
                SELECT 
                    i.Id, 
                    i.TitleAr, 
                    i.TitleEn,
                    i.ShortDescriptionAr, 
                    i.ShortDescriptionEn,
                    i.CategoryId, 
                    c.TitleAr AS CategoryTitleAr, 
                    c.TitleEn AS CategoryTitleEn, 
                    i.UnitId, 
                    u.TitleAr AS UnitTitleAr, 
                    u.TitleEn AS UnitTitleEn, 
                    i.ThumbnailImage, 
                    i.StockStatus,
                    i.Quantity, 
                    i.Price,
                    i.CreatedDateUtc,
                    i.IsNewArrival,
                    i.IsBestSeller,
                    i.IsRecommended,
                    (
                        SELECT 
                            im.Id,
                            im.Path,
                            im.[Order]
                        FROM 
                            dbo.TbItemImages im
                        WHERE 
                            im.ItemId = i.Id 
                            AND im.CurrentState = 1
                        ORDER BY 
                            im.[Order]
                        FOR JSON PATH
                    ) AS ItemImagesJson
                FROM     
                    dbo.TbItems AS i 
                    INNER JOIN dbo.TbCategories AS c ON i.CategoryId = c.Id 
                    INNER JOIN dbo.TbUnits AS u ON i.UnitId = u.Id
                WHERE  
                    (i.CurrentState = 1)
            
GO

IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_DiagramPane1' , N'SCHEMA',N'dbo', N'VIEW',N'VwItems', NULL,NULL))
	EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = ""(H (1[40] 4[20] 2[20] 3) )""
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = ""(H (1 [50] 4 [25] 3))""
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = ""(H (1 [50] 2 [25] 3))""
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = ""(H (4 [30] 2 [40] 3))""
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = ""(H (1 [56] 3))""
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = ""(H (2 [66] 3))""
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = ""(H (4 [50] 3))""
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = ""(V (3))""
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = ""(H (1[56] 4[18] 2) )""
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = ""(H (1 [75] 4))""
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = ""(H (1[66] 2) )""
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = ""(H (4 [60] 2))""
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = ""(H (1) )""
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = ""(V (4))""
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = ""(V (2))""
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = ""i""
            Begin Extent = 
               Top = 7
               Left = 48
               Bottom = 170
               Right = 270
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = ""c""
            Begin Extent = 
               Top = 175
               Left = 48
               Bottom = 338
               Right = 255
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = ""u""
            Begin Extent = 
               Top = 343
               Left = 48
               Bottom = 506
               Right = 255
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = """"
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'VwItems'
ELSE
BEGIN
	EXEC sys.sp_updateextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = ""(H (1[40] 4[20] 2[20] 3) )""
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = ""(H (1 [50] 4 [25] 3))""
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = ""(H (1 [50] 2 [25] 3))""
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = ""(H (4 [30] 2 [40] 3))""
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = ""(H (1 [56] 3))""
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = ""(H (2 [66] 3))""
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = ""(H (4 [50] 3))""
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = ""(V (3))""
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = ""(H (1[56] 4[18] 2) )""
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = ""(H (1 [75] 4))""
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = ""(H (1[66] 2) )""
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = ""(H (4 [60] 2))""
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = ""(H (1) )""
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = ""(V (4))""
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = ""(V (2))""
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = ""i""
            Begin Extent = 
               Top = 7
               Left = 48
               Bottom = 170
               Right = 270
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = ""c""
            Begin Extent = 
               Top = 175
               Left = 48
               Bottom = 338
               Right = 255
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = ""u""
            Begin Extent = 
               Top = 343
               Left = 48
               Bottom = 506
               Right = 255
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = """"
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'VwItems'
END
GO

IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_DiagramPaneCount' , N'SCHEMA',N'dbo', N'VIEW',N'VwItems', NULL,NULL))
	EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'VwItems'
ELSE
BEGIN
	EXEC sys.sp_updateextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'VwItems'
END
GO");

            // Add VwCategoryItems
            migrationBuilder.Sql(@"
CREATE OR ALTER VIEW [dbo].[VwCategoryItems] AS
WITH CategoryHierarchy AS (
    -- Anchor: start with each category itself
    SELECT 
	c.Id AS RootCategoryId,
	c.Id,
	c.TitleAr,
	c.TitleEn,
	c.ParentId,
	c.IsFinal,
	c.ImageUrl,
	c.Icon,
	c.DisplayOrder,
	c.PriceRequired,
	c.TreeViewSerial,
	c.IsFeaturedCategory,
	c.IsHomeCategory,
	c.IsMainCategory,
	c.CreatedDateUtc
    FROM dbo.TbCategories c
    WHERE c.CurrentState = 1
    
    UNION ALL
    
    -- Recursive: get children
    SELECT 
	ch.RootCategoryId,
	c.Id,
	c.TitleAr,
	c.TitleEn,
	c.ParentId,
	c.IsFinal,
	c.ImageUrl,
	c.Icon,
	c.DisplayOrder,
	c.PriceRequired,
	c.TreeViewSerial,
	c.IsFeaturedCategory,
	c.IsHomeCategory,
	c.IsMainCategory,
	c.CreatedDateUtc
    FROM dbo.TbCategories c
    INNER JOIN CategoryHierarchy ch ON c.ParentId = ch.Id
    WHERE c.CurrentState = 1
)
SELECT 
    rc.Id, 
    rc.TitleAr, 
    rc.TitleEn, 
    rc.ParentId, 
    rc.IsFinal, 
    rc.ImageUrl, 
    rc.Icon, 
    rc.DisplayOrder, 
	rc.PriceRequired,
	rc.TreeViewSerial,
    rc.IsFeaturedCategory, 
    rc.IsHomeCategory, 
    rc.IsMainCategory, 
    rc.CreatedDateUtc,
    (
        SELECT 
            i.Id,
            i.TitleAr,
            i.TitleEn,
            i.ThumbnailImage,
            i.Price,
            i.CreatedDateUtc,
            i.CategoryId,
			ch.PriceRequired
        FROM TbItems i
        INNER JOIN CategoryHierarchy ch ON ch.Id = i.CategoryId
        WHERE ch.RootCategoryId = rc.Id
            AND i.CurrentState = 1
        FOR JSON PATH
    ) AS ItemsJson
FROM (
    SELECT DISTINCT RootCategoryId
    FROM CategoryHierarchy
) roots
INNER JOIN dbo.TbCategories rc ON rc.Id = roots.RootCategoryId
WHERE rc.CurrentState = 1;
GO");

            // Add VwUnitWithConversionsUnits
            migrationBuilder.Sql(@"
CREATE OR ALTER VIEW [dbo].[VwUnitWithConversionsUnits] AS
                SELECT 
                    u.Id,
                    u.TitleAr,
                    u.TitleEn,
                    ISNULL((
                        SELECT 
                            uc.FromUnitId As ConversionUnitId,
                            uf.TitleAr,
                            uf.TitleEn,
                            uc.ConversionFactor
                        FROM dbo.TbUnitConversions uc
                        INNER JOIN dbo.TbUnits uf ON uf.Id = uc.FromUnitId 
                        WHERE uc.ToUnitId = u.Id
                        AND uc.CurrentState = 1
                        AND uf.CurrentState = 1
                        FOR JSON PATH
                    ),'[]') AS ConversionUnitsFromJson,
                    ISNULL((
                        SELECT 
                            uct.ToUnitId As ConversionUnitId,
                            ut.TitleAr,
                            ut.TitleEn,
                            uct.ConversionFactor
                        FROM dbo.TbUnitConversions uct
                        INNER JOIN dbo.TbUnits ut ON ut.Id = uct.ToUnitId 
                        WHERE uct.FromUnitId = u.Id
                        AND uct.CurrentState = 1
                        AND ut.CurrentState = 1
                        FOR JSON PATH
                    ),'[]') AS ConversionUnitsToJson
                FROM dbo.TbUnits u
                WHERE u.CurrentState = 1;
            
GO");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP VIEW IF EXISTS [dbo].[VwItems];");
            migrationBuilder.Sql(@"DROP VIEW IF EXISTS [dbo].[VwCategoryItems];");
            migrationBuilder.Sql(@"DROP VIEW IF EXISTS [dbo].[VwUnitWithConversionsUnits];");
        }
    }
}
