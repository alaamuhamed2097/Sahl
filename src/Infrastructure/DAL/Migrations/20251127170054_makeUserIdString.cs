using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class makeUserIdString : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_NotificationPreferences_AspNetUsers_UserId",
            //    table: "TbNotificationPreferences");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NotificationPreferences",
                table: "TbNotificationPreferences");

            migrationBuilder.RenameTable(
                name: "TbNotificationPreferences",
                newName: "TbNotificationPreferences");

            migrationBuilder.RenameIndex(
                name: "IX_NotificationPreferences_UserId",
                table: "TbNotificationPreferences",
                newName: "IX_TbNotificationPreferences_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_NotificationPreferences_CurrentState",
                table: "TbNotificationPreferences",
                newName: "IX_TbNotificationPreferences_CurrentState");

            migrationBuilder.AlterColumn<string>(
                name: "DescriptionEn",
                table: "TbNotifications",
                type: "nvarchar(max)",
                maxLength: 50000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AlterColumn<string>(
                name: "DescriptionAr",
                table: "TbNotifications",
                type: "nvarchar(max)",
                maxLength: 50000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AddColumn<string>(
                name: "Data",
                table: "TbNotifications",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeliveryChannel",
                table: "TbNotifications",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeliveryStatus",
                table: "TbNotifications",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsRead",
                table: "TbNotifications",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Message",
                table: "TbNotifications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "NotificationType",
                table: "TbNotifications",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReadDate",
                table: "TbNotifications",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RecipientID",
                table: "TbNotifications",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RecipientType",
                table: "TbNotifications",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "SentDate",
                table: "TbNotifications",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Severity",
                table: "TbNotifications",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "TbNotifications",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TbNotificationPreferences",
                table: "TbNotificationPreferences",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TbNotificationPreferences_AspNetUsers_UserId",
                table: "TbNotificationPreferences",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TbNotificationPreferences_AspNetUsers_UserId",
                table: "TbNotificationPreferences");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TbNotificationPreferences",
                table: "TbNotificationPreferences");

            migrationBuilder.DropColumn(
                name: "Data",
                table: "TbNotifications");

            migrationBuilder.DropColumn(
                name: "DeliveryChannel",
                table: "TbNotifications");

            migrationBuilder.DropColumn(
                name: "DeliveryStatus",
                table: "TbNotifications");

            migrationBuilder.DropColumn(
                name: "IsRead",
                table: "TbNotifications");

            migrationBuilder.DropColumn(
                name: "Message",
                table: "TbNotifications");

            migrationBuilder.DropColumn(
                name: "NotificationType",
                table: "TbNotifications");

            migrationBuilder.DropColumn(
                name: "ReadDate",
                table: "TbNotifications");

            migrationBuilder.DropColumn(
                name: "RecipientID",
                table: "TbNotifications");

            migrationBuilder.DropColumn(
                name: "RecipientType",
                table: "TbNotifications");

            migrationBuilder.DropColumn(
                name: "SentDate",
                table: "TbNotifications");

            migrationBuilder.DropColumn(
                name: "Severity",
                table: "TbNotifications");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "TbNotifications");

            migrationBuilder.RenameTable(
                name: "TbNotificationPreferences",
                newName: "TbNotificationPreferences");

            migrationBuilder.RenameIndex(
                name: "IX_TbNotificationPreferences_UserId",
                table: "TbNotificationPreferences",
                newName: "IX_NotificationPreferences_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_TbNotificationPreferences_CurrentState",
                table: "TbNotificationPreferences",
                newName: "IX_NotificationPreferences_CurrentState");

            migrationBuilder.AlterColumn<string>(
                name: "DescriptionEn",
                table: "TbNotifications",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldMaxLength: 50000);

            migrationBuilder.AlterColumn<string>(
                name: "DescriptionAr",
                table: "TbNotifications",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldMaxLength: 50000);

            migrationBuilder.AddPrimaryKey(
                name: "PK_NotificationPreferences",
                table: "TbNotificationPreferences",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CurrentState = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    Data = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeliveryChannel = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DeliveryStatus = table.Column<int>(type: "int", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NotificationType = table.Column<int>(type: "int", nullable: false),
                    ReadDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RecipientID = table.Column<int>(type: "int", nullable: false),
                    RecipientType = table.Column<int>(type: "int", nullable: false),
                    SentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Severity = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDateUtc = table.Column<DateTime>(type: "datetime2(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_CurrentState",
                table: "Notifications",
                column: "CurrentState");

            migrationBuilder.AddForeignKey(
                name: "FK_NotificationPreferences_AspNetUsers_UserId",
                table: "TbNotificationPreferences",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
