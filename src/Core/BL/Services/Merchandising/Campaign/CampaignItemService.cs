using AutoMapper;
using BL.Contracts.Service.Merchandising.Campaign;
using BL.Contracts.Service.Vendor;
using Common.Filters;
using DAL.Contracts.Repositories.Merchandising;
using DAL.Services;
using Domains.Entities.Campaign;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs.Campaign;
using Shared.GeneralModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace BL.Services.Merchandising.Campaign
{
	public class CampaignItemService : ICampaignItemService
	{
		private readonly ICampaignItemRepository _campaignItemRepository;
		private readonly ICampaignRepository _campaignRepository;
		private readonly IMapper _mapper;
		public CampaignItemService(ICampaignItemRepository campaignItemRepository, IMapper mapper, ICampaignRepository campaignRepository)
			
		{
			_campaignItemRepository = campaignItemRepository;
			_mapper = mapper;
			_campaignRepository = campaignRepository;
		}

		#region Campaign Items

		/// <summary>
		/// Get all items in a campaign
		/// </summary>
		public async Task<IEnumerable<CampaignItemDto>> GetCampaignItemsAsync(Guid campaignId)
		{
			var items = await _campaignItemRepository.GetCampaignItemsAsync(campaignId);
			return _mapper.Map<List<CampaignItemDto>>(items);
		}
		public async Task<CampaignItemDto> AddItemToCampaignAsync(AddCampaignItemDto dto, Guid userId)
		{
			// Validate campaign exists
			var campaign = await _campaignRepository.GetCampaignByIdAsync(dto.CampaignId);
			if (campaign == null)
			{
				throw new KeyNotFoundException($"Campaign with ID {dto.CampaignId} not found");
			}

			var campaignItem = _mapper.Map<TbCampaignItem>(dto);

			// Set defaults
			campaignItem.Id = Guid.NewGuid();
			campaignItem.CreatedDateUtc = DateTime.UtcNow;
			campaignItem.CreatedBy = userId;
			campaignItem.IsDeleted = false;
			campaignItem.IsActive = true;
			

			
			var result = await _campaignItemRepository.AddItemToCampaignAsync(campaignItem);
			
			//var fullCampaignItem = await _campaignItemRepository.GetCampaignItemsAsync(campaignItem.Id);

			return _mapper.Map<CampaignItemDto>(result);
		}
		/// <summary>
		/// Update sold count when item is purchased
		/// </summary>
		public async Task<bool> UpdateSoldCountAsync(Guid campaignItemId, int quantity)
		{
			return await _campaignItemRepository.IncrementSoldCountAsync(campaignItemId, quantity);
		}

		/// <summary>
		/// Remove item from campaign
		/// </summary>
		public async Task<bool> RemoveItemFromCampaignAsync(Guid campaignItemId, Guid userId)
		{
			return await _campaignItemRepository.RemoveItemFromCampaignAsync(campaignItemId);
		}
		public async Task<ResponseModel<PaginatedSearchResult<CampaignItemDto>>> SearchCampaignItemsAsync(BaseSearchCriteriaModel searchCriteria, Guid campaignId)
		{
			var response = new ResponseModel<PaginatedSearchResult<CampaignItemDto>>();
			try
			{
				// Base query with necessary includes
				var query = _campaignItemRepository.GetQueryable()
							.Include(ci => ci.Campaign) 
							.Include(ci => ci.OfferCombinationPricing)
								.ThenInclude(ocp => ocp.ItemCombination)
									.ThenInclude(ic => ic.Item)
							.Where(ci => !ci.IsDeleted && ci.CampaignId == campaignId); 



				// Apply search filter
				if (!string.IsNullOrWhiteSpace(searchCriteria.SearchTerm))
				{
					var searchTerm = searchCriteria.SearchTerm.ToLower();
					query = query.Where(ci =>
						(ci.OfferCombinationPricing != null &&
						 ci.OfferCombinationPricing.ItemCombination != null &&
						 ci.OfferCombinationPricing.ItemCombination.Item != null &&
						 (ci.OfferCombinationPricing.ItemCombination.Item.TitleEn.ToLower().Contains(searchTerm) ||
						  ci.OfferCombinationPricing.ItemCombination.Item.TitleAr.ToLower().Contains(searchTerm))));
				}

				// Get total count before pagination
				var totalCount = await query.CountAsync();

				// Apply sorting
				query = ApplySorting(query, searchCriteria.SortBy, searchCriteria.SortDirection);

				// Apply pagination
				var pageNumber = searchCriteria.PageNumber > 0 ? searchCriteria.PageNumber : 1;
				var pageSize = searchCriteria.PageSize > 0 ? searchCriteria.PageSize : 10;
				var skip = (pageNumber - 1) * pageSize;

				var campaignItems = await query
					.Skip(skip)
					.Take(pageSize)
					.AsNoTracking()
					.ToListAsync();

				var campaignItemDtos = _mapper.Map<List<CampaignItemDto>>(campaignItems);

				// Build the response
				response.Data = new PaginatedSearchResult<CampaignItemDto>
				{
					Items = campaignItemDtos,
					TotalRecords = totalCount
				};

				response.SetSuccessMessage("Campaign items retrieved successfully");
				response.StatusCode = 200;
			}
			catch (Exception ex)
			{
				response.SetErrorMessage($"Error retrieving campaign items: {ex.Message}");
				response.StatusCode = 500;
			}

			return response;
		}
		//public async Task<ResponseModel<PaginatedSearchResult<CampaignItemDto>>> SearchCampaignItemsForVendorAsync(BaseSearchCriteriaModel searchCriteria, Guid campaignId, Guid userId)
		//{
		//	var response = new ResponseModel<PaginatedSearchResult<CampaignItemDto>>();
		//	try
		//	{
		//		  var vendor = await _dbContext.TbVendors
  //          .AsNoTracking()
  //          .FirstOrDefaultAsync(v => v.UserId == userId && !v.IsDeleted);

		//		if (vendor == null)
		//		{
		//			response.SetErrorMessage("Vendor not found for this user");
		//			response.StatusCode = 404;
		//			return response;
		//		}

				
		//		var query = _campaignItemRepository.GetQueryable()
		//			.Include(ci => ci.Campaign)
		//			.Include(ci => ci.OfferCombinationPricing)
		//				.ThenInclude(ocp => ocp.ItemCombination)
		//					.ThenInclude(ic => ic.Item)
		//			.Where(ci => !ci.IsDeleted
		//				&& ci.CampaignId == campaignId
		//				&& ci.VendorId == vendor.Id); // ✅ Filter by VendorId


		//		// Apply search filter
		//		if (!string.IsNullOrWhiteSpace(searchCriteria.SearchTerm))
		//		{
		//			var searchTerm = searchCriteria.SearchTerm.ToLower();
		//			query = query.Where(ci =>
		//				(ci.OfferCombinationPricing != null &&
		//				 ci.OfferCombinationPricing.ItemCombination != null &&
		//				 ci.OfferCombinationPricing.ItemCombination.Item != null &&
		//				 (ci.OfferCombinationPricing.ItemCombination.Item.TitleEn.ToLower().Contains(searchTerm) ||
		//				  ci.OfferCombinationPricing.ItemCombination.Item.TitleAr.ToLower().Contains(searchTerm))));
		//		}

		//		// Get total count before pagination
		//		var totalCount = await query.CountAsync();

		//		// Apply sorting
		//		query = ApplySorting(query, searchCriteria.SortBy, searchCriteria.SortDirection);

		//		// Apply pagination
		//		var pageNumber = searchCriteria.PageNumber > 0 ? searchCriteria.PageNumber : 1;
		//		var pageSize = searchCriteria.PageSize > 0 ? searchCriteria.PageSize : 10;
		//		var skip = (pageNumber - 1) * pageSize;

		//		var campaignItems = await query
		//			.Skip(skip)
		//			.Take(pageSize)
		//			.AsNoTracking()
		//			.ToListAsync();

		//		var campaignItemDtos = _mapper.Map<List<CampaignItemDto>>(campaignItems);

		//		// Build the response
		//		response.Data = new PaginatedSearchResult<CampaignItemDto>
		//		{
		//			Items = campaignItemDtos,
		//			TotalRecords = totalCount
		//		};

		//		response.SetSuccessMessage("Campaign items retrieved successfully");
		//		response.StatusCode = 200;
		//	}
		//	catch (Exception ex)
		//	{
		//		response.SetErrorMessage($"Error retrieving campaign items: {ex.Message}");
		//		response.StatusCode = 500;
		//	}

		//	return response;
		//}

		// Helper method for sorting
		private IQueryable<TbCampaignItem> ApplySorting(
			IQueryable<TbCampaignItem> query,
			string sortBy,
			string sortDirection)
		{
			var isDescending = sortDirection?.ToLower() == "desc";

			return sortBy?.ToLower() switch
			{
				"name" or "namear" => isDescending
					? query.OrderByDescending(ci => ci.OfferCombinationPricing.ItemCombination.Item.TitleAr)
					: query.OrderBy(ci => ci.OfferCombinationPricing.ItemCombination.Item.TitleAr),

				"nameen" => isDescending
					? query.OrderByDescending(ci => ci.OfferCombinationPricing.ItemCombination.Item.TitleEn)
					: query.OrderBy(ci => ci.OfferCombinationPricing.ItemCombination.Item.TitleEn),

				"isactive" => isDescending
					? query.OrderByDescending(ci => ci.IsActive)
					: query.OrderBy(ci => ci.IsActive),

				"createddateutc" or "created" => isDescending
					? query.OrderByDescending(ci => ci.CreatedDateUtc)
					: query.OrderBy(ci => ci.CreatedDateUtc),

				_ => query.OrderByDescending(ci => ci.CreatedDateUtc) // Default sorting
			};
		}

		#endregion
	}
}
