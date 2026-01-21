using Api.Controllers.v1.Base;
using BL.Contracts.Service.Wallet.Customer;
using Common.Enumerations.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.Order.Payment.PaymentProcessing;
using Shared.DTOs.Wallet.Customer;
using Shared.GeneralModels;

namespace Api.Controllers.v1.Wallet.Customer
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(Roles = nameof(UserType.Customer))]
    public class CustomerWalletController : BaseController
    {
        private readonly ICustomerWalletService _customerWalletService;

        public CustomerWalletController(ICustomerWalletService customerWalletService)
        {
            _customerWalletService = customerWalletService;
        }

        [HttpGet("Balance")]
        public async Task<IActionResult> GetBalance()
        {
            var balance = await _customerWalletService.GetBalanceAsync(GuidUserId.ToString());
            return Ok(new ResponseModel<decimal>
            {
                Success = true,
                Data = balance
            });
        }

        [HttpGet("Transactions")]
        public async Task<IActionResult> GetTransactions()
        {
            var transactions = await _customerWalletService.GetTransactionsAsync(GuidUserId.ToString());
            return Ok(new ResponseModel<IEnumerable<CustomerWalletTransactionsDto>>
            {
                Success = true,
                Data = transactions
            });
        }

        [HttpPost("Charging/Request")]
        public async Task<IActionResult> RequestCharging([FromBody] WalletChargingRequestDto request)
        {
            if (request == null)
                return BadRequest(new ResponseModel<string> { Success = false, Message = "request must be not null." });

            try
            {
                var result = await _customerWalletService.InitiateChargingRequestAsync(request, UserId);
                return Ok(new ResponseModel<PaymentResult>
                {
                    Success = true,
                    Data = result,
                    Message = "Charging request initiated. Use PaymentUrl to complete payment."
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<string> { Success = false, Message = ex.Message });
            }
        }

        [HttpPost("Charging/Complete")]
        public async Task<IActionResult> CompleteCharging([FromBody] ChargingCompletionDto request)
        {
            if (string.IsNullOrEmpty(request.GatewayTransactionId))
                return BadRequest(new ResponseModel<string> { Success = false, Message = "GatewayTransactionId is required" });

            var result = await _customerWalletService.VerifyChargingPaymentAsync(request.GatewayTransactionId, request.IsSuccess, request.FailureReason);

            return Ok(new ResponseModel<bool>
            {
                Success = result,
                Data = result,
                Message = result ? "Charging payment verified and completed." : "Charging verification failed."
            });
        }
    }
}
