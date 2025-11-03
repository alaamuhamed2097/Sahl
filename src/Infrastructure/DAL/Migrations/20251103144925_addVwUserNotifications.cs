using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class addVwUserNotifications : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE VIEW [dbo].[VwUserNotifications] AS
                SELECT 
                    n.Id,
					un.UserId,
                    n.TitleAr,
                    n.TitleEn,
                    n.DescriptionAr	,
                    n.DescriptionEn , 
					un.IsRead,
					n.CreatedDateUtc,
					CASE
						WHEN DATEDIFF(SECOND, n.CreatedDateUtc, GETUTCDATE()) < 60 THEN 'just now'
						WHEN DATEDIFF(MINUTE, n.CreatedDateUtc, GETUTCDATE()) < 2 THEN 'a minute ago'
						WHEN DATEDIFF(MINUTE, n.CreatedDateUtc, GETUTCDATE()) < 60 THEN 
							CAST(DATEDIFF(MINUTE, n.CreatedDateUtc, GETUTCDATE()) AS VARCHAR) + ' minutes ago'
						WHEN DATEDIFF(HOUR, n.CreatedDateUtc, GETUTCDATE()) < 2 THEN 'an hour ago'
						WHEN DATEDIFF(HOUR, n.CreatedDateUtc, GETUTCDATE()) < 24 THEN 
							CAST(DATEDIFF(HOUR, n.CreatedDateUtc, GETUTCDATE()) AS VARCHAR) + ' hours ago'
						WHEN DATEDIFF(DAY, n.CreatedDateUtc, GETUTCDATE()) < 1 AND DATEDIFF(HOUR, n.CreatedDateUtc, GETUTCDATE()) >= 23 THEN 'yesterday'
						WHEN DATEDIFF(DAY, n.CreatedDateUtc, GETUTCDATE()) < 30 THEN 
							CAST(DATEDIFF(DAY, n.CreatedDateUtc, GETUTCDATE()) AS VARCHAR) + ' days ago'
						WHEN DATEDIFF(MONTH, n.CreatedDateUtc, GETUTCDATE()) < 12 THEN 
							CAST(DATEDIFF(MONTH, n.CreatedDateUtc, GETUTCDATE()) AS VARCHAR) + ' months ago'
						ELSE CAST(DATEDIFF(YEAR, n.CreatedDateUtc, GETUTCDATE()) AS VARCHAR) + ' years ago'
					END AS TimeAgo
                FROM dbo.TbNotifications n
				LEFT JOIN TbUserNotifications un ON n.Id = un.NotificationId
                WHERE n.CurrentState = 1 AND
				      un.CurrentState = 1;
            ");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP VIEW IF EXISTS [dbo].[VwUserNotifications]");
        }
    }
}
