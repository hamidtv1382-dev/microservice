using Microsoft.AspNetCore.Mvc;
using Notification_Services.src._02_Application.DTOs.Requests;
using Notification_Services.src._02_Application.DTOs.Responses;
using Notification_Services.src._02_Application.Services.Interfaces;

namespace Notification_Services.src._04_Api.Controllers
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

        [HttpPost]
        public async Task<ActionResult<NotificationResponseDto>> CreateNotification([FromBody] CreateNotificationRequestDto request)
        {
            var result = await _notificationService.CreateNotificationAsync(request);
            return CreatedAtAction(nameof(GetNotification), new { id = result.Id }, result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<NotificationResponseDto>> GetNotification(Guid id)
        {
            var result = await _notificationService.GetNotificationAsync(id);
            return Ok(result);
        }

        [HttpGet("pending")]
        public async Task<ActionResult<IEnumerable<NotificationResponseDto>>> GetPendingNotifications()
        {
            var result = await _notificationService.GetPendingNotificationsAsync();
            return Ok(result);
        }

        [HttpGet("recipient/{recipientId}")]
        public async Task<ActionResult<IEnumerable<NotificationResponseDto>>> GetNotificationsByRecipient(Guid recipientId)
        {
            var result = await _notificationService.GetNotificationsByRecipientAsync(recipientId);
            return Ok(result);
        }

        [HttpPost("{id}/send")]
        public async Task<ActionResult<NotificationStatusResponseDto>> SendNotification(Guid id)
        {
            var result = await _notificationService.SendNotificationAsync(id);
            return Ok(result);
        }
    }
}
