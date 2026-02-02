namespace Notification_Services.src._01_Domain.Core.Events
{
    public class NotificationSentEvent
    {
        public Guid NotificationId { get; set; }
        public DateTime OccurredOn { get; set; }

        public NotificationSentEvent(Guid notificationId)
        {
            NotificationId = notificationId;
            OccurredOn = DateTime.UtcNow;
        }
    }
}
