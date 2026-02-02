namespace Notification_Services.src._02_Application.Interfaces
{
    public interface IExternalMessagingGateway
    {
        Task<bool> SendEmailAsync(string to, string subject, string body, List<string> attachments = null);
        Task<bool> SendSmsAsync(string to, string message);
        Task<bool> SendPushAsync(Guid userId, string title, string body);
    }
}
