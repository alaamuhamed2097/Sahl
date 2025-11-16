using Api.Controllers.Base;
using BL.Contracts.Service.Customer;
using BL.Contracts.Service.Vendor;
using BL.Service.Customer;
using DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Resources;
using Shared.DTOs.Customer;
using Shared.DTOs.Vendor;
using Shared.GeneralModels;
using Shared.GeneralModels.SearchCriteriaModels;

namespace Api.Controllers.User
{
	[Route("api/[controller]")]
	[ApiController]
	public class VendorController : BaseController
	{
		private readonly IVendorService _vendorService;

		
		public VendorController(IVendorService vendorService,
			Serilog.ILogger logger) : base(logger)
		{
			_vendorService = vendorService;
		}



		/// <summary>
		/// Retrieves all vendors.
		/// </summary>
		[HttpGet]
		//[Authorize(Roles = nameof(UserRole.Admin))]
		public async Task<IActionResult> Get()
		{
			try
			{
				var vendor = await _vendorService.GetAllAsync();

				return Ok(new ResponseModel<IEnumerable<VendorDto>>
				{
					Success = true,
					Message = GetResource<NotifiAndAlertsResources>(nameof(NotifiAndAlertsResources.DataRetrieved)),
					Data = vendor
				});
			}
			catch (Exception ex)
			{
				return HandleException(ex);
			}
		}

		/// <summary>
		/// Retrieves a vendor by ID.
		/// </summary>
		/// <param name="id">The ID of the vendor.</param>
		[HttpGet("{id}")]
		//[Authorize(Roles = nameof(UserRole.Admin))]
		public async Task<IActionResult> Get(Guid id)
		{
			try
			{
				if (id == Guid.Empty)
					return BadRequest(new ResponseModel<string>
					{
						Success = false,
						Message = GetResource<NotifiAndAlertsResources>(nameof(NotifiAndAlertsResources.InvalidInputAlert))
					});

				var vendor = await _vendorService.FindByIdAsync(id);
				if (vendor == null)
					return NotFound(new ResponseModel<string>
					{
						Success = false,
						Message = GetResource<NotifiAndAlertsResources>(nameof(NotifiAndAlertsResources.NoDataFound))
					});

				return Ok(new ResponseModel<VendorDto>
				{
					Success = true,
					Message = GetResource<NotifiAndAlertsResources>(nameof(NotifiAndAlertsResources.DataRetrieved)),
					Data = vendor
				});
			}
			catch (Exception ex)
			{
				return HandleException(ex);
			}
		}

		/// <summary>
		/// Retrieves favorite vendors for public display.
		/// </summary>

		/// <summary>
		/// Searches vendors with pagination and filtering.
		/// </summary>
		/// <param name="criteriaModel">Search criteria including pagination and filters.</param>
		[HttpGet("search")]
		//[Authorize(Roles = nameof(UserRole.Admin))]
		public async Task<IActionResult> Search([FromQuery] BaseSearchCriteriaModel criteriaModel)
		{
			try
			{
				// Validate and set default pagination values if not provided
				criteriaModel.PageNumber = criteriaModel.PageNumber < 1 ? 1 : criteriaModel.PageNumber;
				criteriaModel.PageSize = criteriaModel.PageSize < 1 || criteriaModel.PageSize > 100 ? 10 : criteriaModel.PageSize;

				var result = await _vendorService.SearchAsync(criteriaModel);

				if (result == null || !result.Items.Any())
				{
					return Ok(new ResponseModel<PaginatedDataModel<VendorDto>>
					{
						Success = true,
						Message = GetResource<NotifiAndAlertsResources>(nameof(NotifiAndAlertsResources.NoDataFound)),
						Data = result
					});
				}

				return Ok(new ResponseModel<PaginatedDataModel<VendorDto>>
				{
					Success = true,
					Message = GetResource<NotifiAndAlertsResources>(nameof(NotifiAndAlertsResources.DataRetrieved)),
					Data = result
				});
			}
			catch (ArgumentNullException)
			{
				return BadRequest(new ResponseModel<string>
				{
					Success = false,
					Message = GetResource<NotifiAndAlertsResources>(nameof(NotifiAndAlertsResources.InvalidInputAlert))
				});
			}
			catch (ArgumentOutOfRangeException ex)
			{
				return BadRequest(new ResponseModel<string>
				{
					Success = false,
					Message = ex.Message
				});
			}
			catch (Exception ex)
			{
				return HandleException(ex);
			}
		}


		/// <summary>
		/// Adds a new vendor or updates an existing one.
		/// </summary>
		/// <param name="dto">The vendor data.</param>
		[HttpPost("save")]
		//[Authorize(Roles = nameof(UserRole.Admin))]
		public async Task<IActionResult> Save([FromBody] VendorDto dto)
		{
			try
			{
				if (!ModelState.IsValid)
					return BadRequest(new ResponseModel<string>
					{
						Success = false,
						Message = "Invalid vendor data."
					});

				var result = await _vendorService.SaveAsync(dto, GuidUserId);
				if (!result.Success)
					return Ok(new ResponseModel<string>
					{
						Success = false,
						Message = GetResource<NotifiAndAlertsResources>(nameof(NotifiAndAlertsResources.SaveFailed))
					});

				return Ok(new ResponseModel<string>
				{
					Success = true,
					Message = GetResource<NotifiAndAlertsResources>(nameof(NotifiAndAlertsResources.SavedSuccessfully))
				});
			}
			catch (Exception ex)
			{
				return HandleException(ex);
			}
		}

		/// <summary>
		/// Deletes a vendor by ID (soft delete).
		/// </summary>
		/// <param name="id">The ID of the vendor to delete.</param>
		[HttpPost("delete")]
		//[Authorize(Roles = nameof(UserRole.Admin))]
		public async Task<IActionResult> Delete([FromBody] Guid id)
		{
			try
			{
				if (id == Guid.Empty)
					return BadRequest(new ResponseModel<string>
					{
						Success = false,
						Message = "Invalid vendor ID."
					});

				var success = await _vendorService.DeleteAsync(id, GuidUserId);
				if (!success)
					return BadRequest(new ResponseModel<string>
					{
						Success = false,
						Message = GetResource<NotifiAndAlertsResources>(nameof(NotifiAndAlertsResources.DeleteFailed))
					});

				return Ok(new ResponseModel<string>
				{
					Success = true,
					Message = GetResource<NotifiAndAlertsResources>(nameof(NotifiAndAlertsResources.DeletedSuccessfully))
				});
			}
			catch (Exception ex)
			{
				return HandleException(ex);
			}
		}



	}
}
