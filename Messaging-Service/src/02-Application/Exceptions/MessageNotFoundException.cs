namespace Messaging_Service.src._02_Application.Exceptions
{
    public class MessageNotFoundException : Exception
    {
        public MessageNotFoundException(Guid messageId) : base($"Message with ID {messageId} not found.")
        {
        }
    }
}
