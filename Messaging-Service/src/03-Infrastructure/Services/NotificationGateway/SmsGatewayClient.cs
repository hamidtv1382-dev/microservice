using Messaging_Service.src._01_Domain.Core.Interfaces.Services;

namespace Messaging_Service.src._03_Infrastructure.Services.NotificationGateway
{
    public class SmsGatewayClient : INotificationService
    {
        public async Task SendSmsAsync(string phoneNumber, string message)
        {
            await Task.Run(() =>
            {
                Console.WriteLine($"[SMS Gateway] Sending to {phoneNumber}: {message}");
            });
        }

        public Task SendPushAsync(Guid userId, string title, string message)
        {
            throw new NotSupportedException("SmsGatewayClient does not support Push notifications.");
        }

        public Task SendEmailAsync(string email, string subject, string body)
        {
            throw new NotSupportedException("SmsGatewayClient does not support Email notifications.");
        }
    }
}
