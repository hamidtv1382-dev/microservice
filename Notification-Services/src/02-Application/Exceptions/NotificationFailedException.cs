namespace Notification_Services.src._02_Application.Exceptions
{
    public class NotificationFailedException : Exception
    {
        public NotificationFailedException(string message) : base(message) { }
        public NotificationFailedException(string message, Exception innerException) : base(message, innerException) { }
    }
}
