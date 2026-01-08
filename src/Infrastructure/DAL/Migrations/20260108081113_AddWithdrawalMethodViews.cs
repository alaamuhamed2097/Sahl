using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddWithdrawalMethodViews : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add VwWithdrawalMethodsWithFields
            migrationBuilder.Sql(@"
CREATE OR ALTER VIEW [dbo].[VwWithdrawalMethodsWithFields] AS       
  SELECT 
      wm.Id,
      wm.TitleAr,
      wm.TitleEn,
      wm.ImagePath,
      ISNULL((
          SELECT
              f.Id,
              f.WithdrawalMethodId,
              f.TitleAr,
              f.TitleEn,
              f.FieldType
          FROM dbo.TbFields f
          WHERE f.WithdrawalMethodId = wm.Id
          AND f.IsDeleted = 0
          FOR JSON PATH
      ),'[]') AS WithdrawalMethodFieldsJson
  FROM dbo.TbWithdrawalMethods wm
  WHERE wm.IsDeleted = 0;
            
GO");

            // Add VwWithdrawalMethodFieldsValues
            migrationBuilder.Sql(@"
CREATE OR Alter VIEW [dbo].[VwWithdrawalMethodsFieldsValues] AS
SELECT 
    wm.Id AS WithdrawalMethodId,
    wm.TitleAr,
    wm.TitleEn,
    wm.ImagePath,
    ISNULL((
        SELECT 
            u.Id AS UserId,
            wmf.Id,
            wmf.UserWithdrawalMethodId,
            f.Id AS FieldId,
            f.TitleAr AS FieldTitleAr,
            f.TitleEn AS FieldTitleEn,
            f.FieldType,
            wmf.Value
        FROM AspNetUsers u
        INNER JOIN TbFields f ON f.WithdrawalMethodId = wm.Id AND f.IsDeleted = 0
        LEFT JOIN TbUserWithdrawalMethods uwm 
            ON uwm.UserId = u.Id 
            AND uwm.WithdrawalMethodId = wm.Id
            AND ISNULL(uwm.IsDeleted,0) = 0
        LEFT JOIN TbWithdrawalMethodFields wmf 
            ON wmf.UserWithdrawalMethodId = uwm.Id
            AND wmf.FieldId = f.Id
            AND ISNULL(wmf.IsDeleted,0) = 0
        FOR JSON PATH
    ), '[]') AS FieldsJson
FROM TbWithdrawalMethods wm
WHERE wm.IsDeleted = 0;
GO");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop VwWithdrawalMethodsWithFields
            migrationBuilder.Sql(@"DROP VIEW IF EXISTS [dbo].[VwWithdrawalMethodsWithFields];");

            // Drop VwWithdrawalMethodFieldsValues
            migrationBuilder.Sql(@"DROP VIEW IF EXISTS [dbo].[VwWithdrawalMethodsFieldsValues];");
        }
    }
}
