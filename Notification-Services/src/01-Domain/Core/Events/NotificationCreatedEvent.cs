namespace Notification_Services.src._01_Domain.Core.Events
{
    public class NotificationCreatedEvent
    {
        public Guid NotificationId { get; set; }
        public Guid RecipientId { get; set; }
        public DateTime OccurredOn { get; set; }

        public NotificationCreatedEvent(Guid notificationId, Guid recipientId)
        {
            NotificationId = notificationId;
            RecipientId = recipientId;
            OccurredOn = DateTime.UtcNow;
        }
    }
}
