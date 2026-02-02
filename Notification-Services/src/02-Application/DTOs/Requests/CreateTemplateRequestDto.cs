using Notification_Services.src._01_Domain.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace Notification_Services.src._02_Application.DTOs.Requests
{
    public class CreateTemplateRequestDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(200)]
        public string Subject { get; set; }

        [Required]
        public string BodyContent { get; set; }

        [Required]
        public NotificationType Type { get; set; }

        [MaxLength(10)]
        public string LanguageCode { get; set; } = "en";
    }
}
