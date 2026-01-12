using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class InsertMarketStoreDataIntoTbVendor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
        IF NOT EXISTS 
        (
            SELECT 1 
            FROM TbVendors 
            WHERE Id = 'f47ac10b-58cc-4372-a567-0e02b2c3d479'
        )
        BEGIN
            INSERT INTO [dbo].[TbVendors]
            (
                [Id],
                [UserId],
                [PostalCode],
                [Address],
                [IsRealEstateRegistered],
                [Notes],
                [AverageRating],
                [CreatedBy],
                [CreatedDateUtc],
                [UpdatedBy],
                [UpdatedDateUtc],
                [IsDeleted],
                [BirthDate],
                [CityId],
                [FirstName],
                [IdentificationImageBackPath],
                [IdentificationImageFrontPath],
                [IdentificationNumber],
                [IdentificationType],
                [LastName],
                [MiddleName],
                [PhoneCode],
                [PhoneNumber],
                [Status],
                [StoreName],
                [VendorType]
            )
            VALUES
            (
                'f47ac10b-58cc-4372-a567-0e02b2c3d479', -- Id
                '9223ad7c-f56b-4a05-9174-8ea3840bb7bc', -- UserId
                '5345455',                             -- PostalCode
                'Market Address',                      -- Address
                0,                                     -- IsRealEstateRegistered
                NULL,                                  -- Notes
                0.00,                                  -- AverageRating
                '00000000-0000-0000-0000-000000000000',-- CreatedBy
                SYSUTCDATETIME(),                      -- CreatedDateUtc
                NULL,                                  -- UpdatedBy
                NULL,                                  -- UpdatedDateUtc
                0,                                     -- IsDeleted
                SYSUTCDATETIME(),                      -- BirthDate
                '33333333-3333-3333-3333-333333333333',-- CityId
                'Market',                              -- FirstName
                'image.webp',                          -- IdentificationImageBackPath
                'image.webp',                          -- IdentificationImageFrontPath
                '1',                                   -- IdentificationNumber
                1,                                     -- IdentificationType (NationalId)
                'Vendor',                              -- LastName
                'Store',                               -- MiddleName
                '+20',                                 -- PhoneCode
                '111111111',                           -- PhoneNumber
                3,                                     -- Status (Approved)
                'Market Store',                        -- StoreName
                2                                      -- VendorType (Company)
            );
        END
    ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
        DELETE FROM TbVendors
        WHERE Id = 'f47ac10b-58cc-4372-a567-0e02b2c3d479';
    ");
        }

    }
}
