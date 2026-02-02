namespace Messaging_Service.src._02_Application.Services.Interfaces
{
    public interface INotificationApplicationService
    {
        Task SendSmsNotificationAsync(string phoneNumber, string message);
        Task SendEmailNotificationAsync(string email, string subject, string body);
        Task SendPushNotificationAsync(Guid userId, string title, string message);
    }
}
