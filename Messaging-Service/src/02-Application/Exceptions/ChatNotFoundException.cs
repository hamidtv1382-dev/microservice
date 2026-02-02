namespace Messaging_Service.src._02_Application.Exceptions
{
    public class ChatNotFoundException : Exception
    {
        public ChatNotFoundException(Guid chatId) : base($"Chat with ID {chatId} not found.")
        {
        }
    }
}
