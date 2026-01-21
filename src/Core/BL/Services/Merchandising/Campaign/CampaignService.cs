using AutoMapper;
using BL.Contracts.Service.Merchandising.Campaign;
using Common.Filters;
using DAL.Contracts.Repositories.Merchandising;
using Domains.Entities.Campaign;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs.Campaign;
using Shared.GeneralModels;

namespace BL.Services.Merchandising.Campaign;

/// <summary>
/// Campaign Service - manages campaigns and flash sales
/// </summary>
public class CampaignService : ICampaignService
{
	private readonly ICampaignRepository _campaignRepository;
	private readonly IMapper _mapper;

	public CampaignService(
		ICampaignRepository campaignRepository,
		IMapper mapper)
	{
		_campaignRepository = campaignRepository;
		_mapper = mapper;
	}

	#region Campaign Management

	/// <summary>
	/// Get all campaigns (admin)
	/// </summary>
	public async Task<IEnumerable<CampaignDto>> GetAllCampaignsAsync()
	{
		var campaigns = await _campaignRepository.GetAsync();
		return _mapper.Map<IEnumerable<CampaignDto>>(campaigns.ToList());
	}

	/// <summary>
	/// Get all active campaigns
	/// </summary>
	public async Task<IEnumerable<CampaignDto>> GetActiveCampaignsAsync()
	{
		var campaigns = await _campaignRepository.GetActiveCampaignsAsync();
		return _mapper.Map<IEnumerable<CampaignDto>>(campaigns);
	}

	/// <summary>
	/// Get campaign by ID
	/// </summary>
	public async Task<CampaignDto> GetCampaignByIdAsync(Guid id)
	{
		var campaign = await _campaignRepository.GetCampaignByIdAsync(id);
		return campaign != null ? _mapper.Map<CampaignDto>(campaign) : null;
	}

	/// <summary>
	/// Get all active flash sales
	/// </summary>
	public async Task<IEnumerable<CampaignDto>> GetActiveFlashSalesAsync()
	{
		var flashSales = await _campaignRepository.GetActiveFlashSalesAsync();
		return _mapper.Map<IEnumerable<CampaignDto>>(flashSales);
	}

	public async Task<ResponseModel<PaginatedSearchResult<CampaignDto>>> SearchCampaignsAsync(CampaignSearchCriteriaModel searchCriteria)
	{
		var response = new ResponseModel<PaginatedSearchResult<CampaignDto>>();
		try
		{
			var query = _campaignRepository.GetQueryable().Where(c => !c.IsDeleted);

			// ============ Apply Filters ============

			// Search filter
			if (!string.IsNullOrWhiteSpace(searchCriteria.SearchTerm))
			{
				var searchTerm = searchCriteria.SearchTerm.ToLower();
				query = query.Where(c =>
					c.NameEn.ToLower().Contains(searchTerm) ||
					c.NameAr.ToLower().Contains(searchTerm) );
			}


			if (searchCriteria.Status.HasValue)
			{
				if (searchCriteria.Status == 1)
				{
					query = query.Where(c => c.IsActive == true);
				}
				else if (searchCriteria.Status == 2)
				{
					query = query.Where(c => c.IsActive == false);
				}
			}

			// Type filter: 1 = Flash Sale, 2 = Regular
			//if (searchCriteria.Type.HasValue)
			//{
			//	if (searchCriteria.Type == 1)
			//	{
			//		query = query.Where(c => c.IsFlashSale == true);
			//	}
			//	else if (searchCriteria.Type == 2)
			//	{
			//		query = query.Where(c => c.IsFlashSale == false);
			//	}
			//}
			// Date range filters
			//if (searchCriteria.StartDateFrom.HasValue)
			//{
			//	query = query.Where(c => c.StartDate >= searchCriteria.StartDateFrom.Value);
			//}

			//if (searchCriteria.StartDateTo.HasValue)
			//{
			//	query = query.Where(c => c.StartDate <= searchCriteria.StartDateTo.Value);
			//}

			//if (searchCriteria.EndDateFrom.HasValue)
			//{
			//	query = query.Where(c => c.EndDate >= searchCriteria.EndDateFrom.Value);
			//}

			//if (searchCriteria.EndDateTo.HasValue)
			//{
			//	query = query.Where(c => c.EndDate <= searchCriteria.EndDateTo.Value);
			//}

			// Get total count before pagination
			var totalCount = await query.CountAsync();

			// ============ Apply Sorting ============
			query = ApplySorting(query, searchCriteria.SortBy, searchCriteria.SortDirection);

			// ============ Apply Pagination ============
			var pageNumber = searchCriteria.PageNumber > 0 ? searchCriteria.PageNumber : 1;
			var pageSize = searchCriteria.PageSize > 0 ? searchCriteria.PageSize : 10;
			var skip = (pageNumber - 1) * pageSize;

			var campaigns = await query
				.Skip(skip)
				.Take(pageSize)
				.ToListAsync();

			var campaignDtos = _mapper.Map<List<CampaignDto>>(campaigns);

			// Build the response
			response.Data = new PaginatedSearchResult<CampaignDto>
			{
				Items = campaignDtos,
				TotalRecords = totalCount
			};

			response.SetSuccessMessage("Campaigns retrieved successfully");
			response.StatusCode = 200;
		}
		catch (Exception ex)
		{
			//_logger.LogError(ex, "Error retrieving campaigns");
			response.SetErrorMessage($"Error retrieving campaigns: {ex.Message}");
			response.StatusCode = 500;
		}

		return response;
	}
	

	/// <summary>
	/// Create new campaign
	/// </summary>
	public async Task<CampaignDto> CreateCampaignAsync(CreateCampaignDto dto, Guid userId)
	{
		var campaign = _mapper.Map<TbCampaign>(dto);

		// Set defaults
		campaign.Id = Guid.NewGuid();
		campaign.CreatedDateUtc = DateTime.UtcNow;
		campaign.CreatedBy = userId;
		campaign.IsDeleted = false;
		campaign.IsActive = true;

		var result = await _campaignRepository.CreateAsync(campaign, userId);

		if (!result.Success)
		{
			throw new InvalidOperationException("Failed to create campaign");
		}

		var created = await _campaignRepository.GetCampaignByIdAsync(result.Id);
		return _mapper.Map<CampaignDto>(created);
	}

	/// <summary>
	/// Update campaign
	/// </summary>
	public async Task<CampaignDto> UpdateCampaignAsync(UpdateCampaignDto dto, Guid userId)
	{
		var campaign = await _campaignRepository.GetCampaignByIdAsync(dto.Id);
		if (campaign == null)
		{
			throw new KeyNotFoundException($"Campaign with ID {dto.Id} not found");
		}

		// Map updates
		_mapper.Map(dto, campaign);
		campaign.UpdatedDateUtc = DateTime.UtcNow;
		campaign.UpdatedBy = userId;

		var result = await _campaignRepository.UpdateAsync(campaign, userId);

		if (!result.Success)
		{
			throw new InvalidOperationException("Failed to update campaign");
		}

		var updated = await _campaignRepository.GetCampaignByIdAsync(dto.Id);
		return _mapper.Map<CampaignDto>(updated);
	}

	/// <summary>
	/// Delete campaign (soft delete)
	/// </summary>
	public async Task<bool> DeleteCampaignAsync(Guid id, Guid userId)
	{
		// Check if campaign has items
		var items = await _campaignRepository.GetCampaignByIdAsync(id);
		if (items == null)
			throw new InvalidOperationException("Cannot delete campaign with items. Remove all items first.");


		return await _campaignRepository.SoftDeleteAsync(id, userId);
	}

	#endregion

	#region Validation

	/// <summary>
	/// Validate campaign configuration
	/// </summary>
	private void ValidateCampaign(TbCampaign campaign)
	{
		var errors = new List<string>();

		if (string.IsNullOrWhiteSpace(campaign.NameAr))
			errors.Add("Arabic name is required");

		if (string.IsNullOrWhiteSpace(campaign.NameEn))
			errors.Add("English name is required");

		if (campaign.StartDate >= campaign.EndDate)
			errors.Add("End date must be after start date");

		if (campaign.IsFlashSale && !campaign.FlashSaleEndTime.HasValue)
			errors.Add("Flash sale end time is required for flash sales");

		if (errors.Any())
		{
			throw new ArgumentException($"Validation failed: {string.Join(", ", errors)}");
		}
	}

	private IQueryable<TbCampaign> ApplySorting(IQueryable<TbCampaign> query, string? sortBy, string? sortDirection)
	{
		if (string.IsNullOrWhiteSpace(sortBy))
		{
			// Default sorting
			return query.OrderByDescending(c => c.CreatedDateUtc);
		}

		var isDescending = sortDirection?.Equals("desc", StringComparison.OrdinalIgnoreCase) ?? false;

		query = sortBy.ToLower() switch
		{
			"name" => isDescending
				? query.OrderByDescending(c => c.NameEn)
				: query.OrderBy(c => c.NameEn),

			"nameen" => isDescending
				? query.OrderByDescending(c => c.NameAr)
				: query.OrderBy(c => c.NameAr),

			"type" => isDescending
				? query.OrderBy(c => c.IsFlashSale)
				: query.OrderByDescending(c => c.IsFlashSale),

			"status" => isDescending
				? query.OrderBy(c => c.IsActive)
				: query.OrderByDescending(c => c.IsActive),

			"startdate" => isDescending
				? query.OrderByDescending(c => c.StartDate)
				: query.OrderBy(c => c.StartDate),

			"enddate" => isDescending
				? query.OrderByDescending(c => c.EndDate)
				: query.OrderBy(c => c.EndDate),

			"createddate" or "createdat" => isDescending
				? query.OrderByDescending(c => c.CreatedDateUtc)
				: query.OrderBy(c => c.CreatedDateUtc),

			"updateddate" or "updatedat" => isDescending
				? query.OrderByDescending(c => c.UpdatedDateUtc)
				: query.OrderBy(c => c.UpdatedDateUtc),

			_ => query.OrderByDescending(c => c.CreatedDateUtc) // Default
		};

		return query;
	}

	#endregion
}