namespace Notification_Services.src._02_Application.Exceptions
{
    public class InvalidRecipientException : Exception
    {
        public InvalidRecipientException(string message) : base(message) { }
        public InvalidRecipientException(string message, Exception innerException) : base(message, innerException) { }
    }
}
