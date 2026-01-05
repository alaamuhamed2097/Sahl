using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class insertDefaultSystemSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DECLARE @SystemUserId UNIQUEIDENTIFIER = '00000000-0000-0000-0000-000000000000';

-- Tax Settings (Category = 1)
INSERT INTO [dbo].[TbSystemSettings] 
([Id], [SettingKey], [SettingValue], [DataType], [Category], [CreatedBy], [CreatedDateUTC], [IsDeleted])
VALUES 
(NEWID(), 1, '14', 3, 1, @SystemUserId, GETUTCDATE(), 0), -- OrderTaxPercentage (Decimal)
(NEWID(), 2, 'false', 4, 1, @SystemUserId, GETUTCDATE(), 0); -- TaxIncludedInPrice (Boolean)

-- Shipping Settings (Category = 2)
INSERT INTO [dbo].[TbSystemSettings] 
([Id], [SettingKey], [SettingValue], [DataType], [Category], [CreatedBy], [CreatedDateUTC], [IsDeleted])
VALUES 
(NEWID(), 10, '50', 3, 2, @SystemUserId, GETUTCDATE(), 0), -- ShippingAmount (Decimal)
(NEWID(), 11, '500', 3, 2, @SystemUserId, GETUTCDATE(), 0), -- FreeShippingThreshold (Decimal)
(NEWID(), 12, '10', 3, 2, @SystemUserId, GETUTCDATE(), 0), -- ShippingPerKg (Decimal)
(NEWID(), 13, '3', 2, 2, @SystemUserId, GETUTCDATE(), 0); -- EstimatedDeliveryDays (Integer)

-- Order Settings (Category = 4)
INSERT INTO [dbo].[TbSystemSettings] 
([Id], [SettingKey], [SettingValue], [DataType], [Category], [CreatedBy], [CreatedDateUTC], [IsDeleted])
VALUES 
(NEWID(), 20, '0', 3, 4, @SystemUserId, GETUTCDATE(), 0), -- OrderExtraCost (Decimal)
(NEWID(), 21, '50', 3, 4, @SystemUserId, GETUTCDATE(), 0), -- MinimumOrderAmount (Decimal)
(NEWID(), 22, '50000', 3, 4, @SystemUserId, GETUTCDATE(), 0), -- MaximumOrderAmount (Decimal)
(NEWID(), 23, '24', 2, 4, @SystemUserId, GETUTCDATE(), 0); -- OrderCancellationPeriodHours (Integer)

-- Payment Settings (Category = 3)
INSERT INTO [dbo].[TbSystemSettings] 
([Id], [SettingKey], [SettingValue], [DataType], [Category], [CreatedBy], [CreatedDateUTC], [IsDeleted])
VALUES 
(NEWID(), 30, 'true', 4, 3, @SystemUserId, GETUTCDATE(), 0), -- PaymentGatewayEnabled (Boolean)
(NEWID(), 31, 'true', 4, 3, @SystemUserId, GETUTCDATE(), 0), -- CashOnDeliveryEnabled (Boolean)
(NEWID(), 32, '', 1, 3, @SystemUserId, GETUTCDATE(), 0), -- StripePublicKey (String)
(NEWID(), 33, '', 1, 3, @SystemUserId, GETUTCDATE(), 0); -- StripeSecretKey (String)

-- Business Settings (Category = 5)
INSERT INTO [dbo].[TbSystemSettings] 
([Id], [SettingKey], [SettingValue], [DataType], [Category], [CreatedBy], [CreatedDateUTC], [IsDeleted])
VALUES 
(NEWID(), 40, '09:00', 1, 5, @SystemUserId, GETUTCDATE(), 0), -- BusinessHoursStart (String)
(NEWID(), 41, '18:00', 1, 5, @SystemUserId, GETUTCDATE(), 0), -- BusinessHoursEnd (String)
(NEWID(), 42, 'false', 4, 5, @SystemUserId, GETUTCDATE(), 0), -- MaintenanceMode (Boolean)
(NEWID(), 43, 'true', 4, 5, @SystemUserId, GETUTCDATE(), 0); -- AllowGuestCheckout (Boolean)

-- Notification Settings (Category = 6)
INSERT INTO [dbo].[TbSystemSettings] 
([Id], [SettingKey], [SettingValue], [DataType], [Category], [CreatedBy], [CreatedDateUTC], [IsDeleted])
VALUES 
(NEWID(), 50, 'true', 4, 6, @SystemUserId, GETUTCDATE(), 0), -- EmailNotificationsEnabled (Boolean)
(NEWID(), 51, 'true', 4, 6, @SystemUserId, GETUTCDATE(), 0), -- SmsNotificationsEnabled (Boolean)
(NEWID(), 52, 'noreply@yourstore.com', 1, 6, @SystemUserId, GETUTCDATE(), 0); -- OrderConfirmationEmail (String)

-- Security Settings (Category = 7)
INSERT INTO [dbo].[TbSystemSettings] 
([Id], [SettingKey], [SettingValue], [DataType], [Category], [CreatedBy], [CreatedDateUTC], [IsDeleted])
VALUES 
(NEWID(), 60, '5', 2, 7, @SystemUserId, GETUTCDATE(), 0), -- MaxLoginAttempts (Integer)
(NEWID(), 61, '30', 2, 7, @SystemUserId, GETUTCDATE(), 0), -- SessionTimeoutMinutes (Integer)
(NEWID(), 62, '8', 2, 7, @SystemUserId, GETUTCDATE(), 0); -- PasswordMinLength (Integer)

GO");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
