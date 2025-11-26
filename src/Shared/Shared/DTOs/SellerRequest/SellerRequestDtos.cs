using System;
using System.Collections.Generic;

namespace Shared.DTOs.SellerRequest
{
    // Seller Request DTOs
    public class SellerRequestCreateDto
    {
        public Guid VendorId { get; set; }
        public int RequestType { get; set; }
        public string TitleEn { get; set; }
        public string TitleAr { get; set; }
        public string DescriptionEn { get; set; }
        public string DescriptionAr { get; set; }
        public int Priority { get; set; } = 2; // Medium
    }

    public class SellerRequestUpdateDto
    {
        public Guid Id { get; set; }
        public string TitleEn { get; set; }
        public string TitleAr { get; set; }
        public string DescriptionEn { get; set; }
        public string DescriptionAr { get; set; }
        public int Priority { get; set; }
    }

    public class SellerRequestDto
    {
        public Guid Id { get; set; }
        public Guid VendorId { get; set; }
        public string VendorName { get; set; }
        public int RequestType { get; set; }
        public string RequestTypeName { get; set; }
        public int RequestStatus { get; set; }
        public string RequestStatusName { get; set; }
        public string TitleEn { get; set; }
        public string TitleAr { get; set; }
        public string DescriptionEn { get; set; }
        public string DescriptionAr { get; set; }
        public int Priority { get; set; }
        public string PriorityName { get; set; }
        public DateTime? ReviewedAt { get; set; }
        public string ReviewedByUserName { get; set; }
        public string AdminResponse { get; set; }
        public int CommentsCount { get; set; }
        public int DocumentsCount { get; set; }
        public DateTime CreatedDateUtc { get; set; }
        public DateTime? UpdatedDateUtc { get; set; }
    }

    // Request Comment DTOs
    public class RequestCommentCreateDto
    {
        public Guid SellerRequestId { get; set; }
        public Guid UserId { get; set; }
        public string CommentText { get; set; }
        public bool IsInternal { get; set; }
    }

    public class RequestCommentDto
    {
        public Guid Id { get; set; }
        public Guid SellerRequestId { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string UserRole { get; set; }
        public string CommentText { get; set; }
        public bool IsInternal { get; set; }
        public DateTime CreatedDateUtc { get; set; }
    }

    // Request Document DTOs
    public class RequestDocumentCreateDto
    {
        public Guid SellerRequestId { get; set; }
        public string DocumentPath { get; set; }
        public string DocumentName { get; set; }
        public string DocumentType { get; set; }
        public long FileSize { get; set; }
    }

    public class RequestDocumentDto
    {
        public Guid Id { get; set; }
        public Guid SellerRequestId { get; set; }
        public string DocumentPath { get; set; }
        public string DocumentName { get; set; }
        public string DocumentType { get; set; }
        public long FileSize { get; set; }
        public string FileSizeFormatted { get; set; }
        public Guid UploadedByUserId { get; set; }
        public string UploadedByUserName { get; set; }
        public DateTime UploadedAt { get; set; }
    }

    // Status Update DTOs
    public class UpdateRequestStatusDto
    {
        public Guid RequestId { get; set; }
        public int NewStatus { get; set; }
        public string AdminResponse { get; set; }
    }

    // Search DTOs
    public class SellerRequestSearchRequest
    {
        public Guid? VendorId { get; set; }
        public int? RequestType { get; set; }
        public int? RequestStatus { get; set; }
        public int? Priority { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string SearchTerm { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }

    public class SellerRequestStatisticsDto
    {
        public int TotalRequests { get; set; }
        public int PendingRequests { get; set; }
        public int InProgressRequests { get; set; }
        public int ResolvedRequests { get; set; }
        public int RejectedRequests { get; set; }
        public double AverageResolutionTimeHours { get; set; }
        public Dictionary<string, int> RequestsByType { get; set; }
        public Dictionary<string, int> RequestsByPriority { get; set; }
    }
}
