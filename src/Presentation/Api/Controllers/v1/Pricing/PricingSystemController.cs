using Api.Controllers.Base;
using Asp.Versioning;
using BL.Contracts.Service.Pricing;
using Common.Enumerations.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.Pricing;
using Shared.GeneralModels;

namespace Api.Controllers.v1.Pricing
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class PricingSystemController : BaseController
    {
        private readonly IPricingService _pricingService;

        public PricingSystemController(IPricingService pricingService, Serilog.ILogger logger) : base(logger)
        {
            _pricingService = pricingService;
        }

        /// <summary>
        /// Get all pricing systems.
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// Requires Admin role.
        /// </remarks>
        [HttpGet]
        [Authorize(Roles = nameof(UserType.Admin))]
        public async Task<IActionResult> Get()
        {
            var list = await _pricingService.GetAllSystemsAsync();
            return Ok(new ResponseModel<IEnumerable<PricingSystemSettingDto>> { Success = true, Data = list });
        }
    }
}
