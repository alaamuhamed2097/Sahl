using AutoMapper;
using BL.Contracts.Service.Merchandising.PromoCode;
using Common.Enumerations.SellerRequest;
using Common.Filters;
using DAL.Contracts.Repositories;
using DAL.Contracts.Repositories.Merchandising;
using DAL.Models;
using Domains.Entities.Merchandising.CouponCode;
using Domains.Entities.SellerRequest;
using Domains.Entities.ECommerceSystem.Vendor;
using Shared.DTOs.Merchandising.PromoCode;

namespace BL.Services.Merchandising.PromoCode
{
    /// <summary>
    /// Service implementation for vendor promo code participation requests
    /// </summary>
    public class VendorPromoCodeParticipationService : IVendorPromoCodeParticipationService
    {
        private readonly ITableRepository<TbSellerRequest> _sellerRequestRepository;
        private readonly ITableRepository<TbVendor> _vendorRepository;
        private readonly ICouponCodeRepository _couponCodeRepository;
        private readonly IMapper _mapper;

        public VendorPromoCodeParticipationService(
            ITableRepository<TbSellerRequest> sellerRequestRepository,
            ITableRepository<TbVendor> vendorRepository,
            ICouponCodeRepository couponCodeRepository,
            IMapper mapper)
        {
            _sellerRequestRepository = sellerRequestRepository;
            _vendorRepository = vendorRepository;
            _couponCodeRepository = couponCodeRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Submits a participation request for a vendor to join a public promo code
        /// </summary>
        public async Task<(bool Success, string Message, VendorPromoCodeParticipationRequestDto? Data)> SubmitParticipationRequestAsync(
            Guid vendorId,
            CreateVendorPromoCodeParticipationRequestDto request,
            Guid userId)
        {
            try
            {
                if (vendorId == Guid.Empty || request.PromoCodeId == Guid.Empty)
                    return (false, "Invalid vendor or promo code ID", null);

                // Get vendor
                var vendor = await _vendorRepository.FindByIdAsync(vendorId);
                if (vendor == null)
                    return (false, "Vendor not found", null);

                // Verify promo code exists
                var promoCode = await _couponCodeRepository.GetByIdAsync(request.PromoCodeId);
                if (promoCode == null)
                    return (false, "Promo code not found", null);

                // Check if vendor already has a pending or approved request for this promo code
                var existingRequestsList = await _sellerRequestRepository.FindAsync(r =>
                    r.VendorId == vendorId &&
                    r.RequestType == SellerRequestType.PromoCodeParticipation &&
                    (r.Status == SellerRequestStatus.Pending || r.Status == SellerRequestStatus.Approved) &&
                    r.RequestData == request.PromoCodeId.ToString());

                if (existingRequestsList is IEnumerable<TbSellerRequest> requests && requests.Any())
                    return (false, "Vendor already has a pending or approved request for this promo code", null);

                // Create the seller request
                var sellerRequest = new TbSellerRequest
                {
                    VendorId = vendorId,
                    RequestType = SellerRequestType.PromoCodeParticipation,
                    Status = SellerRequestStatus.Pending,
                    TitleEn = $"Promo Code Participation Request - {promoCode.Code}",
                    TitleAr = $"??? ???????? ?? ??? ??????? - {promoCode.Code}",
                    DescriptionEn = request.DescriptionEn ?? $"Request to participate in promo code: {promoCode.Code}",
                    DescriptionAr = request.DescriptionAr ?? $"??? ???????? ?? ??? ???????: {promoCode.Code}",
                    RequestData = request.PromoCodeId.ToString(),
                    SubmittedAt = DateTime.UtcNow,
                    Priority = 1 // Normal priority
                };

                // Save the request
                var saveResult = await _sellerRequestRepository.SaveAsync(sellerRequest, userId);
                if (!saveResult.Success)
                    return (false, "Failed to save participation request", null);

                // Map to DTO
                var result = new VendorPromoCodeParticipationRequestDto
                {
                    Id = sellerRequest.Id,
                    SellerRequestId = sellerRequest.Id,
                    VendorId = vendorId,
                    VendorName = vendor.StoreName,
                    PromoCodeId = request.PromoCodeId,
                    PromoCodeValue = promoCode.Code,
                    PromoCodeTitle = promoCode.TitleEn,
                    Status = (int)sellerRequest.Status,
                    StatusName = sellerRequest.Status.ToString(),
                    DescriptionEn = request.DescriptionEn,
                    DescriptionAr = request.DescriptionAr,
                    Notes = request.Notes,
                    SubmittedAt = sellerRequest.SubmittedAt ?? DateTime.UtcNow,
                    CreatedDateUtc = sellerRequest.CreatedDateUtc
                };

                return (true, "Participation request submitted successfully", result);
            }
            catch (Exception ex)
            {
                return (false, $"Error: {ex.Message}", null);
            }
        }

        /// <summary>
        /// Gets all promo code participation requests for a specific vendor
        /// </summary>
        public async Task<(bool Success, AdvancedPagedResult<VendorPromoCodeParticipationRequestListDto>? Data)> GetVendorParticipationRequestsAsync(
            Guid vendorId,
            BaseSearchCriteriaModel criteria)
        {
            try
            {
                // Get all promo code participation requests for this vendor
                var requestsResult = await _sellerRequestRepository.FindAsync(r =>
                    r.VendorId == vendorId &&
                    r.RequestType == SellerRequestType.PromoCodeParticipation);

                var requests = (requestsResult as IEnumerable<TbSellerRequest>)?.ToList() ?? new List<TbSellerRequest>();

                // Filter by search term if provided
                if (!string.IsNullOrWhiteSpace(criteria.SearchTerm))
                {
                    requests = requests.Where(r =>
                        r.TitleEn.Contains(criteria.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                        r.TitleAr.Contains(criteria.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                        r.DescriptionEn.Contains(criteria.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                        r.DescriptionAr.Contains(criteria.SearchTerm, StringComparison.OrdinalIgnoreCase)).ToList();
                }

                int totalCount = requests.Count;

                // Apply pagination
                var pagedRequests = requests
                    .OrderByDescending(r => r.CreatedDateUtc)
                    .Skip((criteria.PageNumber - 1) * criteria.PageSize)
                    .Take(criteria.PageSize)
                    .ToList();

                // Map to DTOs
                var resultDtos = new List<VendorPromoCodeParticipationRequestListDto>();
                foreach (var req in pagedRequests)
                {
                    if (!string.IsNullOrEmpty(req.RequestData) && Guid.TryParse(req.RequestData, out var promoCodeId))
                    {
                        var promoCode = await _couponCodeRepository.GetByIdAsync(promoCodeId);
                        resultDtos.Add(new VendorPromoCodeParticipationRequestListDto
                        {
                            Id = req.Id,
                            PromoCodeValue = promoCode?.Code ?? string.Empty,
                            PromoCodeTitle = promoCode?.TitleEn ?? string.Empty,
                            Status = (int)req.Status,
                            StatusName = req.Status.ToString(),
                            SubmittedAt = req.SubmittedAt ?? req.CreatedDateUtc,
                            ReviewedAt = req.ReviewedAt
                        });
                    }
                }

                var pagedResult = new AdvancedPagedResult<VendorPromoCodeParticipationRequestListDto>
                {
                    Items = resultDtos,
                    TotalRecords = totalCount,
                    PageNumber = criteria.PageNumber,
                    PageSize = criteria.PageSize,
                    TotalPages = (int)Math.Ceiling(totalCount / (double)criteria.PageSize)
                };

                return (true, pagedResult);
            }
            catch (Exception ex)
            {
                return (false, null);
            }
        }

        /// <summary>
        /// Gets details of a specific participation request
        /// </summary>
        public async Task<(bool Success, string Message, VendorPromoCodeParticipationRequestDto? Data)> GetParticipationRequestAsync(
            Guid requestId,
            Guid vendorId)
        {
            try
            {
                if (requestId == Guid.Empty || vendorId == Guid.Empty)
                    return (false, "Invalid request or vendor ID", null);

                // Get the request
                var sellerRequest = await _sellerRequestRepository.FindByIdAsync(requestId);
                if (sellerRequest == null || sellerRequest.VendorId != vendorId || 
                    sellerRequest.RequestType != SellerRequestType.PromoCodeParticipation)
                    return (false, "Request not found", null);

                // Get vendor
                var vendor = await _vendorRepository.FindByIdAsync(vendorId);
                if (vendor == null)
                    return (false, "Vendor not found", null);

                // Get promo code details
                if (!string.IsNullOrEmpty(sellerRequest.RequestData) && 
                    Guid.TryParse(sellerRequest.RequestData, out var promoCodeId))
                {
                    var promoCode = await _couponCodeRepository.GetByIdAsync(promoCodeId);

                    var result = new VendorPromoCodeParticipationRequestDto
                    {
                        Id = sellerRequest.Id,
                        SellerRequestId = sellerRequest.Id,
                        VendorId = vendorId,
                        VendorName = vendor.StoreName,
                        PromoCodeId = promoCodeId,
                        PromoCodeValue = promoCode?.Code ?? string.Empty,
                        PromoCodeTitle = promoCode?.TitleEn ?? string.Empty,
                        Status = (int)sellerRequest.Status,
                        StatusName = sellerRequest.Status.ToString(),
                        DescriptionEn = sellerRequest.DescriptionEn,
                        DescriptionAr = sellerRequest.DescriptionAr,
                        AdminNotes = sellerRequest.ReviewNotes,
                        SubmittedAt = sellerRequest.SubmittedAt ?? sellerRequest.CreatedDateUtc,
                        ReviewedAt = sellerRequest.ReviewedAt,
                        ReviewedByUserName = sellerRequest.ReviewedByUser?.UserName,
                        CreatedDateUtc = sellerRequest.CreatedDateUtc,
                        UpdatedDateUtc = sellerRequest.UpdatedDateUtc
                    };

                    return (true, "Request retrieved successfully", result);
                }

                return (false, "Request not found", null);
            }
            catch (Exception ex)
            {
                return (false, $"Error: {ex.Message}", null);
            }
        }

        /// <summary>
        /// Cancels a pending participation request
        /// </summary>
        public async Task<(bool Success, string Message)> CancelParticipationRequestAsync(
            Guid requestId,
            Guid vendorId,
            Guid userId)
        {
            try
            {
                if (requestId == Guid.Empty || vendorId == Guid.Empty)
                    return (false, "Invalid request or vendor ID");

                // Get the request
                var sellerRequest = await _sellerRequestRepository.FindByIdAsync(requestId);
                if (sellerRequest == null || sellerRequest.VendorId != vendorId || 
                    sellerRequest.RequestType != SellerRequestType.PromoCodeParticipation)
                    return (false, "Request not found");

                // Only pending requests can be cancelled
                if (sellerRequest.Status != SellerRequestStatus.Pending)
                    return (false, "Only pending requests can be cancelled");

                // Soft delete the request
                var deleted = await _sellerRequestRepository.SoftDeleteAsync(requestId, userId);
                if (!deleted)
                    return (false, "Failed to cancel request");

                return (true, "Request cancelled successfully");
            }
            catch (Exception ex)
            {
                return (false, $"Error: {ex.Message}");
            }
        }
    }
}
