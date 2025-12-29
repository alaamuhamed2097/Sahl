using AutoMapper;
using BL.Contracts.IMapper;
using BL.Contracts.Service.HomeSlider;
using BL.Services.Base;
using DAL.Contracts.Repositories;
using DAL.Models;
using DAL.ResultModels;
using Domains.Entities.HomeSlider;
using Resources;
using Shared.DTOs.HomeSlider;
using Shared.GeneralModels;
using Shared.GeneralModels.SearchCriteriaModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BL.Services.HomeSlider
{
	public class HomePageSliderService : BaseService<TbHomePageSlider, HomePageSliderDto>, IHomePageSliderService
	{
		private readonly ITableRepository<TbHomePageSlider> _baseRepository;
		private readonly IBaseMapper _mapper;

		public HomePageSliderService(ITableRepository<TbHomePageSlider> baseRepository, IBaseMapper mapper)
			: base(baseRepository, mapper)
		{
			_baseRepository = baseRepository;
			_mapper = mapper;
		}

		/// <summary>
		/// Get paginated list of home page sliders with search and filter
		/// </summary>
		public async Task<PagedResult<HomePageSliderDto>> GetPage(BaseSearchCriteriaModel criteriaModel)
		{
			try
			{
				if (criteriaModel == null)
					criteriaModel = new BaseSearchCriteriaModel();

				// Get queryable from repository
				var query = _baseRepository.GetQueryable();

				// Apply filters
				if (!string.IsNullOrWhiteSpace(criteriaModel.SearchTerm))
				{
					var searchTerm = criteriaModel.SearchTerm.Trim().ToLower();
					query = query.Where(x =>
						(x.TitleAr != null && x.TitleAr.ToLower().Contains(searchTerm)) ||
						(x.TitleEn != null && x.TitleEn.ToLower().Contains(searchTerm)));
				}

				// Filter by IsDeleted (active only - not deleted)
				query = query.Where(x => x.IsDeleted == false);

				// Get total count before pagination
				var totalCount = query.Count();

				// Apply sorting
				query = query.OrderBy(x => x.DisplayOrder)
							.ThenByDescending(x => x.CreatedDateUtc);

				// Apply pagination
				var pageNumber = criteriaModel.PageNumber > 0 ? criteriaModel.PageNumber : 1;
				var pageSize = criteriaModel.PageSize > 0 ? criteriaModel.PageSize : 10;

				query = query.Skip((pageNumber - 1) * pageSize)
							.Take(pageSize);

				// Execute query and map to DTOs
				var entities = query.ToList();
				var dtos = _mapper.MapList<TbHomePageSlider, HomePageSliderDto>(entities);

				// Return paged result
				return new PagedResult<HomePageSliderDto>(dtos, totalCount);
			}
			catch (Exception ex)
			{
				// Log error here
				return new PagedResult<HomePageSliderDto>(new List<HomePageSliderDto>(), 0);
			}
		}

		/// <summary>
		/// Get all active sliders within current date range
		/// </summary>
		public async Task<IEnumerable<HomePageSliderDto>> GetActiveSliders()
		{
			try
			{
				var currentDate = DateTime.UtcNow.Date;

				var query = _baseRepository.GetQueryable()
					.Where(x => x.IsDeleted == false &&
							   x.StartDate <= currentDate &&
							   x.EndDate >= currentDate)
					.OrderBy(x => x.DisplayOrder);

				var entities = query.ToList();
				var dtos = _mapper.MapList<TbHomePageSlider, HomePageSliderDto>(entities);

				return dtos;
			}
			catch (Exception ex)
			{
				// Log error here
				return new List<HomePageSliderDto>();
			}
		}

		/// <summary>
		/// Validate date range and display order before save
		/// </summary>
		private bool ValidateSlider(HomePageSliderDto dto, out string errorMessage)
		{
			errorMessage = string.Empty;

			if (dto.StartDate >= dto.EndDate)
			{
				errorMessage = "Start date must be before end date";
				return false;
			}

			if (dto.DisplayOrder < 0)
			{
				errorMessage = "Display order must be a positive number";
				return false;
			}

			if (string.IsNullOrWhiteSpace(dto.ImageUrl))
			{
				errorMessage = "Image URL is required";
				return false;
			}

			return true;
		}

		/// <summary>
		/// Override save to add validation
		/// </summary>
		//public override async Task<SaveResult> SaveAsync(HomePageSliderDto dto, Guid userId)
		//{
		//	if (!ValidateSlider(dto, out string errorMessage))
		//	{
		//		return new SaveResult
		//		{
		//			IsSuccess = false,
		//			Message = errorMessage
		//		};
		//	}

		//	return await base.SaveAsync(dto, userId);
		//}
	}
}