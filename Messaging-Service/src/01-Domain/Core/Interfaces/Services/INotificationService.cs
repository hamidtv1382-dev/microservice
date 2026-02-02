namespace Messaging_Service.src._01_Domain.Core.Interfaces.Services
{
    public interface INotificationService
    {
        Task SendSmsAsync(string phoneNumber, string message);
        Task SendPushAsync(Guid userId, string title, string message);
        Task SendEmailAsync(string email, string subject, string body);
    }
}
