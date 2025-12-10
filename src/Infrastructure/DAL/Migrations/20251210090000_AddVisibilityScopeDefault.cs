using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddVisibilityScopeDefault : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add default constraint for VisibilityScope on TbItem using SQL
            // Default to 1 which is ProductVisibilityStatus.Visible
            migrationBuilder.Sql(@"
                ALTER TABLE [TbItems]
                ADD CONSTRAINT [DF_TbItems_VisibilityScope]
                DEFAULT (1) FOR [VisibilityScope];
            ");
        }

        /// <inheritdoc />
        protected override void Down(Mi




rationBuilder migrationBuilder)
        {
            // Remove default constraint
            migrationBuilder.Sql(@"
                ALTER TABLE [TbItems]
                DROP CONSTRAINT [DF_TbItems_VisibilityScope];
            ");
        }
    }
}
