namespace Messaging_Service.src._02_Application.Exceptions
{
    public class MessageFailedException : Exception
    {
        public MessageFailedException(Guid messageId, string reason) : base($"Message with ID {messageId} failed. Reason: {reason}")
        {
        }
    }
}
