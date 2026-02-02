using Notification_Services.src._01_Domain.Core.Common;
using Notification_Services.src._01_Domain.Core.Enums;
using Notification_Services.src._01_Domain.Core.Events;
using Notification_Services.src._01_Domain.Core.ValueObjects;

namespace Notification_Services.src._01_Domain.Core.Aggregates.Notification
{
    public class Notification : AggregateRoot
    {
        public Guid? TemplateId { get; private set; }
        public NotificationType Type { get; private set; }
        public NotificationStatus Status { get; private set; }
        public string Title { get; private set; }
        public string Message { get; private set; }
        public DateTime? ScheduledAt { get; private set; }
        public DateTime? SentAt { get; private set; }
        public string? ErrorMessage { get; private set; }

        private readonly List<NotificationRecipient> _recipients = new();
        public IReadOnlyCollection<NotificationRecipient> Recipients => _recipients.AsReadOnly();

        private readonly List<NotificationAttachment> _attachments = new();
        public IReadOnlyCollection<NotificationAttachment> Attachments => _attachments.AsReadOnly();

        private Notification() { }

        public Notification(NotificationType type, NotificationTitle title, NotificationMessage message)
        {
            Type = type;
            Status = NotificationStatus.Pending;
            Title = title.Value;
            Message = message.Value;
            AddDomainEvent(new NotificationCreatedEvent(Id, Guid.Empty));
        }

        public void SetTemplate(Guid templateId) => TemplateId = templateId;
        public void AddRecipient(NotificationRecipient recipient) => _recipients.Add(recipient);
        public void AddAttachment(NotificationAttachment attachment) => _attachments.Add(attachment);

        public void Schedule(DateTime scheduledAt)
        {
            if (scheduledAt < DateTime.UtcNow) throw new ArgumentException("Scheduled time must be in the future.");
            ScheduledAt = scheduledAt;
        }

        public void MarkAsSent()
        {
            Status = NotificationStatus.Sent;
            SentAt = DateTime.UtcNow;
            AddDomainEvent(new NotificationSentEvent(Id));
        }

        public void MarkAsFailed(string errorMessage)
        {
            Status = NotificationStatus.Failed;
            ErrorMessage = errorMessage;
            AddDomainEvent(new NotificationFailedEvent(Id, errorMessage));
        }
    }
}