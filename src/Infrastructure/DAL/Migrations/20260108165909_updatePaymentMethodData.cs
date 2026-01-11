using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class updatePaymentMethodData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //    migrationBuilder.Sql("DELETE FROM TbPaymentMethods");

            //    migrationBuilder.Sql(@"
            //        INSERT INTO TbPaymentMethods (Id, TitleEn, TitleAr, MethodType, IsActive, IsDeleted, CreatedBy, CreatedDateUtc)
            //        VALUES 
            //            (NEWID(), 'Cash on Delivery', 'الدفع عند الاستلام', 1, 1, 0, '00000000-0000-0000-0000-000000000000', GETUTCDATE()),
            //            (NEWID(), 'Wallet', 'المحفظة', 2, 1, 0, '00000000-0000-0000-0000-000000000000', GETUTCDATE()),
            //            (NEWID(), 'Credit/Debit Card', 'بطاقة ائتمان/خصم', 3, 1, 0, '00000000-0000-0000-0000-000000000000', GETUTCDATE()),
            //            (NEWID(), 'Wallet and Card', 'المحفظة والبطاقة', 4, 1, 0, '00000000-0000-0000-0000-000000000000', GETUTCDATE())
            //    ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
