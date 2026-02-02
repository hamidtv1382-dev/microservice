using Messaging_Service.src._01_Domain.Core.Interfaces.Services;
using Messaging_Service.src._02_Application.Services.Interfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Messaging_Service.src._02_Application.Services.Implementations
{
    public class NotificationApplicationService : INotificationApplicationService
    {
        private readonly INotificationService _notificationService;

        public NotificationApplicationService(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public async Task SendSmsNotificationAsync(string phoneNumber, string message)
        {
            await _notificationService.SendSmsAsync(phoneNumber, message);
        }

        public async Task SendEmailNotificationAsync(string email, string subject, string body)
        {
            await _notificationService.SendEmailAsync(email, subject, body);
        }

        public async Task SendPushNotificationAsync(Guid userId, string title, string message)
        {
            await _notificationService.SendPushAsync(userId, title, message);
        }
    }
}
