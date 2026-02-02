namespace Notification_Services.src._01_Domain.Core.Events
{
    public class NotificationFailedEvent
    {
        public Guid NotificationId { get; set; }
        public string ErrorMessage { get; set; }
        public DateTime OccurredOn { get; set; }

        public NotificationFailedEvent(Guid notificationId, string errorMessage)
        {
            NotificationId = notificationId;
            ErrorMessage = errorMessage;
            OccurredOn = DateTime.UtcNow;
        }
    }
}
