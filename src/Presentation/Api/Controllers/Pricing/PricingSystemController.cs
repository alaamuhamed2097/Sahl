using Api.Controllers.Base;
using BL.Contracts.Service.Pricing;
using Common.Enumerations.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.Pricing;
using Shared.GeneralModels;

namespace Api.Controllers.Pricing
{
    [ApiController]
    [Route("api/[controller]")]
    public class PricingSystemController : BaseController
    {
        private readonly IPricingService _pricingService;

        public PricingSystemController(IPricingService pricingService, Serilog.ILogger logger) : base(logger)
        {
            _pricingService = pricingService;
        }

        [HttpGet]
        [Authorize(Roles = nameof(UserType.Admin))]
        public async Task<IActionResult> Get()
        {
            var list = await _pricingService.GetAllSystemsAsync();
            return Ok(new ResponseModel<IEnumerable<PricingSystemSettingDto>> { Success = true, Data = list });
        }
    }
}
