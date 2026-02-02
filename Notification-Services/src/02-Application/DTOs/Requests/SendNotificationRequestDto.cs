using System.ComponentModel.DataAnnotations;

namespace Notification_Services.src._02_Application.DTOs.Requests
{
    public class SendNotificationRequestDto
    {
        [Required]
        public Guid NotificationId { get; set; }
    }
}
