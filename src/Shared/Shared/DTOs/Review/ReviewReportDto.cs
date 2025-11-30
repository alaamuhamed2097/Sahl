using Shared.DTOs.Base;
using Common.Enumerations.Review;

namespace Shared.DTOs.Review
{
    public class ReviewReportDto : BaseDto
    {
        public Guid ReportID { get; set; }
        public Guid ReviewID { get; set; }
        public Guid CustomerID { get; set; }
        public string Reason { get; set; } = null!;
        public string? Details { get; set; }
        public ReviewReportStatus Status { get; set; }
    }
}
