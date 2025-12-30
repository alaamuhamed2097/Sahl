using Asp.Versioning;
using BL.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1.Notification
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class RabbitMQNotificationController : ControllerBase
    {
        [HttpGet("rabbitmq")]
        public async Task<IActionResult> RabbitMqTest()
        {
            var testService = new RabbitMqService();
            await testService.SendMessageAsync("Test message");

            return Ok(new { success = true, message = "RabbitMQ test executed. Check server console output." });
        }
    }
}
