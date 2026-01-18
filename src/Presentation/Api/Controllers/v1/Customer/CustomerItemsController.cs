using Api.Controllers.v1.Base;
using Asp.Versioning;
using BL.Contracts.Service.Catalog.Item;
using BL.Contracts.Service.Customer;
using Common.Enumerations.User;
using Common.Filters;
using DAL.Contracts.Repositories.Customer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Resources;
using Shared.DTOs.Catalog.Item;
using Shared.GeneralModels;

namespace Api.Controllers.v1.Catalog
{
    /// <summary>
    /// Advanced item search controller using optimized stored procedure
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CustomerItemsController : BaseController
    {
        private readonly ICustomerRecommendedItemsService _customerRecommendedItemsService;

        public CustomerItemsController(ICustomerRecommendedItemsService customerRecommendedItemsService)
        {
            _customerRecommendedItemsService = customerRecommendedItemsService;
        }

        /// <summary>
        /// Search recommended items for an customer
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpPost("recommended")]
        [Authorize(Roles =nameof(UserType.Customer))]
        public async Task<IActionResult> SearchCustomerRecommendedItems([FromBody] BaseSearchCriteriaModel filter)
        {
            if (filter == null)
                return BadRequest(CreateErrorResponse<PagedSpSearchResultDto>(NotifiAndAlertsResources.InvalidInputAlert));

            // Validate and normalize filter parameters
            ValidateBaseSearchCriteriaModel(filter);

            // Execute stored procedure search
            var result = await _customerRecommendedItemsService.SearchCustomerRecommendedItemsAsync(filter,UserId);

            if (result?.Items?.Any() != true)
                return Ok(CreateSuccessResponse(result, NotifiAndAlertsResources.NoDataFound));

            return Ok(CreateSuccessResponse(result, NotifiAndAlertsResources.DataRetrieved));
        }
    }
}