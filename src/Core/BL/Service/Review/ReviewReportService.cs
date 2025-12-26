using BL.Contracts.IMapper;
using BL.Contracts.Service.Review;
using BL.Extensions;
using BL.Service.Base;
using Common.Enumerations.Review;
using DAL.Contracts.Repositories;
using DAL.Contracts.Repositories.Review;
using DAL.Models;
using DAL.Repositories;
using DAL.ResultModels;
using Domains.Entities.ECommerceSystem.Customer;
using Domains.Entities.ECommerceSystem.Review;
using FirebaseAdmin.Auth;
using Microsoft.EntityFrameworkCore;
using Resources;
using Serilog;
using Shared.DTOs.Review;
using Shared.GeneralModels.SearchCriteriaModels;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace BL.Service.Review
{
	public class ReviewReportService : BaseService<TbReviewReport, ReviewReportDto>, IReviewReportService
	{
		private readonly IReviewReportRepository _reportRepo;
		private readonly IItemReviewRepository _reviewRepo;
		private readonly ITableRepository<TbReviewReport> _tableRepository;
		private readonly ITableRepository<TbCustomer> _tableCustomerRepository;



		private readonly IBaseMapper _mapper;
		private readonly ILogger _logger;

		public ReviewReportService(
			ITableRepository<TbReviewReport> tableRepository,
			IReviewReportRepository reportRepo,
			IItemReviewRepository reviewRepo,
			IBaseMapper mapper,
			ILogger logger,
			ITableRepository<TbCustomer> tableCustomerRepository) : base(tableRepository, mapper)
		{
			_reportRepo = reportRepo;
			_reviewRepo = reviewRepo;
			_mapper = mapper;
			_logger = logger;
			_tableRepository = tableRepository;
			_tableCustomerRepository = tableCustomerRepository;
		}

		public async Task<SaveResult> SubmitReportAsync(
			ReviewReportDto reportDto,
			string userId,
			CancellationToken cancellationToken = default)
		{
			try
			{
				// Validation
				if (reportDto.Reason == default)
					throw new ArgumentException("Reason is required");


				var currentCustomer = await _tableCustomerRepository
		   .GetQueryable()
		   .FirstOrDefaultAsync(c => c.UserId == userId && !c.IsDeleted, cancellationToken);

				// Check if review exists
				var review = await _reviewRepo.FindByIdAsync(reportDto.ItemReviewId, cancellationToken);
				if (review == null)
					throw new KeyNotFoundException($"Review with ID {reportDto.ItemReviewId} not found");

				// Prevent reporting own review
				if (review.CustomerId == currentCustomer.Id   )
					throw new InvalidOperationException("You cannot report your own review");

				// Check if already reported
				var alreadyReported = await _reportRepo.IsAlreadyReportedAsync(
					reportDto.ItemReviewId,
					reportDto.CustomerId,
					cancellationToken);

				if (alreadyReported)
					throw new InvalidOperationException("You have already reported this review");

				// Create report
				var report = _mapper.MapModel<ReviewReportDto, TbReviewReport>(reportDto);
				report.CustomerId = currentCustomer.Id;
				report.ItemReviewId = reportDto.ItemReviewId;
				report.Details = reportDto.Details;
				report.Reason = reportDto.Reason;
				Guid userGuid = Guid.Parse(userId);
				var createdReport = await _reportRepo.CreateAsync(report, userGuid, cancellationToken);
				if(!createdReport.Success)
					throw new Exception("Failed to submit report");

				return createdReport;
			}
			catch (Exception ex)
			{
				_logger.Error(ex, $"Error in {nameof(SubmitReportAsync)}");
				throw;
			}
		}

		public async Task<ReviewReportDto?> GetReportByIdAsync(
			Guid reportId,
			CancellationToken cancellationToken = default)
		{
			try
			{
				var report = await _reportRepo.GetByIdAsync(reportId, cancellationToken);

				if (report == null)
					return null;

				return _mapper.MapModel<TbReviewReport, ReviewReportDto>(report);
			}
			catch (Exception ex)
			{
				_logger.Error(ex, $"Error in {nameof(GetReportByIdAsync)}");
				throw;
			}
		}

		//public async Task<PaginatedDataModel<ReviewReportDto>> GetAllReportsAsync(
		//	ReviewReportStatus? status = null,
		//	int pageNumber = 1,
		//	int pageSize = 10,
		//	CancellationToken cancellationToken = default)
		//{
		//	try
		//	{
		//		var result = await _reportRepo.GetAllReportsAsync(status, pageNumber, pageSize, cancellationToken);
		//		var reportDtos = _mapper.MapList<TbReviewReport, ReviewReportDto>(result.Data).ToList();
		//		return new PaginatedDataModel<ReviewReportDto>(reportDtos, result.TotalCount);
		//	}
		//	catch (Exception ex)
		//	{
		//		_logger.Error(ex, $"Error in {nameof(GetAllReportsAsync)}");
		//		throw;
		//	}
		//}
		public async Task<PagedResult<ReviewReportDto>> GetPaginatedReviewReportsAsync(
	ReviewReportSearchCriteriaModel criteriaModel,
	CancellationToken cancellationToken = default)
		{
			if (criteriaModel == null)
				throw new ArgumentNullException(nameof(criteriaModel));

			if (criteriaModel.PageNumber < 1)
				throw new ArgumentOutOfRangeException(nameof(criteriaModel.PageNumber),
					ValidationResources.PageNumberGreaterThanZero);

			if (criteriaModel.PageSize < 1 || criteriaModel.PageSize > 100)
				throw new ArgumentOutOfRangeException(nameof(criteriaModel.PageSize),
					ValidationResources.PageSizeRange);

			Expression<Func<TbReviewReport, bool>> filter = x => !x.IsDeleted;

			var searchTerm = criteriaModel.SearchTerm?.Trim().ToLower();
			if (!string.IsNullOrWhiteSpace(searchTerm))
			{
				filter = filter.And(x =>
					x.Details != null && x.Details.ToLower().Contains(searchTerm));
			}

			if (criteriaModel.ReviewId.HasValue)
				filter = filter.And(x => x.ItemReviewId == criteriaModel.ReviewId.Value);

			if (criteriaModel.CustomerId.HasValue)
				filter = filter.And(x => x.ItemReview.CustomerId == criteriaModel.CustomerId.Value);

			if (criteriaModel.ReportedByCustomerId.HasValue)
				filter = filter.And(x => x.CustomerId == criteriaModel.ReportedByCustomerId.Value);

			if (criteriaModel.Reason.HasValue)
				filter = filter.And(x => x.Reason == criteriaModel.Reason.Value);

			if (criteriaModel.Status.HasValue)
				filter = filter.And(x => x.Status == criteriaModel.Status.Value);

			if (criteriaModel.FromDate.HasValue)
				filter = filter.And(x => x.CreatedDateUtc >= criteriaModel.FromDate.Value);

			if (criteriaModel.ToDate.HasValue)
				filter = filter.And(x => x.CreatedDateUtc <= criteriaModel.ToDate.Value);

			Func<IQueryable<TbReviewReport>, IOrderedQueryable<TbReviewReport>> orderBy = null;

			if (!string.IsNullOrWhiteSpace(criteriaModel.SortBy))
			{
				switch (criteriaModel.SortBy.ToLowerInvariant())
				{
					case "status":
						orderBy = criteriaModel.SortDirection == "desc"
							? q => q.OrderByDescending(x => x.Status)
							: q => q.OrderBy(x => x.Status);
						break;

					case "createddateutc":
					default:
						orderBy = criteriaModel.SortDirection == "desc"
							? q => q.OrderByDescending(x => x.CreatedDateUtc)
							: q => q.OrderBy(x => x.CreatedDateUtc);
						break;
				}
			}
			else
			{
				orderBy = q => q.OrderByDescending(x => x.CreatedDateUtc);
			}

			var result = await _tableRepository.GetPageAsync(
				criteriaModel.PageNumber,
				criteriaModel.PageSize,
				filter,
				orderBy: orderBy
			);

			var dtoList = _mapper.MapList<TbReviewReport, ReviewReportDto>(result.Items);

			return new PagedResult<ReviewReportDto>(dtoList, result.TotalRecords);
		}


		public async Task<IEnumerable<ReviewReportDto>> GetReportsByReviewIdAsync(
			Guid reviewId,
			CancellationToken cancellationToken = default)
		{
			try
			{
				var reports = await _reportRepo.GetReportsByReviewIdAsync(reviewId, cancellationToken);
				return _mapper.MapList<TbReviewReport, ReviewReportDto>(reports).ToList();
			}
			catch (Exception ex)
			{
				_logger.Error(ex, $"Error in {nameof(GetReportsByReviewIdAsync)}");
				throw;
			}
		}
		private static ReviewStatus MapReportStatusToReviewStatus(ReviewReportStatus reportStatus)
		{
			return reportStatus switch
			{
				ReviewReportStatus.Pending => ReviewStatus.Pending,

				ReviewReportStatus.Reviewed => ReviewStatus.Flagged,
				

				ReviewReportStatus.Resolved => ReviewStatus.Approved,
			

				_ => throw new ArgumentOutOfRangeException(nameof(reportStatus))
			};
		}

		public async Task<SaveResult> ResolveReportAsync(
	Guid reportId,
	Guid adminId,
	CancellationToken cancellationToken = default)
		{
			try
			{
				if (reportId == Guid.Empty)
					throw new ArgumentException(nameof(reportId));

				var report = await _reportRepo.FindByIdAsync(reportId, cancellationToken);
				if (report == null)
					return new SaveResult { Success = false };


				report.Status = ReviewReportStatus.Resolved;
				

				return await _reportRepo.UpdateAsync(report, adminId, cancellationToken);
			}
			catch (Exception ex)
			{
				_logger.Error(ex, $"Error in {nameof(ResolveReportAsync)}");
				throw;
			}
		}


		public async Task<bool> MarkReviewAsFlaggedAsync(
			Guid reviewId,
			string adminId,
			CancellationToken cancellationToken = default)
		{
			try
			{
				var review = await _reviewRepo.FindByIdAsync(reviewId, cancellationToken);

				if (review == null)
					return false;

				review.Status = ReviewStatus.Flagged;

				var result = await _reviewRepo.UpdateAsync(review, Guid.Parse(adminId), cancellationToken);
				return result.Success;
			}
			catch (Exception ex)
			{
				_logger.Error(ex, $"Error in {nameof(MarkReviewAsFlaggedAsync)}");
				throw;
			}
		}
	}
}
