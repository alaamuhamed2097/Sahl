using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class CreateVwVendorItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE OR ALTER VIEW [dbo].[VwRefundDetails]
AS
SELECT 
    r.Id, 
	r.VendorId,
	v.StoreName AS VendorStoreName,
	vu.FirstName+' '+vu.LastName AS VendorName,
	r.CustomerId,
	cu.FirstName+' '+cu.LastName AS CustomerName,
	cu.Email AS CustomerEmail,
	cu.PhoneCode+''+cu.PhoneNumber AS CustomerFullPhoneNumber,
	r.Number,
	r.OrderDetailId,
    r.DeliveryAddressId, 
	caddr.Address AS DeliveryAddress,
	caddr.RecipientName AS DeliveryRecipientName,
	caddr.PhoneCode+''+caddr.PhoneNumber AS DeliveryFullPhoneNumber,
    r.RefundReason,
    r.RefundReasonDetails, 
    r.RejectionReason,
	r.RefundStatus,
	r.ReturnShippingCost,
	r.RefundAmount,
	r.RequestedItemsCount,
	r.ApprovedItemsCount,
	r.RefundTransactionId,
	r.ReturnTrackingNumber,
	r.RequestDateUTC,
	r.ApprovedDateUTC,
	r.ReturnedDateUTC,
	r.RefundedDateUTC,
	r.AdminNotes
FROM     
    dbo.TbRefunds AS r 
    INNER JOIN dbo.TbVendors AS v ON r.VendorId = v.Id AND v.IsDeleted = 0 
	INNER JOIN dbo.AspNetUsers AS vu ON vu.Id = v.UserId AND vu.UserState = 1
	INNER JOIN dbo.TbCustomer AS c ON c.Id = r.CustomerId AND c.IsDeleted = 0
	INNER JOIN dbo.AspNetUsers AS cu ON cu.Id = c.UserId AND cu.UserState= 1
    LEFT JOIN dbo.TbCustomerAddresses AS caddr ON caddr.Id = r.DeliveryAddressId AND caddr.IsDeleted = 0
WHERE  
       r.IsDeleted = 0 ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.Sql(@"DROP VIEW IF EXISTS [dbo].[VwRefundDetails]");
        }
    }
}
