using Notification_Services.src._01_Domain.Core.Enums;

namespace Notification_Services.src._02_Application.DTOs.Responses
{
    public class TemplateResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Subject { get; set; }
        public string BodyContent { get; set; }
        public NotificationType Type { get; set; }
        public string LanguageCode { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
