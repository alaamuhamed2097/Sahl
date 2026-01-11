using Api.Controllers.v1.Base;
using Asp.Versioning;
using BL.Contracts.Service.Order.Payment;
using Microsoft.AspNetCore.Mvc;
using Resources;
using Shared.DTOs.Order.Payment;
using Shared.GeneralModels;

namespace Api.Controllers.v1.Order
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class PaymentMethodController : BaseController
    {
        private readonly IPaymentMethodService _paymentMethodService;

        public PaymentMethodController(IPaymentMethodService paymentMethodService)
        {
            _paymentMethodService = paymentMethodService;
        }

        /// <summary>
        /// Get Active Payment Methods
        /// </summary>
        /// <remarks>
        /// API Version: 1.0+
        /// </remarks>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetActivePaymentMethods()
        {
            var methods = await _paymentMethodService.GetActivePaymentMethodsAsync();

            return Ok(new ResponseModel<List<PaymentMethodDto>>
            {
                Success = true,
                Message = NotifiAndAlertsResources.DataRetrieved,
                Data = methods
            });
        }
    }
}