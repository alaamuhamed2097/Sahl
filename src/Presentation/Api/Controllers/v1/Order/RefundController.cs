using Api.Controllers.v1.Base;
using Asp.Versioning;
using BL.Contracts.Service.Order.Payment;
using BL.Services.Order.Payment;
using Common.Enumerations.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.Order.Payment.Refund;
using Shared.GeneralModels;

namespace Api.Controllers.v1.Order
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize]
    public class RefundController : BaseController
    {
        private readonly IRefundService _refundService;

        public RefundController(IRefundService refundService)
        {
            _refundService = refundService;
        }

        // GET api/v1/Refund/order/{orderId}
        [HttpGet("order/{orderId}")]
        public async Task<IActionResult> GetByOrder(Guid orderId)
        {
            var result = await _refundService.GetRefundRequestByOrderDetailIdAsync(orderId);
            // Since RefundRequestDto now has Id, and Dashboard expects RefundDto (which matches RefundRequestDto mostly),
            // Deserialization should work fine.

            if (result == null)
            {
                return Ok(new ResponseModel<RefundRequestDto?>
                {
                    Success = true,
                    Message = "No refund found",
                    Data = null
                });
            }

            return Ok(new ResponseModel<RefundRequestDto>
            {
                Success = true,
                Data = result
            });
        }

        // POST api/v1/Refund/changeRefundStatus
        [HttpPost("changeRefundStatus")]
        public async Task<IActionResult> ChangeStatus([FromBody] RefundResponseDto dto)
        {
            var updateDto = new UpdateRefundStatusDto
            {
                NewStatus = dto.CurrentState,
                Notes = dto.AdminComments,
                RejectionReason = dto.CurrentState == RefundStatus.Rejected ? dto.AdminComments : null,
                RefundAmount = dto.RefundAmount > 0 ? dto.RefundAmount : null,
                // ApprovedItemsCount and TrackingNumber might need mapping if in RefundResponseDto
            };

            var result = await _refundService.UpdateRefundStatusAsync(dto.RefundId, updateDto, GuidUserId.ToString());

            if (result.IsSuccess)
            {
                return Ok(new ResponseModel<bool>
                {
                    Success = true,
                    Message = "Status updated successfully",
                    Data = true
                });
            }
            else
            {
                return Ok(new ResponseModel<bool>
                {
                    Success = false,
                    Message = result.ErrorMessage,
                    Data = false
                });
            }
        }
    }
}
