using Notification_Services.src._01_Domain.Core.Enums;

namespace Notification_Services.src._02_Application.DTOs.Responses
{
    public class NotificationResponseDto
    {
        public Guid Id { get; set; }
        public NotificationType Type { get; set; }
        public NotificationStatus Status { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public DateTime? ScheduledAt { get; set; }
        public DateTime? SentAt { get; set; }
        public string? ErrorMessage { get; set; }
        public List<NotificationRecipientDto> Recipients { get; set; }
        public List<NotificationAttachmentDto> Attachments { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class NotificationRecipientDto
    {
        public Guid Id { get; set; }
        public RecipientType Type { get; set; }
        public string Contact { get; set; }
        public bool IsRead { get; set; }
    }

    public class NotificationAttachmentDto
    {
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public string FileUrl { get; set; }
    }
}
