namespace Messaging_Service.src._02_Application.Exceptions
{
    public class MessageNotApprovedException : Exception
    {
        public MessageNotApprovedException(Guid messageId) : base($"Message w ith ID {messageId} was not approved.")
        {
        }
    }
}
