using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class AlterTbItemReview : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HelpfulCount",
                table: "TbItemReviews");

            migrationBuilder.RenameColumn(
                name: "NotHelpfulCount",
                table: "TbItemReviews",
                newName: "HelpfulVoteCount");

            migrationBuilder.AlterColumn<string>(
                name: "ReviewTitle",
                table: "TbItemReviews",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "ReviewText",
                table: "TbItemReviews",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ReviewNumber",
                table: "TbItemReviews",
                type: "nvarchar(12)",
                maxLength: 12,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HelpfulVoteCount",
                table: "TbItemReviews",
                newName: "NotHelpfulCount");

            migrationBuilder.AlterColumn<string>(
                name: "ReviewTitle",
                table: "TbItemReviews",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "ReviewText",
                table: "TbItemReviews",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(300)",
                oldMaxLength: 300);

            migrationBuilder.AlterColumn<string>(
                name: "ReviewNumber",
                table: "TbItemReviews",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(12)",
                oldMaxLength: 12);

            migrationBuilder.AddColumn<int>(
                name: "HelpfulCount",
                table: "TbItemReviews",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
