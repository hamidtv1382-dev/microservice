using Notification_Services.src._01_Domain.Core.Enums;

namespace Notification_Services.src._02_Application.DTOs.Responses
{
    public class NotificationStatusResponseDto
    {
        public Guid NotificationId { get; set; }
        public NotificationStatus Status { get; set; }
        public string? ErrorMessage { get; set; }
    }   
}
