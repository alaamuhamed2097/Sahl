namespace Domains.Entities.ECommerceSystem.Review
{
    public class TbDeliveryReview : BaseEntity
    {
        public int ReviewNumber { get; set; }
        public Guid OrderID { get; set; }
        public Guid CustomerID { get; set; }
        public decimal? PackagingRating { get; set; }
        public decimal? CourierDeliveryRating { get; set; }
        public decimal? OverallRating { get; set; }
        public decimal? ShippingAmount { get; set; }
        public DateTime? ReviewDate { get; set; }
        public string Status { get; set; } = null!;
    }
}
