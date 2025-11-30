namespace Domains.Entities.ECommerceSystem.Review
{
    public class TbSalesReview : BaseEntity
    {
        public int ReviewNumber { get; set; }
        public Guid OrderItemID { get; set; }
        public Guid CustomerID { get; set; }
        public Guid VendorID { get; set; }
        public decimal? ProductAccuracyRating { get; set; }
        public decimal? ShippingSpeedRating { get; set; }
        public decimal? CommunicationRating { get; set; }
        public decimal? ServiceRating { get; set; }
        public decimal? OverallRating { get; set; }
        public DateTime? ReviewDate { get; set; }
        public string Status { get; set; } = null!;
    }
}
