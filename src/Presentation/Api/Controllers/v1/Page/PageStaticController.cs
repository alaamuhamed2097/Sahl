using Api.Controllers.v1.Base;
using Asp.Versioning;
using BL.Contracts.Service.Order.Cart;
using BL.Contracts.Services.Page;
using Common.Enumerations;
using Common.Enumerations.User;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Resources;
using Shared.DTOs.Page;
using Shared.GeneralModels;
using Shared.GeneralModels.SearchCriteriaModels;

namespace Api.Controllers.v1.Page
{
	[ApiController]
	[ApiVersion("1.0")]
	[Route("api/v{version:apiVersion}/[controller]")]
	[Authorize(Roles = nameof(UserRole.Admin))]
	public class PageStaticController : BaseController
	{
		private readonly IPageService _pageService;

		public PageStaticController(IPageService pageService)
		{
			_pageService = pageService;
		}

		/// <summary>
		/// Get all pages
		/// </summary>
		[HttpGet]
		[Authorize(Roles = nameof(UserRole.Admin))]
		public async Task<IActionResult> GetAll()
		{

			var pages = await _pageService.GetAllAsync();

			return Ok(new ResponseModel<IEnumerable<PageDto>>
			{
				Success = true,
				Message = NotifiAndAlertsResources.DataRetrieved,
				Data = pages
			});

		}

		/// <summary>
		/// Get page by ID
		/// </summary>
		[HttpGet("{id}")]
		[Authorize(Roles = nameof(UserRole.Admin))]
		public async Task<IActionResult> Get(Guid id)
		{

			if (id == Guid.Empty)
			{
				return BadRequest(new ResponseModel<string>
				{
					Success = false,
					Message = NotifiAndAlertsResources.InvalidInputAlert
				});
			}

			var page = await _pageService.GetByIdAsync(id);
			if (page == null)
			{
				return NotFound(new ResponseModel<string>
				{
					Success = false,
					Message = NotifiAndAlertsResources.NoDataFound
				});
			}

			return Ok(new ResponseModel<PageDto>
			{
				Success = true,
				Message = NotifiAndAlertsResources.DataRetrieved,
				Data = page
			});

		}

		/// <summary>
		/// Get page by type
		/// </summary>
		[HttpGet("by-type/{pageType}")]
		[AllowAnonymous]
		public async Task<IActionResult> GetByType(PageType pageType)
		{

			var page = await _pageService.GetByTypeAsync(pageType);
			if (page == null)
			{
				return NotFound(new ResponseModel<string>
				{
					Success = false,
					Message = NotifiAndAlertsResources.NoDataFound
				});
			}

			return Ok(new ResponseModel<PageDto>
			{
				Success = true,
				Message = NotifiAndAlertsResources.DataRetrieved,
				Data = page
			});

		}

		/// <summary>
		/// Search pages with pagination (Admin only)
		/// </summary>
		[HttpGet("search")]
		[Authorize(Roles = nameof(UserRole.Admin))]
		public async Task<IActionResult> Search([FromQuery] BaseSearchCriteriaModel criteria)
		{

			criteria.PageNumber = criteria.PageNumber < 1 ? 1 : criteria.PageNumber;
			criteria.PageSize = criteria.PageSize < 1 || criteria.PageSize > 100 ? 10 : criteria.PageSize;

			var result = await _pageService.SearchAsync(criteria);

			return Ok(new ResponseModel<PagedResult<PageDto>>
			{
				Success = true,
				Message = result.Items.Any() ? NotifiAndAlertsResources.DataRetrieved : NotifiAndAlertsResources.NoDataFound,
				Data = result
			});

		}

		/// <summary>
		/// Create or update page (Admin only)
		/// </summary>
		[HttpPost("save")]
		[Authorize(Roles = nameof(UserRole.Admin))]
		public async Task<IActionResult> Save([FromBody] PageDto dto)
		{

			if (!ModelState.IsValid)
			{
				return BadRequest(new ResponseModel<string>
				{
					Success = false,
					Message = NotifiAndAlertsResources.InvalidInputAlert
				});
			}

			var success = await _pageService.SaveAsync(dto, GuidUserId);
			if (!success)
			{
				return BadRequest(new ResponseModel<string>
				{
					Success = false,
					Message = NotifiAndAlertsResources.SaveFailed
				});
			}

			return Ok(new ResponseModel<string>
			{
				Success = true,
				Message = NotifiAndAlertsResources.SavedSuccessfully
			});

		}

		/// <summary>
		/// Delete page (Admin only)
		/// </summary>
		[HttpPost("delete")]
		[Authorize(Roles = nameof(UserRole.Admin))]
		public async Task<IActionResult> Delete([FromBody] Guid pageId)
		{

			if (pageId == Guid.Empty)
			{
				return BadRequest(new ResponseModel<string>
				{
					Success = false,
					Message = NotifiAndAlertsResources.InvalidInputAlert
				});
			}

			var success = await _pageService.DeleteAsync(pageId, GuidUserId);
			if (!success)
			{
				return BadRequest(new ResponseModel<string>
				{
					Success = false,
					Message = NotifiAndAlertsResources.DeleteFailed
				});
			}

			return Ok(new ResponseModel<string>
			{
				Success = true,
				Message = NotifiAndAlertsResources.DeletedSuccessfully
			});
		}

	}
}
