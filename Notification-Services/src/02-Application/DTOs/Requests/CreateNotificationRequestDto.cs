using Notification_Services.src._01_Domain.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace Notification_Services.src._02_Application.DTOs.Requests
{
    public class CreateNotificationRequestDto
    {
        [Required]
        public NotificationType Type { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        [Required]
        [MaxLength(5000)]
        public string Message { get; set; }

        public List<CreateRecipientDto> Recipients { get; set; } = new List<CreateRecipientDto>();
        public List<string> AttachmentUrls { get; set; } = new List<string>();
        public DateTime? ScheduledAt { get; set; }
    }

    public class CreateRecipientDto
    {
        [Required]
        public RecipientType Type { get; set; }

        [Required]
        public string Contact { get; set; }
    }
}
