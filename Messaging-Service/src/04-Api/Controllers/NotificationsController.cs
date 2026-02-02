using Messaging_Service.src._02_Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Messaging_Service.src._04_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationApplicationService _notificationService;

        public NotificationsController(INotificationApplicationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpPost("sms")]
        public async Task<IActionResult> SendSms([FromBody] SendSmsRequestDto request)
        {
            await _notificationService.SendSmsNotificationAsync(request.PhoneNumber, request.Message);
            return Ok();
        }

        [HttpPost("email")]
        public async Task<IActionResult> SendEmail([FromBody] SendEmailRequestDto request)
        {
            await _notificationService.SendEmailNotificationAsync(request.Email, request.Subject, request.Body);
            return Ok();
        }

        [HttpPost("push")]
        public async Task<IActionResult> SendPush([FromBody] SendPushRequestDto request)
        {
            await _notificationService.SendPushNotificationAsync(request.UserId, request.Title, request.Message);
            return Ok();
        }
    }

    public class SendSmsRequestDto
    {
        public string PhoneNumber { get; set; }
        public string Message { get; set; }
    }

    public class SendEmailRequestDto
    {
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }

    public class SendPushRequestDto
    {
        public System.Guid UserId { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
    }
}
