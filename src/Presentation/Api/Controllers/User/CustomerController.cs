using Api.Controllers.Base;
using BL.Contracts.Service.Customer;
using Common.Enumerations.User;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Resources;
using Shared.DTOs.Brand;
using Shared.DTOs.Customer;
using Shared.DTOs.Vendor;
using Shared.GeneralModels;
using Shared.GeneralModels.SearchCriteriaModels;


namespace Api.Controllers.User
{
	[Route("api/[controller]")]
	[ApiController]
	public class CustomerController : BaseController
	{
		private readonly ICustomerService _customerService;

		public CustomerController(ICustomerService customerService,
			Serilog.ILogger logger) : base(logger)
		{
			_customerService = customerService;
		}


		/// <summary>
		/// Retrieves all customers.
		/// </summary>
		[HttpGet]
		//[Authorize(Roles = nameof(UserRole.Admin))]
		public async Task<IActionResult> Get()
		{
			try
			{
				var customer = await _customerService.GetAllAsync();

				return Ok(new ResponseModel<IEnumerable<CustomerDto>>
				{
					Success = true,
					Message = GetResource<NotifiAndAlertsResources>(nameof(NotifiAndAlertsResources.DataRetrieved)),
					Data = customer
				});
			}
			catch (Exception ex)
			{
				return HandleException(ex);
			}
		}

		/// <summary>
		/// Retrieves a customer by ID.
		/// </summary>
		/// <param name="id">The ID of the customer.</param>
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

				var customer = await _customerService.FindByIdAsync(id);
				if (customer == null)
					return NotFound(new ResponseModel<string>
					{
						Success = false,
						Message = GetResource<NotifiAndAlertsResources>(nameof(NotifiAndAlertsResources.NoDataFound))
					});

				return Ok(new ResponseModel<CustomerDto>
				{
					Success = true,
					Message = GetResource<NotifiAndAlertsResources>(nameof(NotifiAndAlertsResources.DataRetrieved)),
					Data = customer
				});
			}
			catch (Exception ex)
			{
				return HandleException(ex);
			}
		}

		
		/// <summary>
		/// Searches customers with pagination and filtering.
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

				var result = await _customerService.SearchAsync(criteriaModel);

				if (result == null || !result.Items.Any())
				{
					return Ok(new ResponseModel<PaginatedDataModel<CustomerDto>>
					{
						Success = true,
						Message = GetResource<NotifiAndAlertsResources>(nameof(NotifiAndAlertsResources.NoDataFound)),
						Data = result
					});
				}

				return Ok(new ResponseModel<PaginatedDataModel<CustomerDto>>
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
		/// Adds a new customer or updates an existing one.
		/// </summary>
		/// <param name="dto">The customer data.</param>
		[HttpPost("save")]
		//[Authorize(Roles = nameof(UserRole.Admin))]
		public async Task<IActionResult> Save([FromBody] CustomerDto dto)
		{
			try
			{
				if (!ModelState.IsValid)
					return BadRequest(new ResponseModel<string>
					{
						Success = false,
						Message = "Invalid customer data."
					});

				var result = await _customerService.SaveAsync(dto, GuidUserId);
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
		/// Deletes a customer by ID (soft delete).
		/// </summary>
		/// <param name="id">The ID of the customer to delete.</param>
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
						Message = "Invalid customer ID."
					});

				var success = await _customerService.DeleteAsync(id, GuidUserId);
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

